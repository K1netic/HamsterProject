﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour {

	GameObject itemSelected;

	void OnEnable()
	{
		itemSelected = GameObject.Find ("ResumeButton");
		itemSelected.GetComponent<Button> ().Select ();
	}
}
