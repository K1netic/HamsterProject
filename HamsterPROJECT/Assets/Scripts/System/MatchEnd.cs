using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MatchEnd : MonoBehaviour {

//	[SerializeField] float delayBeforeEndOfGame;
	int nbPlayersAlive;
	[SerializeField] GameObject scoreDisplay;
	public GameObject[] arrows;
	int winner;
	bool gameOver = false;
	int nbPlayersActive;

	// Used to count number of players that reached the number of kills required to win
	int count = 0;

	bool arrowSet = false;
 
	// Use this for initialization
	void Start () {
		scoreDisplay.SetActive (false);
		// default value, stays at 42 if nobody won
		// 0, 1, 2 and 3 are reserved to players
		winner = 42;
	}
		
	// Update is called once per frame
	void Update () {

		// Count number of players in game
		nbPlayersAlive = 0;
		for (int i = 0; i < GameManager.playersAlive.Length; i ++)
		{
			if (GameManager.playersAlive [i] == true)
				nbPlayersAlive++;
		}

		for (int i = 0; i < GameManager.playersActive.Length; i ++)
		{
			if (GameManager.playersActive [i] == true)
				nbPlayersActive++;
		}

		// One or less players remaining
		if (nbPlayersAlive <= 1 && nbPlayersActive > 0 )
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

//		if (GameManager.gameModeType == GameManager.gameModes.Kills && System.Array.IndexOf(GameManager.playersScores, GameManager.goal) != null)
			


		// Add a brick to stop the game if one player reached the targeted number of kills
	}

	IEnumerator DisplayScore()
	{
		yield return new WaitForSeconds (1f);
		FreezeGame ();
		scoreDisplay.SetActive (true);

		yield return new WaitForSeconds (1f);

		if (GameManager.gameModeType == GameManager.gameModes.LastManStanding)
		{
			// Keep playing if nobody reached the game goal
			if (Input.GetButton ("Submit_P1") && GameManager.playersScores[winner] < GameManager.goal )
			{
				// Reload Scene
				SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
			}

			// Stop playing if one player reached the game goal
			if (Input.GetButton ("Submit_P1") && GameManager.playersScores[winner] == GameManager.goal )
			{
//				GameManager.ResetScore ();
				SceneManager.LoadScene ("Results");
			}
		}
			
		if (GameManager.gameModeType == GameManager.gameModes.Kills && Input.GetButton("Submit_P1"))
		{
			for (int i = 0; i < GameManager.playersActive.Length; i ++)
			{
				if (GameManager.playersScores [i] >= GameManager.goal )
				{
					count++;
				}
			}

			// if at least one player reached the goal...
			if (count > 0)
			{
//				GameManager.ResetScore ();
				SceneManager.LoadScene ("Results");
			}
			else 
			{
				SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
			}
		}
	}

	void FreezeGame()
	{
		if (!arrowSet)
		{
			arrowSet = true;
			arrows = GameObject.FindGameObjectsWithTag ("Arrow");

			foreach (GameObject arrow in arrows)
			{
				if (arrow != null)
				{
					arrow.GetComponent<Hook> ().isFrozen = true;
					arrow.transform.parent.GetChild (0).GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Static;
				}
			}
		}
	}
}
