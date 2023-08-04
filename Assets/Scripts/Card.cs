using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    //References to other components
    public GameManager gameManager;
    public PyramidSolitaireDirector solitaire;
    public PlayerInput playerInput;
    public CardRenderer cardRenderer;

    //Position and movement variables
    public Vector3 startPosition;
    public Vector3 targetPosition;
    private float targetHeight;
    private Vector3 cardVelocity;
    private float cardHeightVelocity;
    public bool isMoving = false;
    public bool isFlipping = false;

    //The deck this card is in
    public Deck currentDeck;

    public bool hoverable;
    public bool faceUp = false;

    //Whether the card has been moved to the waste pile or not
    public bool wasted = false;

    //If this card gets placed in the pyramid, these variables reference the card's position
    public int pyramidRow;
    public int pyramidColumn;
    
    public enum Suits
    {
        Hearts,
        Clubs,
        Diamonds,
        Spades
    }

    //The card's suit
    public Suits suit;
    //The card's value
    public int value;
    

    void Awake()
    {
        //Assign necessary components
        gameManager = FindObjectOfType<GameManager>();
        solitaire = FindObjectOfType<PyramidSolitaireDirector>();
        cardRenderer = GetComponent<CardRenderer>();
        playerInput = FindObjectOfType<PlayerInput>();

        targetPosition = transform.position;
    }

    //Initialise the card with the assigned suit and value
    public void InitializeCard(int _value, Suits _suit)
    {
        suit = _suit;
        value = _value;
        Sprite backfaceSprite = gameManager.backfaceSpriteBlack;

        int suitval;
        switch (_suit)
        {
            case Suits.Hearts:
                suitval = 0;
                backfaceSprite = gameManager.backfaceSpriteRed;
                break;
            case Suits.Clubs:
                suitval = 1;
                break;
            case Suits.Diamonds:
                suitval = 2;
                backfaceSprite = gameManager.backfaceSpriteRed;
                break;
            case Suits.Spades:
                suitval = 3;
                break;
            default:
                suitval = 0;
                break;
        }


        int cardSpriteID = (_value - 1) + suitval * 13;
        cardRenderer.SetCardSprite(gameManager.cardSprites[cardSpriteID], backfaceSprite);
        transform.name = "Card" + (_value - 1).ToString();
    }

    //Check if the mouse is hovering over the card, so the player can highlight and select it
    private void OnMouseOver()
    {
        bool isCovered = false;
        if (currentDeck != null)
        {
            if (currentDeck.topCard != null && currentDeck.topCard != this)
            {
                //If the card is in a deck and it isn't the top card, it can't be highlighted
                isCovered = true;
            }
        }
        if (playerInput.hoveringCard != this && hoverable && faceUp && !isCovered && playerInput.selection1 != this) 
        {
            playerInput.hoveringCard = this;
            if (playerInput.selection1 != null)
            {
                cardRenderer.Select();
            }
            else
            {
                cardRenderer.Highlight(); 
            }
        }
    }

    private void OnMouseExit()
    {
        //Unlightlight the card
        if (playerInput.hoveringCard == this && playerInput.selection1 != this)
        {
            playerInput.hoveringCard = null;
            cardRenderer.Unhighlight();
        }
    }

    //Move a card to the top of a new deck
    public void DrawCard(Deck newDeck, float newHeight, bool flipOver = true)
    {
        targetHeight = newHeight;
        Vector3 newPos = newDeck.transform.position;
        targetPosition = newPos;
        startPosition = transform.position;
        isMoving = true;
        isFlipping = flipOver;
        if (flipOver)
        {
            faceUp = !faceUp;
        }
    }

    //Move the card to a new position (not to a deck)
    public void PlaceCard(Vector3 newPos, bool flipOver,float newHeight = 0f)
    {
        targetHeight = newHeight;
        targetPosition = newPos;
        startPosition = transform.position;
        isMoving = true;
        isFlipping = flipOver;
        if (flipOver)
        {
            faceUp = !faceUp;
        }
        currentDeck = null;
    }

    //Flip the card over
    public void FlipCard()
    {
        faceUp = !faceUp;
        cardRenderer.FlipCardInPlace();
    }


    void Update()
    {
        //Move the card to its new position
        if (isMoving)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref cardVelocity, 0.2f);
            cardRenderer.height = Mathf.SmoothDamp(cardRenderer.height, targetHeight, ref cardHeightVelocity, 0.2f);

            if (transform.position == targetPosition)
            {
                isMoving = false; //Stop moving card
            }
        }

    }
}
