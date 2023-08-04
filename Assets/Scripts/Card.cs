using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public GameManager gameManager;
    public PyramidSolitaireDirector solitaire;

    public PlayerInput playerInput;
    public CardRenderer cardRenderer;

    public Vector3 startPosition;
    public Vector3 targetPosition;
    private float targetHeight;
    private Vector3 cardVelocity;
    private float cardHeightVelocity;

    public bool isMoving = false;
    public bool isFlipping = false;

    public Deck currentDeck;

    public bool hoverable;
    public bool faceUp = false;

    public bool wasted = false;

    public int pyramidRow;
    public int pyramidColumn;



    public enum Suits
    {
        Hearts,
        Clubs,
        Diamonds,
        Spades
    }

    public Suits suit;
    public int value;
    

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        solitaire = FindObjectOfType<PyramidSolitaireDirector>();
        cardRenderer = GetComponent<CardRenderer>();
        playerInput = FindObjectOfType<PlayerInput>();

        targetPosition = transform.position;
    }

    //Check if the mouse is hovering over the card, so the player can highlight and select it
    private void OnMouseOver()
    {
        bool isCovered = false;
        if (currentDeck != null)
        {
            if (currentDeck.topCard != null)
            {
                if (currentDeck.topCard != this)
                {
                    isCovered = true;
                }
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

    public void FlipCard()
    {
        faceUp = !faceUp;
        cardRenderer.FlipCardInPlace();
    }

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


        int cardSpriteID = (_value-1) + suitval*13;
        cardRenderer.SetCardSprite(gameManager.cardSprites[cardSpriteID], backfaceSprite);
        transform.name = "Card" + (_value - 1).ToString();
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
