using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapButton : MonoBehaviour {

	LevelSelection lvlSelect;
	[SerializeField] int packID;

	// Use this for initialization
	void Start () {
		lvlSelect = FindObjectOfType<LevelSelection>();
	}

	// Load a series of rounds (pack)
	public void LoadMatch()
	{
		if (Time.timeScale != 1) Time.timeScale = 1;
		AudioManager.instance.PlaySound ("UI_gameLaunch", "UI");
		MusicManager.instance.StopMusic ("menu");
		lvlSelect.SelectPack(packID);
		lvlSelect.LoadNextLevel ("");
	}

	public void Select()
	{
		this.transform.GetChild (0).gameObject.SetActive (true);
		AudioManager.instance.PlaySound ("UI_Pick", "UI");
	}

	public void Deselect()
	{
		this.transform.GetChild (0).gameObject.SetActive (false);
	}
}
