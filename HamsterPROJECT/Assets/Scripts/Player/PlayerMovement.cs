using UnityEngine;
using UnityEngine.UI;
using InControl;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    Balancing balanceData;

    [HideInInspector]
	public Rigidbody2D rigid;
    [HideInInspector]
    public bool hooked;
    [HideInInspector]
    public Vector2 jointDirection;
    [SerializeField]
    GameObject dashEcho;

	//Movement
	
    [HideInInspector]
    public bool lockMovement;
    /*[HideInInspector]
    public Vector3 childRedAxis;*/
    float airControlForce;
    float hookMovementForce;
    [SerializeField]
    State currentState;
    [HideInInspector]
    public float speed;

    //Dash
    float dashTime;
    float dashForce;
    float dashCDTime;
    bool dashInCD;
    bool lockMovementDash;
    [SerializeField]
    GameObject shootPos;
    Sprite playerSprite;

    float drag;

    //Fast Fall
    /*float smoothTime = 0.3f;
    float xVelocity = 0.0f;
    float fastFallSpeed = 200;
	// Value under which vertical joystick input will trigger fastFall
	float fastFallVerticalThreshold = - 0.5f;
	// Value over which horizontal joystick input will cancel fastFall 
	float fastFallHorizontalThreshold = 0.1f;*/

    public string playerNumber;
	public InputDevice playerInputDevice;

    void Start()
	{
        //S'il y a une erreur ici s'assurer que le prefab "Balancing" est bien dans la scène
        balanceData = GameObject.Find("Balancing").GetComponent<Balancing>();

        hookMovementForce = balanceData.hookMovementForce;
        airControlForce = balanceData.airControlForce;
        dashTime = balanceData.dashTime;
        dashForce = balanceData.dashForce;
        dashCDTime = balanceData.dashCDTime;
        /*fastFallSpeed = balanceData.fastFallSpeed;
        fastFallVerticalThreshold = balanceData.fastFallVerticalThreshold;
        fastFallHorizontalThreshold = balanceData.fastFallHorizontalThreshold;  */

        rigid = this.GetComponent<Rigidbody2D> ();
        playerSprite = GetComponent<SpriteRenderer>().sprite;

        switch (playerNumber)
        {
            case "_P1":
                gameObject.layer = 8;
                gameObject.GetComponent<PlayerLifeManager>().layerMaskDeath = (1 << 9) | (1 << 10) | (1 << 11);
                break;
            case "_P2":
                gameObject.layer = 9;
                gameObject.GetComponent<PlayerLifeManager>().layerMaskDeath = (1 << 8) | (1 << 10) | (1 << 11);
                break;
            case "_P3":
                gameObject.layer = 10;
                gameObject.GetComponent<PlayerLifeManager>().layerMaskDeath = (1 << 8) | (1 << 9) | (1 << 11);
                break;
            case "_P4":
                gameObject.layer = 11;
                gameObject.GetComponent<PlayerLifeManager>().layerMaskDeath = (1 << 8) | (1 << 9) | (1 << 10);
                break;
            default:
                break;
        }
    }

    void FixedUpdate()
    {

        speed = rigid.velocity.magnitude;

        if (currentState != State.hooked)
			currentState = State.inAir;

		Movement();
		Dash();

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

    void Dash()
    {
        if (!lockMovement && !lockMovementDash)
        {
			if (playerInputDevice.RightBumper.WasPressed && !dashInCD)
            {
                dashInCD = true;
                lockMovementDash = true;
                rigid.AddForce((shootPos.transform.position - transform.position).normalized* dashForce, ForceMode2D.Impulse);
                InvokeRepeating("DashEffect", 0, 0.04f);
                Invoke("UnlockMovementDash", dashTime);
                Invoke("CancelDashEffect", dashTime * 3.5f);
                Invoke("ResetDashCD", dashCDTime);
				playerInputDevice.Vibrate (0f, balanceData.mediumVibration);
				StartCoroutine (CancelVibration (balanceData.smallVibrationDuration));
            }
        }
    }

    void DashEffect()
    {
        GameObject effect = Instantiate(dashEcho, transform.position, transform.rotation);
        effect.GetComponent<EchoFade>().playerSprite = playerSprite;
    }

    void CancelDashEffect()
    {
        CancelInvoke("DashEffect");
    }

    void Movement()
    {
        //Switch permettant de gérer le mouvement en fonction de l'état du joueur
        if (!lockMovement && !lockMovementDash)
        {
            switch (currentState)
            {
                case State.hooked:
                    //Il faut imaginer l'espace découpé selon les diagonales avec comme centre la tete de grappin, cela découpe alors l'espace en 4 triangles
                    //Test si le joueur est dans un des 2 triangles de gauche ou de droite
                    if ((jointDirection.x >= 0 && jointDirection.y >= -.5f && jointDirection.y <= .5f)
                    || (jointDirection.x <= 0 && jointDirection.y >= -.5f && jointDirection.y <= .5f))
                    {//LEFT & RIGHT
					rigid.AddForce(new Vector2(0, playerInputDevice.LeftStickY.Value) * hookMovementForce);
                    }
                    //Test si le joueur est en haut ou en bas
                    else if ((jointDirection.y >= 0 && jointDirection.x >= -.5f && jointDirection.x <= .5f)
                    || (jointDirection.y <= 0 && jointDirection.x >= -.5f && jointDirection.x <= .5f))
                    {//BOT & TOP
					rigid.AddForce(new Vector2(playerInputDevice.LeftStickX.Value, 0) * hookMovementForce);
                    }
                    break;
                case State.inAir:
				rigid.AddForce(Vector3.right * playerInputDevice.LeftStickX.Value * airControlForce);
                    break;
                default:
                    break;
            }
        }
    }

	void OnCollisionEnter2D(Collision2D collision)
	{
		// Collision avec plateformes ou joueur
		if (collision.gameObject.layer == 16 || collision.gameObject.tag == "Player")
		{
			playerInputDevice.Vibrate (0f, balanceData.lightVibration);
			StartCoroutine (CancelVibration (balanceData.mediumVibrationDuration));
		}
	}

    void ResetDashCD()
    {
        dashInCD = false;
    }

    public void StateHooked()
    {
        currentState = State.hooked;
    }

    public void StateNotHooked()
    {
        currentState = State.inAir;
    }

    //Rend le contrôle au joueur et modifie le drag pendant 0.1 secondes pour freiner le dash
    void UnlockMovementDash()
    {
        lockMovementDash = false;
        if (!lockMovement)
        {
            drag = rigid.drag;
            rigid.drag = 10;
            Invoke("ResetDrag", .1f);
        }
    }

    void ResetDrag()
    {
        rigid.drag = drag;
    }

    enum State
    {
        inAir, hooked,
    }

	IEnumerator CancelVibration(float delay)
	{
		yield return new WaitForSeconds (delay);
		playerInputDevice.StopVibration ();
	}
}