using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuUI : MonoBehaviour
{
    //this makes sure the UI gets the right gamemanager and links to the right buttons and functions

    private GameManager gameManager;

    public Button buttonToPlayLevel;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        buttonToPlayLevel.onClick.AddListener(AskGMToStartGame);
    }

    private void AskGMToStartGame()
    {
        gameManager.LoadGamePlay();
    }

}
