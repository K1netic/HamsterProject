using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using InControl;
using UnityEngine.UI;

public class MapSelector : MonoBehaviour {

	bool allowInteraction = false;
	Vector3 selectedObjectPosition = new Vector3(0f,0f,0f);
	bool selectionHasChanged;
	GameObject currentSelectedObject;
	GameObject previousSelectedObject;
	public string selectedObjectName;

	GameObject lastSelectedOject;
	Text mapTitle;

	GameObject itemSelected;

	void OnEnable()
	{
		if (lastSelectedOject != null)
			itemSelected = lastSelectedOject;
		else 
			itemSelected = GameObject.Find ("Maps").transform.GetChild(0).gameObject;
		StartCoroutine (WaitBeforeAllowingActivationOfButton ());
		MusicManager.instance.PlayMusic("menu");
	}

	void Start()
	{
		mapTitle = GameObject.Find("MapName").GetComponent<Text>();
	}

	// Update is called once per frame
	void Update()
	{
		if (allowInteraction)
		{
			previousSelectedObject = currentSelectedObject;
			currentSelectedObject = EventSystem.current.currentSelectedGameObject;
			// Move selector to the newly selected element when a player changes the selection
			if (currentSelectedObject != previousSelectedObject && currentSelectedObject != null)
			{
				AudioManager.instance.PlaySound("UI_pick", "UI");
				selectedObjectPosition = new Vector2(currentSelectedObject.transform.position.x, currentSelectedObject.transform.position.y);
				selectedObjectName = currentSelectedObject.name;
				// Updating the title of the screen with the selected pack's name
				mapTitle.text = selectedObjectName;
				this.transform.position = new Vector2(selectedObjectPosition.x, selectedObjectPosition.y);
			}

			foreach (InputDevice dev in InputManager.ActiveDevices)
			{
				if (dev.Action1.WasPressed)
				{
					allowInteraction = false;
					lastSelectedOject = EventSystem.current.currentSelectedGameObject;
				}
			}
		}
	}
	
	IEnumerator WaitBeforeAllowingActivationOfButton()
	{
		yield return new WaitForSeconds (0.4f);
		EventSystem.current.firstSelectedGameObject = itemSelected;
		itemSelected.GetComponent<Button> ().Select ();
		currentSelectedObject = EventSystem.current.currentSelectedGameObject;
		allowInteraction = true;
	}
}