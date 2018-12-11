using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiButtonFunctions : MonoBehaviour {

	[SerializeField] string previousScene;
	[SerializeField] string sceneToLoadOnClick;
	[SerializeField] GameManager.gameModes gameModeToSet;

	void FixedUpdate()
	{
		if (Input.GetButton("Cancel_P1") && previousScene != null && previousScene != "")
		{
			SceneManager.LoadScene (previousScene);
		}
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

}
