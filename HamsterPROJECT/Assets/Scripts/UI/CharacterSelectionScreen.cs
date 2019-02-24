using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using InControl;

public class CharacterSelectionScreen : MonoBehaviour {

	[SerializeField] public List<PlayerSelectionPanel> panels = new List<PlayerSelectionPanel>();
	public bool ready = false;
	int readyCount = 0;
	//Used to make sure all panels are checked before the players are considered all ready
	bool checkPanels = true;
	[SerializeField] string sceneToLoad;
	[SerializeField] GameObject readyText;
	bool allowValidation = false;
	public int activatedPlayers = 0;
	[SerializeField] string previousScene;

	// Dynamic selectable characters
	public static int nbCharactersAvailable;
	public static bool[] selectableCharacters;

	//Audio
	float delay = 0.1f;

	void Awake()
	{
		// Load Characters
		GameObject[] tab = Resources.LoadAll<GameObject> ("Prefabs");
		GameManager.Characters = new List<GameObject>(tab);
		nbCharactersAvailable = GameManager.nbOfCharacters;
		selectableCharacters = new bool[GameManager.nbOfCharacters];
		selectableCharacters.SetValue (true, 0);
		selectableCharacters.SetValue (true, 1);
		selectableCharacters.SetValue (true, 2);
		selectableCharacters.SetValue (true, 3);
		selectableCharacters.SetValue (true, 4);

        if (PlayerPrefs.GetInt("NotFirstTime") == 0)
            GameObject.Find("TutorialHint").SetActive(false);
    }

	void Start()
	{
		GameObject.Find ("UISource").GetComponent<AudioSource>().Stop ();
		MusicManager.instance.PlayMusic ("menu");
		#region DataRecovering
		for (int i = 0; i < GameManager.playersActive.Length; i++)
		{
			if (GameManager.playersActive[i] == true)
			{
				panels[i].state = PlayerSelectionPanel.SelectionPanelState.Activated;
				panels[i].GetComponent<PlayerSelectionPanel>().characterSelected = int.Parse(GameManager.playersCharacters[i].name);
				panels[i].GetComponent<PlayerSelectionPanel>().characterSprite.sprite = panels [i].GetComponent<PlayerSelectionPanel>().characterSprites[panels[i].characterSelected];
				panels[i].device = GameManager.playersInputDevices[i];
			}
		}
		#endregion
	}

	void FixedUpdate()
	{
		if (checkPanels)
		{
			checkPanels = false;
			readyText.SetActive (false);
			readyCount = 0;
			activatedPlayers = 0;

			foreach (PlayerSelectionPanel pan in panels)
			{
				if (pan.state == PlayerSelectionPanel.SelectionPanelState.Validated)
					readyCount++;
				if (pan.state == PlayerSelectionPanel.SelectionPanelState.Activated)
					activatedPlayers++;
			}

			// Conditions to start : at least two panels are validated
			// + no panel in the Activated state
			if (readyCount >= 2 && activatedPlayers == 0)
				ready = true;
			else
				ready = false;
			
			checkPanels = true;
		}

		if (ready)
		{
			readyText.SetActive (true);
			StartCoroutine (AvoidValidationWithSingleInput ());
			if (allowValidation)
			{
				foreach (InputDevice dev in InputManager.ActiveDevices)
				{
					//Load Game Modes Screen when any players pressed a command (start, select...)
					if (dev.Action1.WasPressed && ready)
					{
						PlayerInfos ();
						StartCoroutine(LoadGameModes ());
					}
				}
			}
		}

		foreach (InputDevice dev in InputManager.ActiveDevices)
		{
			if (dev.Action2.WasPressed
				&& previousScene != null && previousScene != "" 
				&& activatedPlayers == 0 
				&& readyCount == 0)
			{
				MusicManager.instance.StopMusic ("menu");
				StartCoroutine (LoadPreviousScene ());
			}

			if (dev.Action4.WasPressed)
			{
				SceneManager.LoadScene("Tutorial");
			}
		}

	}

	IEnumerator AvoidValidationWithSingleInput()
	{
		yield return new WaitForSeconds (0.2f);
		allowValidation = true;
	}

	void Update()
	{
		InputDevice inputDevice = InputManager.ActiveDevice;

		if (inputDevice.Action1.WasPressed)
		{
			if (ThereIsNoPanelUsingDevice( inputDevice))
			{
				ActivatePlayerSelectionPanel(inputDevice);
			}
		}
	}

	bool ThereIsNoPanelUsingDevice( InputDevice inputDevice )
	{
		return FindPanelUsingDevice( inputDevice ) == null;
	}

	PlayerSelectionPanel FindPanelUsingDevice( InputDevice inputDevice )
	{
		for (var i = 0; i < panels.Count; i++)
		{
			if (panels[i].device == inputDevice)
			{
				return panels[i];
			}
		}

		return null;
	}

	void ActivatePlayerSelectionPanel( InputDevice inputDevice )
	{
		for (var i = 0; i < GameManager.nbOfPlayers; i++)
		{
			if (panels [i].device == null)
			{
				panels [i].state = PlayerSelectionPanel.SelectionPanelState.Activated;
				panels [i].device = inputDevice;
				return;
			}
		}
	}

	#region Deactivating Panel when a device is detached
	void OnDeviceDetached( InputDevice inputDevice )
	{
		for (var i = 0; i < panels.Count; i++)
		{
			if (GameManager.playersInputDevices[i] == inputDevice)
			{
				GameManager.playersInputDevices[i] = null;
				panels [i].device = null;
				panels[i].state = PlayerSelectionPanel.SelectionPanelState.Deactivated;
			}
		}
	}
	#endregion

	// Writing playerInfos in the GameManager
	void PlayerInfos()
	{
		for (int i = 0; i < panels.Count; i++)
		{
			if (panels [i].state == PlayerSelectionPanel.SelectionPanelState.Validated)
			{
				GameManager.playersActive [i] = true;
				//Set selected characters
				GameManager.playersCharacters [i] = panels [i].GetComponent<PlayerSelectionPanel>().validatedCharacter;
				GameManager.playersCharacters [i].transform.GetChild (0).GetComponent<PlayerMovement> ().playerNumber = "_P" + (i + 1).ToString();
				GameManager.playersInputDevices [i] = panels [i].device;
			}
		}
	}

	IEnumerator LoadPreviousScene()
	{
		AudioManager.instance.PlaySound ("UI_cancel", "UI");
		yield return new WaitForSeconds (delay);
		SceneManager.LoadScene (previousScene);
	}

	IEnumerator LoadGameModes()
	{
		AudioManager.instance.PlaySound ("UI_validate", "UI");
		yield return new WaitForSeconds (delay);
		SceneManager.LoadScene (sceneToLoad);
	}
}
