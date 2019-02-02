using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CircularPlatform : MonoBehaviour {

	[SerializeField]
	GameObject pivot;
	[SerializeField]
	float moveSpeed;
    [SerializeField]
    float rigidBodyMass = 1;

    Rigidbody2D rigid;
    Vector2 pivotPos;
    public Vector2 initialVelocity;
    public float force;

    void Start () {
		pivotPos = pivot.transform.position;
		rigid = GetComponent<Rigidbody2D>();

        initialVelocity = moveSpeed * Vector2.Perpendicular(pivotPos - (Vector2)transform.position).normalized;

        force = Mathf.Pow(initialVelocity.magnitude, 2.0f) * Vector2.Distance(pivotPos,(Vector2)transform.position) / rigidBodyMass;
    }
	
	void FixedUpdate () {
        rigid.AddForce((pivotPos - (Vector2)transform.position).normalized * force);

        Debug.DrawRay(transform.position, Vector2.Perpendicular(pivotPos - (Vector2)transform.position).normalized,Color.red,1000);
    }

    
}


