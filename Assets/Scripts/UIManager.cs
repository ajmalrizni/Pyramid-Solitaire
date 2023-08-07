using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    //Handles all the display and behaviour of all the UI in the game
    public TMP_Text titleTxt;
    public TMP_Text infoTxt;    
    public GameObject rulesObject;    
    
    public TMP_Text titleShadowTxt;
    public TMP_Text infoShadowTxt;


    public GameObject playButton;
    public GameObject playButtonHard;
    public GameObject discardButton;
    public GameObject rulesButton;
    public GameObject restartButton;

    
    public void ShowRules()
    {//When the player clicks the Rules button
        rulesButton.SetActive(false);
        rulesObject.SetActive(true);

        titleTxt.gameObject.SetActive(false);
        titleShadowTxt.gameObject.SetActive(false);
    }

    //When the player wins the game, tell the player they won and remove the gameplay buttons
    public void WinGame()
    {
        titleTxt.gameObject.SetActive(true);
        titleShadowTxt.gameObject.SetActive(true);

        discardButton.SetActive(false);

        titleTxt.text = "You won! :)";
        titleShadowTxt.text = "You won! :)";

        infoTxt.gameObject.SetActive(false);
        infoShadowTxt.gameObject.SetActive(false);
    }

    //The draw pile and discard pile are both empty, so there are no more cards left to discard 
    public void RemoveDiscardButton()
    {
        discardButton.SetActive(false);
    }

    //Begin the game but don't allow player input yet
    public void PrepareNewGame()
    {
        playButton.SetActive(false);
        playButtonHard.SetActive(false);
        rulesButton.SetActive(false);
        restartButton.SetActive(false);

        titleTxt.gameObject.SetActive(false);
        titleShadowTxt.gameObject.SetActive(false);

        rulesObject.SetActive(false);

        infoTxt.gameObject.SetActive(false);
        infoShadowTxt.gameObject.SetActive(false);
        
    }

    //When the player can start playing the game
    public void StartPlaying()
    {
        discardButton.SetActive(true);
        restartButton.SetActive(true);

        infoTxt.gameObject.SetActive(true);
        infoShadowTxt.gameObject.SetActive(true);
    }

    //Updates the info UI to show how many cards are remaining
    public void ShowRemainingCards(int remaining)
    {
        string remainingTxt;
        if (remaining > 1)
        {
            remainingTxt = remaining.ToString() + " cards remaining";

        }
        else
        {
            remainingTxt = "ONLY ONE CARD LEFT!";
        }
        infoTxt.text = remainingTxt;
        infoShadowTxt.text = remainingTxt;
    }
}
