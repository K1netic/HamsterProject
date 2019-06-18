using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour {

	[SerializeField] Sprite[] tutorialPages;
	[SerializeField] Animator mainMenuAnimator;
	[SerializeField] Animator characterSelectionAnimator;
	[SerializeField] GameObject nextHint;
	[SerializeField] GameObject backToMenuHint;
	[SerializeField] GameObject previousHint;
	[SerializeField] Image img;
	ScreenManager screenManager;

	int pageIndex = 0;

	void OnEnable()
	{
		pageIndex = 0;
		img.sprite = tutorialPages [pageIndex];
	}

	void Start()
	{
		backToMenuHint.SetActive (false);
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
			AudioManager.instance.PlaySound ("UI_validate", "UI");
			StartCoroutine(CloseWait());
		}
		else if (InputManager.ActiveDevice.Action2.WasPressed && pageIndex == 0)
		{
			AudioManager.instance.PlaySound ("UI_cancel", "UI");
			StartCoroutine(CloseWait());
		}
			
		if (pageIndex == 2)
		{
			nextHint.SetActive (false);
			backToMenuHint.SetActive (true);
		}
		else
		{
			nextHint.SetActive (true);
			backToMenuHint.SetActive (false);
		}
	}

	IEnumerator CloseWait()
	{
		yield return new WaitForSeconds(0.2f);
		screenManager.CloseCurrent ();
		screenManager.OpenPanel (mainMenuAnimator);
	}
}
