using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour {

	[SerializeField] string sceneToLoad;
	float delay = 0.1f;
	AudioManager mngr;

	void Start()
	{
		mngr = FindObjectOfType<AudioManager> ();
	}

	void FixedUpdate () {
		if (Input.anyKeyDown)
		{
			StartCoroutine(TitleScreenValidation());
		}
	}

	IEnumerator TitleScreenValidation()
	{
		mngr.PlaySound ("UI_titleScreenValidation", mngr.UIsource);
		yield return new WaitForSeconds (delay);
		SceneManager.LoadScene (sceneToLoad);
	}
}
