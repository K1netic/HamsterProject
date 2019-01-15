using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerResultPanel : MonoBehaviour {

	public int playerResultPanelID;
	public Image background;
	Image characterSprite;

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
	}
}
