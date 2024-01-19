using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip defaultBackgroundSoundtrack;
    public AudioSource backgroundAudioSource;

    public AudioSource coinAudiosource;
    public AudioSource crashSound;
    public AudioSource buttonClickSound;

    private void Start()
    {
        GameManager.Instance.OnGameOpen.AddListener(PlayDefaultSoundtrack);

        GameManager.Instance.OnAudioVolumeChanged.AddListener(UpdateAudioVolume);

        GameManager.Instance.OnLoadLevel.AddListener(PlayLevelSoundtrack);

        GameManager.Instance.OnGameQuit.AddListener(PlayDefaultSoundtrack);

        GameManager.Instance.OnCoinPickup.AddListener(PlayCoinPickup);
        GameManager.Instance.OnCollision.AddListener(PlayCollisionSound);
        GameManager.Instance.OnButtonClick.AddListener(PlayButtonClickSound);


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
        backgroundAudioSource.clip = GameManager.Instance.GetCurrentlySelectedLevel().levelMusic;
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

    private void UpdateAudioVolume()
    {
        float newVolume = GameManager.Instance.globalSoundeffectVolume;

        coinAudiosource.volume = newVolume;
        crashSound.volume = newVolume;
        buttonClickSound.volume = newVolume;
    }
}
