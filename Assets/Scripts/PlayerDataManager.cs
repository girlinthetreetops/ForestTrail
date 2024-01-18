using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public GameManager gameManager;

    //data
    public int playerGold;

    //temporary level data
    public int lives;
    public float levelTimer = 0f;
    private bool isTimerRunning;


    private void Start()
    {
        gameManager.OnGameOpen.AddListener(ResetLevelData);
        gameManager.OnLoadLevel.AddListener(ResetLevelData);
        gameManager.OnGameQuit.AddListener(ResetLevelData);

        gameManager.OnGameStart.AddListener(StartLevelTimerFromZero);
        gameManager.OnGamePause.AddListener(PauseLevelTimer);
        gameManager.onGameUnpause.AddListener(EnablLevelTimer);
        gameManager.OnCoinPickup.AddListener(AddGold);
        gameManager.OnCollision.AddListener(TakeDamage);

        lives = 3;
    }

    private void Update()
    {
        if (lives <=0)
        {
            gameManager.GameOver();
            lives = 3;
        }

        if (isTimerRunning)
        {
            levelTimer += Time.deltaTime;
        }
    }

    public void StartLevelTimerFromZero()
    {
        isTimerRunning = true;
        levelTimer = 0f;
        
    }

    public void PauseLevelTimer()
    {
        isTimerRunning = false;
    }

    public void EnablLevelTimer()
    {
        isTimerRunning = true;
    }

    public void ResetLevelData()
    {
        lives = 3;
        levelTimer = 0;
    }

    public void AddGold()
    {
        playerGold += 1;
    }

    public void TakeDamage()
    {
        lives -= 1;
    }

    public void TryUpdateHighscore()
    {
        UpdateLevelHighscore(levelTimer, gameManager.GetCurrentlySelectedLevel());
    }

    public void UpdateLevelHighscore(float score, LevelClass level)
    {
        if (score > level.highScore)
        {
            level.highScore = score;
        }
    }

    
}
