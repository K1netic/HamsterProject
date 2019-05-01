using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameModesScreen : MonoBehaviour {

	GameObject itemSelected;

	void OnEnable()
	{
		itemSelected = GameObject.Find ("GameModeSelection");
		EventSystem.current.firstSelectedGameObject = itemSelected;
		StartCoroutine (WaitBeforeAllowingActivationOfButton ());
  	}

	IEnumerator WaitBeforeAllowingActivationOfButton()
	{
		yield return new WaitForSeconds (0.5f);
		EventSystem.current.firstSelectedGameObject = itemSelected;
		itemSelected.GetComponent<ChangeGameMode> ().Select ();
	}
}
