using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InControl;

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

	[SerializeField] List<Transform> spawnPoints = new List<Transform>();

	[SerializeField] bool TestWithoutUI;

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


	}

	void Start()
	{
		gameHasStarted = false;
		coroutineLimiter = false;

        foreach (GameObject spawn in GameObject.FindGameObjectsWithTag("Spawn"))
        {
            spawnPoints.Add(spawn.transform);
        }

		if (TestWithoutUI)
		{
			Players = GameObject.FindGameObjectsWithTag("Player");

			for (int i = 0; i < Players.Length; i ++)
			{
				GameObject newPlayer = Instantiate (Players [i].transform.parent.gameObject);
				newPlayer.transform.position = Players[i].transform.position;
				newPlayer.transform.GetChild (0).GetComponent<Rigidbody2D> ().isKinematic = true;
				Destroy (Players [i].transform.parent.gameObject);
				newPlayer.transform.GetChild (0).GetComponent<PlayerMovement> ().playerInputDevice = InputManager.Devices [i];
			}
		}

		else
		{
			// Instantiate Players depending on which were validated in the character selection screen
			for (int i = 0; i < GameManager.playersActive.Length; i ++)
			{
				if (GameManager.playersActive[i] == true)
				{
					InstantiatePlayer (i);
				}
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
		GameObject inst = GameManager.playersCharacters[playerIndex];
		int j = Random.Range(0, spawnPoints.Count);
		inst.transform.position = spawnPoints[j].transform.position;
		spawnPoints.Remove(spawnPoints[j]);
		inst.transform.GetChild (0).GetComponent<Rigidbody2D> ().isKinematic = true;
		GameObject newPlayer = Instantiate (inst);

		// Setting InputDevice
		newPlayer.transform.GetChild (0).GetComponent<PlayerMovement> ().playerInputDevice = GameManager.playersInputDevices [playerIndex];
	}

	void UnfreezePlayers()
	{
		Players = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject player in Players)
		{
			player.GetComponent<Rigidbody2D> ().isKinematic = false;
		}
	}

//	void ShowLifeBars()
//	{
//		for(int i = 0; i < GameManager.playersActive.Length; i++)
//		{
//			if (GameManager.playersActive [i])
//				LifeBars [i].SetActive (true);
//		}
//	}

//	void RandomSpawnPoint()
//	{
//		Random.Range (1, spawnPoints.Length);
//	}

}