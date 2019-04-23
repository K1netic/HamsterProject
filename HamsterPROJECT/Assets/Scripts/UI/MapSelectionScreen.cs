using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapSelectionScreen : MonoBehaviour {

	GameObject itemSelected;

	void OnEnable()
	{
		itemSelected = GameObject.Find ("Maps").transform.GetChild(0).gameObject;
		StartCoroutine (WaitBeforeAllowingActivationOfButton ());
	}

	IEnumerator WaitBeforeAllowingActivationOfButton()
	{
		yield return new WaitForSeconds (0.1f);
		EventSystem.current.firstSelectedGameObject = itemSelected;
		itemSelected.GetComponent<Button> ().Select ();
	}

}
