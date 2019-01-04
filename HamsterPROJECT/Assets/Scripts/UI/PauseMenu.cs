using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

	[SerializeField] GameObject pauseMenu;
	GameObject[] arrows;

//	string plyrNumber;

	bool isOpen = false;
	bool arrowSet = false;

	void Update()
	{
		if (Input.GetButtonDown("Pause_P1") && !isOpen && MatchStart.gameHasStarted)
		{
			OpenPauseMenu ();
			FreezePlayers ();
		}

		else if ((Input.GetButtonDown("Pause_P1") || Input.GetButtonDown("Cancel_P1")) && isOpen)
		{
			ClosePauseMenu ();
			UnfreezePlayers ();
		}
	}

	void OpenPauseMenu()
	{
		pauseMenu.SetActive (true);
	
		Time.timeScale = 0;
		isOpen = true;
	}

	public void ClosePauseMenu()
	{
		pauseMenu.SetActive (false);
		Time.timeScale = 1;
		isOpen = false;
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
			if (arrow != null)
				arrow.GetComponent<Hook> ().isFrozen = true;
		}
	}

	void UnfreezePlayers()
	{
		foreach (GameObject arrow in arrows)
		{
			if (arrow != null)
				arrow.GetComponent<Hook> ().isFrozen = false;
		}
	}
}
