using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using InControl;

public class ResultsScreen : MonoBehaviour {

	int winnerScore;
	int winnerIndex;
	[SerializeField] Color winColor;
	[SerializeField] PlayerResultPanel[] panels;
	[SerializeField] string sceneToLoad;

	// Audio
	float delay = 0.1f;
	AudioManager mngr;

	void Start()
	{
		mngr = FindObjectOfType<AudioManager> ();
		winnerScore = GameManager.playersScores.Max();
		winnerIndex = System.Array.IndexOf(GameManager.playersScores, winnerScore);
		panels [winnerIndex].background.color = winColor;
	}
		
	void Update()
	{
		if (InputManager.ActiveDevice.AnyButtonWasPressed)
		{
			GameManager.ResetScores ();
			SceneManager.LoadScene (sceneToLoad);
		}
	}

	IEnumerator LoadEndGame()
	{
		mngr.PlaySound ("UI_validatePlus", mngr.UIsource);
		yield return new WaitForSeconds (delay);
		SceneManager.LoadScene (sceneToLoad);
	}
}
