using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplayer : MonoBehaviour {

	[SerializeField] int ScoreDisplayerID;
	// Use this for initialization
	void Start () {
		// Don't display score if the corresponding player is not in the game
		for(int i = 0; i < GameManager.playersActive.Length; i++)
		{
			if (ScoreDisplayerID -1 == i && !GameManager.playersActive [i])
				this.gameObject.SetActive (false);
		}

		// Update score text
		this.GetComponent<Text>().text = 
			"Score P" + ScoreDisplayerID.ToString() + " : " + GameManager.playersScores [ScoreDisplayerID - 1].ToString ();	
	}
}
