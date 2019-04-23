using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InControl;
using UnityEngine.EventSystems;

public class ControlsScheme : MonoBehaviour {

	[SerializeField] GameObject PauseMenu;
	GameObject itemSelected;

	// Update is called once per frame
	void Update () {
		if (InputManager.ActiveDevice.AnyButtonWasPressed)
		{
			AudioManager.instance.PlaySound ("UI_cancel", "UI");
			PauseMenu.SetActive (true);
			itemSelected = GameObject.Find ("Controls");
			EventSystem.current.firstSelectedGameObject = itemSelected;
			itemSelected.GetComponent<Button> ().Select ();
			this.gameObject.SetActive(false);
		}
	}
	
}
