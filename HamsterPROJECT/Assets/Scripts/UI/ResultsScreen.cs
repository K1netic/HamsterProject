using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class ResultsScreen : MonoBehaviour {

	int winnerScore;
	int winnerIndex;
	[SerializeField] Color winColor;

	[SerializeField] PlayerResultPanel[] panels;

	[SerializeField] string sceneToLoad;

	void Start()
	{
		winnerScore = GameManager.playersScores.Max();
		winnerIndex = System.Array.IndexOf(GameManager.playersScores, winnerScore);
		panels [winnerIndex].background.color = winColor;
	}
		
	void Update()
	{
		if (Input.GetButtonDown("Pause_P1"))
		{
			GameManager.ResetScore ();
			SceneManager.LoadScene (sceneToLoad);
		}
	}
}
