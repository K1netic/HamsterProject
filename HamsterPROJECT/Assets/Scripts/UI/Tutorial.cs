using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour {

	[SerializeField] Sprite[] tutorialPages;
	[SerializeField] Animator characterSelectionScreenAnimator;
	[SerializeField] Animator titleScreenAnimator;
	[SerializeField] GameObject nextHint;
	[SerializeField] GameObject playHint;
	[SerializeField] GameObject previousHint;
	[SerializeField] Image img;
	ScreenManager screenManager;

	float delay = 0.1f;
	int pageIndex = 0;

	void Start()
	{
		playHint.SetActive (false);
		screenManager = FindObjectOfType<ScreenManager> ();
	}

	void Update () 
	{
		if (InputManager.ActiveDevice.Action1.WasPressed && pageIndex < 2)
		{
			AudioManager.instance.PlaySound ("UI_validate", "UI");
			pageIndex += 1;
			img.sprite = tutorialPages [pageIndex];
		}
		else if (InputManager.ActiveDevice.Action2.WasPressed && pageIndex > 0)
		{
			AudioManager.instance.PlaySound ("UI_cancel", "UI");
			pageIndex -= 1;
			img.sprite = tutorialPages [pageIndex];
		}
		else if (InputManager.ActiveDevice.Action1.WasPressed && pageIndex == 2)
		{
			AudioManager.instance.PlaySound ("UI_gameLaunch", "UI");
			screenManager.OpenPanel (characterSelectionScreenAnimator);
		}
		else if (InputManager.ActiveDevice.Action2.WasPressed && pageIndex == 0)
		{
			AudioManager.instance.PlaySound ("UI_cancel", "UI");
			screenManager.CloseCurrent ();
			if(PlayerPrefs.GetInt("NotFirstTime") == 1)
				screenManager.OpenPanel (characterSelectionScreenAnimator);
			else 
				screenManager.OpenPanel (titleScreenAnimator);
		}
			
		if (pageIndex == 2)
		{
			nextHint.SetActive (false);
			playHint.SetActive (true);
		}
		else
		{
			nextHint.SetActive (true);
			playHint.SetActive (false);
		}
			

	}
}
