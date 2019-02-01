﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour {

	public static LevelSelection instance = null;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
		else 
		{
			Destroy (gameObject);
		}
	}

	[SerializeField] List<string> pack = new List<string>();
	List<string> currentPack = new List<string>();
	List<string> levels = new List<string>();
	string packName;
	string lastLevelPlayed;
	string levelToLoad;

	void Start()
	{
		// Set current pack to base pack
		currentPack = pack;
		packName = "P1";
//		// Add pack levels to levels pool
//		foreach (string level in currentPack)
//		{
//			levels.Add (level);
//		}
	}

	void SelectPack()
	{
		//TODO : For future versions that will propose different set of levels
		//As of prototype Alpha version, only one pack is avaliable
	}

	public void LoadNextLevel(string currentLevel)
	{
		//Reset levels to all levels available in pack
		levels.Clear ();
		for (int i = 0; i < currentPack.Count; i ++)
		{
			levels.Add (currentPack[i]);
		}

		//Remove last level played from last series of rounds to avoid starting a new series of rounds with the same level
		if (GameManager.lastLevelPlayed != null)
		{
			levels.Remove (GameManager.lastLevelPlayed);
		}

		//Removing current level played to avoid direct repetition of levels
		if (currentLevel != null && currentLevel != "")
		{
			levels.Remove(currentLevel);
		}

		//Choosing next level to load from the remaining levels
		levelToLoad = levels[Random.Range (0, levels.Count)];

		//Loading next scene
		if (levelToLoad != null)
		{
			if (Time.timeScale != 1) Time.timeScale = 1;
			SceneManager.LoadScene (levelToLoad);
		}

		else 
		{
			print ("Trying to load next level - no level found");
		}

	}
}
