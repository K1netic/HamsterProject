using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionState : MonoBehaviour {

	[HideInInspector]
	public float speed;

	Rigidbody2D TAMERE;
	SpriteRenderer TONPERE;

	// Use this for initialization
	void Start () {
		TAMERE = this.GetComponent<Rigidbody2D>();
		TONPERE = this.GetComponent<SpriteRenderer> ();

	}

	// Update is called once per frame
	void Update () {
		speed = TAMERE.velocity.magnitude;
		if (speed >= 35f) {
			TONPERE.color = Color.red;
		} 
		else
		{
			TONPERE.color = Color.blue;
		}
			
	}
}
