using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardRenderer : MonoBehaviour
{
    //References to the card's SpriteRenderer components
    public SpriteRenderer cardRend;
    public SpriteRenderer shadowRend;
    public SpriteRenderer outlineRend;

    //The card's height from the floor without the jump height
    public float height;
    //The card's height from the floor with the jump height
    public float realHeight;

    public Card card;

    private Sprite frontFace;
    private Sprite backFace;

    private Vector3 currentCardSize = Vector3.one;
    private Vector3 cardScaleVelocity;
    public Vector3 HighlightScale;

    public float jumpMultiplier = 2f;
    
    // Start is called before the first frame update
    void Awake()
    {
        card = GetComponent<Card>();
    }

    public void FlipCardInPlace()
    {
        cardRend.sprite = frontFace;
    }

    public void SetCardSprite(Sprite frontSprite, Sprite backSprite)
    {
        //Change the card's sprites to the ones assigned
        frontFace = frontSprite;
        backFace = backSprite;

        cardRend.sprite = backFace;

        Unhighlight();
    }

    public void Highlight()
    {
        //Highlight the card by changing the card's render colour, outline colour, and scale
        cardRend.color = card.gameManager.highlightCardColor;
        outlineRend.color = card.gameManager.highlightOutlineColor;
        currentCardSize = HighlightScale;
    }

    public void Unhighlight()
    {
        //Reset the card's colour, outline colour, and scale
        cardRend.color = card.gameManager.normalCardColor;
        outlineRend.color = card.gameManager.normalOutlineColor;
        currentCardSize = Vector3.one;
    }

    public void Select()
    {
        //Select the card by changing the card's render colour, outline colour, and scale
        cardRend.color = card.gameManager.selectedCardColor;
        outlineRend.color = card.gameManager.highlightOutlineColor;
        currentCardSize = HighlightScale;
    }



    // Update is called once per frame
    void Update()
    {
        float jumpOffset = 0f;
        cardRend.transform.localScale = Vector3.SmoothDamp(cardRend.transform.localScale, currentCardSize, ref cardScaleVelocity, 0.02f);

        if (card.isMoving)
        {
            //Make the card jump up while moving for visual effect
            float totaltDist = Vector3.Distance(card.startPosition, card.targetPosition);
            float completedDist = Vector3.Distance(transform.position, card.targetPosition);
            float completionRatio = completedDist / totaltDist;
            jumpOffset = Mathf.Sin(completionRatio*Mathf.PI);

            if (card.isFlipping)
            {
                //If the card is flipping while moving, rotate the transform
                float cardAngle = Mathf.LerpAngle(0f, 180f, completionRatio);
                transform.eulerAngles = new Vector3(0, cardAngle, 0);

                if (cardAngle >= 90f)
                {
                    //Change the card sprite to its front face sprite
                    cardRend.sprite = frontFace;
                }
            }
        }

        //Move the card transforms to the required position
        realHeight = height + jumpOffset * jumpMultiplier;
        cardRend.transform.position = transform.position + new Vector3(0f, realHeight, -realHeight);
        shadowRend.transform.position = transform.position + new Vector3(realHeight, -realHeight, 0.1f);
    }
}
