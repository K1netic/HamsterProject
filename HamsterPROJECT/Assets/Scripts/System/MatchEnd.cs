using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using InControl;

public class MatchEnd : MonoBehaviour {

//	[SerializeField] float delayBeforeEndOfGame;
	int nbPlayersAlive;
	GameObject scoreDisplay;
	[HideInInspector] public GameObject[] arrows;
	int winner;
	bool gameOver = false;
	int nbPlayersActive;
	public static bool matchEnded = false;

	// Used to count number of players that reached the number of kills required to win
	int count = 0;

	bool arrowSet = false;

	// Audio
	MusicManager music;

	LevelSelection lvlSelect;
	MatchStart mtchstrt;

    private void Awake()
    {
			music = GameObject.FindObjectOfType<MusicManager> ();
			mtchstrt = GameObject.FindObjectOfType<MatchStart>();
    }

    // Use this for initialization
    void OnEnable ()
		{
            //mngr = FindObjectOfType<AudioManager> ();
			matchEnded = false;
			// default value, stays at 42 if nobody won
			// 0, 1, 2 and 3 are reserved to players
			winner = 42;
			lvlSelect = FindObjectOfType<LevelSelection> ();
		}

    private void Start()
    {
        if (!mtchstrt.TestWithoutUI && !GameManager.inMenu)
        {
            scoreDisplay = GameObject.Find("ScoreDisplayer");
            scoreDisplay.SetActive(false);
        }
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
		if (GameManager.gameModeType == GameManager.gameModes.Deathmatch && 
			System.Array.IndexOf(GameManager.playersScores, GameManager.rounds) != -1)
		{
			music.StopMusic ("battle");
			StartCoroutine(DisplayScore ());
		}
	}

	IEnumerator DisplayScore()
	{
		matchEnded = true;
		yield return new WaitForSeconds (1f);
		FreezeGame ();
		scoreDisplay.SetActive (true);
		//mngr.PlaySound ("UI_matchEnd", //mngr.UIsource);

		yield return new WaitForSeconds (0.5f);

		if (GameManager.gameModeType == GameManager.gameModes.LastManStanding)
		{
			// Keep playing if nobody reached the game goal
			foreach (InputDevice dev in InputManager.Devices)
			{
				if ((dev.Action1.WasPressed && GameManager.playersScores[winner] < GameManager.rounds) || winner == 42)
				{
					GameManager.lastLevelPlayed = "";
					lvlSelect.LoadNextLevel(SceneManager.GetActiveScene ().name);
				}

				// Stop playing if one player reached the game goal
				if (dev.Action1.WasPressed && GameManager.playersScores[winner] == GameManager.rounds )
				{
					EndOfMatch ();
				}
			}

		}
			
		if (GameManager.gameModeType == GameManager.gameModes.Deathmatch && InputManager.ActiveDevice.Action1)
		{
			for (int i = 0; i < GameManager.playersActive.Length; i ++)
			{
				if (GameManager.playersScores [i] >= GameManager.rounds )
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
				GameManager.lastLevelPlayed = "";
				lvlSelect.LoadNextLevel(SceneManager.GetActiveScene ().name);
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
		GameManager.lastLevelPlayed = SceneManager.GetActiveScene ().name;
		ScreenManager.screenToInitiallyOpen = "ResultsPanel";
		SceneManager.LoadScene ("Menu");
	}
}
