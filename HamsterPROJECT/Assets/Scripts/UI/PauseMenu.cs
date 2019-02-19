using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using InControl;

public class PauseMenu : MonoBehaviour {

	GameObject pauseMenu;
	[HideInInspector] public GameObject[] arrows;
	bool arrowSet = false;

	// Audio
	AudioManager mngr;

    private void Awake()
    {
        pauseMenu = GameObject.Find("PauseMenu");
    }

    void Start()
	{
        pauseMenu.SetActive(false);
		mngr = FindObjectOfType<AudioManager> ();
	}

	void Update()
	{
		if (InputManager.CommandWasPressed && !pauseMenu.activeSelf && MatchStart.gameHasStarted && !MatchEnd.matchEnded)
		{
			OpenPauseMenu ();
		}

		else if ((InputManager.CommandWasPressed || InputManager.ActiveDevice.Action2.WasPressed) && pauseMenu.activeSelf) 
		{
			ClosePauseMenu ();
		}
	}

	void OpenPauseMenu()
	{
		mngr.PlaySound ("UI_pauseMenuEnabled", "UI");
		FreezePlayers ();
		CancelAllVibrations ();
		pauseMenu.SetActive (true);
		Time.timeScale = 0;
	}

	public void ClosePauseMenu()
	{
		mngr.PlaySound ("UI_pauseMenuDisabled", "UI");
		pauseMenu.SetActive (false);
		Time.timeScale = 1;
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
