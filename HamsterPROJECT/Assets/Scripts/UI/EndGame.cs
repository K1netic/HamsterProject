using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EndGame : MonoBehaviour {

	GameObject itemSelected;

	void OnEnable()
	{
		MusicManager.instance.PlayMusic("menu");
		itemSelected = GameObject.Find ("RematchButton");
		StartCoroutine (WaitBeforeAllowingActivationOfButton ());
	}

	IEnumerator WaitBeforeAllowingActivationOfButton()
	{
		yield return new WaitForSeconds (0.25f);
		EventSystem.current.firstSelectedGameObject = itemSelected;
		itemSelected.GetComponent<Button> ().Select ();
	}
}
