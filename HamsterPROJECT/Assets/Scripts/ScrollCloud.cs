using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollCloud : MonoBehaviour {

	[SerializeField]
	float scrollSpeed = -0.5f;

	Vector3 startPos;

	// Use this for initialization
	void Start () {
		startPos =  new Vector3(transform.position.x, transform.position.y, transform.position.z);
		print (startPos);
	}
	
	// Update is called once per frame
	void Update () {
		float nowPos = Mathf.Repeat (Time.time * scrollSpeed, 20);
		transform.position = startPos + Vector3.right * nowPos;
	}
}
