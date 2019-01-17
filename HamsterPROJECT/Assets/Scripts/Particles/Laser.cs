using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

	[SerializeField]
	Color RingColor;

	[SerializeField]
	GameObject center;
	LineRenderer centerLine;
	[SerializeField]
	GameObject glow;
	LineRenderer glowLine;
	[SerializeField]
	GameObject ring;
	LineRenderer ringLine;

	bool round = true;

	// Use this for initialization
	void Start () {
		
		centerLine = center.GetComponent<LineRenderer> ();
		glowLine = glow.GetComponent<LineRenderer> ();
		ringLine = ring.GetComponent<LineRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (round) {
			centerLine.startWidth = 1f;
			centerLine.endWidth = 1f;
			glowLine.startWidth = 1.5f;
			glowLine.endWidth = 1.5f;
			ringLine.startWidth = 2f;
			ringLine.endWidth = 2f;
			round = !round;
		} 
		else {
			centerLine.startWidth = 0.9f;
			centerLine.endWidth = 0.9f;
			glowLine.startWidth = 1.8f;
			glowLine.endWidth = 1.8f;
			ringLine.startWidth = 2.05f;
			ringLine.endWidth = 2.05f;
			round = !round;
		}
	/*	timeRemaining -= Time.deltaTime;
		if(timeRemaining <= timeHooked/2)
			t += (Time.deltaTime / timeRemaining)/2;
		line.startColor = Color.Lerp(colorRope, Color.black,t);
		line.endColor = Color.Lerp(colorRope, Color.black,t);*/


	}
}
