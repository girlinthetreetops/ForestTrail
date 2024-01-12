using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//this class is for the UI on gameplay scene. it makes sure the UI gets the right gamemanager and links to the right buttons and functions

public class GamePlayUI : MonoBehaviour
{
    //references
    private GameManager gameManager;

    public Button pauseButton;

    public TMP_Text currentScore;
    public TMP_Text highScore;
    public TMP_Text gold;

    [SerializeField] GameObject gamePlayUI;
    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject gameOverscreen;

    //other
    public bool isGamePaused;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        pauseButton.onClick.AddListener(PauseToggle);
    }

    private void PauseToggle()
    {
        if (!isGamePaused)
        {
            isGamePaused = true;
            gamePlayUI.SetActive(false);
            pauseScreen.SetActive(true);
        } else
        {
            isGamePaused = false;
            gamePlayUI.SetActive(true);
            pauseScreen.SetActive(false);
        }
    }
}
