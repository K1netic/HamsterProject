using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using InControl;
using UnityEngine.UI;

public class ResultsScreen : MonoBehaviour {

	int winnerScore;
	int winnerIndex;
	[SerializeField] PlayerResultPanel[] panels;
	[SerializeField] Animator endGameScreen;
//	[SerializeField] string sceneToLoad;
	Sprite sprt;
	ScreenManager screenManager;

	// Audio
	float delay = 0.1f;
	MusicManager music;
	RectTransform rect;

	bool activateInput = false;

	void Awake()
	{
		music = GameObject.FindObjectOfType<MusicManager> ();
		winnerScore = GameManager.playersScores.Max();
		winnerIndex = System.Array.IndexOf(GameManager.playersScores, winnerScore);
		sprt = Resources.Load<Sprite>("Results/" + GameManager.playersCharacters [winnerIndex].name);
		screenManager = FindObjectOfType<ScreenManager> ();
	}
	void Start()
	{
		music.StopMusic ("battle");
		AudioManager.instance.PlaySound ("UI_resultsScreen", "UI");
		panels [winnerIndex].borders.sprite = sprt; 
		rect = panels [winnerIndex].GetComponent<RectTransform> ();
		rect.anchorMax = new Vector2(rect.anchorMax.x, rect.anchorMax.y + 0.15f);
		rect.anchorMin = new Vector2(rect.anchorMin.x, rect.anchorMin.y + 0.15f);
		StartCoroutine (InputActivating());
	}
		
	void Update()
	{
		foreach (InputDevice dev in InputManager.ActiveDevices)
		{
			if (activateInput && (dev.AnyButtonWasPressed || dev.CommandWasPressed))
			{
				GameManager.ResetScores ();
//				SceneManager.LoadScene (sceneToLoad);
				screenManager.CloseCurrent ();
				screenManager.OpenPanel (endGameScreen);
				AudioManager.instance.PlaySound ("UI_validate", "UI");
			}
		}
	}

//	IEnumerator LoadEndGame()
//	{
//		AudioManager.instance.PlaySound ("UI_validate", "UI");
//		yield return new WaitForSeconds (delay);
//		SceneManager.LoadScene (sceneToLoad);
//	}

	// Utilisé pour éviter de skip l'écran de résultats par erreur
	IEnumerator InputActivating()
	{
		yield return new WaitForSeconds (3.0f);
		activateInput = true;
	}
}
