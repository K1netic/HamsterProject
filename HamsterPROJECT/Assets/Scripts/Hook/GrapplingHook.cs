using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour {


	public LineRenderer line;
	DistanceJoint2D joint;
	Vector3 targetPos;
	RaycastHit2D hit;
	public float distance=10f; // distance of the hook
	public LayerMask mask;
	public float step = 0.2f; //speed of retractation
    string playerNumber;
    public bool grappling = true;

	void Start () {
        playerNumber = GetComponent<PlayerMovement>().playerNumber;
        //Disables the line and rendering of the joint
        joint = GetComponent<DistanceJoint2D> ();
		joint.enabled = false;
		line.enabled = false;
	}

	void Update () {

		//if the rope bool is false, the hook become a rope
		if (grappling == true) {
			//retracte the grap and break it
			if (joint.distance > .1f) { 
				joint.distance -= step;
			} else {
				line.enabled = false;
				joint.enabled = false;
			}
		}

		if (Input.GetButtonDown ("Hook" + playerNumber)) {

			// get the axis to aim
			Vector3 inputDirection;
			inputDirection.x = Input.GetAxis ("Horizontal" + playerNumber);
			inputDirection.y = Input.GetAxis ("Vertical" + playerNumber);
			inputDirection.z = 0;
			targetPos = inputDirection;
			//targetPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			//targetPos.z = 0;

			hit = Physics2D.Raycast (transform.position, targetPos /*- transform.position*/, distance, mask);

			if(hit.collider!=null && hit.collider.gameObject.GetComponent<Rigidbody2D>()!= null)
			{
				joint.enabled = true;
				Vector2 connectPoint = hit.point - new Vector2 (hit.collider.transform.position.x, hit.collider.transform.position.y);
				connectPoint.x = connectPoint.x / hit.collider.transform.localScale.x;
				connectPoint.y = connectPoint.y / hit.collider.transform.localScale.y;
				joint.connectedAnchor = connectPoint;

				joint.connectedBody = hit.collider.gameObject.GetComponent<Rigidbody2D> ();
				joint.distance = Vector2.Distance (transform.position,hit.point);

				line.enabled = true;
				line.SetPosition (0, transform.position);
				line.SetPosition (1, hit.point);

				line.GetComponent<RopeRatio>().grabPos=hit.point;
			}
		}
			
		if (Input.GetButton ("Hook" + playerNumber)) {
			line.SetPosition (0, transform.position);
		}

		if (Input.GetButtonUp ("Hook" + playerNumber)) 
		{
			joint.enabled = false;
			line.enabled = false;
		}

	}
}
