using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using InControl;

public class PauseMenu : MonoBehaviour {

	[SerializeField] GameObject pauseMenu;
	public GameObject[] arrows;
	bool arrowSet = false;

	// Audio
	AudioManager mngr;

	void Start()
	{
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
		mngr.PlaySound ("UI_pauseMenuEnabled", mngr.UIsource);
		FreezePlayers ();
		pauseMenu.SetActive (true);
		Time.timeScale = 0;
	}

	public void ClosePauseMenu()
	{
		mngr.PlaySound ("UI_pauseMenuDisabled", mngr.UIsource);
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
