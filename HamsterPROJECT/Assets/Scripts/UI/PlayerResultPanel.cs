using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerResultPanel : MonoBehaviour {

	public int playerResultPanelID;
	public Image background;
	Image characterSprite;
	[SerializeField] Text scoreText;
	[SerializeField] Text killsText;
	[SerializeField] Text deathsText;
	[SerializeField] Text selfDestructsText;

	void Awake()
	{
		// Determining if the panel must be activated (i.e. the corresponding player was registered)
		if (GameManager.playersActive [playerResultPanelID - 1])
			this.gameObject.SetActive (true);
		else 
			this.gameObject.SetActive (false);

		characterSprite = this.transform.GetChild (1).GetComponent<Image>();
	}

	// Use this for initialization
	void Start ()
	{
		background = this.GetComponent<Image> ();

		if (this.gameObject.activeSelf)
		{
			characterSprite.sprite = GameManager.playersCharacters [playerResultPanelID -1].transform.GetChild (0).GetComponent<SpriteRenderer> ().sprite;
		}

		// Text infos
		scoreText.text = "Score : " + GameManager.playersScores [playerResultPanelID - 1].ToString ();
		killsText.text = "Kills : " + GameManager.playersKills [playerResultPanelID - 1].ToString ();
		deathsText.text = "Deaths : " + GameManager.playersDeaths [playerResultPanelID - 1].ToString ();
		selfDestructsText.text = "Self : " + GameManager.playersSelfDestructs [playerResultPanelID - 1].ToString ();
	}
}
