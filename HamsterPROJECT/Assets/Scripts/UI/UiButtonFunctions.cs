using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiButtonFunctions : MonoBehaviour {

	[SerializeField] string previousScene;
	[SerializeField] string sceneToLoadOnClick;
	[SerializeField] GameManager.gameModes gameModeToSet;
	[SerializeField] GameObject controlsScheme;
	AudioManager mngr;
	LevelSelection lvlSelect;
	MusicManager music;

	void Start()
	{
		mngr = FindObjectOfType<AudioManager> ();
		lvlSelect = FindObjectOfType<LevelSelection>();
		music = GameObject.FindObjectOfType<MusicManager> ();
	}

	void FixedUpdate()
	{
//		// Update text to match GameManager value
//		if (textToChange != null)
//			textToChange.text = GameManager.rounds.ToString ();
	}

	public void LoadScene()
	{
		//reset timeScale in case a scene is loaded from the pause menu
		if (Time.timeScale != 1) Time.timeScale = 1;
		mngr.PlaySound ("UI_validate", "UI");
		SceneManager.LoadScene (sceneToLoadOnClick);
	}

	public void Quit()
	{
		Application.Quit ();
	}

	public void ChangeValue()
	{
		//mngr.PlaySound ("UI_pick", //mngr.UIsource);
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

	public void Select()
	{
		this.GetComponent<Text> ().color = new Color (216, 191, 0);
		this.transform.GetChild (0).gameObject.SetActive (true);
		mngr.PlaySound ("UI_highlight", "UI");
	}

	public void Deselect()
	{
		this.GetComponent<Text> ().color = new Color (255, 255, 255);
		this.transform.GetChild (0).gameObject.SetActive (false);
		mngr.PlaySound ("UI_highlight", "UI");
	}

	// Utilisé pour les éléments qui ont deux flèches de sélection
	public void TwoWaySelect()
	{
		this.GetComponent<Text> ().color = new Color (216, 191, 0);
		this.transform.GetChild (0).gameObject.SetActive (true);
		this.transform.GetChild (1).gameObject.SetActive (true);
		mngr.PlaySound ("UI_highlight", "UI");
	}

	// Utilisé pour les éléments qui ont deux flèches de sélection
	public void TwoWayDeselect()
	{
		this.GetComponent<Text> ().color = new Color (255, 255, 255);
		this.transform.GetChild (0).gameObject.SetActive (false);
		this.transform.GetChild (1).gameObject.SetActive (false);
		mngr.PlaySound ("UI_highlight", "UI");
	}

	public void Validate()
	{
		mngr.PlaySound ("UI_validate", "UI");
	}

	// Load a series of rounds (pack)
	public void LoadMatch()
	{
		if (Time.timeScale != 1) Time.timeScale = 1;
		mngr.PlaySound ("UI_gameLaunch", "UI");
		MusicManager.instance.StopMusic ("menu");
		lvlSelect.LoadNextLevel ("");
	}

	public void OpenControlsScheme()
	{
		mngr.PlaySound ("UI_validate", "UI");
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
