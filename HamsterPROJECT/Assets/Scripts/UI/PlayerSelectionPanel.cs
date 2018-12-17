using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectionPanel : MonoBehaviour {

	public string playerSelectionPanelID;
	public enum SelectionPanelState {Deactivated, Activated, Validated};
	public SelectionPanelState state;
	[SerializeField] Color activationColor;
	[SerializeField] Sprite[] charactersSprites;
	int nbCharactersAvailable;
	[SerializeField] public int characterSelected = 0;
	 
	[SerializeField] Image backgroundImg;
	[SerializeField] public Image characterSprite;

	bool blockStickMovement = false;

	CharacterSelectionScreen select;

	void Start()
	{
		backgroundImg = this.GetComponent<Image> ();
		state = SelectionPanelState.Deactivated;

		nbCharactersAvailable = charactersSprites.Length -1;
		select = GameObject.Find ("CharacterSelectionScripts").GetComponent<CharacterSelectionScreen> ();
	}

	// Update is called once per frame
	void Update () {
		
		#region StateManagement
		if (Input.GetButtonDown ("Submit" + playerSelectionPanelID) && state == SelectionPanelState.Deactivated )
		{
			state = SelectionPanelState.Activated;
			select.activatedPlayers ++;
		}
			
		else if (Input.GetButtonDown ("Submit" + playerSelectionPanelID) && state == SelectionPanelState.Activated)
		{
			state = SelectionPanelState.Validated;
			select.activatedPlayers --;
		}

		if (Input.GetButtonDown("Cancel" + playerSelectionPanelID) && state == SelectionPanelState.Activated)
		{
			state = SelectionPanelState.Deactivated;
			select.activatedPlayers --;
		}

		else if (Input.GetButtonDown("Cancel" + playerSelectionPanelID) && state == SelectionPanelState.Validated)
		{
			state = SelectionPanelState.Activated;
			select.activatedPlayers ++;
			select.ready = false;
		}
		#endregion

		#region ActionsToDoForEachState
		switch (state)
		{
		case SelectionPanelState.Deactivated:
			backgroundImg.color = Color.gray;
			characterSprite.gameObject.SetActive(false);
			break;
		case SelectionPanelState.Activated:
			characterSprite.color = Color.white;
			//Hiding "ready" text
			this.transform.GetChild (1).gameObject.SetActive (false);
			//Activate character selection
			characterSprite.gameObject.SetActive(true);
			CharacterSelection();
			break;
		case SelectionPanelState.Validated:
			characterSprite.color = activationColor;
			this.transform.GetChild (1).gameObject.SetActive (true);
			break;
		}
		#endregion
	}

	void CharacterSelection()
	{
		if (Input.GetAxisRaw ("Horizontal" + playerSelectionPanelID) == 1 && !blockStickMovement)
		{
			if (characterSelected < nbCharactersAvailable)
				characterSelected += 1;
			else
				characterSelected = 0;
			characterSprite.sprite = Resources.Load<Sprite> ("CharacterSprites/SelectionScreen/" + characterSelected.ToString ());
			blockStickMovement = true;
		} 

		else if (Input.GetAxisRaw ("Horizontal" + playerSelectionPanelID) == -1 && !blockStickMovement)
		{
			if (characterSelected > 0)
				characterSelected -= 1;
			else
				characterSelected = nbCharactersAvailable;
			characterSprite.sprite = Resources.Load<Sprite> ("CharacterSprites/SelectionScreen/" + characterSelected.ToString ());
			blockStickMovement = true;
		} 

		else if (Input.GetAxisRaw ("Horizontal" + playerSelectionPanelID) < 0.2 && Input.GetAxisRaw ("Horizontal" + playerSelectionPanelID) > -0.2)
			blockStickMovement = false;
	}

}
