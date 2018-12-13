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
    float bonusCroquettes;
    float timeBonusCroquettes;
    float gravityWithParachute;
    float timeWithParachute;

    string playerNumber;
    PlayerMovement playerMovementScript;
    Rigidbody2D playerRigid;
    float stockGravity;

    private void Start()
    {
        //S'il y a une erreur ici s'assurer que le prefab "Balancing" est bien dans la scène
        balanceData = GameObject.Find("Balancing").GetComponent<Balancing>();

        timeCDItem = balanceData.timeCDItem;
        bonusCroquettes = balanceData.bonusCroquettes;
        timeBonusCroquettes = balanceData.timeBonusCroquettes;
        gravityWithParachute = balanceData.gravityWithParachute;
        timeWithParachute = balanceData.timeWithParachute;

        playerMovementScript = GetComponent<PlayerMovement>();
        playerRigid = GetComponent<Rigidbody2D>();
        playerNumber = playerMovementScript.playerNumber;
        stockGravity = playerRigid.gravityScale;
        item  = GameObject.Find("Item"+playerMovementScript.playerNumber).GetComponent<Text>();
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
                        playerRigid.gravityScale /= gravityWithParachute;
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
