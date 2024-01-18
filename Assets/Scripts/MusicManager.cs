using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip defaultBackgroundSoundtrack;


    public GameManager gameManager;
    public AudioSource backgroundAudioSource;

    public AudioSource coinAudiosource;
    public AudioSource crashSound;
    public AudioSource buttonClickSound;

    private void Start()
    {
        gameManager.OnGameOpen.AddListener(PlayDefaultSoundtrack);
        gameManager.OnLoadLevel.AddListener(PlayLevelSoundtrack);
        gameManager.OnGameQuit.AddListener(PlayDefaultSoundtrack);

        gameManager.OnCoinPickup.AddListener(PlayCoinPickup);
        gameManager.OnCollision.AddListener(PlayCollisionSound);
        gameManager.OnButtonClick.AddListener(PlayButtonClickSound);
    }

    private void PlayDefaultSoundtrack()
    {
        if (backgroundAudioSource.clip != defaultBackgroundSoundtrack)
        {
            backgroundAudioSource.clip = defaultBackgroundSoundtrack;
            backgroundAudioSource.Play();
        }
    }

    private void PlayLevelSoundtrack()
    {
        backgroundAudioSource.clip = gameManager.GetCurrentlySelectedLevel().levelMusic;
        backgroundAudioSource.Play();
    }

    private void PlayCoinPickup()
    {
        coinAudiosource.Play();

    }

    private void PlayCollisionSound()
    {
        crashSound.Play();

    }

    private void PlayButtonClickSound()
    {
        buttonClickSound.Play();
    }

    public void SetBackgroundMusicVolume(float newVolume)
    {
        backgroundAudioSource.volume = newVolume;
    }
}
