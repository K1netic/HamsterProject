using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	public static int goal;
		
	public static bool[] playersActive = new bool[4];
	public static int[] playersSprites = new int[4];
	public static bool[] playersAlive = new bool[4];
	public static int[] playersScores = new int[4];

	void Awake()
	{
		// Don't destroy the game manager when reloading/changing scene
		DontDestroyOnLoad (gameObject);
		playersActive.SetValue (false, 0);
		playersActive.SetValue (false, 1);
		playersActive.SetValue (false, 2);
		playersActive.SetValue (false, 3);
	}

	//Score

}
