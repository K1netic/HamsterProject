using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class ControlsScheme : MonoBehaviour {

	[SerializeField] GameObject PauseMenu;

	// Update is called once per frame
	void Update () {
		if (InputManager.ActiveDevice.AnyButtonWasPressed)
		{
			PauseMenu.SetActive (true);
			this.gameObject.SetActive (false);
			AudioManager.instance.PlaySound ("UI_cancel", "UI");
		}
	}
}
