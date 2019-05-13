using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InControl;
using UnityEngine.SceneManagement;
public class MatchStart : MonoBehaviour {

	// Ready/Fight countdown
	Text beginText;
	bool coroutineLimiter = false;
	bool activateBegin = false;
	public static bool gameHasStarted = false;

    GameObject progressBar;
	float timeAtBegin;

	// Players 
	[SerializeField] GameObject[] Players;
	string plyrNmbr;
    int howManyPlayers = 0;

    public List<Transform> spawnPoints = new List<Transform>();

	[SerializeField] public bool TestWithoutUI;

	GameObject beforeReadyHint;

    GameObject PlayerPrefab;

	void Awake ()
	{
        if (SceneManager.GetActiveScene().name == "Menu")
            GameManager.inMenu = true;
        else 
            GameManager.inMenu = false;

        PlayerPrefab = Resources.Load<GameObject>("Prefabs/PlayerPrefab");

		// Set all active Players to Alive (true) at beginning of match
		for (int i = 0; i < GameManager.playersActive.Length; i ++)
		{
			if (GameManager.playersActive [i] == true)
			{
                howManyPlayers++;
                GameManager.playersAlive [i] = true;
			}
		}

        if (!TestWithoutUI && !GameManager.inMenu)
        {
            beginText = GameObject.Find("BeginText").GetComponent<Text>();
            progressBar = GameObject.Find("ProgressBar");
            beforeReadyHint = GameObject.Find ("BeforeReadyText");
        }
    }

	void Start()
	{
        if (!GameManager.inMenu)
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
                    newPlayer.transform.GetChild (0).GetComponent<Rigidbody2D> ().isKinematic = false;
                    Destroy (Players [i].transform.parent.gameObject);
                    newPlayer.transform.GetChild (0).GetComponent<PlayerMovement> ().playerInputDevice = InputManager.Devices [i];
                }
            }

            else
            {
                if(howManyPlayers == 2)
                {
                    // Instantiate Players depending on which were validated in the character selection screen
                    for (int i = 0; i < GameManager.playersActive.Length; i++)
                    {
                        if (GameManager.playersActive[i] == true)
                        {
                            InstantiateTwoPlayers(i);
                        }
                    }
                }
                else
                {
                    // Instantiate Players depending on which were validated in the character selection screen
                    for (int i = 0; i < GameManager.playersActive.Length; i++)
                    {
                        if (GameManager.playersActive[i] == true)
                        {
                            InstantiatePlayer(i);
                        }
                    }
                }
            }
        }
		
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (!TestWithoutUI && !GameManager.inMenu)
        {
            foreach (InputDevice dev in InputManager.ActiveDevices)
            {
                if ((dev.AnyButtonWasPressed || dev.CommandWasPressed || dev.LeftTrigger.WasPressed || dev.RightTrigger.WasPressed) && !activateBegin)
                {
                    activateBegin = true;
                    beforeReadyHint.SetActive (false);
                    progressBar.GetComponent<Animator>().enabled = true;
                    AudioManager.instance.PlaySound ("UI_readyFight", "UI");
                }
            }

            if (activateBegin && !coroutineLimiter)
            {
                timeAtBegin = Time.time;
                StartCoroutine (BeginCount ());
                coroutineLimiter = true;
            }
        }
        else
            gameHasStarted = true;
	}

    void InstantiateTwoPlayers(int playerIndex)
    {
		// GameObject inst = GameManager.playersCharacters[playerIndex];
        GameObject inst = Instantiate(PlayerPrefab);

		// Spawn points management
        int j = Random.Range(0, spawnPoints.Count);
        inst.transform.position = spawnPoints[j].transform.position;
        if(playerIndex == 0)
        {
            Transform currentSpawn = spawnPoints[j];
            switch (currentSpawn.name)
            {
                case "SpawnPoint1":
                    for (int i = 0; i < spawnPoints.Count; i++)
                    {
                        if (spawnPoints[i].name == "SpawnPoint2")
                            spawnPoints.Remove(spawnPoints[i]);
                    }
                    break;
                case "SpawnPoint2":
                    for (int i = 0; i < spawnPoints.Count; i++)
                    {
                        switch (spawnPoints[i].name)
                        {
                            case "SpawnPoint1":
                                spawnPoints.Remove(spawnPoints[i]);
                                break;
                            case "SpawnPoint3":
                                spawnPoints.Remove(spawnPoints[i]);
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case "SpawnPoint3":
                    for (int i = 0; i < spawnPoints.Count; i++)
                    {
                        switch (spawnPoints[i].name)
                        {
                            case "SpawnPoint2":
                                spawnPoints.Remove(spawnPoints[i]);
                                break;
                            case "SpawnPoint4":
                                spawnPoints.Remove(spawnPoints[i]);
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case "SpawnPoint4":
                    for(int i = 0; i< spawnPoints.Count; i++)
                    {
                        if (spawnPoints[i].name == "SpawnPoint3")
                            spawnPoints.Remove(spawnPoints[i]);
                    }
                    break;
                default:
                    break;
            }
            spawnPoints.Remove(currentSpawn);
        }

        //Set sprite, name and player number
        inst.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = GameManager.playersSprites[playerIndex];
        inst.name = GameManager.playersSprites[playerIndex].name;
        inst.transform.GetChild(0).GetComponent<PlayerMovement> ().playerNumber = GameManager.playersNumbers[playerIndex];
        inst.transform.GetChild(1).GetComponent<Hook>().inverseRetractation = GameManager.playersTractConfig[playerIndex];
        inst.transform.GetChild(0).GetComponent<Rigidbody2D>().isKinematic = true;
    
        inst.transform.GetChild(0).GetComponent<PlayerMovement>().playerInputDevice = GameManager.playersInputDevices[playerIndex];
    }

    void InstantiatePlayer(int playerIndex)
    {
		// GameObject inst = GameManager.playersCharacters[playerIndex];
        GameObject inst = Instantiate(PlayerPrefab);

        //Set sprite, name and player number
        inst.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = GameManager.playersSprites[playerIndex];
        inst.name = GameManager.playersSprites[playerIndex].name;
        inst.transform.GetChild(0).GetComponent<PlayerMovement> ().playerNumber = GameManager.playersNumbers[playerIndex];
        inst.transform.GetChild(1).GetComponent<Hook>().inverseRetractation = GameManager.playersTractConfig[playerIndex];
        int j = Random.Range(0, spawnPoints.Count);
        inst.transform.position = spawnPoints[j].transform.position;
        spawnPoints.Remove(spawnPoints[j]);
        inst.transform.GetChild(0).GetComponent<Rigidbody2D>().isKinematic = true;
        inst.transform.GetChild(0).GetComponent<PlayerMovement>().playerInputDevice = GameManager.playersInputDevices[playerIndex];
    }

	IEnumerator BeginCount()
	{
		beginText.text = "Ready ?";
		yield return new WaitForSeconds (1.0f);
		beginText.text = "Fight !";
		UnfreezePlayers ();
		gameHasStarted = true;
		MusicManager.instance.PlayMusic ("battle");
		yield return new WaitForSeconds (0.5f);
		beginText.text = "";
	}

	void UnfreezePlayers()
	{
		Players = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject player in Players)
		{
			player.GetComponent<Rigidbody2D> ().isKinematic = false;
		}
	}

}