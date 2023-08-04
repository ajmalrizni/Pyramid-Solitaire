using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // Start is called before the first frame update

    public Card hoveringCard;
    public Card selection1;

    public PyramidSolitaireDirector solitaire;
    private GameManager gameManager;

    void Awake()
    {
        gameManager = GetComponent<GameManager>();
    }

    private void TakeGameplayInput()
    {
        if (Input.GetButtonDown("Fire1") && hoveringCard != null)
        {
            selection1 = hoveringCard;
            hoveringCard.cardRenderer.Select();
        }

        if (Input.GetButtonUp("Fire1") && selection1 != null)
        {
            if (hoveringCard != null)
            {
                hoveringCard.cardRenderer.Highlight();

                if (hoveringCard != selection1)
                {
                    solitaire.TryMatchCards(hoveringCard, selection1);

                }
                else
                {
                    solitaire.TryMatchCards(hoveringCard);
                }
            }
            selection1.cardRenderer.Unhighlight();
            selection1 = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        TakeGameplayInput();


        if (Input.GetKeyDown("r"))
        {
            gameManager.RestartGame();
        }
    }
}
