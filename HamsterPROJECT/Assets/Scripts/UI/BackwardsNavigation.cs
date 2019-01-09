using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
		if (Input.GetButtonDown("Cancel_P1") && previousScene != null && previousScene != "")
		{
			StartCoroutine (LoadPreviousScene ());
		}
	}

	IEnumerator LoadPreviousScene()
	{
		mngr.PlaySound ("UI_cancel", mngr.UIsource);
		yield return new WaitForSeconds (delay);
		SceneManager.LoadScene (previousScene);
	}
}
