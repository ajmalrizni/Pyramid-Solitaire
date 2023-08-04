using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public Sprite[] cardSprites;

    public Sprite backfaceSpriteRed;
    public Sprite backfaceSpriteBlack;

    public Color normalCardColor;
    public Color highlightCardColor;
    public Color selectedCardColor;    
    
    public Color normalOutlineColor;
    public Color highlightOutlineColor;

    public UIManager ui;

    public float cardThickness;


    void Awake()
    {
        cardSprites = Resources.LoadAll<Sprite>("Sprites/PlayingCards");
        ui = GetComponent<UIManager>();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
