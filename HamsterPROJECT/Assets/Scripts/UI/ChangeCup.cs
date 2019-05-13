using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InControl;
using UnityEngine.EventSystems;

public class ChangeCup : Selectable {

	bool blockStickMovement = false;
	BaseEventData baseEvent;

	Text textToChange;
	Text cupTitleText;

	void Start()
	{
		textToChange = this.transform.GetChild (2).GetComponent<Text>();
		cupTitleText = this.GetComponent<Text>();
	}

	void Update()
	{
		if (EventSystem.current.currentSelectedGameObject == this.gameObject)
		{
			// Augment value of rounds
			if ((InputManager.ActiveDevice.LeftStickX.Value >= 0.8f || InputManager.ActiveDevice.DPadRight.WasPressed ) && !blockStickMovement)
			{
				IncreaseValue();
				AudioManager.instance.PlaySound ("UI_pick", "UI");
				UpdateDescriptionText();
				UpdateCupText();
				blockStickMovement = true;
			}

			else if ((InputManager.ActiveDevice.LeftStickX.Value <= -0.8f || InputManager.ActiveDevice.DPadLeft.WasPressed) && !blockStickMovement)
			{
				DecreaseValue();
				AudioManager.instance.PlaySound ("UI_pick", "UI");				
				UpdateDescriptionText();
				UpdateCupText();
				blockStickMovement = true;
			} 

			else if (Mathf.Abs(InputManager.ActiveDevice.LeftStickX.Value) < 0.2f)
				blockStickMovement = false;
		}
		
	}

	public void IncreaseValue()
	{
		switch (GameManager.rounds)
		{
		case 3:
			GameManager.rounds = 5;
			break;
		case 5:
			GameManager.rounds = 8;
			break;
		case 8: 
			GameManager.rounds = 3;
			break;
		default:
			break;
		}
	}

	public void DecreaseValue()
	{
		switch (GameManager.rounds)
		{
		case 3:
			GameManager.rounds = 8;
			break;
		case 5:
			GameManager.rounds = 3;
			break;
		case 8: 
			GameManager.rounds = 5;
			break;
		default:
			break;
		}
	}

	// Update text to match GameManager value
	public void UpdateDescriptionText()
	{
		textToChange.text = "FIGHT TO " + GameManager.rounds.ToString () + " WINS/KILLS";
	}

	public void UpdateCupText()
	{
		switch(GameManager.rounds)
		{
			case 3:
				cupTitleText.text = "LITTLE CUP";
				break;
			case 5:
				cupTitleText.text = "NORMAL CUP";
				break;
			case 8:
				cupTitleText.text = "BIG CUP";
				break;
		}
	}
}

	
