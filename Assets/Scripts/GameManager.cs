using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public UIManager ui;
    public Sprite[] cardSprites;

    //Card rendering variables
    public Sprite backfaceSpriteRed;
    public Sprite backfaceSpriteBlack;

    public Color normalCardColor;
    public Color highlightCardColor;
    public Color selectedCardColor;    
    
    public Color normalOutlineColor;
    public Color highlightOutlineColor;

    

    public float cardThickness;


    void Awake()
    {
        //Load all the card sprites from the resources folder
        cardSprites = Resources.LoadAll<Sprite>("Sprites/PlayingCards");
        //Get the UIManager reference from this game object
        ui = GetComponent<UIManager>();
    }

    //Restart Game
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    //Quit game 
    public void QuitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }
}
