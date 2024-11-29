using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
	[Header("Audio Source")]
	[SerializeField] AudioSource musicSource;
	[SerializeField] AudioSource SFXSource;

	[Header("Audio Clip")]
	public AudioClip[] bgmClips;

	public bool control = true;
	public AudioClip uiButton;
	public AudioClip dialogueButton;
	public void PlayBGM(int index)
	{
		if (index >= 0 && index < bgmClips.Length)
		{
			musicSource.clip = bgmClips[index];
			musicSource.Play();
		}
	}
	public void PlaySFX(AudioClip clip)
	{
		SFXSource.PlayOneShot(clip);
	}
	public void controlAudio()
	{
		control = !control;
		if (control)
		{
			AudioListener.volume = 1f;
			Debug.Log(control);
		}
		else
		{
			AudioListener.volume = 0f;
			Debug.Log(control);
		}
	}
}
