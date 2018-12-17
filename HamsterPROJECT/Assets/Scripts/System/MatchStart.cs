using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	void Awake () {
		// Set all active players to Alive (true) at beginning of match
		for (int i = 0; i < GameManager.playersActive.Length; i ++)
		{
			if (GameManager.playersActive [i] == true)
				GameManager.playersAlive [i] = true;
		}
	}

	void Start()
	{
		// Instantiate players depending on which were validated in the character selection screen
		for (int i = 0; i < GameManager.playersActive.Length; i ++)
		{
			if (GameManager.playersActive[i] == true)
			{
				Invoke("Instantiate_P" + (i + 1).ToString() ,0);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		// Gérer la déconnexion de manette
	}

	void Instantiate_P1()
	{
		P1.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite> ("CharacterSprites/InGame/" + GameManager.playersSprites[0].ToString ());
		P1.transform.position = spawnPoint1.position;
		Instantiate (P1, spawnPoint1);
	}

	void Instantiate_P2()
	{
		P2.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite> ("CharacterSprites/InGame/" + GameManager.playersSprites[1].ToString ());
		P2.transform.position = spawnPoint2.position;
		Instantiate (P2, spawnPoint2);
	}

	void Instantiate_P3()
	{
		P3.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite> ("CharacterSprites/InGame/" + GameManager.playersSprites[2].ToString ());
		P3.transform.position = spawnPoint3.position;
		Instantiate (P3, spawnPoint3);
	}

	void Instantiate_P4()
	{
		P4.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite> ("CharacterSprites/InGame/" + GameManager.playersSprites[3].ToString ());
		P4.transform.position = spawnPoint4.position;
		Instantiate (P4, spawnPoint4);
	}

//	void RandomSpawnPoint()
//	{
//		Random.Range (1, spawnPoints.Length);
//	}

}