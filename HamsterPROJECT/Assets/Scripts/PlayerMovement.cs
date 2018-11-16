using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float maxSpeed = 50;
	float smoothTime = 0.3f;
	float xVelocity = 0.0f;

    int playerDirection = 1;

    Rigidbody2D rigid;

	bool lockMovement;

    // Use this for initialization 
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame 
    void FixedUpdate()
    {
		// Conditions that don't allow movement
        if (!lockMovement)
        {
            //Handling player direction
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                transform.localScale = new Vector3(1, 1, 0);
                playerDirection = 1;
            }
            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                transform.localScale = new Vector3(-1, 1, 0);
                playerDirection = -1;
            }

            //Input -> start moving
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                float acceleration = Mathf.SmoothDamp(0, playerDirection * maxSpeed, ref xVelocity, smoothTime);
                rigid.velocity = new Vector2(acceleration, rigid.velocity.y);
            }
        }
    }
}