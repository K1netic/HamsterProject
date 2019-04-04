using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

	AudioSource musicSource;
	[SerializeField] AudioClip menu;
	[SerializeField] AudioClip battle;

	public static MusicManager instance = null;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
		else 
		{
			Destroy (gameObject);
		}
		musicSource = GetComponent<AudioSource> ();
	}

	public void PlayMusic(string musicName)
	{
		if (musicSource.isPlaying)
			return;

		switch(musicName)
		{
		case "menu":
			musicSource.clip = menu;
			break;
		case "battle":
			musicSource.clip = battle;
			break;
		default :
			return;
		}

		musicSource.Play ();
	}

	public void StopMusic(string musicName)
	{
		switch(musicName)
		{
		case "menu":
			musicSource.clip = menu;
			break;
		case "battle":
			musicSource.clip = battle;
			break;
		default :
			return;
		}
		musicSource.Stop ();
	}
}
