using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreElement : MonoBehaviour {

	ScoreDisplayer disp;
	Color iconColor;

	GameObject skull;
	GameObject star;

	GameObject iconToActivate;
	RectTransform iconRect;

	[SerializeField] int id;

	// Use this for initialization
	void Start () {
		disp = transform.parent.parent.GetComponent<ScoreDisplayer>();
		iconColor = disp.iconColor;
		skull = transform.GetChild(0).gameObject;
		star = transform.GetChild(1).gameObject;

		// Définition des icônes à afficher en fonction du mode de jeu
		if (GameManager.gameModeType == GameManager.gameModes.Deathmatch)
		{
			iconToActivate = skull;
			star.SetActive(false);
		}
		else if (GameManager.gameModeType == GameManager.gameModes.LastManStanding)
		{
			iconToActivate = star;
			skull.SetActive(false);
		}

		// Affiche l'icône courante si son index est inférieur ou égal au score du joueur
		if (id > GameManager.playersScores [disp.ScoreDisplayerID]) 
			iconToActivate.SetActive(false);
		else 
			iconToActivate.SetActive(true);

		// Ne lancer l'animation de l'icône de score que pour les nouveaux points acquis
		if (id <= disp.baseScore)
			iconToActivate.GetComponent<Animator>().SetBool("newScore", false);
		else
			iconToActivate.GetComponent<Animator>().SetBool("newScore", true);

		iconRect = iconToActivate.GetComponent<RectTransform>();
		iconRect.offsetMin = new Vector2(0,0);
		iconRect.offsetMax = new Vector2(0,0);
		iconRect.localScale = new Vector3(0.8f,0.8f,0.8f);
		iconToActivate.GetComponent<Image>().color = iconColor;
	}
}
