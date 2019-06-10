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
	GameObject itemSelected;
	GameObject lastSelectedOject;

	// Éléments à modifier en fonction de la sélection
	Text mapTitle;
	[SerializeField] Image mapMiniature;
	[SerializeField] Text mapDescription;

	[SerializeField] Sprite[] miniatures;
	[SerializeField] string[] descriptions;
	ActivateInput inputActivationScript;

	void OnEnable()
	{
		inputActivationScript = transform.parent.GetComponent<ActivateInput>();
		if (lastSelectedOject != null)
			itemSelected = lastSelectedOject;
		else 
			itemSelected = GameObject.Find ("Maps").transform.GetChild(0).gameObject;
		StartCoroutine (WaitBeforeAllowingActivationOfButton ());
		UpdateInfos(GameObject.Find ("Maps").transform.GetChild(0).gameObject.name);
	}

	void Start()
	{
		mapTitle = GameObject.Find("MapName").GetComponent<Text>();
		MusicManager.instance.PlayMusic("menu");
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
				selectedObjectPosition = new Vector2(currentSelectedObject.transform.position.x, currentSelectedObject.transform.position.y);
				selectedObjectName = currentSelectedObject.name;
				// Updating the title of the screen with the selected pack's name
				mapTitle.text = selectedObjectName;
				this.transform.position = new Vector2(selectedObjectPosition.x, selectedObjectPosition.y);
				UpdateInfos(selectedObjectName);
				lastSelectedOject = EventSystem.current.currentSelectedGameObject;
			}

			foreach (InputDevice dev in InputManager.ActiveDevices)
			{
				if (dev.Action1.WasPressed)
				{
					allowInteraction = false;
				}
			}
		}
	}
	
	IEnumerator WaitBeforeAllowingActivationOfButton()
	{
		yield return new WaitUntil(() => inputActivationScript.inputOK);
		EventSystem.current.firstSelectedGameObject = itemSelected;
		itemSelected.GetComponent<Button> ().Select ();
		currentSelectedObject = EventSystem.current.currentSelectedGameObject;
		allowInteraction = true;
	}

	void UpdateInfos(string mapName)
	{
		switch(mapName)
		{
			case "Alpha":
				mapMiniature.sprite = miniatures[Random.Range(0,3)];
				mapDescription.text = descriptions[0];
				break;
			case "Proto":
				mapMiniature.sprite = miniatures[Random.Range(0,3)];
				mapDescription.text = descriptions[1];
				break;
			case "Factory":
				mapMiniature.sprite = miniatures[0];
				mapDescription.text = descriptions[2];
				break;
			case "Submarine":
				mapMiniature.sprite = miniatures[1];
				mapDescription.text = descriptions[3];
				break;
			case "Mars":
				mapMiniature.sprite = miniatures[2];
				mapDescription.text = descriptions[4];
				break;
			case "Forest":
				mapMiniature.sprite = miniatures[3];
				mapDescription.text = descriptions[5];
				break;
		}
	}
}