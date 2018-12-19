using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class ResultsScreen : MonoBehaviour {

	int winnerScore;
	int winnerIndex;
	int winnerIndex2;
	[SerializeField] Color winColor;

	[SerializeField] PlayerResultPanel[] panels;

	void Start()
	{
		winnerScore = GameManager.playersScores.Max();

		winnerIndex = System.Array.IndexOf(GameManager.playersScores, winnerScore);
		winnerIndex2 = System.Array.LastIndexOf(GameManager.playersScores, winnerScore);
		Debug.Log (winnerIndex.ToString ());
		Debug.Log (winnerIndex2.ToString ());

		if (winnerIndex != winnerIndex2)
		{
			panels [winnerIndex2].background.color = winColor;
		}

		panels [winnerIndex].background.color = winColor;
	}
		
	void Update()
	{
		if (Input.GetButtonDown("Pause_P1"))
		{
			SceneManager.LoadScene ("EndGame");
		}
	}
}
