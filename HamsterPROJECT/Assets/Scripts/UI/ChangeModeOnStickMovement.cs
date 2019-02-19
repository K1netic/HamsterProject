using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InControl;
using UnityEngine.EventSystems;

public class ChangeModeOnStickMovement : Selectable {

	bool blockStickMovement = false;
	BaseEventData baseEvent;
	AudioManager mngr;

	void Start()
	{
		mngr = FindObjectOfType<AudioManager> ();
	}

	void Update()
	{
		if (IsHighlighted(baseEvent))
		{
			if (Mathf.Abs(InputManager.ActiveDevice.LeftStickX.Value) >= 0.8f && !blockStickMovement)
			{
				if (GameManager.gameModeType == GameManager.gameModes.LastManStanding)
				{
					GameManager.gameModeType = GameManager.gameModes.Deathmatch;
					this.GetComponent<Text> ().text = "Deathmatch";
				}
				else
				{
					GameManager.gameModeType = GameManager.gameModes.LastManStanding;
					this.GetComponent<Text> ().text = "Last Man Standing";
				}
				mngr.PlaySound ("UI_pick", "UI");

				blockStickMovement = true;
			}

			else if (Mathf.Abs(InputManager.ActiveDevice.LeftStickX.Value) < 0.2f)
				blockStickMovement = false;
		}
	}
}
