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
			if (ScoreDisplayerID == i && !GameManager.playersActive [i])
				this.gameObject.SetActive (false);
		}

		// Update score text
		transform.GetChild(1).GetComponent<Text>().text = " : " + GameManager.playersScores [ScoreDisplayerID].ToString ();
        // if(GameManager.playersActive[ScoreDisplayerID])
        transform.GetChild(0).GetComponent<Image>().sprite = GameManager.playersSprites[ScoreDisplayerID];
        
        /*if (GameManager.playersCharacters[ScoreDisplayerID])
        {
            switch (GameManager.playersCharacters[ScoreDisplayerID].name)
            {
                case "0":
                    transform.GetChild(0).GetComponent<Text>().color = new Color(1, 0.5568f, 0);
                    break;
                case "1":
                    transform.GetChild(0).GetComponent<Text>().color = new Color(1, 0, 1);
                    break;
                case "2":
                    transform.GetChild(0).GetComponent<Text>().color = new Color(0.2019f, 0.5660f, 0.1361f);
                    break;
                case "3":
                    transform.GetChild(0).GetComponent<Text>().color = new Color(1, 1, 0);
                    break;
                case "4":
                    transform.GetChild(0).GetComponent<Text>().color = new Color(0, 0.2901f, 1);
                    break;
                default:
                    break;
            }
        }*/
    }
}
