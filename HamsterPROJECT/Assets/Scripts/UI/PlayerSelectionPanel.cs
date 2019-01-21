using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

public class PlayerSelectionPanel : MonoBehaviour {

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

	// Alternative to unity input method
	public PlayerIndex selectionPanelPlayerIndex;
	bool coroutineLimiter = false;

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
		// Activation
		if (GamePad.GetState (selectionPanelPlayerIndex).Buttons.A == ButtonState.Pressed 
			&& state == SelectionPanelState.Deactivated
			&& !coroutineLimiter)
		{
			coroutineLimiter = true;
			StartCoroutine(ChangeState(SelectionPanelState.Activated));
		}

		// Validation
		else if (GamePad.GetState (selectionPanelPlayerIndex).Buttons.A == ButtonState.Pressed 
			&& state == SelectionPanelState.Activated 
			&& CharacterSelectionScreen.selectableCharacters[characterSelected] == true
			&& !coroutineLimiter)
		{
			coroutineLimiter = true;
			StartCoroutine(ChangeState(SelectionPanelState.Validated, false));
			validatedCharacter = GameManager.Characters[characterSelected];
		}
			
		// Deactivation
		if (GamePad.GetState (selectionPanelPlayerIndex).Buttons.B == ButtonState.Pressed 
			&& state == SelectionPanelState.Activated
			&& !coroutineLimiter)
		{
			coroutineLimiter = true;
			StartCoroutine(ChangeState(SelectionPanelState.Deactivated));
			mngr.PlaySound ("UI_cancel", mngr.UIsource);
		}

		// Unvalidation
		else if (GamePad.GetState (selectionPanelPlayerIndex).Buttons.B == ButtonState.Pressed
			&& state == SelectionPanelState.Validated
			&& !coroutineLimiter)
		{
			coroutineLimiter = true;
			StartCoroutine(ChangeState(SelectionPanelState.Activated, true));
			mngr.PlaySound ("UI_cancel", mngr.UIsource);
			select.ready = false;
		}

		// Trying to validate on a character not avalaible
		else if (GamePad.GetState (selectionPanelPlayerIndex).Buttons.A == ButtonState.Pressed
			&& state == SelectionPanelState.Activated 
			&& CharacterSelectionScreen.selectableCharacters[characterSelected] == false
			&& !coroutineLimiter)
		{
			//Play error sound
			//Display "not available text"
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

	IEnumerator ChangeState(SelectionPanelState stateToApply)
	{
		yield return new WaitForSeconds(0.1f);
		state = stateToApply;
		coroutineLimiter = false;
	}

	IEnumerator ChangeState(SelectionPanelState stateToApply, bool stateToSetCharacterSelect)
	{
		yield return new WaitForSeconds(0.1f);
		state = stateToApply;
		CharacterSelectionScreen.selectableCharacters [characterSelected] = stateToSetCharacterSelect;
		coroutineLimiter = false;
	}
		
	void CharacterSelection()
	{
		// Display character as unavailable if that's the case
		if (!CharacterSelectionScreen.selectableCharacters [characterSelected])
			notAvailable.gameObject.SetActive (true);
		else
			notAvailable.gameObject.SetActive (false);

		if (GamePad.GetState (selectionPanelPlayerIndex).ThumbSticks.Left.X == 1.0f && !blockStickMovement)
		{
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

		else if (GamePad.GetState (selectionPanelPlayerIndex).ThumbSticks.Left.X == -1.0f && !blockStickMovement)
		{
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

		else if (Mathf.Abs(GamePad.GetState (selectionPanelPlayerIndex).ThumbSticks.Left.X) < 0.2f)
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
