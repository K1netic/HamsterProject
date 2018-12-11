using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour {

	[SerializeField] string playerNumber;

	void Start () {
		this.GetComponent<Text> ().text = 
			"Score P" + playerNumber + " : " + GameManager.playersScores [int.Parse(playerNumber) - 1].ToString ();
	}
}
