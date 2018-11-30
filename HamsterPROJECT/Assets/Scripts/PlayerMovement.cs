using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
	Rigidbody2D rigid;
	Transform feetPos;

	//Movement
    [SerializeField] float maxSpeed = 100;
	float smoothTime = 0.3f;
	float xVelocity = 0.0f;
    int playerDirection = 1;
	bool lockMovement = false;
    public float bonusSpeed = 1;

    //Ground Check
    bool isGrounded = false;
	[SerializeField] float checkRadius = 1.0f;
	[SerializeField] LayerMask groundLayer;

	//Fast Fall
	[SerializeField] float fastFallSpeed = 200;
	// Value under which vertical joystick input will trigger fastFall
	[SerializeField] float fastFallVerticalThreshold = - 0.5f;
	// Value over which horizontal joystick input will cancel fastFall 
	[SerializeField] float fastFallHorizontalThreshold = 0.1f;

	[SerializeField] public string playerNumber;

	void Start()
	{
		rigid = this.GetComponent<Rigidbody2D> ();
		feetPos = this.transform.GetChild (0).transform;
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
                float acceleration = Mathf.SmoothDamp(0, playerDirection * maxSpeed * bonusSpeed, ref xVelocity, smoothTime);
                rigid.velocity = new Vector2(acceleration, rigid.velocity.y);
            }
        }

		//FastFall
		if (Input.GetAxisRaw("Vertical" + playerNumber) < fastFallVerticalThreshold
			&& !isGrounded
			&& Mathf.Abs(Input.GetAxisRaw("Horizontal" + playerNumber)) < fastFallHorizontalThreshold)
		{
			float acceleration = Mathf.SmoothDamp(0, -1 * fastFallSpeed, ref xVelocity, smoothTime);
			rigid.velocity = new Vector2(rigid.velocity.x, acceleration);
		}
    }
}