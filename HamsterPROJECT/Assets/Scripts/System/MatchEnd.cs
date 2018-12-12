using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MatchEnd : MonoBehaviour {

//	[SerializeField] float delayBeforeEndOfGame;
	int nbPlayersAlive;
	[SerializeField] GameObject scoreDisplay;
	int winner;
	bool gameOver = false;
 
	// Use this for initialization
	void Start () {
		scoreDisplay.SetActive (false);
		// default value, stays at 42 if nobody won
		// 0, 1, 2 and 3 are reserved to players
		winner = 42;
	}
	
	// Update is called once per frame
	void Update () {

		// Count remaining players
		nbPlayersAlive = 0;
		for (int i = 0; i < GameManager.playersAlive.Length; i ++)
		{
			if (GameManager.playersAlive [i] == true)
				nbPlayersAlive++;
		}

		// One or less players remaining
		if (nbPlayersAlive <= 1)
		{
			if (GameManager.gameModeType == GameManager.gameModes.LastManStanding && !gameOver)
			{
				gameOver = true;
				// Default value in case there was no remaining player
				// Winner determination
				for (int i = 0; i < GameManager.playersAlive.Length; i ++)
				{
					if (GameManager.playersAlive [i] == true)
					{
						winner = i;
					}
				}

				// if at least one player won 
				if (winner != 42)
				{
					GameManager.playersScores [winner] += 1;
				}
			}

			StartCoroutine(DisplayScore ());
		}
	}

	IEnumerator DisplayScore()
	{
		yield return new WaitForSeconds (1f);
		scoreDisplay.SetActive (true);
		yield return new WaitForSeconds (1f);

		// Keep playing if nobody reached the game goal
		if (Input.GetButton ("Submit_P1") && GameManager.playersScores[winner] < GameManager.goal )
		{
			// Reload Scene
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}

		// Stop playing if one player reached the game goal
		if (Input.GetButton ("Submit_P1") && GameManager.playersScores[winner] == GameManager.goal )
		{
			GameManager.ResetScore ();
			SceneManager.LoadScene ("EndGame");
		}
	}
}
