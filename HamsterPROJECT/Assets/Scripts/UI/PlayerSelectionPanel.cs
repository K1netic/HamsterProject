using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectionPanel : MonoBehaviour {

	public string playerSelectionPanelID;
	public enum SelectionPanelState {Deactivated, Activated, Validated};
	public SelectionPanelState state;
	public int characterSelected = 0;
	 
	[SerializeField] Image backgroundImg;
	[SerializeField] public Image characterSprite;
	[SerializeField] Image notAvailable;

	bool blockStickMovement = false;

	CharacterSelectionScreen select;
	public GameObject validatedCharacter;

	//Audio
	AudioManager mngr;
	bool validate = false;
	bool activate = false;

	void Start()
	{
		mngr = FindObjectOfType<AudioManager> ();
		backgroundImg = this.GetComponent<Image> ();
		state = SelectionPanelState.Deactivated;

		select = GameObject.Find ("CharacterSelectionScripts").GetComponent<CharacterSelectionScreen> ();
	}

	// Update is called once per frame
	void Update () {
		
		#region StateManagement
		if (Input.GetButtonDown ("Submit" + playerSelectionPanelID) && state == SelectionPanelState.Deactivated )
		{
			state = SelectionPanelState.Activated;
		}
			
		else if (Input.GetButtonDown ("Submit" + playerSelectionPanelID) 
			&& state == SelectionPanelState.Activated 
			&& CharacterSelectionScreen.selectableCharacters[characterSelected] == true)
		{
			state = SelectionPanelState.Validated;
			validatedCharacter = GameManager.Characters[characterSelected];
			CharacterSelectionScreen.selectableCharacters[characterSelected] = false;
		}

		else if (Input.GetButtonDown ("Submit" + playerSelectionPanelID) 
			&& state == SelectionPanelState.Activated 
			&& CharacterSelectionScreen.selectableCharacters[characterSelected] == false)
		{
			//Play error sound
			//Display "not available text"
		}

		if (Input.GetButtonDown("Cancel" + playerSelectionPanelID) && state == SelectionPanelState.Activated)
		{
			mngr.PlaySound ("UI_cancel", mngr.UIsource);
			state = SelectionPanelState.Deactivated;
		}

		else if (Input.GetButtonDown("Cancel" + playerSelectionPanelID) && state == SelectionPanelState.Validated)
		{
			mngr.PlaySound ("UI_cancel", mngr.UIsource);
			state = SelectionPanelState.Activated;
			select.ready = false;
			CharacterSelectionScreen.selectableCharacters[characterSelected] = true;
		}
		#endregion

		#region ActionsToDoForEachState
		switch (state)
		{
		case SelectionPanelState.Deactivated:
			activate = false;
			backgroundImg.color = Color.gray;
			characterSprite.gameObject.SetActive(false);
			break;
		case SelectionPanelState.Activated:
			validate = false;
			PlayActivationSound();
			backgroundImg.color = Color.gray;
			//Hiding "ready" text
			this.transform.GetChild (1).gameObject.SetActive (false);
			//Activate character selection
			characterSprite.gameObject.SetActive(true);
			CharacterSelection();
			break;
		case SelectionPanelState.Validated:
			PlayValidateSound();
			backgroundImg.color = Color.green;
			this.transform.GetChild (1).gameObject.SetActive (true);
			break;
		}
		#endregion
	}

	void CharacterSelection()
	{
		// Display character as unavailable if that's the case
		if (!CharacterSelectionScreen.selectableCharacters [characterSelected])
			notAvailable.gameObject.SetActive (true);
		else
			notAvailable.gameObject.SetActive (false);

		if (Input.GetAxisRaw ("Horizontal" + playerSelectionPanelID) == 1 && !blockStickMovement)
		{
			mngr.PlaySound ("UI_pick", mngr.UIsource);
			if (characterSelected < CharacterSelectionScreen.nbCharactersAvailable)
			{
				characterSelected += 1;
			}
			else
			{
				characterSelected = 0;
			}
			characterSprite.sprite = GameManager.Characters[characterSelected].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
			blockStickMovement = true;
		} 

		else if (Input.GetAxisRaw ("Horizontal" + playerSelectionPanelID) == -1 && !blockStickMovement)
		{
			mngr.PlaySound ("UI_pick", mngr.UIsource);
			if (characterSelected > 0)
			{
				characterSelected -= 1;
			}
			else
			{
				characterSelected = CharacterSelectionScreen.nbCharactersAvailable;
			}
			characterSprite.sprite = GameManager.Characters[characterSelected].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
			blockStickMovement = true;
		} 

		else if (Input.GetAxisRaw ("Horizontal" + playerSelectionPanelID) < 0.2 && Input.GetAxisRaw ("Horizontal" + playerSelectionPanelID) > -0.2)
			blockStickMovement = false;
	}

	void PlayActivationSound()
	{
		if (!activate)
		{
			mngr.PlaySound ("UI_panelActivation", mngr.UIsource);
			activate = true;
		}
	}

	void PlayValidateSound()
	{
		if (!validate)
		{
			mngr.PlaySound ("UI_validate", mngr.UIsource);
			validate = true;
		}
	}
}
