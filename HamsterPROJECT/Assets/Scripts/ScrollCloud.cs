using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollCloud : MonoBehaviour {

	[SerializeField]
	float scrollSpeed = -0.5f;
	float scale;
	Vector3 startPos;

	// Use this for initialization
	void Start () {
		startPos = transform.position;
		scale = transform.parent.GetComponent<Transform>().localScale.x;
		print (scale);
	}
	
	// Update is called once per frame
	void Update () {
		float nowPos = Mathf.Repeat (Time.time * scrollSpeed, 40*scale);
		transform.position = startPos + Vector3.right * nowPos;

	}
}
