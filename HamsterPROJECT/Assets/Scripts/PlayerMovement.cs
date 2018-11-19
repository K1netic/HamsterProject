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
	bool lockMovement;
	bool isGrounded;

    Rigidbody2D rigid;
	Transform feetPos;

	[SerializeField] float checkRadius;
	[SerializeField] LayerMask groundLayer;

	[SerializeField] float fastFallSpeed = 100;

	[SerializeField] string playerNumber;

    // Use this for initialization 
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
		feetPos = transform.GetChild (0).transform;
    }

    void FixedUpdate()
    {
		isGrounded = Physics2D.OverlapCircle (feetPos.position, checkRadius, groundLayer);

		//Movement Lock
		if (isGrounded && Input.GetButton ("Lock" + playerNumber))
			lockMovement = true;
		else
			lockMovement = false;

		//Movement
        if (!lockMovement)
        {
            //Handling player direction
			if (Input.GetAxisRaw("Horizontal" + playerNumber) > 0)
            {
                transform.localScale = new Vector3(1, 1, 0);
                playerDirection = 1;
            }
			if (Input.GetAxisRaw("Horizontal" + playerNumber) < 0)
            {
                transform.localScale = new Vector3(-1, 1, 0);
                playerDirection = -1;
            }

            //Input -> start moving
			if (Input.GetAxisRaw("Horizontal" + playerNumber) != 0)
            {
                float acceleration = Mathf.SmoothDamp(0, playerDirection * maxSpeed, ref xVelocity, smoothTime);
                rigid.velocity = new Vector2(acceleration, rigid.velocity.y);
            }
        }

		//FastFall
		if (Input.GetAxisRaw("Vertical" + playerNumber) < - 0.5f && !isGrounded)
		{
			float acceleration = Mathf.SmoothDamp(0, -1 * fastFallSpeed, ref xVelocity, smoothTime);
			rigid.velocity = new Vector2(rigid.velocity.x, acceleration);
		}
    }
}