using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

	[SerializeField] AudioSource musicSource;
	[SerializeField] AudioClip menu;
	[SerializeField] AudioClip battle;

	void Awake()
	{
		DontDestroyOnLoad (transform.gameObject);
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
			break;
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
			break;
		}
		musicSource.Stop ();
	}
}
