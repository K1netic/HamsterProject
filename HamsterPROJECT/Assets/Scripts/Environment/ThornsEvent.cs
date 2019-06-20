using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornsEvent : MonoBehaviour {

	void TransitionIn()
    {
        GameObject.Find("ThornsSystem").GetComponent<Thorns>().SwitchSpriteShape();
    }

    void TransitionOut()
    {
        GameObject.Find("ThornsSystem").GetComponent<Thorns>().LaunchThornsBall();
    }
}
