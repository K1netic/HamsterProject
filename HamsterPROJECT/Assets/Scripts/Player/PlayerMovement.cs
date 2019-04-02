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
    public Vector2 jointDirection;
    [SerializeField]
    GameObject dashEcho;
    [SerializeField]
    GameObject speedEffect;

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
    [HideInInspector]
    public bool dashRecoveryWithHook;
    float dashTime;
    float dashForce;
    float dashCDTime;
    bool dashInCD;
    [HideInInspector]
    public bool lockMovementDash;
    [SerializeField]
    GameObject shootPos;
    Sprite playerSprite;
    SpeedEffect speedEffectScript;
    float drag;
    Vector2 lastFramePosition;
    [HideInInspector]
    public Vector2 playerDirection;
    float dragEndOfDash;
    [HideInInspector]
    public float gravity;
    float inDashStatusTime;
    GameObject dashRecovery;

    public string playerNumber;
	public InputDevice playerInputDevice;

    float maxSpeed = 76f;

    void Start()
	{
        Application.targetFrameRate = 60;
        //S'il y a une erreur ici s'assurer que le prefab "Balancing" est bien dans la scène
        balanceData = GameObject.Find("Balancing").GetComponent<Balancing>();

        hookMovementForce = balanceData.hookMovementForce;
        airControlForce = balanceData.airControlForce;
        dashTime = balanceData.dashTime;
        dashForce = balanceData.dashForce;
        dashCDTime = balanceData.dashCDTime;
        dragEndOfDash = balanceData.dragEndOfDash;
        dashRecoveryWithHook = balanceData.dashRecoveryWithHook;
        inDashStatusTime = balanceData.inDashStatusTime;

        rigid = this.GetComponent<Rigidbody2D> ();
        speedEffectScript = speedEffect.GetComponent<SpeedEffect>();

        dashRecovery = Resources.Load<GameObject>("Prefabs/Dash/DashRecovery");

        gravity = rigid.gravityScale;

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

        lastFramePosition = transform.position;
    }

    void FixedUpdate()
    {
        if (MatchStart.gameHasStarted)
        {
            speed = rigid.velocity.magnitude;
            speedEffectScript.playerSpeed = speed;

            if (currentState != State.hooked)
                currentState = State.inAir;

            Movement();
            Dash();
        }

		Debug.Log (playerInputDevice);
    }

    private void LateUpdate()
    {
        rigid.velocity = Vector3.ClampMagnitude(rigid.velocity, maxSpeed);
    }

    void Dash()
    {
        if (!dashInCD)
            CancelInvoke("ResetDashCD");
        if (!lockMovement && !lockMovementDash)
        {
            if (playerInputDevice.RightBumper.WasPressed && !dashInCD)
            {
                dashInCD = true;
                lockMovementDash = true;
                StartCoroutine("DoDash");
            }
        }
    }

    IEnumerator DoDash()
    {
        rigid.velocity = Vector3.zero;
        rigid.gravityScale = 0;
        yield return new WaitForSeconds(.1f);
        rigid.gravityScale = gravity;
        rigid.AddForce((shootPos.transform.position - transform.position).normalized * dashForce, ForceMode2D.Impulse);
        InvokeRepeating("DashEffect", 0, 0.04f);
        Invoke("StopDash", dashTime);
        Invoke("UnlockMovementDash", inDashStatusTime);
        Invoke("CancelDashEffect", dashTime * 3.5f);
        Invoke("ResetDashCD", dashCDTime);
        StartCoroutine(CancelVibration(Vibrations.PlayVibration("Dash", playerInputDevice)));
        AudioManager.instance.PlaySound("dash", playerNumber);
    }

    void UnlockMovementDash()
    {
        lockMovementDash = false;
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
        playerDirection = ((Vector2)transform.position - lastFramePosition).normalized;
        speedEffectScript.playerDirection = playerDirection;
        //Switch permettant de gérer le mouvement en fonction de l'état du joueur
        if (!lockMovement && !lockMovementDash)
        {
            switch (currentState)
            {
                case State.hooked:
                    if(speed < 10)
                    {
                        if(playerInputDevice.LeftStickX.Value != 0 || playerInputDevice.LeftStickY.Value != 0)
                            rigid.AddForce((shootPos.transform.position - transform.position).normalized * hookMovementForce);
                    }
                    else
                    {
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
                    }
                    break;
                case State.inAir:
				rigid.AddForce(Vector3.right * playerInputDevice.LeftStickX.Value * airControlForce);
                    break;
                default:
                    break;
            }
        }
        lastFramePosition = transform.position;
    }

	void OnCollisionEnter2D(Collision2D collision)
	{
		// Collision avec plateformes ou joueur
		if (collision.gameObject.layer == 28 || collision.gameObject.layer == 16 || collision.gameObject.tag == "Player")
		{
			StartCoroutine (CancelVibration (Vibrations.PlayVibration("CollisionPlayerPlayer", playerInputDevice)));
            AudioManager.instance.PlaySound("playerHitPlatform",playerNumber);
        }
	}

    public void ResetDashCD()
    {
        if (dashInCD)
        {
            AudioManager.instance.PlaySound("dashRecovery", playerNumber);
            Instantiate(dashRecovery, transform.position, transform.rotation, transform);
        }
            
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

    //Modifie le drag pendant 0.1 secondes pour freiner le dash
    void StopDash()
    {
        if (!lockMovement)
        {
            drag = rigid.drag;
            rigid.drag = dragEndOfDash;
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