using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiButtonFunctions : MonoBehaviour {

	[SerializeField] string sceneToLoadOnClick;
	[SerializeField] Animator nextScreen;
	[SerializeField] GameManager.gameModes gameModeToSet;
	[SerializeField] GameObject controlsScheme;
	[SerializeField] int packID; 
	//Only use from pause menu
	[SerializeField] string screenNameToInitiallyOpen;
	LevelSelection lvlSelect;
	MusicManager music;
	ScreenManager screenManager;

	void Start()
	{
		lvlSelect = FindObjectOfType<LevelSelection>();
		music = GameObject.FindObjectOfType<MusicManager> ();
		screenManager = FindObjectOfType<ScreenManager> ();
	}

//	void FixedUpdate()
//	{
//		// Update text to match GameManager value
//		if (textToChange != null)
//			textToChange.text = GameManager.rounds.ToString ();
//	}

	public void LoadScene()
	{
		//reset timeScale in case a scene is loaded from the pause menu
		if (Time.timeScale != 1) Time.timeScale = 1;
		AudioManager.instance.PlaySound ("UI_validate", "UI");
		ScreenManager.screenToInitiallyOpen = screenNameToInitiallyOpen;
		MusicManager.instance.StopMusic ("menu");
		GameManager.ResetScores ();
		SceneManager.LoadScene (sceneToLoadOnClick);
	}

	public void OpenNextScreen()
	{
		AudioManager.instance.PlaySound ("UI_validate", "UI");
		screenManager.CloseCurrent ();
		screenManager.OpenPanel (nextScreen);
	}

	public void Quit()
	{
		Application.Quit ();
	}

	public void LoadLastMap()
	{
		//reset timeScale in case a scene is loaded from the pause menu
		if (Time.timeScale != 1) Time.timeScale = 1;
		SceneManager.LoadScene (GameManager.lastLevelPlayed);
	}

	public void Select()
	{
		this.GetComponent<Text> ().color = new Color (216, 191, 0);
		this.transform.GetChild (0).gameObject.SetActive (true);
		AudioManager.instance.PlaySound ("UI_highlight", "UI");
	}

	public void Deselect()
	{
		this.GetComponent<Text> ().color = new Color (255, 255, 255);
		this.transform.GetChild (0).gameObject.SetActive (false);
		AudioManager.instance.PlaySound ("UI_highlight", "UI");
	}

	// Utilisé pour les éléments qui ont deux flèches de sélection
	public void TwoWaySelect()
	{
		this.GetComponent<Text> ().color = new Color (216, 191, 0);
		this.transform.GetChild (0).gameObject.SetActive (true);
		this.transform.GetChild (1).gameObject.SetActive (true);
		AudioManager.instance.PlaySound ("UI_highlight", "UI");
	}

	// Utilisé pour les éléments qui ont deux flèches de sélection
	public void TwoWayDeselect()
	{
		this.GetComponent<Text> ().color = new Color (255, 255, 255);
		this.transform.GetChild (0).gameObject.SetActive (false);
		this.transform.GetChild (1).gameObject.SetActive (false);
		AudioManager.instance.PlaySound ("UI_highlight", "UI");
	}

	public void Validate()
	{
		AudioManager.instance.PlaySound ("UI_validate", "UI");
	}

	// Load a series of rounds (pack)
	public void LoadMatch()
	{
		if (Time.timeScale != 1) Time.timeScale = 1;
		AudioManager.instance.PlaySound ("UI_gameLaunch", "UI");
		MusicManager.instance.StopMusic ("menu");
		lvlSelect.SelectPack(packID);
		lvlSelect.LoadNextLevel ("");
	}

	public void OpenControlsScheme()
	{
		AudioManager.instance.PlaySound ("UI_validate", "UI");
		controlsScheme.SetActive (true);
		this.transform.parent.gameObject.SetActive (false);
	}

	public void StopBattleMusic()
	{
		AudioLowPassFilter filter = music.gameObject.GetComponent<AudioLowPassFilter> ();
		MusicManager.instance.StopMusic ("battle");
		music.GetComponent<AudioSource> ().volume *= 4.0f;
		filter.enabled = false;
	}

	public void StopMenuMusic()
	{
		MusicManager.instance.StopMusic ("menu");
	}

}
