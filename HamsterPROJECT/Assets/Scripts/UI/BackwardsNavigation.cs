using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using InControl;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BackwardsNavigation : MonoBehaviour {

	ScreenManager screenManager;
	[SerializeField] Animator previousScreen;

	void Start()
	{
		screenManager = FindObjectOfType<ScreenManager> ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (InputManager.ActiveDevice.Action2.WasPressed)
		{
			AudioManager.instance.PlaySound ("UI_cancel", "UI");
			screenManager.CloseCurrent ();
			screenManager.OpenPanel (previousScreen);
			EventSystem.current.SetSelectedGameObject(null);
		}
	}

}
