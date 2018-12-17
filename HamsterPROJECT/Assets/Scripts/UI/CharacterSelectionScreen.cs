﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectionScreen : MonoBehaviour {

	[SerializeField] PlayerSelectionPanel[] panels;
	public bool ready = false;
	int readyCount = 0;
	//Used to make sure all panels are checked before the players are considered all ready
	bool checkPanels = true;
	[SerializeField] string sceneToLoad;
	[SerializeField] GameObject readyText;
	public int activatedPlayers = 0;
	[SerializeField] string previousScene;

	void Start()
	{
		#region DataRecovering
		for (int i = 0; i < GameManager.playersActive.Length; i++)
		{
			if (GameManager.playersActive[i] == true)
			{
				panels[i].state = PlayerSelectionPanel.SelectionPanelState.Activated;
				panels[i].GetComponent<PlayerSelectionPanel>().characterSelected = GameManager.playersSprites[i] ;
				panels[i].GetComponent<PlayerSelectionPanel>().characterSprite.sprite = Resources.Load<Sprite> ("CharacterSprites/SelectionScreen/" + panels[i].GetComponent<PlayerSelectionPanel>().characterSelected.ToString ());
			}
		}
		#endregion
	}

	void FixedUpdate()
	{
		if (!ready && checkPanels)
		{
			checkPanels = false;
			readyText.SetActive (false);
			readyCount = 0;
			activatedPlayers = 0;

			foreach (PlayerSelectionPanel pan in panels)
			{
				if (pan.state == PlayerSelectionPanel.SelectionPanelState.Validated)
					readyCount++;
			}

			// Conditions to start : at least two panels are validated
			// + no panel in the Activated state
			if (readyCount >= 2 && activatedPlayers == 0)
				ready = true;
			else
				ready = false;
			
			checkPanels = true;
		}
		else if (ready)
		{
			readyText.SetActive (true);
			//Load Map Selection Screen when all players are ready and P1 presses A
			if (Input.GetButtonDown ("Submit_P1"))
			{
				PlayerInfos ();
				SceneManager.LoadScene (sceneToLoad);
			}
		}

		if (Input.GetButtonDown("Cancel_P1") && previousScene != null && previousScene != "" && activatedPlayers == 0)
		{
			SceneManager.LoadScene (previousScene);
		}
	}

	// Writing playerInfos in the GameManager
	void PlayerInfos()
	{
		//Player 1 -> PlayerSelectionPanel 
		for (int i = 0; i < panels.Length; i++)
		{
			if (panels [i].state == PlayerSelectionPanel.SelectionPanelState.Validated)
			{
				GameManager.playersActive [i] = true;
				GameManager.playersSprites [i] = panels [i].characterSelected;
			}
		}
	}
}
