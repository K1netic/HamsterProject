using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    Balancing balanceData;

    [HideInInspector]
    public Text item;

    bool cdItem;
    [HideInInspector]
    public string currentItem;

    float timeCDItem = 1f;
//    float bonusCroquettes;
    float timeBonusCroquettes;
//    float gravityWithParachute;
//    float timeWithParachute;

    string playerNumber;
    PlayerMovement playerMovementScript;
    Rigidbody2D playerRigid;
    float stockGravity;

    private void Start()
    {
        //S'il y a une erreur ici s'assurer que le prefab "Balancing" est bien dans la scène
        balanceData = GameObject.Find("Balancing").GetComponent<Balancing>();

        playerMovementScript = GetComponent<PlayerMovement>();
        playerRigid = GetComponent<Rigidbody2D>();
        playerNumber = playerMovementScript.playerNumber;
        stockGravity = playerRigid.gravityScale;//Nécessaire pour l'utilisation du parachute
        item  = GameObject.Find("Item"+playerMovementScript.playerNumber).GetComponent<Text>();//Récupération du texte pour UI
    }

    // Update is called once per frame
    void Update () {
        //Si le joueur appuye sur le bouton ITEM et que l'utilisation d'un objet n'est pas en CD l'objet possédé est utilisé
        if (Input.GetButtonDown("Item" + playerNumber) && !cdItem)
        {
            cdItem = true;
            //Switch pour savoir quel item le joueur possède
            switch (currentItem)
            {
                case "SteelRope":
                    currentItem = "None";
                    item.text = "None";
                    break;
                case "PeakBall":
                    currentItem = "None";
                    item.text = "None";
                    break;
                case "ExplosiveHook":
                    currentItem = "None";
                    item.text = "None";
                    break;
                case "Parachute":
                    currentItem = "None";
                    item.text = "None";
                    //Si le joueur n'est pas au sol le parachute s'ouvre et modifie la gravité du joueur pour un temps donné
                    /*if (!playerMovementScript.isGrounded)
                    {
                        playerRigid.gravityScale /= gravityWithParachute;
                        playerRigid.constraints = RigidbodyConstraints2D.FreezeRotation;
                        Invoke("CloseParachute", timeWithParachute);
                    }
                    else
                    {
                        //Handle what happen if we open parachute on the ground, I think we should not allow this
                    }*/
                    break;
                case "Shield":
                    currentItem = "None";
                    item.text = "None";
                    break;
                case "Miracle":
                    currentItem = "None";
                    item.text = "None";
                    break;
                case "CocaineCroquettes":
                    //Change la valeur de bonusSpeed dans le script PlayerMovement pour un temps donné
                    currentItem = "None";
                    item.text = "None";
                    //playerMovementScript.bonusSpeed = bonusCroquettes;
                    Invoke("ResetCroquettesBonus", timeBonusCroquettes);
                    break;
                case "ExplosivePoop":
                    currentItem = "None";
                    item.text = "None";
                    break;
                case "None":
                    break;
                default:
                    break;
            }
            Invoke("ResetCD", timeCDItem);
        }
	}

    void ResetCD()
    {
        cdItem = false;
    }

    void ResetCroquettesBonus()
    {
        //playerMovementScript.bonusSpeed = 1;
    }

    void CloseParachute()
    {
        playerRigid.gravityScale = stockGravity;
        playerRigid.constraints &= ~RigidbodyConstraints2D.FreezeRotation;
    }
}
