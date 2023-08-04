using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyramidSolitaireDirector : MonoBehaviour
{
    //References to the 3 piles in Pyramid Solitaire
    public Deck drawPile;
    public Deck discardPile;
    public Deck wastePile;

    //The transform to parent pyramid cards to
    public Transform pyramidTop;


    //Horizontal and Vertical separation of cards in the pyramid
    public float x_pyramidSpace;
    public float y_pyramidSpace;

    public UIManager ui;

    //The number of cards remaining in the pyramid
    public int remainingCards;

    //Data structure to hold references to all the cards added to the pyramid.
    //Cards are not removed from this structure when they're moved to the waste pile, but they're marked as "wasted"
    private List<List<Card>> pyramidCards = new List<List<Card>>();

    public bool drawPileEmpty = false;

    //Hard Mode variable
    bool isHardMode = false;

    float cooldownTimer;

    void Start()
    {
        drawPile.FillDeck();
    }

    private void Update()
    {
        //Handle the cooldown on clicking the Discard button
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer < 0)
        {
            //Cooldown is complete, cards can be discarded again
            cooldownTimer = 0;
        }
    }

    public void DrawPileIsEmpty()
    {
        drawPileEmpty = true;
    }

    //Discard a card to either the discard pile or waste pile
    public void DiscardCard()
    {
        if (cooldownTimer == 0)
        {
            //If the draw pile is not empty, draw from the draw pile, otherwise draw from the discard pile
            if (!drawPileEmpty)
            {
                drawPile.DrawTopCardToDeck(discardPile, true, true);
            }
            else
            {
                discardPile.DrawTopCardToDeck(wastePile, false, false);
            }
            //Reset the cooldown timer
            cooldownTimer = 0.6f;
        }
    }

    //Remove a card from the pyramid or the discard pile and move it to the waste pile
    public void WasteCard(Card cardToWaste)
    {
        if (cardToWaste.currentDeck != null)
        {
            //Remove card from the discard pile and move to the waste pile
            cardToWaste.currentDeck.DrawTopCardToDeck(wastePile,false,false);
        }
        else
        {
            //Add card to the waste pile
            wastePile.AddCardToDeck(cardToWaste, false, false);
            //One less card in the pyramid
            remainingCards--;
        }

        //The card is in the waste pile
        cardToWaste.wasted = true;

        //If there are no cards remaining in the pyramid, the player wins the game
        if (remainingCards == 0)
        {
            ui.WinGame();
        }
        else if (isHardMode) //It's not necessary to check if cards are uncovered in easy mode
        {
            CheckForUncoveredCards();
        }
    }

    //Go through each card in the pyramid and check if there are any cards covering it
    private void CheckForUncoveredCards()
    {
        foreach (List<Card> pyramidRow in pyramidCards)
        {
            foreach (Card pyramidCard in pyramidRow)
            {
                //If the card is still in the pyramid and not on the last row, check if it's uncovered
                if (pyramidCard.currentDeck == null && pyramidCard.pyramidRow < 6)
                {
                    bool cardCovered = CheckCardCovered(pyramidCard);
                    //If the pyramid card is uncovered and face down, flip it so it's face up
                    if (!cardCovered && !pyramidCard.faceUp)
                    {
                        pyramidCard.FlipCard();
                    }
                }
            }
        }
    }

    //Check if a card in the pyramid is covered by any cards on top of it
    public bool CheckCardCovered(Card checkCard)
    {
        //For a card to not be covered, the card in the same position in the row below and the card directly to the right in the row below need to not still be in the pyramid
        Card leftBelowCard = pyramidCards[checkCard.pyramidRow + 1][checkCard.pyramidColumn];
        Card rightBelowCard = pyramidCards[checkCard.pyramidRow + 1][checkCard.pyramidColumn + 1];
        if (leftBelowCard.wasted && rightBelowCard.wasted)
        {
            return false; //Card is uncovered
        }
        else
        {
            return true; //Card is covered
        }
    }

    //Check if two cards can be paired together, or one card if it's a King
    public void TryMatchCards(Card card1,Card card2 = null)
    {
        if (card2 == null)
        {
            // If only one card is selected and it's a King, remove it from the pyramid
            if (card1.value == 13)
            {
                WasteCard(card1);
                ui.ShowRemainingCards(remainingCards);
            }
        }
        else
        {
            //If the values of the paired cards add up to 13, remove them from the pyramid
            if (card1.value + card2.value == 13)
            {
                WasteCard(card1);
                WasteCard(card2);
                ui.ShowRemainingCards(remainingCards);
            }
            else
            {
                //The cards cannot be paired together
                ScreenShake.Shake(0.2f, 0.2f);
            }
        }
    }

    //Deal cards from the draw pile to the pyramid
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
                //Calculate the position each card should move to in the pyramid
                float yOffset = -r* y_pyramidSpace;
                float rowWidth = (cardn-1)* x_pyramidSpace;
                float xOffset = (-rowWidth / 2) + (i * x_pyramidSpace);

                bool flip = true;

                //If playing in hard move, only the cards on the bottom row should be face up at the start
                if (isHardMode)
                {
                    flip = (r == 6);
                }

                Card drawnCard = drawPile.DrawTopCardToPyramid(pyramidTop.position+new Vector3(xOffset,yOffset, -r * 0.001f),true, flip,r * 0.005f, r,i);
                drawnCard.transform.SetParent(pyramidTop);

                Debug.Log("Card drawn to pyramid at position (" + r + "," + i + ")");
                pyramidRow.Add(drawnCard);
                
                remainingCards++;
                yield return new WaitForSeconds(0.2f);
            }
            //Add the new row to the list of pyramid card rows
            pyramidCards.Add(pyramidRow);
            cardn++;
        }
        yield return new WaitForSeconds(0.2f);
        //Discard the first card from the draw pile to the discard pile
        DiscardCard();
        yield return new WaitForSeconds(1f);

        ui.StartPlaying();
        ui.ShowRemainingCards(remainingCards);
    }

    //Start playing Pyramid Solitaire at the desired difficulty level
    public void PlayPyramidSolitaire(bool hardmodeOn)
    {
        isHardMode = hardmodeOn;
        ui.PrepareNewGame();
        StartCoroutine(DealPyramidCards());
    }

}
