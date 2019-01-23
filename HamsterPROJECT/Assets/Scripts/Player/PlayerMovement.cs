using UnityEngine;
using UnityEngine.UI;
using InControl;

public class PlayerMovement : MonoBehaviour
{
    Balancing balanceData;

    [HideInInspector]
	public Rigidbody2D rigid;
    [HideInInspector]
    public bool hooked;
    [HideInInspector]
    public Vector2 jointDirection;

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

    float drag;

    Text counter;

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

        counter = GameObject.Find("Counter" + playerNumber).GetComponent<Text>();

        switch (playerNumber)
        {
            case "_P1":
                gameObject.layer = 8;
                break;
            case "_P2":
                gameObject.layer = 9;
                break;
            case "_P3":
                gameObject.layer = 10;
                break;
            case "_P4":
                gameObject.layer = 11;
                break;
            default:
                break;
        }
    }

    void FixedUpdate()
    {

		UpdateSpeed();

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

    void UpdateSpeed()
    {
        speed = rigid.velocity.magnitude;
        counter.text = (int)speed + " km/h";
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
                Invoke("UnlockMovementDash", dashTime);
                Invoke("ResetDashCD", dashCDTime);
            }
        }
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
}