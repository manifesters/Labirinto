using System.Collections;
using DataPersistence;
using Helper;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        // Load volume settings when Settings scene starts
        if (PlayerPrefs.HasKey("BGM Volume"))
        {
            bgmSlider.value = PlayerPrefs.GetFloat("BGM Volume");
        }
        else
        {
            bgmSlider.value = -0.5f;
        }

        if (PlayerPrefs.HasKey("SFX Volume"))
        {
            sfxSlider.value = PlayerPrefs.GetFloat("SFX Volume");
        }
        else
        {
            sfxSlider.value = 0.5f;
        }

        SetBGMVolume();
        SetSFXVolume();
    }

    public void SetBGMVolume()
    {
        float volume = bgmSlider.value;
        AudioManager.Instance.SetBGMVolume(volume);
        PlayerPrefs.SetFloat("BGM Volume", volume); 
    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        AudioManager.Instance.SetSFXVolume(volume); 
        PlayerPrefs.SetFloat("SFX Volume", volume); 
    }

    public void SaveAndExit()
    {
        StartCoroutine(SaveAndLoadHomeScene());
    }

    private IEnumerator SaveAndLoadHomeScene()
    {
        // Save game data
        DataPersistenceManager.Instance.SaveGame();

        // Optional: wait for one frame to ensure save completes if async
        yield return null;

        // Load the home scene after saving
        GameManager.Instance.LoadScene("Main", GameState.HOME);
    }
}
