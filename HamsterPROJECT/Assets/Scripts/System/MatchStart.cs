using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchStart : MonoBehaviour {

	//Players Prefabs
	[SerializeField] GameObject P1;
	[SerializeField] GameObject P2;
	[SerializeField] GameObject P3;
	[SerializeField] GameObject P4;
//	[SerializeField] Transform[] spawnPoints;
	[SerializeField] Transform spawnPoint1;
	[SerializeField] Transform spawnPoint2;
	[SerializeField] Transform spawnPoint3;
	[SerializeField] Transform spawnPoint4;

	// Ready/Fight countdown
	Text beginText;
	bool coroutineLimiter = false;
	bool activateBegin = false;
	public static bool gameHasStarted = false;
	[SerializeField] GameObject[] players;
	[SerializeField] Texture2D emptyProgressBar;
	[SerializeField] Texture2D fullProgressBar;
	float timeAtBegin;

	void Awake () 
	{
		// Set all active players to Alive (true) at beginning of match
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
		// Instantiate players depending on which were validated in the character selection screen
		for (int i = 0; i < GameManager.playersActive.Length; i ++)
		{
			if (GameManager.playersActive[i] == true)
			{
				Invoke("Instantiate_P" + (i + 1).ToString() ,0);
			}
		}
//		FreezePlayers ();
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
		if (activateBegin && !gameHasStarted)
		{
			GUI.DrawTexture (new Rect (1920f/2f - 250, 700, 500, 50), emptyProgressBar);
			GUI.DrawTexture (new Rect (1920f/2f - 250 , 700, 500 * (Time.time - timeAtBegin), 50), fullProgressBar);
		}
	}

	IEnumerator BeginCount()
	{
		
//		beginText.text = "3";
//		yield return new WaitForSeconds (1.0f);
//		beginText.text = "2";
//		yield return new WaitForSeconds (1.0f);
		beginText.text = "Ready ?";
		yield return new WaitForSeconds (1.0f);
		beginText.text = "Fight !";
//		GUIElement.Destroy()
		UnfreezePlayers ();
		gameHasStarted = true;
		yield return new WaitForSeconds (0.5f);
		beginText.text = "";
	}

	void Instantiate_P1()
	{
		P1.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite> ("CharacterSprites/InGame/" + GameManager.playersSprites[0].ToString ());
		P1.transform.position = spawnPoint1.transform.position;
		P1.transform.GetChild(0).GetComponent<Rigidbody2D> ().isKinematic = true;
		Instantiate (P1);
	}

	void Instantiate_P2()
	{
		P2.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite> ("CharacterSprites/InGame/" + GameManager.playersSprites[1].ToString ());
		P2.transform.position = spawnPoint2.transform.position;
		P2.transform.GetChild(0).GetComponent<Rigidbody2D> ().isKinematic = true;
		Instantiate (P2);
	}

	void Instantiate_P3()
	{
		P3.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite> ("CharacterSprites/InGame/" + GameManager.playersSprites[2].ToString ());
		P3.transform.position = spawnPoint3.transform.position;
		P3.transform.GetChild(0).GetComponent<Rigidbody2D> ().isKinematic = true;
		Instantiate (P3);
	}

	void Instantiate_P4()
	{
		P4.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite> ("CharacterSprites/InGame/" + GameManager.playersSprites[3].ToString ());
		P4.transform.position = spawnPoint4.transform.position;
		P4.transform.GetChild(0).GetComponent<Rigidbody2D> ().isKinematic = true;
		Instantiate (P4);
	}

//	void FreezePlayers()
//	{
//		foreach (GameObject player in players)
//		{
//			Debug.Log ("freezing player");
//			player.GetComponent<Rigidbody2D> ().isKinematic = true;
//		}
//	}

	void UnfreezePlayers()
	{
		players = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject player in players)
		{
			player.GetComponent<Rigidbody2D> ().isKinematic = false;
		}
	}

//	void RandomSpawnPoint()
//	{
//		Random.Range (1, spawnPoints.Length);
//	}

}