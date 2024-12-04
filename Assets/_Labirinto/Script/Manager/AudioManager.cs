using Helper;
using UnityEngine;

public class AudioManager : SingletonMonobehaviour<AudioManager>
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("BGM Clips")]
    [SerializeField] public AudioClip bgm_splash;
    [SerializeField] public AudioClip bgm_home;
    [SerializeField] public AudioClip bgm_starting_story;
    [SerializeField] public AudioClip bgm_labirinto;

    [Header("SFX Clips")]
    [SerializeField] public AudioClip button_click;
    [SerializeField] public AudioClip sfx_inventory_click;
    [SerializeField] public AudioClip sfx_walking;
    [SerializeField] public AudioClip sfx_correct;
    [SerializeField] public AudioClip sfx_error;
    [SerializeField] public AudioClip sfx_typing;

    public void Start()
    {
        if (PlayerPrefs.HasKey("BGM Volume"))
        {
            SetBGMVolume(PlayerPrefs.GetFloat("BGM Volume"));
        }
        else
        {
            SetBGMVolume(0.5f); 
        }

        if (PlayerPrefs.HasKey("SFX Volume"))
        {
            SetSFXVolume(PlayerPrefs.GetFloat("SFX Volume"));
        }
        else
        {
            SetSFXVolume(0.5f); 
        }
    }

    public void SetBGMVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("BGM Volume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        SFXSource.volume = volume;
        PlayerPrefs.SetFloat("SFX Volume", volume); 
    }

    // -------- Play BGM or SFX -----------
    public void PLAY_SPLASH_BGM()
    {
        if (bgm_splash != null)
        {
            musicSource.clip = bgm_splash;
            musicSource.Play();
            Debug.Log($"Playing BGM: {bgm_splash.name}");
        }
        else
        {
            Debug.LogWarning("BGM clip is null.");
        }
    }

    public void PLAY_HOME_BGM()
    {
        if (bgm_home != null)
        {
            musicSource.clip = bgm_home;
            musicSource.Play();
            Debug.Log($"Playing BGM: {bgm_home.name}");
        }
        else
        {
            Debug.LogWarning("BGM clip is null.");
        }
    }

    public void PLAY_STORYMODE()
    {
        if (bgm_labirinto != null)
        {
            musicSource.clip = bgm_starting_story;
            musicSource.Play();
            Debug.Log($"Playing BGM: {bgm_starting_story.name}");
        }
        else
        {
            Debug.LogWarning("BGM clip is null.");
        }
    }

    public void PLAY_LABIRINTO_BGM()
    {
        if (bgm_labirinto != null)
        {
            musicSource.clip = bgm_labirinto;
            musicSource.Play();
            Debug.Log($"Playing BGM: {bgm_labirinto.name}");
        }
        else
        {
            Debug.LogWarning("BGM clip is null.");
        }
    }

    public void StopBGM()
    {
        musicSource.Stop();
    }

    // Play SFX
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            SFXSource.PlayOneShot(clip);
        }
    }
}
