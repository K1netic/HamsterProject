using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackwardsNavigation : MonoBehaviour {

	[SerializeField] string previousScene;
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetButton("Cancel_P1") && previousScene != null && previousScene != "")
		{
			SceneManager.LoadScene (previousScene);
		}
	}
}
