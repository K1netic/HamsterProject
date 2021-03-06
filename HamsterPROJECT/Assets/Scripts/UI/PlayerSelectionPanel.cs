﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InControl;
using UnityEditor;

public class PlayerSelectionPanel : MonoBehaviour {

	// Panel state
	public enum SelectionPanelState {Deactivated, Activated, Validated};
	public SelectionPanelState state;

	// Collections of images to load and apply on various changes
	[HideInInspector] public Sprite[] characterSprites = new Sprite[GameManager.nbOfCharacters];
	[HideInInspector] public Sprite[] unavailableCharacterSprites = new Sprite[GameManager.nbOfCharacters];
	[SerializeField] Image border;
	Sprite basicBorder;
	Sprite[] validatedBorders = new Sprite[GameManager.nbOfCharacters];

	// Images to activate/deactive which receive the collections of images
	[SerializeField] Image notAvailable;
	[SerializeField] public Animator doorDownAnimator;
	[SerializeField] public Animator doorUpAnimator;

	public Image characterSprite;
	[SerializeField] GameObject guid;
	GameObject leftArrow;
	GameObject rightArrow;

	// Character selection
	[HideInInspector] public int characterSelected = 0;
	bool blockStickMovement = false;
	CharacterSelectionScreen select;
	[HideInInspector] public GameObject validatedCharacter;

	// Character instanciation
	GameObject characterPrefab;
	GameObject newPlayer;
	[SerializeField] int panelId;
	bool blockCharacterSelection = false;
	[SerializeField] GameObject UIElements;
	[SerializeField] GameObject RoomElements;

	// Input device
	public InputDevice device;

	// Control options
	public bool tract = false;

	void Awake()
	{
		characterSprites = Resources.LoadAll<Sprite> ("CharacterSelection/ValidatedCharacters");
	}

	void OnEnable()
	{
		state = SelectionPanelState.Deactivated;
	}

	void Start()
	{
		select = GameObject.Find ("CharacterSelectionScripts").GetComponent<CharacterSelectionScreen> ();
		characterPrefab = Resources.Load<GameObject> ("Prefabs/PlayerPrefab");
		unavailableCharacterSprites = Resources.LoadAll<Sprite> ("CharacterSelection/UnavailableCharacters");
		validatedBorders = Resources.LoadAll<Sprite> ("CharacterSelection/Borders");
		basicBorder = Resources.Load<Sprite> ("CharacterSelection/PlayerBord");
		leftArrow = guid.transform.GetChild (0).gameObject;
		rightArrow = guid.transform.GetChild (1).gameObject;
	}

