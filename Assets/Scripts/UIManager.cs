using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public PlayerDataManager dataManager;
    public MusicManager musicManager;

    //screens
    public GameObject logoUI;
    public GameObject mainUI;
    public GameObject pauseUI;
    public GameObject gameplayUI;
    public GameObject gameOverUI;
    public GameObject countDownUI;
    public GameObject settingsUI;

    //mainmenu elements
    public TMP_Text levelTitle;
    public Image levelThumbnail;
    public TMP_Text levelHighscoreDisplay;

    public TMP_Text goldDisplayMM;

    //gameplay elements
    public TMP_Text scoreDisplay;
    public TMP_Text livesDisplay;
    public TMP_Text highscoreDisplay;
    public TMP_Text goldDisplay;

    //CountDown screen elements
    public TMP_Text countDownText;


    //Settings screen elements
    public Slider musicVolumeSlider;
    public Slider difficultySlider;
    public Slider soundVolumeSlider;

    //Events to respond to 
    private void Start()
    {
        GameManager.Instance.OnGameOpen.AddListener(loadLogoScreen);

        GameManager.Instance.OnMainMenuOpen.AddListener(GameOpenPrep);
        GameManager.Instance.OnMainMenuOpen.AddListener(loadMainMenu);

        GameManager.Instance.OnSelectedLevelChange.AddListener(UpdateMmLevelCard);


        GameManager.Instance.OnLoadLevel.AddListener(OpenCountDownUI);
        GameManager.Instance.OnLoadLevel.AddListener(StartCountdown);

        GameManager.Instance.OnGameStart.AddListener(UpdateGameplayUI);
        GameManager.Instance.OnGameStart.AddListener(loadGameplayUI);

        GameManager.Instance.OnGamePause.AddListener(loadPauseUI);

        GameManager.Instance.onGameUnpause.AddListener(loadGameplayUI);

        GameManager.Instance.OnGameQuit.AddListener(loadMainMenu);

        GameManager.Instance.OnGameOver.AddListener(loadGameoverUI);

        GameManager.Instance.OnCoinPickup.AddListener(UpdateGameplayUI);
        GameManager.Instance.OnHeartPickup.AddListener(UpdateGameplayUI);

        GameManager.Instance.OnCollision.AddListener(UpdateGameplayUI);


        //sliders
        musicVolumeSlider.onValueChanged.AddListener(UpdateMusicVolume);
        difficultySlider.onValueChanged.AddListener(UpdateDifficulty);
        soundVolumeSlider.onValueChanged.AddListener(UpdateAudioVolume);
    }

    private void GameOpenPrep()
    {
        //load data so that no field is empty...
        UpdateGameplayUI();
        UpdateMmLevelCard();

    }


    private void Update()
    {
        scoreDisplay.SetText(dataManager.levelTimer.ToString("0"));
    }

    public void UpdateMmLevelCard()
    {
        goldDisplayMM.SetText(dataManager.playerGold.ToString());

        levelTitle.SetText(GameManager.Instance.currentlySelectedLevel.levelTitle);

        levelThumbnail.sprite = GameManager.Instance.currentlySelectedLevel.levelThumbnail;

        levelHighscoreDisplay.SetText(GameManager.Instance.currentlySelectedLevel.highScore.ToString("0"));
    }

    public void UpdateGameplayUI()
    {
        highscoreDisplay.SetText(GameManager.Instance.GetCurrentlySelectedLevel().highScore.ToString("0"));
        livesDisplay.SetText(dataManager.lives.ToString());
        goldDisplay.SetText(dataManager.playerGold.ToString());

        goldDisplayMM.SetText(dataManager.playerGold.ToString());
    }

    public void OpenCountDownUI()
    {
        logoUI.SetActive(false);
        mainUI.SetActive(false);
        pauseUI.SetActive(false);
        gameplayUI.SetActive(false);
        gameOverUI.SetActive(false);
        countDownUI.SetActive(true);
    }

    public void StartCountdown()
    {
        StartCoroutine("CountDownProcess");
    }

    public IEnumerator CountDownProcess()
    {
        countDownText.SetText("3");

        yield return new WaitForSeconds(1f);

        countDownText.SetText("2");

        yield return new WaitForSeconds(1f);

        countDownText.SetText("1");

        yield return new WaitForSeconds(1f);

        GameManager.Instance.StartGameplay();
    }

    public void loadLogoScreen()
    {
        logoUI.SetActive(true);
        mainUI.SetActive(false);
        pauseUI.SetActive(false);
        gameplayUI.SetActive(false);
        gameOverUI.SetActive(false);
        countDownUI.SetActive(false);
        settingsUI.SetActive(false);
    }

    public void loadMainMenu()
    {
        logoUI.SetActive(false);
        mainUI.SetActive(true);
        pauseUI.SetActive(false);
        gameplayUI.SetActive(false);
        gameOverUI.SetActive(false);
        countDownUI.SetActive(false);
        settingsUI.SetActive(false);
    }

    public void loadPauseUI()
    {
        logoUI.SetActive(false);
        mainUI.SetActive(false);
        pauseUI.SetActive(true);
        gameplayUI.SetActive(false);
        gameOverUI.SetActive(false);
        countDownUI.SetActive(false);
        settingsUI.SetActive(false);
    }

    public void loadGameplayUI()
    {
        logoUI.SetActive(false);
        mainUI.SetActive(false);
        pauseUI.SetActive(false);
        gameplayUI.SetActive(true);
        gameOverUI.SetActive(false);
        countDownUI.SetActive(false);
        settingsUI.SetActive(false);
    }

    public void loadGameoverUI()
    {
        logoUI.SetActive(false);
        mainUI.SetActive(false);
        pauseUI.SetActive(false);
        gameplayUI.SetActive(false);
        gameOverUI.SetActive(true);
        countDownUI.SetActive(false);
        settingsUI.SetActive(false);
    }

    public void loadSettingsUI()
    {
        logoUI.SetActive(false);
        mainUI.SetActive(false);
        pauseUI.SetActive(false);
        gameOverUI.SetActive(false);
        countDownUI.SetActive(false);
        settingsUI.SetActive(true);
    }

    public void UpdateMusicVolume(float sliderValue)
    {
        musicManager.SetBackgroundMusicVolume(sliderValue);
    }

    public void UpdateDifficulty(float newDifficulty)
    {
        GameManager.Instance.SetDifficulty(newDifficulty);
    }
    public void UpdateAudioVolume(float audioVolume)
    {
        GameManager.Instance.SetAudioVolume(audioVolume); 
    }
}



