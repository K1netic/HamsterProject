using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public float speed;
	private Vector3 direction;

    [HideInInspector]
    public string playerNumber;

    bool hooked;

    void Start(){
        direction = new Vector3(Input.GetAxis("Horizontal" + playerNumber), Input.GetAxis("Vertical" + playerNumber), 0);
		direction = direction.normalized;
        if(direction == Vector3.zero)
        {
            direction = Vector3.right;
        }
	}

	void Update () {
        if(!hooked)
		    transform.position += direction / speed;
	}

	public void Destruction(){
		Destroy (gameObject);
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print(collision.gameObject.name);
    }
}
