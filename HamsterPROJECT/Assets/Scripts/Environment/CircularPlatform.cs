using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CircularPlatform : MonoBehaviour {

	[SerializeField]
	GameObject pivot;
	[SerializeField]
	float moveSpeed;

    Rigidbody2D rigid;
    Vector3 pivotPos;
    float t;
    float radius;

    void Start () {
		pivotPos = pivot.transform.position;
		rigid = GetComponent<Rigidbody2D>();
        radius = Vector2.Distance(pivotPos, transform.position);
    }
	
	void FixedUpdate () {
        t += Time.deltaTime;
        rigid.MovePosition(pivotPos + new Vector3(Mathf.Sin(t * moveSpeed) * radius, Mathf.Cos(t * moveSpeed) * radius, 0));
    }

    
}


