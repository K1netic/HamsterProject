using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour {

	[SerializeField] string sceneToLoad;
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.anyKeyDown)
		{
			//TODO : Add a delay
			SceneManager.LoadScene (sceneToLoad);
		}
	}
}
