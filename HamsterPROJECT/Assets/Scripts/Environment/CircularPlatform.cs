using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CircularPlatform : MonoBehaviour {

	[SerializeField]
	float moveSpeed;

    Rigidbody2D rigid;
    Vector3 pivotPos;
    Vector3 vectorToTarget;
    float t;
    float radius;
    float angle;

    void Start () {
        pivotPos = transform.parent.transform.position;
		rigid = GetComponent<Rigidbody2D>();
        radius = Vector2.Distance(pivotPos, transform.position);
    }
	
	void FixedUpdate () {
        t += Time.deltaTime;
        rigid.MovePosition(pivotPos + new Vector3(Mathf.Sin(t * moveSpeed) * radius, Mathf.Cos(t * moveSpeed) * radius, 0));

        vectorToTarget = transform.position - pivotPos;
        angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        rigid.MoveRotation(angle);
    }

    
}


