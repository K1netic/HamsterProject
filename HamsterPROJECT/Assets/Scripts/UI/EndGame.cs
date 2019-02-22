using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour {

	void Start()
	{
		MusicManager.instance.PlayMusic("menu");
	}
}
