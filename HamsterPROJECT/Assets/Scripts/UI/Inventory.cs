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

    string playerNumber;

    private void Start()
    {
        playerNumber = GetComponent<PlayerMovement>().playerNumber;
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
                    break;
                case "PeakBall":
                    print("Player" + playerNumber + " used " + currentItem);
                    currentItem = "None";
                    break;
                case "ExplosiveHook":
                    print("Player" + playerNumber + " used " + currentItem);
                    currentItem = "None";
                    break;
                case "Parachute":
                    print("Player" + playerNumber + " used " + currentItem);
                    currentItem = "None";
                    break;
                case "Shield":
                    print("Player" + playerNumber + " used " + currentItem);
                    currentItem = "None";
                    break;
                case "Miracle":
                    print("Player" + playerNumber + " used " + currentItem);
                    currentItem = "None";
                    break;
                case "CocaineCroquettes":
                    print("Player" + playerNumber + " used " + currentItem);
                    currentItem = "None";
                    break;
                case "ExplosivePoop":
                    print("Player" + playerNumber + " used " + currentItem);
                    currentItem = "None";
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
}
