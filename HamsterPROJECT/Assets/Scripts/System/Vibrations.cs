using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class Vibrations : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton ("Hook_P1"))
			GamePad.SetVibration (PlayerIndex.One, 1.0f, 1.0f);
		else if (Input.GetButton ("Lock_P1"))
			GamePad.SetVibration (PlayerIndex.One, 1.0f, 0.0f);
		else if (Input.GetButton ("SwitchState_P1"))
			GamePad.SetVibration (PlayerIndex.One, 0.0f, 1.0f);
		else
			GamePad.SetVibration (PlayerIndex.One, 0.0f, 0.0f);
	}
}
