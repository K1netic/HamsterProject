using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using InControl;

public class TitleScreen : MonoBehaviour {

	[SerializeField] string sceneToLoad;
	float delay = 0.1f;
	MusicManager music;

    void Start()
	{
		PlayerPrefs.DeleteAll ();
		AudioManager.instance.PlaySound("UI_titleJingle", "UI");
        Cursor.visible = false;
	}

	void FixedUpdate () {
		if (InputManager.CommandWasPressed)
		{
			StartCoroutine(TitleScreenValidation());
		}
	}

	IEnumerator TitleScreenValidation()
	{
		AudioManager.instance.PlaySound ("UI_titleScreenValidation", "UI");
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
