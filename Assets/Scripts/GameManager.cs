using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    //Singleton establishment 
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(this); } else { Instance = this; DontDestroyOnLoad(Instance); }

        currentlySelectedLevel = levels[0];//Until I add more levels... the selected level will be set to 0 at start as default.
    }

    //Level fields
    public List<LevelClass> levels;
    public LevelClass currentlySelectedLevel;

    //gameplay prefs
    private float difficultySpeed = 0.5f;
    public float globalSoundeffectVolume = 0.3f;

    //Events
    public UnityEvent OnButtonClick;

    public UnityEvent OnGameOpen;

    public UnityEvent OnMainMenuOpen;

    public UnityEvent OnSelectedLevelChange;

    public UnityEvent OnAudioVolumeChanged;

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
        int currentIndex = levels.IndexOf(currentlySelectedLevel);
        int nextIndex = (currentIndex + 1) % levels.Count; // Use modulo to wrap around to 0 when reaching the end
        SetCurrentLevel(nextIndex);
    }

    public void chooosePreviousLevel()
    {
        int currentIndex = levels.IndexOf(currentlySelectedLevel);
        int previousIndex = (currentIndex - 1 + levels.Count) % levels.Count; // Ensure the result is non-negative
        SetCurrentLevel(previousIndex);
    }

    public void SetCurrentLevel(int newLevelIndex)
    {
        currentlySelectedLevel = levels[newLevelIndex];
        OnSelectedLevelChange.Invoke();
    }

    public LevelClass GetCurrentlySelectedLevel()
    {
        return currentlySelectedLevel;
    }

    public void SetDifficulty(float newDifficulty)
    {
            difficultySpeed = newDifficulty;
    }

    public float GetDifficulty()
    {
        return difficultySpeed;
    }

    public void SetAudioVolume(float newVolume)
    {
        globalSoundeffectVolume = newVolume;
        OnAudioVolumeChanged.Invoke();
    }

}
