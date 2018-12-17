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

	void FixedUpdate()
	{
		// Update text to match GameManager value
		if (textToChange != null)
			textToChange.text = GameManager.goal.ToString ();
	}

	public void LoadScene(){
		SceneManager.LoadScene (sceneToLoadOnClick);
	}

	public void Quit()
	{
		if (Application.isEditor)
		{
			UnityEditor.EditorApplication.isPlaying = false;
		}
		else
		{
			Application.Quit ();
		}
	}

	public void SetGameMode()
	{
		GameManager.gameModeType = gameModeToSet;
	}

	public void ChangeValue()
	{
		switch (GameManager.goal)
		{
		case 3:
			GameManager.goal = 5;
			break;
		case 5:
			GameManager.goal = 8;
			break;
		case 8: 
			GameManager.goal = 10;
			break;
		case 10:
			GameManager.goal = 3;
			break;
		default:
			break;
		}
	}
}
