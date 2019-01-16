﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchStart : MonoBehaviour {

	// Ready/Fight countdown
	Text beginText;
	bool coroutineLimiter = false;
	bool activateBegin = false;
	public static bool gameHasStarted = false;
	[SerializeField] Texture2D emptyProgressBar;
	[SerializeField] Texture2D fullProgressBar;
	float timeAtBegin;

	// Players 
	[SerializeField] GameObject[] Players;
	string plyrNmbr;

	[SerializeField] Transform[] spawnPoints;

	[SerializeField] GameObject[] LifeBars;

	void Awake ()
	{
		// Set all active Players to Alive (true) at beginning of match
		for (int i = 0; i < GameManager.playersActive.Length; i ++)
		{
			if (GameManager.playersActive [i] == true)
			{
				GameManager.playersAlive [i] = true;
			}
		}

		beginText = GameObject.Find("BeginText").GetComponent<Text>();

		ShowLifeBars ();
	}

	void Start()
	{
		gameHasStarted = false;
		coroutineLimiter = false;
		// Instantiate Players depending on which were validated in the character selection screen
		for (int i = 0; i < GameManager.playersActive.Length; i ++)
		{
			if (GameManager.playersActive[i] == true)
			{
				InstantiatePlayer (i);
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.anyKeyDown)
		{
			activateBegin = true;
		}

		if (activateBegin && !coroutineLimiter)
		{
			timeAtBegin = Time.time;
			StartCoroutine (BeginCount ());
			coroutineLimiter = true;
		}
	}

	void OnGUI()
	{
		// Draw progress bar
		if (activateBegin && !gameHasStarted)
		{
			GUI.DrawTexture (new Rect (1920f/2f - 250, 700, 500, 50), emptyProgressBar);
			GUI.DrawTexture (new Rect (1920f/2f - 250 , 700, 500 * (Time.time - timeAtBegin), 50), fullProgressBar);
		}
	}

	IEnumerator BeginCount()
	{
		beginText.text = "Ready ?";
		yield return new WaitForSeconds (1.0f);
		beginText.text = "Fight !";
		UnfreezePlayers ();
		gameHasStarted = true;
		yield return new WaitForSeconds (0.5f);
		beginText.text = "";
	}

	void InstantiatePlayer(int playerIndex)
	{
		plyrNmbr = "_P" + (playerIndex + 1).ToString ();
		GameObject inst = GameManager.playersCharacters[playerIndex];
		inst.transform.position = spawnPoints[playerIndex].transform.position;
		inst.transform.GetChild (0).GetComponent<PlayerMovement> ().playerNumber = plyrNmbr;
		inst.transform.GetChild (0).GetComponent<Rigidbody2D> ().isKinematic = true;
		Instantiate (inst);
	}

	void UnfreezePlayers()
	{
		Players = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject player in Players)
		{
			player.GetComponent<Rigidbody2D> ().isKinematic = false;
		}
	}

	void ShowLifeBars()
	{
		for(int i = 0; i < GameManager.playersActive.Length; i++)
		{
			if (GameManager.playersActive [i])
				LifeBars [i].SetActive (true);
		}
	}

//	void RandomSpawnPoint()
//	{
//		Random.Range (1, spawnPoints.Length);
//	}

}