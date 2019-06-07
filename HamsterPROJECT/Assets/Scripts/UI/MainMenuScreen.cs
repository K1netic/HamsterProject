using System.Collections;
using System.Collections.Generic;
using InControl;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuScreen : MonoBehaviour {
	GameObject itemSelected;
	ScreenManager manager;
	ActivateInput inputActivationScript;

	bool dontLoop = false;

	void OnEnable()
	{
		MusicManager.instance.PlayMusic("menu");
		itemSelected = GameObject.Find ("PlayButton");
		manager = GameObject.Find("ScreenManager").GetComponent<ScreenManager>();
		inputActivationScript = transform.parent.GetComponent<ActivateInput>();
		StartCoroutine (WaitBeforeAllowingActivationOfButton ());
	}

	IEnumerator WaitBeforeAllowingActivationOfButton()
	{
		yield return new WaitUntil(() => inputActivationScript.inputOK);
		EventSystem.current.firstSelectedGameObject = itemSelected;
		itemSelected.GetComponent<Button> ().Select ();
	}
}
