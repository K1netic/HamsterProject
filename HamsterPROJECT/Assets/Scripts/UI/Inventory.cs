using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    [SerializeField]
    public Text item;
    [SerializeField]
    float timeCDItem = 1f;
    public string currentItem;
    bool cdItem;

    //Je vais pour l'instant gérer les stats des objets directement depuis ce script, mais il faudra à la fin créer un script "Equilibrage"
    // et récupérer les valeurs dans ce script afin d'être sur que tout les joeurs ont les mêmes bonus
    [SerializeField]
    float bonusCroquettes;
    [SerializeField]
    float timeBonusCroquettes;
    [SerializeField]
    float gravityWithParachute;
    [SerializeField]
    float timeWithParachute;

    string playerNumber;
    PlayerMovement playerMovementScript;
    Rigidbody2D playerRigid;
    float stockGravity;

    private void Start()
    {
        playerMovementScript = GetComponent<PlayerMovement>();
        playerRigid = GetComponent<Rigidbody2D>();
        playerNumber = playerMovementScript.playerNumber;
        stockGravity = playerRigid.gravityScale;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetButtonDown("Item" + playerNumber) && !cdItem)
        {
            cdItem = true;
            switch (currentItem)
            {
                case "SteelRope":
                    print("Player" + playerNumber + " used " + currentItem);
                    currentItem = "None";
                    item.text = "None";
                    break;
                case "PeakBall":
                    print("Player" + playerNumber + " used " + currentItem);
                    currentItem = "None";
                    item.text = "None";
                    break;
                case "ExplosiveHook":
                    print("Player" + playerNumber + " used " + currentItem);
                    currentItem = "None";
                    item.text = "None";
                    break;
                case "Parachute":
                    print("Player" + playerNumber + " used " + currentItem);
                    currentItem = "None";
                    item.text = "None";
                    if (!playerMovementScript.isGrounded)
                    {
                        playerRigid.gravityScale = gravityWithParachute;
                        playerRigid.constraints = RigidbodyConstraints2D.FreezeRotation;
                        Invoke("CloseParachute", timeWithParachute);
                    }
                    else
                    {
                        //Handle what happen if we open parachute on the ground, I think we should not allow this
                    }
                    break;
                case "Shield":
                    print("Player" + playerNumber + " used " + currentItem);
                    currentItem = "None";
                    item.text = "None";
                    break;
                case "Miracle":
                    print("Player" + playerNumber + " used " + currentItem);
                    currentItem = "None";
                    item.text = "None";
                    break;
                case "CocaineCroquettes":
                    print("Player" + playerNumber + " used " + currentItem);
                    currentItem = "None";
                    item.text = "None";
                    playerMovementScript.bonusSpeed = bonusCroquettes;
                    Invoke("ResetCroquettesBonus", timeBonusCroquettes);
                    break;
                case "ExplosivePoop":
                    print("Player" + playerNumber + " used " + currentItem);
                    currentItem = "None";
                    item.text = "None";
                    break;
                case "None":
                    print("Player" + playerNumber + " has no item to use");
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
        playerMovementScript.bonusSpeed = 1;
    }

    void CloseParachute()
    {
        playerRigid.gravityScale = stockGravity;
        playerRigid.constraints &= ~RigidbodyConstraints2D.FreezeRotation;
    }
}
