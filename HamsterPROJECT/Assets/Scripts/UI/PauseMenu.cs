using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

	[SerializeField] GameObject pauseMenu;
	public GameObject[] arrows;
	bool arrowSet = false;

	// Audio
	float delay = 0.1f;
	AudioManager mngr;

	void Start()
	{
		mngr = FindObjectOfType<AudioManager> ();
	}

	void Update()
	{
		if (Input.GetButtonDown("Pause_P1") && !pauseMenu.activeSelf && MatchStart.gameHasStarted && !MatchEnd.matchEnded)
		{
			OpenPauseMenu ();
		}

		else if ((Input.GetButtonDown("Pause_P1") || Input.GetButtonDown("Cancel_P1")) && pauseMenu.activeSelf) 
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
