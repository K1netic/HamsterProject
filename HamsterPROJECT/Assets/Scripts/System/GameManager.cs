using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using InControl;

public class GameManager : MonoBehaviour {

	private static GameManager instance;

	public static GameManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = GameObject.FindObjectOfType<GameManager> ();
			}

			return instance;
		}
	}

	public enum gameModes
	{
		LastManStanding,
		Kills
	}

	public static int nbOfPlayers = 4;
	public static int nbOfCharacters = 5;

	public static gameModes gameModeType;
		
	// Numbers of matches won to win a full game
	public static int rounds = 5;
		
	public static bool[] playersActive = new bool[nbOfPlayers];
	public static bool[] playersAlive = new bool[nbOfPlayers];
	//Base characters
	public static List<GameObject> Characters = new List<GameObject>();
	//Characters selected by players
	public static GameObject[] playersCharacters = new GameObject[nbOfPlayers];

	//Players metrics
	public static int[] playersScores = new int[nbOfPlayers];
	public static int[] playersKills = new int[nbOfPlayers];
	public static int[] playersDeaths = new int[nbOfPlayers];
	public static int[] playersSelfDestructs = new int[nbOfPlayers];

	public static InputDevice[] playersInputDevices = new InputDevice[nbOfPlayers];

	public static string lastLevelPlayed;

	void Awake()
	{
		// Don't destroy the game manager when reloading/changing scene
		DontDestroyOnLoad (gameObject);
		playersActive.SetValue (false, 0);
		playersActive.SetValue (false, 1);
		playersActive.SetValue (false, 2);
		playersActive.SetValue (false, 3);
	}

	public static void ResetScores()
	{
		for (int i = 0; i < playersScores.Length; i ++)
		{
			playersScores [i] = 0;
			playersKills [i] = 0;
			playersScores [i] = 0;
			playersDeaths [i] = 0;
		}
	}
}
