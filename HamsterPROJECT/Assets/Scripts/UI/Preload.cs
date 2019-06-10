using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preload : MonoBehaviour {

	[SerializeField] GameObject[] screensToPreload;

	void Awake()
	{
		foreach(GameObject screen in screensToPreload)
		{
			screen.SetActive(true);
			if(screen.name == "CharacterSelectionPanel")
				PreloadCharacterInstantiations(screen);
			screen.SetActive(false);
		}
	}

	void PreloadCharacterInstantiations(GameObject characterSelectionScreen)
	{
		List<PlayerSelectionPanel> panels = characterSelectionScreen.transform.GetChild(0).GetComponent<CharacterSelectionScreen>().panels;
		for(int i = 0; i < GameManager.nbOfPlayers; i ++)
		{
			panels[i].state = PlayerSelectionPanel.SelectionPanelState.Validated;
			// panels[i].state = PlayerSelectionPanel.SelectionPanelState.Activated;
			panels[i].state = PlayerSelectionPanel.SelectionPanelState.Deactivated;
		}

		GameManager.ClearData();
	}

}
