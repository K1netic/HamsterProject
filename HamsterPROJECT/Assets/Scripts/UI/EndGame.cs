using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EndGame : MonoBehaviour {

	GameObject itemSelected;
	ActivateInput inputActivationScript;

	void OnEnable()
	{
		inputActivationScript = transform.parent.GetComponent<ActivateInput>();
		MusicManager.instance.PlayMusic("menu");
		itemSelected = GameObject.Find ("RematchButton");
		StartCoroutine (WaitBeforeAllowingActivationOfButton ());
	}

	IEnumerator WaitBeforeAllowingActivationOfButton()
	{
		yield return new WaitUntil(() => inputActivationScript.inputOK);
		EventSystem.current.firstSelectedGameObject = itemSelected;
		itemSelected.GetComponent<Button> ().Select ();
	}
}
