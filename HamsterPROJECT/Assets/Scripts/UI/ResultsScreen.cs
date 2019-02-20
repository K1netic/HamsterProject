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
	[SerializeField] string sceneToLoad;
	Sprite sprt;

	// Audio
	float delay = 0.1f;
	AudioManager mngr;
	MusicManager music;
	RectTransform rect;

	bool activateInput = false;

	void Awake()
	{
		sprt = Resources.Load<Sprite>("Results/" + GameManager.playersCharacters [winnerIndex].name);
		mngr = FindObjectOfType<AudioManager> ();
		music = GameObject.FindObjectOfType<MusicManager> ();
	}
	void Start()
	{
		music.StopMusic ("battle");
		mngr.PlaySound ("UI_resultsScreen", "UI");
		winnerScore = GameManager.playersScores.Max();
		winnerIndex = System.Array.IndexOf(GameManager.playersScores, winnerScore);
		panels [winnerIndex].borders.sprite = sprt; 
		rect = panels [winnerIndex].GetComponent<RectTransform> ();
		rect.anchorMax = new Vector2(rect.anchorMax.x, rect.anchorMax.y + 0.15f);
		rect.anchorMin = new Vector2(rect.anchorMin.x, rect.anchorMin.y + 0.15f);
		StartCoroutine (InputActivating());
	}
		
	void Update()
	{
		if (activateInput && InputManager.ActiveDevice.AnyButtonWasPressed || InputManager.CommandWasPressed)
		{
			GameManager.ResetScores ();
			SceneManager.LoadScene (sceneToLoad);
		}
	}

	IEnumerator LoadEndGame()
	{
		mngr.PlaySound ("UI_validate", "UI");
		yield return new WaitForSeconds (delay);
		SceneManager.LoadScene (sceneToLoad);
	}

	// Utilisé pour éviter de skip l'écran de résultats par erreur
	IEnumerator InputActivating()
	{
		yield return new WaitForSeconds (3.0f);
		activateInput = true;
	}
}
