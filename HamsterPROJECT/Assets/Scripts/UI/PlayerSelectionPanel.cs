using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;
using InControl;

public class PlayerSelectionPanel : MonoBehaviour {

	// Panel state
	public enum SelectionPanelState {Deactivated, Activated, Validated};
	public SelectionPanelState state;

	// Collections of images to load and apply on various changes
	[HideInInspector] public Sprite[] characterSprites = new Sprite[GameManager.nbOfCharacters];
	Sprite[] validatedBorders = new Sprite[GameManager.nbOfCharacters];
	Sprite deactivatedBorder;
	Sprite activatedBorder;

	// Images to activate/deactive which receive the collections of images
	[SerializeField] Image notAvailable;
	[SerializeField] Image border;
	public Image characterSprite;
	[SerializeField] GameObject guid;
	GameObject leftArrow;
	GameObject rightArrow;

	// Character selection
	[HideInInspector] public int characterSelected = 0;
	bool blockStickMovement = false;
	CharacterSelectionScreen select;
	[HideInInspector] public GameObject validatedCharacter;

	//Audio
	AudioManager mngr;
	bool validate = false;
	bool activate = false;

	public InputDevice device;

	void Start()
	{
		mngr = FindObjectOfType<AudioManager> ();
		state = SelectionPanelState.Deactivated;
		select = GameObject.Find ("CharacterSelectionScripts").GetComponent<CharacterSelectionScreen> ();
		characterSprites = Resources.LoadAll<Sprite> ("CharacterSelection/ValidatedCharacters");
		validatedBorders = Resources.LoadAll<Sprite> ("CharacterSelection/Borders");
		deactivatedBorder = Resources.Load<Sprite> ("CharacterSelection/Bordgray");
		activatedBorder = Resources.Load<Sprite> ("CharacterSelection/Bordwhite");
		leftArrow = guid.transform.GetChild (0).gameObject;
		rightArrow = guid.transform.GetChild (1).gameObject;
	}

	// Update is called once per frame
	void Update () {

		if (device != null)
		{
			#region StateManagement
			// Activation
			if (device.Action1.WasPressed
				&& state == SelectionPanelState.Deactivated)
			{
				PlayActivationSound();
				state = SelectionPanelState.Activated;
			}

			// Validation
			else if (device.Action1.WasPressed
				&& state == SelectionPanelState.Activated 
				&& CharacterSelectionScreen.selectableCharacters[characterSelected] == true)
			{
				state = SelectionPanelState.Validated;
				PlayValidateSound();
				validatedCharacter = GameManager.Characters[characterSelected];
				CharacterSelectionScreen.selectableCharacters [characterSelected] = false;
			}

			// Deactivation
			if (device.Action2.WasPressed
				&& state == SelectionPanelState.Activated)
			{
				state = SelectionPanelState.Deactivated;
				mngr.PlaySound ("UI_cancel", mngr.UIsource);
				device = null;
			}

			// Unvalidation
			else if (device.Action2.WasPressed
				&& state == SelectionPanelState.Validated)
			{
				state = SelectionPanelState.Activated;
				mngr.PlaySound ("UI_cancel", mngr.UIsource);
				select.ready = false;
				CharacterSelectionScreen.selectableCharacters [characterSelected] = true;
			}

			// Trying to validate on a character not avalaible
			else if (device.Action1.WasPressed
				&& state == SelectionPanelState.Activated 
				&& CharacterSelectionScreen.selectableCharacters[characterSelected] == false)
			{
				//Play error sound
				//Display "not available text"
			}
			#endregion
		}

		#region ActionsToDoForEachState
		switch (state)
		{
		case SelectionPanelState.Deactivated:
			activate = false;
			characterSprite.gameObject.SetActive(false);
			guid.SetActive(true);
			leftArrow.SetActive(false);
			rightArrow.SetActive(false);
			border.sprite = deactivatedBorder;
			notAvailable.gameObject.SetActive (false);
			break;

		case SelectionPanelState.Activated:
			validate = false;
			PlayActivationSound();
			characterSprite.gameObject.SetActive(true);
			guid.SetActive(true);
			leftArrow.SetActive(true);
			rightArrow.SetActive(true);
			characterSprite.sprite = characterSprites[characterSelected];
			border.sprite = activatedBorder;
			//Activate character selection
			CharacterSelection();
			break;

		case SelectionPanelState.Validated:
			PlayValidateSound();
			guid.SetActive(false);
			border.sprite = validatedBorders[characterSelected];
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

		if (device.LeftStickX.Value >= 0.8f && !blockStickMovement)
		{
			if (characterSelected < CharacterSelectionScreen.nbCharactersAvailable - 1)
			{
				characterSelected += 1;
			}
			else
			{
				characterSelected = 0;
			}
			characterSprite.sprite = characterSprites[characterSelected];
			mngr.PlaySound ("UI_pick", mngr.UIsource);
			blockStickMovement = true;
		} 

		else if (device.LeftStickX.Value <= -0.8f && !blockStickMovement)
		{
			if (characterSelected > 0)
			{
				characterSelected -= 1;
			}
			else
			{
				characterSelected = CharacterSelectionScreen.nbCharactersAvailable - 1;
			}
			characterSprite.sprite = characterSprites[characterSelected];
			mngr.PlaySound ("UI_pick", mngr.UIsource);
			blockStickMovement = true;
		} 

		else if (Mathf.Abs(device.LeftStickX.Value) < 0.2f)
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
