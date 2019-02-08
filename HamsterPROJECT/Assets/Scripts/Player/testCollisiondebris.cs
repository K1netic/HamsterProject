using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testCollisiondebris : MonoBehaviour {

	[SerializeField]
	GameObject player;
	Transform position;


	// Use this for initialization
	void Start () {
		position = gameObject.transform;
	}
	
	// Update is called once per frame
	void Update () {
		position.position = player.transform.position;
	}
}
