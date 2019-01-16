﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

	public static gameModes gameModeType;
		
	// Numbers of matches won to win a full game
	public static int goal = 1;
		
	public static bool[] playersActive = new bool[4];
	public static bool[] playersAlive = new bool[4];
	//Base characters
	public static List<GameObject> Characters = new List<GameObject>();
	//Characters selected by players
	public static GameObject[] playersCharacters = new GameObject[4];

	//Players metrics
	public static int[] playersScores = new int[4];
	public static int[] playersKills = new int[4];
	public static int[] playersDeaths = new int[4];

	public static string lastBattleMap;

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
		for (int i = 0; i < GameManager.playersScores.Length; i ++)
		{
			GameManager.playersScores [i] = 0;
			GameManager.playersKills [i] = 0;
			GameManager.playersScores [i] = 0;
		}
	}
}
