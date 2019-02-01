using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelection : MonoBehaviour {

	public static AudioManager instance = null;

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

	// Pool of levels
	[SerializeField] List<string> pool = new List<string>();
	int rounds;
	string lastLevelPlayed;

	void Update () 
	{
		
	}
}
