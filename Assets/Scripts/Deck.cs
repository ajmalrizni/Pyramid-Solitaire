using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public string deckName;

    public GameManager gameManager;
    public PyramidSolitaireDirector solitaire;

    //cardList contains a list of all the cards in the deck
    [HideInInspector]
    private List<Card> cardList = new List<Card>();

    //the Card at the top of the deck
    public Card topCard;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        solitaire = FindObjectOfType<PyramidSolitaireDirector>();
    }

    //Draw the top card from the deck and move it to a new location
    public Card DrawTopCardToPyramid(Vector3 newPosition, bool makeHoverable, bool flipOver, float newHeight = 0f,int pyramidR = -1,int pyramidC = -1)
    {
        topCard = cardList[cardList.Count - 1];
        topCard.PlaceCard(newPosition, flipOver, newHeight);
        cardList.Remove(topCard);
        topCard.hoverable = makeHoverable;
        topCard.currentDeck = null; //Reset the card's deck

        //Set the card's pyramid variables
        topCard.pyramidRow = pyramidR;
        topCard.pyramidColumn = pyramidC;

        Card topCardOld = topCard;
        topCard = cardList[cardList.Count - 1]; //Set the deck's top card to the new top card

        return topCardOld;
    }

    //Draw a card from this deck to another deck
    public void DrawTopCardToDeck(Deck newDeck, bool makeHoverable, bool flipOver)
    {
        topCard = cardList[cardList.Count - 1];
        topCard.hoverable = makeHoverable;

        newDeck.AddCardToDeck(topCard, makeHoverable, flipOver);
        cardList.Remove(topCard);

        if (cardList.Count > 0)
        {
            topCard = cardList[cardList.Count - 1]; //Set the deck's top card to the new top card
        }
        else
        {
            //If there are no cards left in the pile, the deck has no top card
            topCard = null;
            if (deckName == "DrawPile")
            {
                //Remove the Discard button if there are no cards left in the discard pile
                //gameManager.ui.DrawPileIsEmpty();
                solitaire.DrawPileIsEmpty();
            }
        }
    }

    //Add a new card to the top of this deck
    public void AddCardToDeck(Card newCard, bool makeHoverable,bool flipOver)
    {
        newCard.DrawCard(this, gameManager.cardThickness * cardList.Count,flipOver);
        newCard.hoverable = makeHoverable;
        newCard.currentDeck = this;
        cardList.Add(newCard);
        newCard.transform.SetParent(transform);
        topCard = newCard; //Set the new card as the deck's top card
    }


    
    public void ShuffleDeck()
    {
        //Shuffle Algorithm based on the modern version of the Fisher–Yates shuffle
        for (int i = 0; i < cardList.Count-2; i++)
        {
            int j = Random.Range(i, cardList.Count);
            var tempi = cardList[i];
            cardList[i] = cardList[j];
            cardList[j] = tempi;
        }

        //Set the card's positions and the new top card
        int ii = 0;
        foreach(Card card in cardList)
        {
            card.transform.position = transform.position;
            card.GetComponent<CardRenderer>().height = ii * gameManager.cardThickness;

            if (ii == cardList.Count - 1)
            {
                topCard = card;
            }
            ii++;
        }
    }



    public void FillDeck()
    {
        //Fill the deck and assign the correct suits and values
        for (int i = 0; i < 52; i++)
        {
            Card.Suits suite = (Card.Suits)((decimal)i / 13);
            int val = i % 13 + 1;

            GameObject newCardObject = Instantiate(gameManager.cardPrefab,transform.position,Quaternion.identity, transform);
            Card newCard = newCardObject.GetComponent<Card>();
            newCard.InitializeCard(val, suite);
            newCard.currentDeck = this;
            cardList.Add(newCard);
            ShuffleDeck(); //Shuffle the new deck so every game is unique
        }
    }
}