	// Update is called once per frame
	void Update () {

		if (device != null)
		{
			#region StateManagement
			// Activation (Decativated -> Activated)
			if (device.Action1.WasPressed
				&& state == SelectionPanelState.Deactivated)
			{
				state = SelectionPanelState.Activated;
				AudioManager.instance.PlaySound ("UI_panelActivation", "UI");
			}

			// Validation (Activated -> Validated)
			else if (device.Action1.WasPressed
				&& state == SelectionPanelState.Activated 
				&& CharacterSelectionScreen.selectableCharacters[characterSelected] == true
				&& select.inputActivationScript.inputOK)
			{
				state = SelectionPanelState.Validated;
				AudioManager.instance.PlaySound ("UI_validate", "UI");
				InstantiatePlayer();
				validatedCharacter = newPlayer;
			}

			// Deactivation (Activated -> Deactivated)
			if (device.Action2.WasPressed
				&& state == SelectionPanelState.Activated)
			{
				state = SelectionPanelState.Deactivated;
				AudioManager.instance.PlaySound ("UI_cancel", "UI");
				device = null;
			}

			// Unvalidation (Validated -> Activated)
			else if (device.Action2.WasPressed
				&& state == SelectionPanelState.Validated)
			{
				state = SelectionPanelState.Activated;
				UIElements.SetActive(true);
				AudioManager.instance.PlaySound ("UI_cancel", "UI");
				select.ready = false;
				CharacterSelectionScreen.selectableCharacters [characterSelected] = true;
				Destroy (newPlayer.gameObject);
				blockCharacterSelection = false;
				device.StopVibration ();
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
			characterSprite.gameObject.SetActive(false);
			guid.SetActive(true);
			leftArrow.SetActive(false);
			rightArrow.SetActive(false);
			doorDownAnimator.SetBool("open", false);
			doorUpAnimator.SetBool("open", false);
			notAvailable.gameObject.SetActive (false);
			border.sprite = basicBorder;
			break;

		case SelectionPanelState.Activated:
			if (CharacterSelectionScreen.selectableCharacters[characterSelected] == false)
			{
				if (characterSelected < GameManager.nbOfCharacters - 1)
					FindNextSelectableCharacter(characterSelected + 1);
				else 
					FindNextSelectableCharacter(0);
			}
			UIElements.SetActive(true);
			RoomElements.SetActive(false);
			characterSprite.gameObject.SetActive(true);
			guid.SetActive(true);
			leftArrow.SetActive(true);
			rightArrow.SetActive(true);
			doorDownAnimator.SetBool("open", true);
			doorUpAnimator.SetBool("open", true);
			characterSprite.sprite = characterSprites[characterSelected];
			//Activate character selection
			CharacterSelection();
			border.sprite = basicBorder;
			break;

		case SelectionPanelState.Validated:
			border.sprite = validatedBorders[characterSelected];
			guid.SetActive(false);
			CharacterSelectionScreen.selectableCharacters [characterSelected] = false;
			blockCharacterSelection = true;
			UIElements.SetActive(false);
			RoomElements.SetActive(true);
			if (!MatchStart.gameHasStarted) 
				MatchStart.gameHasStarted = true;
			if (device.CommandWasPressed)
			{
				if (tract)
					tract = false;
				else if (!tract)
					tract = true;
				AudioManager.instance.PlaySound("UI_pick", "UI");
				newPlayer.GetComponentInChildren<Hook>().inverseRetractation = tract;
			}
			break;
		}
		#endregion
	}
		
	void CharacterSelection()
	{
		// Display character as unavailable if that's the case
		if (!CharacterSelectionScreen.selectableCharacters [characterSelected])
		{
			characterSprite.sprite = unavailableCharacterSprites [characterSelected];
			notAvailable.gameObject.SetActive (true);
		}
		else
		{
			characterSprite.sprite = characterSprites [characterSelected];
			notAvailable.gameObject.SetActive (false);
		}
			
		if ((device.LeftStickX.Value >= 0.8f || device.DPadRight.WasPressed) && !blockStickMovement && select.inputActivationScript.inputOK)
		{
			if (characterSelected < GameManager.nbOfCharacters - 1)
				FindNextSelectableCharacter(characterSelected + 1);
			else 
				FindNextSelectableCharacter(0);
			characterSprite.sprite = characterSprites[characterSelected];
			AudioManager.instance.PlaySound ("UI_pick", "UI");
			blockStickMovement = true;
		} 

		else if ((device.LeftStickX.Value <= -0.8f || device.DPadLeft.WasPressed) && !blockStickMovement && select.inputActivationScript.inputOK)
		{
			if (characterSelected > 0)
				FindPreviousSelectableCharacter(characterSelected - 1);

			else 
				FindPreviousSelectableCharacter(GameManager.nbOfCharacters - 1);

			characterSprite.sprite = characterSprites[characterSelected];
			AudioManager.instance.PlaySound ("UI_pick", "UI");
			blockStickMovement = true;
		} 

		else if (Mathf.Abs(device.LeftStickX.Value) < 0.2f )
			blockStickMovement = false;

	}

	void InstantiatePlayer()
	{
		GameObject inst = characterPrefab;
		inst.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = characterSprites[characterSelected];
		inst.transform.GetChild (0).GetComponent<PlayerMovement> ().playerNumber = "_P" + (panelId + 1).ToString();
		GameManager.playersInputDevices [panelId] = this.device;
		inst.transform.GetChild(0).GetComponent<Rigidbody2D> ().isKinematic = false;
		switch(panelId)
		{
		case 0:
			inst.transform.position = new Vector2 (-20f,9.5f);
			break;
		case 1:
			inst.transform.position = new Vector2 (20f,9.5f);
			break;
		case 2:
			inst.transform.position = new Vector2 (-20f,-13.5f);
			break;
		case 3:
			inst.transform.position = new Vector2 (20f,-13.5f);
			break;
		}
		newPlayer = Instantiate(inst);
		newPlayer.transform.GetChild(0).GetComponent<PlayerMovement>().playerInputDevice = GameManager.playersInputDevices[panelId];
		newPlayer.GetComponentInChildren<Hook>().inverseRetractation = tract;
		GameManager.playersSprites[panelId] = newPlayer.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
	}

	void FindNextSelectableCharacter(int index)
	{
		for (int i = index; i < GameManager.nbOfCharacters; i++)
		{
			if (CharacterSelectionScreen.selectableCharacters [i])
			{
				characterSelected = i;
				return;
			}
		}

		for (int i = 0; i < index - 1; i ++)
		{
			if (CharacterSelectionScreen.selectableCharacters [i])
			{
				characterSelected = i;
				return;
			}
		}
	}

	void FindPreviousSelectableCharacter(int index)
	{
		for (int i = index; i >= 0; i--)
		{
			if (CharacterSelectionScreen.selectableCharacters [i])
			{
				characterSelected = i;
				return;
			}
		}

		for (int i = GameManager.nbOfCharacters - 1; i > index + 1; i--)
		{
			if (CharacterSelectionScreen.selectableCharacters [i])
			{
				characterSelected = i;
				return;
			}
		}
	}

}
