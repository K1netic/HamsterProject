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

	void OnApplicationQuit()
	{
		GameObject basePrefab = Resources.Load<GameObject> ("Prefabs/PlayerPrefab");
		basePrefab.transform.position = new Vector3(0f,0f,0f);
		basePrefab.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
	}
}
