using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using InControl;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour {

	[HideInInspector] public GameObject[] arrows;
	bool arrowSet = false;
	[SerializeField] bool deactivatePauseMenu = true;
	GameObject pauseMenu;

	// Audio
	MusicManager music;
	AudioSource source;
	AudioLowPassFilter filter;

	GameObject itemSelected;

	MatchStart mtchstrt;

    private void Awake()
    {
        pauseMenu = GameObject.Find("PauseMenu");
		mtchstrt = GameObject.FindObjectOfType<MatchStart>();
    }

    void Start()
	{
		if (deactivatePauseMenu && !mtchstrt.TestWithoutUI) 
			pauseMenu.SetActive(false);
		music = FindObjectOfType<MusicManager> ();
		filter = music.gameObject.GetComponent<AudioLowPassFilter> ();
		source = music.GetComponent<AudioSource> ();
	}

	void Update()
	{
		foreach (InputDevice dev in InputManager.Devices)
		{
			if (dev.CommandWasPressed && !pauseMenu.activeSelf && MatchStart.gameHasStarted && !MatchEnd.matchEnded)
			{
				OpenPauseMenu ();
			}

			else if ((dev.CommandWasPressed || dev.Action2.WasPressed) && pauseMenu.activeSelf) 
			{
				ClosePauseMenu ();
			}
		}
	}

	void OpenPauseMenu()
	{
		AudioManager.instance.thornsSource.Stop();
		AudioManager.instance.bombSource.Pause();
		AudioManager.instance.meteorSource.Pause();
		AudioManager.instance.PlaySound ("UI_pauseMenuEnabled", "UI");
		source.volume *= 0.25f;
		filter.enabled = true;
		FreezePlayers ();
		CancelAllVibrations ();
		Time.timeScale = 0;
		pauseMenu.SetActive (true);
	}

	public void ClosePauseMenu()
	{
		Time.timeScale = 1;
		pauseMenu.SetActive (false);
		AudioManager.instance.PlaySound ("UI_pauseMenuDisabled", "UI");
		AudioManager.instance.bombSource.UnPause();
		AudioManager.instance.meteorSource.UnPause();
		source.volume *= 4.0f;
		filter.enabled = false;
		UnfreezePlayers ();
		arrowSet = false;
	}

	void FreezePlayers()
	{
		if (!arrowSet)
		{
			arrowSet = true;
			arrows = GameObject.FindGameObjectsWithTag ("Arrow");
		}

		foreach (GameObject arrow in arrows)
		{
			if (arrow != null) arrow.GetComponent<Hook> ().isFrozen = true;
		}
	}

	void CancelAllVibrations()
	{
		foreach(InputDevice device in InputManager.Devices)
		{
			device.StopVibration ();
		}
	}

	public void UnfreezePlayers()
	{
		if (!arrowSet)
		{
			arrowSet = true;
			arrows = GameObject.FindGameObjectsWithTag ("Arrow");
		}

		foreach (GameObject arrow in arrows)
		{
			if (arrow != null) arrow.GetComponent<Hook> ().isFrozen = false;
		}
	}
}
