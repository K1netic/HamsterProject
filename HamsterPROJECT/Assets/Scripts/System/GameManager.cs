﻿using System.Collections;
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
		Deathmatch
	}

	public static int nbOfPlayers = 4;
	public static int nbOfCharacters = 5;

	public static gameModes gameModeType;
		
	// Numbers of matches won to win a full game
	public static int rounds = 5;
		
	public static bool[] playersActive = new bool[nbOfPlayers];
	public static bool[] playersAlive = new bool[nbOfPlayers];

	public static Sprite[] playersSprites = new Sprite[nbOfPlayers];
	public static string[] playersNumbers = new string[nbOfPlayers];
	public static bool[] playersTractConfig = new bool[nbOfPlayers];
	public static InputDevice[] playersInputDevices = new InputDevice[nbOfPlayers];

	//Players metrics
	public static int[] playersScores = new int[nbOfPlayers];
	public static int[] playersKills = new int[nbOfPlayers];
	public static int[] playersDeaths = new int[nbOfPlayers];
	public static int[] playersSelfDestructs = new int[nbOfPlayers];


	public static string lastLevelPlayed;

	public static bool inMenu;

	public static float delayMenu = 0.25f;

	public static bool firstLaunch = true;

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
			playersSelfDestructs [i] = 0;
		}
	}

	public static void ClearData()
	{
		for (int i = 0; i < nbOfPlayers; i ++)
		{
			playersActive[i] = false;
			playersAlive[i] = false;
			playersSprites[i] = null;
			playersNumbers[i] = null;
			playersTractConfig[i] = false;
			playersInputDevices[i] = null;
		}
	}

    public static int HowManyPlayersPlaying()
    {
        int res = 0;
        for (int i = 0; i < GameManager.playersActive.Length; i++)
        {
            if (GameManager.playersActive[i] == true)
            {
                res++;
            }
        }
        return res;
    }

    public static int HowManyPlayersAlive()
    {
        int res = 0;
        for (int i = 0; i < GameManager.playersAlive.Length; i++)
        {
            if (GameManager.playersAlive[i] == true)
            {
                res++;
            }
        }
        return res;
    }
}
