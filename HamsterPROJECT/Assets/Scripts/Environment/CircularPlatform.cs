using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CircularPlatform : MonoBehaviour {

	[SerializeField]
	GameObject pivot;
	Vector2 pivotPos;
	Vector2 direction;
	Rigidbody2D rigid;
	[SerializeField]
	float moveSpeed;
	DistanceJoint2D joint;
    float distance;

	// Use this for initialization
	void Start () {
		pivotPos = pivot.transform.position;
		rigid = GetComponent<Rigidbody2D>();
		joint = GetComponent<DistanceJoint2D>();
		joint.maxDistanceOnly = false;
        distance = Vector2.Distance(transform.position, pivotPos);
	}
	
	// Update is called once per frame
	void Update () {
        joint.distance = distance;
		direction = (pivotPos - (Vector2)transform.position).normalized;
		float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
		rigid.velocity = moveSpeed * transform.right;
		Debug.DrawRay(transform.position, Vector2.Perpendicular(pivotPos - (Vector2)transform.position).normalized,Color.red,1000);
	}
}
