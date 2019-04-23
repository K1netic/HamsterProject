using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuScreen : MonoBehaviour {
	GameObject itemSelected;

	void OnEnable()
	{
		GameObject.Find ("UISource").GetComponent<AudioSource>().Stop ();
		MusicManager.instance.PlayMusic("menu");
		itemSelected = GameObject.Find ("PlayButton");
		StartCoroutine (WaitBeforeAllowingActivationOfButton ());
	}

	IEnumerator WaitBeforeAllowingActivationOfButton()
	{
		yield return new WaitForSeconds (0.1f);
		EventSystem.current.firstSelectedGameObject = itemSelected;
		itemSelected.GetComponent<Button> ().Select ();
	}
}
