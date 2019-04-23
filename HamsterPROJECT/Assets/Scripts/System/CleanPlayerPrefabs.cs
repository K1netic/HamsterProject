using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CleanPlayerPrefabs : MonoBehaviour {

	public static CleanPlayerPrefabs instance = null;

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
	string path = "Assets/Resources/Prefabs/TemporaryPrefabs";

	void OnApplicationQuit()
	{
		GameObject basePrefab = Resources.Load<GameObject> ("Prefabs/PlayerPrefab");
		basePrefab.transform.position = new Vector3(0f,0f,0f);
		basePrefab.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;

		if (Directory.Exists(path)) 
		{ 
			Directory.Delete(path, true);
		}
 		Directory.CreateDirectory(path);
	}
}
