using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    //Singleton establishment 
    public static GameManager Instance { get; private set; }
    private void Awake() { if (Instance != null && Instance != this) { Destroy(this); } else { Instance = this; DontDestroyOnLoad(Instance); } }

    //Level fields
    public List<LevelClass> levels;
    public LevelClass currentlySelectedLevel;

    //gameplay prefs
    private float difficultySpeed = 0.5f;

    //Events
    public UnityEvent OnButtonClick;

    public UnityEvent OnGameOpen;

    public UnityEvent OnMainMenuOpen;

    public UnityEvent OnSelectedLevelChange;

    public UnityEvent OnLoadLevel; //Level loading stage 1

    public UnityEvent OnGameStart; //after loading, start play

    public UnityEvent OnCollision;

    public UnityEvent OnCoinPickup;

    public UnityEvent OnGamePause;

    public UnityEvent onGameUnpause;

    public UnityEvent OnGameOver;

    public UnityEvent OnGameQuit;


    //key bools
    public bool isGamePaused;
    public bool isInCountdown;

    private void Start()
    {
        currentlySelectedLevel = levels[1];//Until I add more levels... the selected level will be set to 0 at start as default.
        OpenGame();
        isInCountdown = false;
        isGamePaused = false;
    }

    //debug to trigger events
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GameOver();
        }
    }

    public void OpenGame()
    {
        OnGameOpen.Invoke();
    }

    public void OpenMainMenu()
    {
        OnMainMenuOpen.Invoke();
    }

    public void LoadLevel()
    {
        isInCountdown = true;
        OnLoadLevel.Invoke();
    }

    public void ClickButton()
    {
        OnButtonClick.Invoke();
    }

    public void StartGameplay()
    {
        isInCountdown = false;
        isGamePaused = false;
        OnGameStart.Invoke();
    }

    public void Collide()
    {
        OnCollision.Invoke();
    }

    public void CoinPickup()
    {
        OnCoinPickup.Invoke();
    }

    public void PauseToggle()
    {
        if (!isGamePaused)
        {
            isGamePaused = true;
            Time.timeScale = 0;
            OnGamePause.Invoke();
        } else
        {
            isGamePaused = false;
            Time.timeScale = 1;
            onGameUnpause.Invoke();
        }
    }

    public void GameOver()
    {
        OnGameOver.Invoke();
    }

    public void QuitGame()
    {
        OnGameQuit.Invoke();

        isGamePaused = false;
        Time.timeScale = 1;
    }

    public void chooseNextLevel()
    {

    }

    public void chooosePreviousLevel()
    {

    }

    public void SetCurrentLevel(int levelIndex)
    {
        currentlySelectedLevel = levels[levelIndex];
        OnSelectedLevelChange.Invoke();
    }

    public LevelClass GetCurrentlySelectedLevel()
    {
        return currentlySelectedLevel;
    }

    public void SetDifficulty(float newDifficulty)
    {
        if (newDifficulty < 1 || newDifficulty > 0)
        {
            difficultySpeed = newDifficulty;
        }
    }

    public float GetDifficulty()
    {
        return difficultySpeed;
    }

}
