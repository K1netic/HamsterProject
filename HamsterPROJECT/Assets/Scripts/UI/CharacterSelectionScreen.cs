using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using InControl;
using UnityEditor;
using UnityEngine.EventSystems;

public class CharacterSelectionScreen : MonoBehaviour {

	[SerializeField] public List<PlayerSelectionPanel> panels = new List<PlayerSelectionPanel>();
	public bool ready = false;
	int readyCount = 0;
	//Used to make sure all panels are checked before the players are considered all ready
	bool checkPanels = true;
	[SerializeField] GameObject readyText;
	public int activatedPlayers = 0;

	// Dynamic selectable characters
	public static bool[] selectableCharacters;
	GameObject characterPrefab;

	[SerializeField] Material spriteDefault;

	ScreenManager screenManager;
	[SerializeField] Animator previousScreenAnimator;
	[SerializeField] Animator nextScreenAnimator;
	[SerializeField] Animator gamesModesScreenAnimator;
	Animator characterScreenAnimator;

	[SerializeField] public GameObject system;

	ActivateInput inputActivationScript;

	void Awake()
	{
		characterPrefab = Resources.Load<GameObject> ("Prefabs/PlayerPrefab");

        // if (PlayerPrefs.GetInt("NotFirstTime") == 0)
        //     GameObject.Find("TutorialHint").SetActive(false);
    }

	void Start()
	{
		// GameObject.Find ("UISource").GetComponent<AudioSource>().Stop ();
		// MusicManager.instance.PlayMusic ("menu");
		screenManager = FindObjectOfType<ScreenManager> ();
		characterScreenAnimator = GameObject.Find ("CharacterSelectionPanel").gameObject.GetComponent<Animator> ();
		inputActivationScript = transform.parent.GetComponent<ActivateInput>();
		MusicManager.instance.PlayMusic("menu");
	}

	void OnEnable()
	{
		selectableCharacters = new bool[GameManager.nbOfCharacters];
		selectableCharacters.SetValue (true, 0);
		selectableCharacters.SetValue (true, 1);
		selectableCharacters.SetValue (true, 2);
		selectableCharacters.SetValue (true, 3);
		selectableCharacters.SetValue (true, 4);

		#region DataRecovering
		for (int i = 0; i < GameManager.playersActive.Length; i++)
		{
			if (GameManager.playersActive[i] == true)
			{
				panels[i].state = PlayerSelectionPanel.SelectionPanelState.Activated;
				panels[i].GetComponent<PlayerSelectionPanel>().characterSelected = int.Parse(GameManager.playersSprites[i].name);
				panels[i].GetComponent<PlayerSelectionPanel>().characterSprite.sprite = panels [i].GetComponent<PlayerSelectionPanel>().characterSprites[panels[i].characterSelected];
				panels[i].device = GameManager.playersInputDevices[i];
			}
		}
		#endregion

		// EventSystem.current.SetSelectedGameObject(null);
	}

	void Update()
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

		if (readyCount >= 1)
		{
			system.GetComponentInChildren<MatchStart>().enabled = true;
			system.GetComponentInChildren<MatchEnd>().enabled = false;
			system.GetComponentInChildren<PauseMenu>().enabled = false;
			system.GetComponentInChildren<FeedbacksOnDeath>().enabled = false;
			system.SetActive(true);
		}

		else if (readyCount == 0)
		{
			system.SetActive(false);
		}

		if (ready)
		{
			readyText.SetActive (true);
			foreach (InputDevice dev in InputManager.ActiveDevices)
			{
				//Open Next Screen when LB and RB are pressed simulatenously
				if (dev.LeftBumper.IsPressed && dev.RightBumper.IsPressed && ready)
				{
					PlayerInfos ();
					AudioManager.instance.PlaySound ("UI_validate", "UI");
					screenManager.OpenPanel (nextScreenAnimator);
				}
			}
		}

		foreach (InputDevice dev in InputManager.ActiveDevices)
		{
			if (dev.Action2.WasPressed
				&& activatedPlayers == 0 
				&& readyCount == 0)
			{
				AudioManager.instance.PlaySound ("UI_cancel", "UI");
				foreach(PlayerSelectionPanel pan in panels)
				{
					pan.doorUpAnimator.SetBool("open", false);
					pan.doorDownAnimator.SetBool("open", false);
				}
				screenManager.OpenPanel (previousScreenAnimator);
				DeleteClones ();
			}

			// Equivalent of Y Button 
			if (dev.Action4.WasPressed)
			{
				PlayerInfos();
				AudioManager.instance.PlaySound ("UI_validate", "UI");
				CancelAllVibrations();
				screenManager.OpenPanel (gamesModesScreenAnimator);
			}
		}

		if (inputActivationScript.inputOK)
		{
			foreach(InputDevice inputDevice in InputManager.ActiveDevices)
			{
				if (inputDevice.Action1.WasPressed)
				{
					if (ThereIsNoPanelUsingDevice(inputDevice))
					{
						ActivatePlayerSelectionPanel(inputDevice);
					}
				}
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

	// Deactivating Panel when a device is detached
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

	// Writing playerInfos in the GameManager
	void PlayerInfos()
	{
		for (int i = 0; i < panels.Count; i++)
		{
			if (panels [i].state == PlayerSelectionPanel.SelectionPanelState.Validated)
			{
				GameManager.playersActive [i] = true;
				//Set selected characters
				GameManager.playersInputDevices [i] = panels [i].device;
				GameManager.playersNumbers[i] = "_P" + (i + 1).ToString();
				GameManager.playersTractConfig[i] = panels[i].tract;
			}
		}
		DeleteClones ();
		CancelAllVibrations();
		MatchStart.gameHasStarted = false;
	}

	void DeleteClones()
	{
		GameObject[] playerClones = GameObject.FindGameObjectsWithTag ("Player");
		foreach(GameObject clone in playerClones)
		{
			Destroy (clone.transform.parent.gameObject);
		}
	}

	void CancelAllVibrations()
	{
		foreach(InputDevice device in InputManager.Devices)
		{
			device.StopVibration ();
		}
	}

}
