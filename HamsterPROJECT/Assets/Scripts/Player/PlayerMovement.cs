using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    Balancing balanceData;

    [HideInInspector]
	public Rigidbody2D rigid;
	public Transform feetPos;
    [HideInInspector]
    public bool hooked;
    [HideInInspector]
    public Vector2 jointDirection;

	//Movement
    float maxSpeed = 100;
	float smoothTime = 0.3f;
	float xVelocity = 0.0f;
    int playerDirection = 1;
    [HideInInspector]
    public bool lockMovementKnockBack;
	bool lockMovement = false;
    [HideInInspector]
    public float bonusSpeed = 1;
    /*[HideInInspector]
    public Vector3 childRedAxis;*/
    float airControlForce;
    float hookMovementForce;

    State currentState;

    //Ground Check
    [HideInInspector]
    public bool isGrounded = false;
	float checkRadius = 1.0f;
	LayerMask groundLayer;

	//Fast Fall
	/*float fastFallSpeed = 200;
	// Value under which vertical joystick input will trigger fastFall
	float fastFallVerticalThreshold = - 0.5f;
	// Value over which horizontal joystick input will cancel fastFall 
	float fastFallHorizontalThreshold = 0.1f;*/

	[SerializeField] public string playerNumber;

    void Start()
	{
        //S'il y a une erreur ici s'assurer que le prefab "Balancing" est bien dans la scène
        balanceData = GameObject.Find("Balancing").GetComponent<Balancing>();

        hookMovementForce = balanceData.hookMovementForce;
        airControlForce = balanceData.airControlForce;
        maxSpeed = balanceData.maxSpeedPlayer;
        checkRadius = balanceData.checkRadius;
        groundLayer = balanceData.groundLayer;
        /*fastFallSpeed = balanceData.fastFallSpeed;
        fastFallVerticalThreshold = balanceData.fastFallVerticalThreshold;
        fastFallHorizontalThreshold = balanceData.fastFallHorizontalThreshold;  */      

        rigid = this.GetComponent<Rigidbody2D> ();
    }

    void FixedUpdate()
    {
		isGrounded = Physics2D.OverlapCircle (feetPos.position, checkRadius, groundLayer);

        if (isGrounded && currentState != State.hooked)
        {
            currentState = State.grounded;
        }
        else if(currentState != State.hooked)
        {
            currentState = State.inAir;
        }

		//Movement Lock
		if (isGrounded && Input.GetButton ("Lock" + playerNumber))
			lockMovement = true;
		else
			lockMovement = false;

        switch (currentState)
        {
            case State.grounded:
                //Movement
                if (!lockMovement && !lockMovementKnockBack)
                {
                    //Handling player direction
                    if (Input.GetAxisRaw("Horizontal" + playerNumber) > 0)
                    {
                        playerDirection = 1;
                    }
                    if (Input.GetAxisRaw("Horizontal" + playerNumber) < 0)
                    {
                        playerDirection = -1;
                    }

                    //Input -> start moving
                    if (Input.GetAxisRaw("Horizontal" + playerNumber) != 0)
                    {
                        float acceleration = Mathf.SmoothDamp(0, playerDirection * maxSpeed * bonusSpeed, ref xVelocity, smoothTime);
                        rigid.velocity = new Vector2(acceleration, rigid.velocity.y);
                    }
                }
                break;
            case State.hooked:
                //rigid.AddForce(Vector3.right * Input.GetAxisRaw("Horizontal"+playerNumber) * hookMovementForce);

                //Déplacement grâce au référenciel de l'enfant
                //rigid.AddForce(childRedAxis * Input.GetAxis("Horizontal" + playerNumber) * hookMovementForce);

                //Déplacement selon la flèche
                if((jointDirection.x >= 0 && jointDirection.y >= -.5f && jointDirection.y <= .5f) 
                || (jointDirection.x <= 0 && jointDirection.y >= -.5f && jointDirection.y <= .5f))
                {//LEFT & RIGHT
                    rigid.AddForce(new Vector2(0, Input.GetAxis("Vertical" + playerNumber)) * hookMovementForce);
                }
                else if((jointDirection.y >= 0 && jointDirection.x >= -.5f && jointDirection.x <= .5f)
                || (jointDirection.y <= 0 && jointDirection.x >= -.5f && jointDirection.x <= .5f))
                {//BOT & TOP
                    rigid.AddForce(new Vector2(Input.GetAxis("Horizontal" + playerNumber), 0) * hookMovementForce);
                }
                break;
            case State.inAir:
                rigid.AddForce(Vector3.right * Input.GetAxisRaw("Horizontal" + playerNumber) * airControlForce);
                break;
            default:
                break;
        }

        //FastFall
        /*if (Input.GetAxisRaw("Vertical" + playerNumber) < fastFallVerticalThreshold
            && !isGrounded
            && Mathf.Abs(Input.GetAxisRaw("Horizontal" + playerNumber)) < fastFallHorizontalThreshold
            && currentState == State.inAir
            )
		{
			float acceleration = Mathf.SmoothDamp(0, -1 * fastFallSpeed, ref xVelocity, smoothTime);
			rigid.velocity = new Vector2(rigid.velocity.x, acceleration);
		}*/
    }

    public void StateHooked()
    {
        currentState = State.hooked;
    }

    public void StateNotHooked()
    {
        currentState = State.inAir;
    }

    enum State
    {
        grounded, hooked, inAir
    }
}