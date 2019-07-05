using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplayer : MonoBehaviour {

	[SerializeField] public int ScoreDisplayerID;
    RectTransform rect;

    [SerializeField] GameObject socle_3;
    [SerializeField] GameObject socle_5;
    [SerializeField] GameObject socle_8;
    [SerializeField] GameObject socle_10;

    public Color iconColor;

    public int baseScore;
    bool firstActivation = false;

	// Use this for initialization
    void OnEnable()
    {
        if (!firstActivation)
        {
            baseScore = GameManager.playersScores[ScoreDisplayerID];
            firstActivation = true;
        }
    }

	void Start () {

		// Don't display score if the corresponding player is not in the game
		for(int i = 0; i < GameManager.playersActive.Length; i++)
		{
			if (ScoreDisplayerID == i && !GameManager.playersActive [i])
				this.gameObject.SetActive (false);
		}

        rect = this.GetComponent<RectTransform>();

		transform.GetChild(0).GetComponent<Image>().sprite = GameManager.playersSprites[ScoreDisplayerID];
        
        // Définie la couleur des icônes à afficher en fonction du personnage choisi par le joueur
        if (this.gameObject.activeSelf)
        {
            switch (GameManager.playersSprites[ScoreDisplayerID].name)
            {
                case "0":
                    iconColor = new Color(1, 0.674f, 0); //255, 172, 0
                    break;
                case "1":
                    iconColor = new Color(1, 0.451f, 0.694f); //255, 115, 177
                    break;
                case "2":
                    iconColor = new Color(0, 0.537f, 1); //0,137,255
                    break;
                case "3":
                    iconColor = new Color(0.067f, 0.71f, 0.051f); //17, 181, 13
                    break;
                case "4":
                    iconColor = new Color(0.906f, 0, 0.32f); //231, 0, 81
                    break;
                default:
                    break;
            }

            // Modifie la taille du parent pour centrer l'affichage du score au centre
            // Active le groupe de socles à afficher en fonction du nombre de rounds total
            switch (GameManager.rounds)
            {
                case 3:
                    rect.anchorMin = new Vector2(0.35f,rect.anchorMin.y);
                    rect.anchorMax = new Vector2(0.6f,rect.anchorMax.y);
                    socle_3.SetActive(true);
                    socle_3.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
                    break;
                case 5:
                    //doesn't change jack shit
                    rect.anchorMin = new Vector2(0.3f,rect.anchorMin.y);
                    rect.anchorMax = new Vector2(0.65f,rect.anchorMax.y);
                    socle_5.SetActive(true);
                    socle_5.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
                    break;
                case 8:
                    rect.anchorMin = new Vector2(0.225f,rect.anchorMin.y);
                    rect.anchorMax = new Vector2(0.725f,rect.anchorMax.y);
                    socle_8.SetActive(true);
                    socle_8.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
                    break;
                case 10:
                    rect.anchorMin = new Vector2(0.175f,rect.anchorMin.y);
                    rect.anchorMax = new Vector2(0.775f,rect.anchorMax.y);
                    socle_10.SetActive(true);
                    socle_10.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
                    break;
                default:
                    break;
            }
        }
    }
}
