using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateInput : MonoBehaviour {

	// [HideInInspector]
	public bool inputOK = false;
	// Use this for initialization
	public void inputActivation()
	{
		inputOK = true;
	}

	public void inputDeactivation()
	{
		inputOK = false;
	}
}
