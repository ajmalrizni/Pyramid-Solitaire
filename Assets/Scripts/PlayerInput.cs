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
        //If the player clicks while highlighting a card, that card is selected
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

                //If the player releases the mouse button while a card is selected, check if the cards can be paired
                if (hoveringCard != selection1)
                {
                    solitaire.TryMatchCards(hoveringCard, selection1);

                }
                else
                {
                    //If the selected card is the same as the highlighted card, check if that card alone can be removed
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

        //Restart the game
        if (Input.GetKeyDown(KeyCode.R))
        {
            gameManager.RestartGame();
        }        
        
        //Quit the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameManager.QuitGame();
        }
    }
}
