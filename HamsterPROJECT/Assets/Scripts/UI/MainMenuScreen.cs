using System.Collections;
using System.Collections.Generic;
using InControl;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuScreen : MonoBehaviour {
	GameObject itemSelected;
	ScreenManager manager;

	void OnEnable()
	{
		GameObject.Find ("UISource").GetComponent<AudioSource>().Stop ();
		MusicManager.instance.PlayMusic("menu");
		itemSelected = GameObject.Find ("PlayButton");
		manager = GameObject.Find("ScreenManager").GetComponent<ScreenManager>();
		StartCoroutine (WaitBeforeAllowingActivationOfButton ());
	}

	IEnumerator WaitBeforeAllowingActivationOfButton()
	{
		yield return new WaitForSeconds(0.25f);
		EventSystem.current.firstSelectedGameObject = itemSelected;
		itemSelected.GetComponent<Button> ().Select ();
	}

}
