using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyramidSolitaireDirector : MonoBehaviour
{
    public Deck drawPile;
    public Deck discardPile;
    public Deck wastePile;

    public Vector3 pyramidTop;



    public float x_pyramidSpace;
    public float y_pyramidSpace;

    public UIManager ui;

    public int remainingCards;

    private List<List<Card>> pyramidCards = new List<List<Card>>();

    private bool drawPileEmpty = false;

    bool isHardMode = false;

    // Start is called before the first frame update
    void Start()
    {
        drawPile.FillDeck();
    }

    public void DrawPileIsEmpty()
    {
        drawPileEmpty = true;
    }

    public void DiscardCard()
    {
        if (!drawPileEmpty)
        {
            drawPile.DrawTopCardToDeck(discardPile, true, true);
        }
        else
        {
            discardPile.DrawTopCardToDeck(wastePile, false, false);
        }
    }
    public void WasteCard(Card cardToWaste)
    {
        if (cardToWaste.currentDeck != null)
        {
            cardToWaste.currentDeck.DrawTopCardToDeck(wastePile,false,false);
        }
        else
        {
            wastePile.AddCardToDeck(cardToWaste, false, false);
            remainingCards--;
        }

        cardToWaste.wasted = true;

        if (remainingCards == 0)
        {
            ui.WinGame();
        }
        else
        {
            foreach (List<Card> pyramidRow in pyramidCards)
            {
                foreach (Card pyramidCard in pyramidRow)
                {
                    if (pyramidCard.currentDeck == null && pyramidCard.pyramidRow < 6)
                    {
                        bool cardCovered = CheckCardCovered(pyramidCard);
                        if (!cardCovered && !pyramidCard.faceUp)
                        {
                            pyramidCard.FlipCard();
                        }
                    }
                }
            }
        }
    }

    public bool CheckCardCovered(Card checkCard)
    {
        Card leftBelowCard = pyramidCards[checkCard.pyramidRow + 1][checkCard.pyramidColumn];
        Card rightBelowCard = pyramidCards[checkCard.pyramidRow + 1][checkCard.pyramidColumn + 1];
        if (leftBelowCard.wasted && rightBelowCard.wasted)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void TryMatchCards(Card card1,Card card2 = null)
    {
        if (card2 == null)
        {
            if (card1.value == 13)
            {
                WasteCard(card1);
                ui.ShowRemainingCards(remainingCards);
            }
        }
        else
        {
            if (card1.value + card2.value == 13)
            {
                WasteCard(card1);
                WasteCard(card2);
                
                ui.ShowRemainingCards(remainingCards);
            }
            else
            {
                ScreenShake.Shake(0.2f, 0.2f);
            }
        }
    }

    IEnumerator DealPyramidCards()
    {
        pyramidCards = new List<List<Card>>();
        remainingCards = 0;

        int cardn = 1;
        for (int r = 0; r < 7; r++)
        {
            List<Card> pyramidRow = new List<Card>();
            
            for (int i = 0; i < cardn; i++)
            {
                float yOffset = -r* y_pyramidSpace;
                float rowWidth = (cardn-1)* x_pyramidSpace;
                float xOffset = (-rowWidth / 2) + (i * x_pyramidSpace);

                bool flip = true;

                if (isHardMode)
                {
                    flip = (r == 6);
                }

                Card drawnCard = drawPile.DrawTopCardToPyramid(pyramidTop+new Vector3(xOffset,yOffset, -r * 0.001f),true, flip,r * 0.005f, r,i);
                Debug.Log("(" + r + "," + i + ")");
                pyramidRow.Add(drawnCard);
                
                remainingCards++;
                yield return new WaitForSeconds(0.2f);
            }
            pyramidCards.Add(pyramidRow);
            cardn++;
        }
        yield return new WaitForSeconds(0.2f);
        DiscardCard();
        yield return new WaitForSeconds(1f);

        ui.StartPlaying();
        ui.ShowRemainingCards(remainingCards);
    }

    public void PlayPyramidSolitaire(bool hardmodeOn)
    {
        isHardMode = hardmodeOn;
        ui.PrepareNewGame();
        StartCoroutine(DealPyramidCards());
    }

}
