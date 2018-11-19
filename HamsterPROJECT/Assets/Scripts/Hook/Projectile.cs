using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public float speed;
	private Vector3 direction;

	void Start(){
		direction = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
		direction = direction.normalized;
	}

	void Update () {
		transform.position += direction / speed;
	}

	public void Destruction(){
		Destroy (gameObject);
	}
}
