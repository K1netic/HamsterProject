using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class PauseMenu : MonoBehaviour {

	[SerializeField] GameObject pauseMenu;

	string plyrNumber;

	void Start()
	{
		plyrNumber = this.GetComponent<PlayerMovement> ().playerNumber;
	}

	void Update()
	{
		if (Input.GetButton("Pause" + plyrNumber))
		{
			OpenPauseMenu ();
		}
	}

	public void OpenPauseMenu()
	{
		pauseMenu.SetActive (true);
		Time.timeScale = 0;
	}
}
