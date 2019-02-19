using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using InControl;

public class BackwardsNavigation : MonoBehaviour {

	[SerializeField] string previousScene;
	float delay = 0.1f;
	AudioManager mngr;

	void Start()
	{
		mngr = FindObjectOfType<AudioManager> ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (InputManager.ActiveDevice.Action2.WasPressed && previousScene != null && previousScene != "")
		{
			StartCoroutine (LoadPreviousScene ());
		}
	}

	IEnumerator LoadPreviousScene()
	{
		mngr.PlaySound ("UI_cancel", "UI");
		yield return new WaitForSeconds (delay);
		SceneManager.LoadScene (previousScene);
	}
}
