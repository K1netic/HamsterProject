using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionOnHook : MonoBehaviour {

    Vector3 jointDirection;
    GameObject player;
    DistanceJoint2D joint;
    Hook hookScript;
    //PlayerMovement playerMovementScript;

    // Use this for initialization
    void Start () {
        player = transform.parent.gameObject;
        //playerMovementScript = player.GetComponent<PlayerMovement>();
        joint = player.GetComponent<DistanceJoint2D>();
        hookScript = GameObject.Find("Arrow" + player.GetComponent<PlayerMovement>().playerNumber).GetComponent<Hook>();
	}

    private void FixedUpdate()
    {
        //playerMovementScript.childRedAxis = transform.right;
        if (joint.enabled && hookScript.currentProjectile != null)
        {
            Vector3 jointDirection = (hookScript.currentProjectile.transform.position - transform.parent.transform.position).normalized;
            transform.rotation = Quaternion.FromToRotation(Vector3.up, jointDirection);
        }
    }
}
