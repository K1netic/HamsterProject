using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour {

	public static LevelSelection instance = null;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
		else 
		{
			Destroy (gameObject);
		}
	}

	[SerializeField] List<string> pack1 = new List<string>();
	[SerializeField] List<string> pack2 = new List<string>();
	[SerializeField] List<string> pack3 = new List<string>();
	[SerializeField] List<string> pack4 = new List<string>();

    List<string> currentPack = new List<string>();
	public List<string> levels = new List<string>();
	string lastLevelPlayed;
	string levelToLoad;

	public void SelectPack(int packID)
	{
		switch (packID)
		{
			case 1:
			currentPack = pack1;
			break;
			case 2:
			currentPack = pack2;
			break;
			case 3:
			currentPack = pack3;
			break;
			case 4:
			currentPack = pack4;
			break;
            default:
			currentPack = pack1;
			break;
		}
	}

    public void LoadNextLevel(string currentLevel)
    {
        if (currentLevel == "")
        {
            levels.Clear();
            for (int i = 0; i < currentPack.Count; i++)
            {
                levels.Add(currentPack[i]);
            }
        }
        else
        {
            levels.Remove(currentLevel);
        }

        //Remove last level played from last series of rounds to avoid starting a new series of rounds with the same level
        if (GameManager.lastLevelPlayed != null)
        {
            levels.Remove(GameManager.lastLevelPlayed);
        }

        if (levels.Count == 0)
        {
            for (int i = 0; i < currentPack.Count; i++)
            {
                levels.Add(currentPack[i]);
            }
            levels.Remove(currentLevel);
        }

        //Choosing next level to load from the remaining levels
        levelToLoad = levels[Random.Range(0, levels.Count)];

        //Loading next scene
        if (levelToLoad != null)
        {
            if (Time.timeScale != 1) Time.timeScale = 1;
            SceneManager.LoadScene(levelToLoad);
        }
        else
        {
            print("Trying to load next level - no level found");
        }
    }

}
