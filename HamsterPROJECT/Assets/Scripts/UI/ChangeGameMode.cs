using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InControl;
using UnityEngine.EventSystems;

public class ChangeGameMode : Selectable {

	bool blockStickMovement = false;

	void Update()
	{
		if (EventSystem.current.currentSelectedGameObject == this.gameObject)
		{
			if ((Mathf.Abs(InputManager.ActiveDevice.LeftStickX.Value) >= 0.8f || InputManager.ActiveDevice.DPadX.WasPressed ) && !blockStickMovement)
			{
				if (GameManager.gameModeType == GameManager.gameModes.LastManStanding)
				{
					GameManager.gameModeType = GameManager.gameModes.Deathmatch;
					UpdateText();
				}
				else
				{
					GameManager.gameModeType = GameManager.gameModes.LastManStanding;
					UpdateText();
				}
				AudioManager.instance.PlaySound ("UI_pick", "UI");

				blockStickMovement = true;
			}

			else if (Mathf.Abs(InputManager.ActiveDevice.LeftStickX.Value) < 0.2f)
				blockStickMovement = false;
		}
	}

	public void UpdateText()
	{
		switch(GameManager.gameModeType)
		{
			case GameManager.gameModes.LastManStanding:
				this.GetComponent<Text>().text = "Last Man Standing";
			break;
			case GameManager.gameModes.Deathmatch:
				this.GetComponent<Text>().text = "Deathmatch";
			break;
		}
	}
}
