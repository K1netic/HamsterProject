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

	// Audio
	float delay = 0.1f;
	AudioManager mngr;
	RectTransform rect;

	void Start()
	{
		//mngr = FindObjectOfType<AudioManager> ();
		winnerScore = GameManager.playersScores.Max();
		winnerIndex = System.Array.IndexOf(GameManager.playersScores, winnerScore);
		panels[winnerIndex].background = Resources.Load<Image>("Results/" + GameManager.playersCharacters [winnerIndex].name);
		rect = panels [winnerIndex].GetComponent<RectTransform> ();
		rect.anchorMax = new Vector2(rect.anchorMax.x, rect.anchorMax.y + 0.15f);
		rect.anchorMin = new Vector2(rect.anchorMin.x, rect.anchorMin.y + 0.15f);
	}
		
	void Update()
	{
		if (InputManager.ActiveDevice.AnyButtonWasPressed || InputManager.CommandWasPressed)
		{
			GameManager.ResetScores ();
			SceneManager.LoadScene (sceneToLoad);
		}
	}

	IEnumerator LoadEndGame()
	{
		//mngr.PlaySound ("UI_validatePlus", //mngr.UIsource);
		yield return new WaitForSeconds (delay);
		SceneManager.LoadScene (sceneToLoad);
	}
}
