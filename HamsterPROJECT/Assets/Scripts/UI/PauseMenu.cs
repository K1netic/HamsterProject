using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class PauseMenu : MonoBehaviour {

	[SerializeField] GameObject pauseMenu;
	GameObject[] arrows;

//	string plyrNumber;

	bool isOpen = false;

	void Start()
	{
//		plyrNumber = this.GetComponent<PlayerMovement> ().playerNumber;
		arrows = GameObject.FindGameObjectsWithTag("Arrow");
	}

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
		foreach (GameObject arrow in arrows)
		{
			arrow.GetComponent<Hook> ().isFrozen = true;
		}
	}

	void UnfreezePlayers()
	{
		foreach (GameObject arrow in arrows)
		{
			arrow.GetComponent<Hook> ().isFrozen = false;
		}
	}
}
