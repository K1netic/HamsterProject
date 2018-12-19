using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class screenTest : MonoBehaviour {

	void Update()
	{
		// Use this for initialization
		if (Input.GetButtonDown("Pause_P1"))
		{
			SceneManager.LoadScene("Results");
		}
	}

}
