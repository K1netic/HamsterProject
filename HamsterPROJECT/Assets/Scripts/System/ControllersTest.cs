using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{

	string[] joysticks; 
	// Update is called once per frame
	void Update ()
	{
		joysticks = Input.GetJoystickNames ();
		Debug.Log ("nb of joysticks : " + joysticks.Length);
		foreach (string joy in joysticks)
		{
			Debug.Log ("joystick name : " + joy);
		}
	}
}

