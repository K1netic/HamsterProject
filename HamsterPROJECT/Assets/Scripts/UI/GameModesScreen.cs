using System.Collections;
using System.Collections.Generic;
using InControl;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameModesScreen : MonoBehaviour {

	ScreenManager screenManager;
	ChangeGameMode gameMode;
	ChangeCup cup;
	[SerializeField] Animator previousScreen;
	GameObject itemSelected;
	GameManager.gameModes storedGameModeType;
	int storedRounds;
	GameManager.gameModes defaultGameMode = GameManager.gameModes.LastManStanding;
	int defaultRounds = 5;

	void OnEnable()
	{
		itemSelected = GameObject.Find ("GameModeSelection");
		EventSystem.current.firstSelectedGameObject = itemSelected;
		storedRounds = GameManager.rounds;
		storedGameModeType = GameManager.gameModeType;
		StartCoroutine (WaitBeforeAllowingActivationOfButton ());
  	}

	void Start()
	{
		screenManager = FindObjectOfType<ScreenManager> ();
		gameMode = FindObjectOfType<ChangeGameMode>();
		cup = FindObjectOfType<ChangeCup>();
	}

	void Update () {
		// Reset les changements de modes de jeu
		if (InputManager.ActiveDevice.Action2.WasPressed)
		{
			AudioManager.instance.PlaySound ("UI_cancel", "UI");
			GameManager.rounds = storedRounds;
			GameManager.gameModeType = storedGameModeType;
			gameMode.UpdateText();
			cup.UpdateCupText();
			cup.UpdateDescriptionText();
		}

		// Confirmer les changements de jeu
		else if (InputManager.ActiveDevice.Action1.WasPressed)
		{
			AudioManager.instance.PlaySound ("UI_validate", "UI");
			CloseGameModes();
		}

		// Appliquer les valeurs par défaut
		else if (InputManager.ActiveDevice.Action4.WasPressed)
		{
			AudioManager.instance.PlaySound ("UI_cancel", "UI");
			GameManager.rounds = defaultRounds;
			GameManager.gameModeType = defaultGameMode;
			gameMode.UpdateText();
			cup.UpdateCupText();
			cup.UpdateDescriptionText();
		}
	}

	IEnumerator WaitBeforeAllowingActivationOfButton()
	{
		yield return new WaitForSeconds (GameManager.delayMenu);
		EventSystem.current.firstSelectedGameObject = itemSelected;
		itemSelected.GetComponent<ChangeGameMode> ().Select ();
	}

	void CloseGameModes()
	{
		screenManager.CloseCurrent ();
		screenManager.OpenPanel (previousScreen);
		EventSystem.current.SetSelectedGameObject(null);
	}
}
