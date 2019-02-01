using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiButtonFunctions : MonoBehaviour {

	[SerializeField] string previousScene;
	[SerializeField] string sceneToLoadOnClick;
	[SerializeField] GameManager.gameModes gameModeToSet;
	[SerializeField] Text textToChange;
	AudioManager mngr;

	void Start()
	{
		mngr = FindObjectOfType<AudioManager> ();
	}

	void FixedUpdate()
	{
		// Update text to match GameManager value
		if (textToChange != null)
			textToChange.text = GameManager.rounds.ToString ();
	}

	public void LoadScene()
	{
		//reset timeScale in case a scene is loaded from the pause menu
		if (Time.timeScale != 1) Time.timeScale = 1;
		SceneManager.LoadScene (sceneToLoadOnClick);
	}

	public void Quit()
	{
		Application.Quit ();
	}

	public void SetGameMode()
	{
		GameManager.gameModeType = gameModeToSet;
	}

	public void ChangeValue()
	{
		mngr.PlaySound ("UI_pick", mngr.UIsource);
		switch (GameManager.rounds)
		{
		case 1:
			GameManager.rounds = 3;
			break;
		case 3:
			GameManager.rounds = 5;
			break;
		case 5:
			GameManager.rounds = 8;
			break;
		case 8: 
			GameManager.rounds = 10;
			break;
		case 10:
			GameManager.rounds = 1;
			break;
		default:
			break;
		}
	}

	public void LoadLastMap()
	{
		//reset timeScale in case a scene is loaded from the pause menu
		if (Time.timeScale != 1) Time.timeScale = 1;
		SceneManager.LoadScene (GameManager.lastLevelPlayed);
	}

	public void HighlightSound()
	{
		mngr.PlaySound ("UI_highlight", mngr.UIsource);
	}

	public void ClickSound()
	{
		mngr.PlaySound ("UI_validate", mngr.UIsource);
	}

	public void PickSound()
	{
		mngr.PlaySound ("UI_pick", mngr.UIsource);
	}

	public void HighlightPlusSound()
	{
		mngr.PlaySound ("UI_highlightPlus", mngr.UIsource);
	}

	public void ValidatePlusSound()
	{
		mngr.PlaySound ("UI_validatePlus", mngr.UIsource);
	}

	public void GameLaunchSound()
	{
		mngr.PlaySound ("UI_gameLaunch", mngr.UIsource);
	}
}
