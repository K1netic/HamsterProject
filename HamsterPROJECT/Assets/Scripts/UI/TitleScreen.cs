using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using InControl;
using UnityEngine.EventSystems;

public class TitleScreen : MonoBehaviour {

	ScreenManager screenManager;
	[SerializeField] Animator nextScreenAnimator;

    void Start()
	{
		//PlayerPrefs.DeleteAll ();
		AudioManager.instance.PlaySound("UI_titleJingle", "UI");
        Cursor.visible = false;
		screenManager = FindObjectOfType<ScreenManager> ();
	}

	void Update () {
		foreach (InputDevice dev in InputManager.ActiveDevices)
		{
			if (dev.Action1.WasPressed)
			{
				OpenNextScreen ();
			}

			else if (InputManager.ActiveDevice.Action2.WasPressed)
			{
				Application.Quit ();
			}
		}
	}

	void OnEnable()
	{
		EventSystem.current.firstSelectedGameObject = null;
	}

	void OpenNextScreen()
	{
		AudioManager.instance.PlaySound ("UI_titleScreenValidation", "UI");
		screenManager.OpenPanel (nextScreenAnimator);
		// if(PlayerPrefs.GetInt("NotFirstTime") == 1)
		// 	screenManager.OpenPanel (nextScreenAnimator);
		// else 
		// 	screenManager.OpenPanel (tutorialScreenAnimator);
	}
}
