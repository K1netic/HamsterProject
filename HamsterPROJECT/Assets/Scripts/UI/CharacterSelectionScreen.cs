using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectionScreen : MonoBehaviour {

	[SerializeField] PlayerSelectionPanel[] panels;
	bool ready = false;
	int readyCount = 0;
	//Used to make sure all panels are checked before the players are considered all ready
	bool checkPanels = true;
	[SerializeField] string sceneToLoad;
	[SerializeField] GameObject readyText;

	void Update()
	{
		if (!ready && checkPanels)
		{
			checkPanels = false;
			readyText.SetActive (false);
			readyCount = 0;
			//If at least two panels are validated...
			foreach (PlayerSelectionPanel pan in panels)
			{
				if (pan.state == PlayerSelectionPanel.SelectionPanelState.Validated)
					readyCount++;
			}
			//...then the game can start
			if (readyCount >= 2)
				ready = true;
			else
				ready = false;
			
			checkPanels = true;
		} 
		else
		{
			readyText.SetActive (true);
			//Load Map Selection Screen when all players are ready and P1 presses A
			if (Input.GetButton ("Submit_P1"))
			{
				PlayerInfos ();
				SceneManager.LoadScene (sceneToLoad);
			} 
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
