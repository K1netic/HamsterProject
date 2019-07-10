using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour {

    private void Awake()
    {
        Cursor.visible = false;
    }

    void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
