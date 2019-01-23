using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using InControl;

public class MatchEnd : MonoBehaviour {

//	[SerializeField] float delayBeforeEndOfGame;
	int nbPlayersAlive;
	[SerializeField] GameObject scoreDisplay;
	public GameObject[] arrows;
	int winner;
	bool gameOver = false;
	int nbPlayersActive;
	public static bool matchEnded = false;

	// Used to count number of players that reached the number of kills required to win
	int count = 0;

	bool arrowSet = false;

	// Audio
	AudioManager mngr;

	// Use this for initialization
	void Start ()
	{
		mngr = FindObjectOfType<AudioManager> ();
		scoreDisplay.SetActive (false);
		matchEnded = false;
		// default value, stays at 42 if nobody won
		// 0, 1, 2 and 3 are reserved to players
		winner = 42;
	}

	// Update is called once per frame
	void Update ()
	{
		// Count number of players in game
		nbPlayersAlive = 0;
		for (int i = 0; i < GameManager.playersAlive.Length; i++)
		{
			if (GameManager.playersAlive [i] == true)
				nbPlayersAlive++;
		}

		for (int i = 0; i < GameManager.playersActive.Length; i++)
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

		// Stop the game if one player reached the targeted number of kills
		if (GameManager.gameModeType == GameManager.gameModes.Kills && 
			System.Array.IndexOf(GameManager.playersScores, GameManager.goal) != -1)
		{
			StartCoroutine(DisplayScore ());
		}
	}

	IEnumerator DisplayScore()
	{
		matchEnded = true;
		yield return new WaitForSeconds (1f);
		FreezeGame ();
		scoreDisplay.SetActive (true);
		mngr.PlaySound ("UI_matchEnd", mngr.UIsource);

		yield return new WaitForSeconds (1f);

		if (GameManager.gameModeType == GameManager.gameModes.LastManStanding)
		{
			// Keep playing if nobody reached the game goal
			if (InputManager.ActiveDevice.Action1 && GameManager.playersScores[winner] < GameManager.goal )
			{
				// Reload Scene
				SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
			}

			// Stop playing if one player reached the game goal
			if (InputManager.ActiveDevice.Action1 && GameManager.playersScores[winner] == GameManager.goal )
			{
				EndOfMatch ();
			}
		}
			
		if (GameManager.gameModeType == GameManager.gameModes.Kills && InputManager.ActiveDevice.Action1)
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
				EndOfMatch ();
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

	// Instruction for when the match is over
	void EndOfMatch()
	{
		GameManager.lastBattleMap = SceneManager.GetActiveScene ().name;
		mngr.PlaySound ("UI_matchEndValidation", mngr.UIsource);
		SceneManager.LoadScene ("Results");
	}
}
