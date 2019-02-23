using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameModes : MonoBehaviour {

	void Start()
	{
		MusicManager.instance.PlayMusic("menu");
        if (PlayerPrefs.GetInt("NotFirstTime") != 1)
        {
            EventSystem.current.firstSelectedGameObject = GameObject.Find("TrainingRoom");
            PlayerPrefs.SetInt("NotFirstTime", 1);
            PlayerPrefs.Save();
        }
    }

}
