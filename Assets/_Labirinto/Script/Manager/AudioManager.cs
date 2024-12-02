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
    [SerializeField] public AudioClip bgm_labirinto;

	[Header("SFX Clips")]
    [SerializeField] public AudioClip button_click;
    [SerializeField] public AudioClip inventory_click;
	[SerializeField] public AudioClip walking;



	public bool control = true;

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

	// play sfx
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            SFXSource.PlayOneShot(clip);
        }
    }

    public void ControlAudio()
    {
        control = !control;
        AudioListener.volume = control ? 1f : 0f;
        Debug.Log($"Audio control toggled: {control}");
    }
}
