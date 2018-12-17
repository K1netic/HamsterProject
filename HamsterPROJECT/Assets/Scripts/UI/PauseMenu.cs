using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class PauseMenu : MonoBehaviour {

	[SerializeField] GameObject pauseMenu;

//	string plyrNumber;

	bool isOpen = false;

//	void Start()
//	{
//		plyrNumber = this.GetComponent<PlayerMovement> ().playerNumber;
//	}

	void Update()
	{
		if (Input.GetButtonDown("Pause_P1") && !isOpen)
		{
			OpenPauseMenu ();
		}

		else if ((Input.GetButtonDown("Pause_P1") || Input.GetButtonDown("Cancel_P1")) && isOpen)
		{
			ClosePauseMenu ();
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
}
