using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardRenderer : MonoBehaviour
{
    public SpriteRenderer cardRend;
    public SpriteRenderer shadowRend;
    public SpriteRenderer outlineRend;

    public float height;
    public float realHeight;

    public Card card;

    private Sprite frontFace;
    private Sprite backFace;

    private Vector3 currentCardSize = Vector3.one;
    private Vector3 cardScaleVelocity;
    public Vector3 HighlightScale;

    public float flipDuration = 2f;
    
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
        frontFace = frontSprite;
        backFace = backSprite;

        cardRend.sprite = backFace;

        Unhighlight();
    }

    public void Highlight()
    {
        cardRend.color = card.gameManager.highlightCardColor;
        outlineRend.color = card.gameManager.highlightOutlineColor;
        currentCardSize = HighlightScale;
    }

    public void Unhighlight()
    {
        cardRend.color = card.gameManager.normalCardColor;
        outlineRend.color = card.gameManager.normalOutlineColor;
        currentCardSize = Vector3.one;
    }

    public void Select()
    {
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
            float totaltDist = Vector3.Distance(card.startPosition, card.targetPosition);
            float completedDist = Vector3.Distance(transform.position, card.targetPosition);
            float completionRatio = completedDist / totaltDist;
            jumpOffset = Mathf.Sin(completionRatio*Mathf.PI);

            if (card.isFlipping)
            {
                float cardAngle = Mathf.LerpAngle(0f, 180f, completionRatio);
                transform.eulerAngles = new Vector3(0, cardAngle, 0);

                if (cardAngle >= 90f)
                {
                    cardRend.sprite = frontFace;
                }
            }
        }

        realHeight = height + jumpOffset;
        cardRend.transform.position = transform.position + new Vector3(0f, realHeight, -realHeight);
        shadowRend.transform.position = transform.position + new Vector3(realHeight, -realHeight, 0.1f);
    }
}
