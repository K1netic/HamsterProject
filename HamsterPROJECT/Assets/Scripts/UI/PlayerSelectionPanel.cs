using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectionPanel : MonoBehaviour {

	public string playerSelectionPanelID;
	public enum SelectionPanelState {Deactivated, Activated, Validated};
	public SelectionPanelState state;
	[SerializeField] GameObject[] Characters;
	int nbCharactersAvailable;
	[SerializeField] public int characterSelected = 0;
	 
	[SerializeField] Image backgroundImg;
	[SerializeField] public Image characterSprite;

	bool blockStickMovement = false;

	CharacterSelectionScreen select;

	//Audio
	float delay = 0.1f;
	AudioManager mngr;
	bool validate = false;
	bool activate = false;

	void Start()
	{
		mngr = FindObjectOfType<AudioManager> ();
		backgroundImg = this.GetComponent<Image> ();
		state = SelectionPanelState.Deactivated;

		nbCharactersAvailable = Characters.Length -1;
		select = GameObject.Find ("CharacterSelectionScripts").GetComponent<CharacterSelectionScreen> ();
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
			mngr.PlaySound ("UI_cancel", mngr.UIsource);
			state = SelectionPanelState.Deactivated;
		}

		else if (Input.GetButtonDown("Cancel" + playerSelectionPanelID) && state == SelectionPanelState.Validated)
		{
			mngr.PlaySound ("UI_cancel", mngr.UIsource);
			state = SelectionPanelState.Activated;
			select.ready = false;
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
		if (Input.GetAxisRaw ("Horizontal" + playerSelectionPanelID) == 1 && !blockStickMovement)
		{
			mngr.PlaySound ("UI_pick", mngr.UIsource);
			if (characterSelected < nbCharactersAvailable)
				characterSelected += 1;
			else
				characterSelected = 0;
			characterSprite.sprite = Characters[characterSelected].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
			blockStickMovement = true;
		} 

		else if (Input.GetAxisRaw ("Horizontal" + playerSelectionPanelID) == -1 && !blockStickMovement)
		{
			mngr.PlaySound ("UI_pick", mngr.UIsource);
			if (characterSelected > 0)
				characterSelected -= 1;
			else
				characterSelected = nbCharactersAvailable;
			characterSprite.sprite = Characters[characterSelected].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
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
