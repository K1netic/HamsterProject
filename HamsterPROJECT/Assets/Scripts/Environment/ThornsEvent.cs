using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornsEvent : MonoBehaviour {

    [SerializeField]
    string thornsName = "ThornsSystem";

	void TransitionIn()
    {
        GameObject.Find(thornsName).GetComponent<Thorns>().SwitchSpriteShape();
    }

    void TransitionOut()
    {
        GameObject.Find(thornsName).GetComponent<Thorns>().LaunchThornsBall();
    }
}
