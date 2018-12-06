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
	[SerializeField] int characterSelected = 0;
	 
	[SerializeField] Image backgroundImg;
	[SerializeField] Image characterSprite;

	bool blockStickMovement = false;

	void Start()
	{
		backgroundImg = this.GetComponent<Image> ();
		state = SelectionPanelState.Deactivated;

		nbCharactersAvailable = charactersSprites.Length -1;
	}

	// Update is called once per frame
	void Update () {
		#region StateManagement
		if (Input.GetButtonDown ("Submit" + playerSelectionPanelID) && state == SelectionPanelState.Deactivated )
		{
			state = SelectionPanelState.Activated;
		}
			
		else if (Input.GetButtonDown ("Submit" + playerSelectionPanelID) && state == SelectionPanelState.Activated)
		{
			state = SelectionPanelState.Validated;
		}

		if (Input.GetButtonDown("Cancel" + playerSelectionPanelID) && state == SelectionPanelState.Activated)
		{
			state = SelectionPanelState.Deactivated;
		}

		else if (Input.GetButtonDown("Cancel" + playerSelectionPanelID) && state == SelectionPanelState.Validated)
		{
			state = SelectionPanelState.Activated;
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
			backgroundImg.color = activationColor;
			//Hiding "ready" text
			this.transform.GetChild (1).gameObject.SetActive (false);
			//Activate character selection
			characterSprite.gameObject.SetActive(true);
			CharacterSelection();
			break;
		case SelectionPanelState.Validated:
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
