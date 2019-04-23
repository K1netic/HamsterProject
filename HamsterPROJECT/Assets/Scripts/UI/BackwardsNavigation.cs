using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using InControl;

public class BackwardsNavigation : MonoBehaviour {

	float delay = 0.1f;
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
		}
	}

}
