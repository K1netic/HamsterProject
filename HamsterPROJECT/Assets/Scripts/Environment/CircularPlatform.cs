using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CircularPlatform : MonoBehaviour {

	[SerializeField]
	GameObject pivot;
	Vector2 pivotPos;
	Rigidbody2D rigid;
	[SerializeField]
	float moveSpeed;


    void Start () {
		pivotPos = pivot.transform.position;
		rigid = GetComponent<Rigidbody2D>();
	}
	
	void FixedUpdate () {
        rigid.velocity = moveSpeed * Vector2.Perpendicular(pivotPos - (Vector2)transform.position).normalized;
        Debug.DrawRay(transform.position, Vector2.Perpendicular(pivotPos - (Vector2)transform.position).normalized,Color.red,1000);
    }
}
