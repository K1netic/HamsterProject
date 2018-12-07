﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    Balancing balanceData;

    [SerializeField]
    Text lifeText;
    int playerHP;

	Rigidbody2D rigid;
	public Transform feetPos;
    [HideInInspector]
    public bool hooked;

	//Movement
    float maxSpeed = 100;
	float smoothTime = 0.3f;
	float xVelocity = 0.0f;
    int playerDirection = 1;
	bool lockMovement = false;
    [HideInInspector]
    public float bonusSpeed = 1;

    State currentState;

    //Ground Check
    [HideInInspector]
    public bool isGrounded = false;
	float checkRadius = 1.0f;
	LayerMask groundLayer;

	//Fast Fall
	float fastFallSpeed = 200;
	// Value under which vertical joystick input will trigger fastFall
	float fastFallVerticalThreshold = - 0.5f;
	// Value over which horizontal joystick input will cancel fastFall 
	float fastFallHorizontalThreshold = 0.1f;

	[SerializeField] public string playerNumber;

    void Start()
	{
        //S'il y a une erreur ici s'assurer que le prefab "Balancing" est bien dans la scène
        balanceData = GameObject.Find("Balancing").GetComponent<Balancing>();

        maxSpeed = balanceData.maxSpeedPlayer;
        checkRadius = balanceData.checkRadius;
        groundLayer = balanceData.groundLayer;
        fastFallSpeed = balanceData.fastFallSpeed;
        fastFallVerticalThreshold = balanceData.fastFallVerticalThreshold;
        fastFallHorizontalThreshold = balanceData.fastFallHorizontalThreshold;
        playerHP = balanceData.playerMaxHP;

        rigid = this.GetComponent<Rigidbody2D> ();

        UpdateLifeUI();
    }

    void FixedUpdate()
    {      
        if(playerHP <= 0)
        {
            Dead();
        }

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
                break;
            case State.hooked:
                GetComponent<Rigidbody2D>().AddForce(Vector3.right * Input.GetAxisRaw("Horizontal"+playerNumber) * 25);
                
                break;
            case State.inAir:
                break;
            default:
                break;
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

    public void TakeDamage(int damage)
    {
        playerHP -= damage;
        UpdateLifeUI();
    }

    void UpdateLifeUI()
    {
        lifeText.text = "Current HP : " + playerHP;
    }

    void Dead()
    {
        Destroy(transform.parent.gameObject);
    }
}