using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedMeter : MonoBehaviour {

    Text text;

	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
        
	}
	
	// Update is called once per frame
	void Update () {
        text.text = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().speed.ToString("#.00");
    }
}
