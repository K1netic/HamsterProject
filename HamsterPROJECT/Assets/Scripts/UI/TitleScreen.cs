using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using InControl;

public class TitleScreen : MonoBehaviour {

	[SerializeField] string sceneToLoad;
	float delay = 0.1f;
	AudioManager mngr;

    void Start()
	{
        Cursor.visible = false;
		mngr = FindObjectOfType<AudioManager> ();
	}

	void FixedUpdate () {
		if (InputManager.CommandWasPressed)
		{
			StartCoroutine(TitleScreenValidation());
		}
	}

	IEnumerator TitleScreenValidation()
	{
		mngr.PlaySound ("UI_titleScreenValidation", mngr.UIsource);
		yield return new WaitForSeconds (delay);
        if(PlayerPrefs.GetInt("FirstTime") == 1)
		    SceneManager.LoadScene (sceneToLoad);
        else
        {
            PlayerPrefs.SetInt("FirstTime", 1);
            PlayerPrefs.Save();
            SceneManager.LoadScene("Tutorial");
        }
	}
}
