using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerResultPanel : MonoBehaviour {

	public int playerResultPanelID;
	public Image borders;
	[SerializeField] Image characterSprite;
	[SerializeField] Text scoreText;
	[SerializeField] Text killsText;
	[SerializeField] Text deathsText;
	[SerializeField] Text selfDestructsText;

	void Awake()
	{
		// Determining if the panel must be activated (i.e. the corresponding player was registered)
		if (GameManager.playersActive [playerResultPanelID])
			this.gameObject.SetActive (true);
		else 
			this.gameObject.SetActive (false);
	}

	// Use this for initialization
	void Start ()
	{
		if (this.gameObject.activeSelf)
		{
			characterSprite.sprite = GameManager.playersCharacters [playerResultPanelID].transform.GetChild (0).GetComponent<SpriteRenderer> ().sprite;
		}

		// Text infos
		scoreText.text = "Score : " + GameManager.playersScores [playerResultPanelID].ToString ();
		killsText.text = "Kills : " + GameManager.playersKills [playerResultPanelID].ToString ();
		deathsText.text = "Deaths : " + GameManager.playersDeaths [playerResultPanelID ].ToString ();
		selfDestructsText.text = "Self : " + GameManager.playersSelfDestructs [playerResultPanelID].ToString ();
	}
}
