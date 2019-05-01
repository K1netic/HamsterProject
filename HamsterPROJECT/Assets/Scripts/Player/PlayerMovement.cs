using UnityEngine;
using UnityEngine.UI;
using InControl;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    Balancing balanceData;

    //SERIALIZED OBJECT
    [SerializeField]
    GameObject dashEcho;
    [SerializeField]
    GameObject speedEffect;
    [SerializeField]
    GameObject shootPos;

    //PLAYER'S INFORMATIONS
    [HideInInspector]
    public Rigidbody2D rigid;
    [HideInInspector]
    public Vector2 jointDirection;
    Sprite playerSprite;
    [HideInInspector]
    public Hook hookScript;
    public string playerNumber;
    public InputDevice playerInputDevice;

    //MOVEMENT
    [SerializeField]
    public LayerMask layerMaskGround;
    //[HideInInspector]
    public bool lockMovement;
    [HideInInspector]
    public float speed;
    public State currentState;
    float airControlForce;
    float hookMovementForce;
    float groundedControlForce;
    SpeedEffect speedEffectScript;

    //DASH
    [HideInInspector]
    public Vector2 playerDirection;
    [HideInInspector]
    public bool dashRecoveryWithHook;
    //[HideInInspector]
    public bool lockMovementDash;
    [HideInInspector]
    public float gravity;
    float dashTime;
    float dashForce;
    float dashCDTime;
    float drag;
    float dragEndOfDash;
    float inDashStatusTime;
    bool dashInCD;
    Vector2 lastFramePosition;
    GameObject dashRecovery;

    //DASHEFFECT 
    Gradient dashEffectGradient = new Gradient();
    GradientColorKey[] colorDashEffect = new GradientColorKey[2];
    GradientAlphaKey[] alphaDashEffect = new GradientAlphaKey[2];
    ParticleSystem.ColorOverLifetimeModule dashEffectColorLifeTime;
    ParticleSystem.MainModule dashEffectColor;
    GameObject dashReadyEffect;

    void Start()
	{
        /*
        lockMovement = false;
        speed = 0;
        lockMovementDash = false;
        dashReadyEffect.SetActive(true);
        dashInCD = false;*/

        Application.targetFrameRate = 60;
        //S'il y a une erreur ici s'assurer que le prefab "Balancing" est bien dans la scène
        balanceData = GameObject.Find("Balancing").GetComponent<Balancing>();

        groundedControlForce = balanceData.groundedControlForce;
        hookMovementForce = balanceData.hookMovementForce;
        airControlForce = balanceData.airControlForce;
        dashTime = balanceData.dashTime;
        dashForce = balanceData.dashForce;
        dashCDTime = balanceData.attackCDTime;
        dragEndOfDash = balanceData.dragEndOfDash;
        dashRecoveryWithHook = balanceData.attackRecoveryWithHook;
        inDashStatusTime = balanceData.inDashStatusTime;

        rigid = this.GetComponent<Rigidbody2D>();
        speedEffectScript = speedEffect.GetComponent<SpeedEffect>();
        dashReadyEffect = transform.GetChild(2).gameObject;
        dashEffectColor = dashReadyEffect.GetComponent<ParticleSystem>().main;
        dashEffectColorLifeTime = dashReadyEffect.GetComponent<ParticleSystem>().colorOverLifetime;

        dashRecovery = Resources.Load<GameObject>("Prefabs/Dash/DashRecovery");

        gravity = rigid.gravityScale;

        playerSprite = GetComponent<SpriteRenderer>().sprite;

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
        GetComponent<PlayerLifeManager>().playerLayer = gameObject.layer;

        lastFramePosition = transform.position;

        //Switch gérant la couleur du dash effect
        switch (playerSprite.name)
        {
            case "0":
                dashEffectColor.startColor = new Color(1,1,0);
                colorDashEffect[1].color = new Color(.9215686f, 0.7294118f, 0.345098f);
                break;
            case "1":
                dashEffectColor.startColor = new Color(0.9960784f, 0.5686275f, 0.7568628f);
                colorDashEffect[1].color = new Color(0.9960784f, 0.5686275f, 0.7568628f);
                break;
            case "2":
                dashEffectColor.startColor = new Color(0, 1, 1);
                colorDashEffect[1].color = new Color(0.2313726f, 0.572549f, 0.9882353f);
                break;
            case "3":
                dashEffectColor.startColor = new Color(.3f,1,0);
                colorDashEffect[1].color = new Color(0.4627451f, 0.7372549f, 0.2862745f);
                break;
            case "4":
                dashEffectColor.startColor = new Color(1, 0, 0);
                colorDashEffect[1].color = new Color(0.9098039f, 0.1176471f, 0.3176471f);
                break;
            default:
                print("Default case switch start PlayerMovement.cs");
                break;
        }
        alphaDashEffect[0].alpha = 1;
        alphaDashEffect[0].time = 0;
        alphaDashEffect[1].alpha = 1;
        alphaDashEffect[1].time = 1;
        colorDashEffect[0].color = new Color(1, 1, 1);
        colorDashEffect[0].time = 0;
        colorDashEffect[1].time = 1;
        Gradient grad = new Gradient();
        grad.SetKeys(colorDashEffect, alphaDashEffect);
        dashEffectColorLifeTime.color = grad;
    }

    void FixedUpdate()
    {
        if (MatchStart.gameHasStarted)
        {
            speed = rigid.velocity.magnitude;
            speedEffectScript.playerSpeed = speed;

            CheckState();
            Movement();
            Dash();
        }
    }

    void CheckState()
    {
        if (Physics2D.OverlapCircle(transform.position, 1.2f, layerMaskGround))
        {
            if(currentState != State.hooked)
            {
                currentState = State.grounded;
            }
        }else if (currentState != State.hooked)
        {
            currentState = State.inAir;
        }
    }

    void Dash()
    {
        if (!dashInCD)
            CancelInvoke("ResetDashCD");
        if (!lockMovement && !lockMovementDash)
        {
            if (playerInputDevice.RightBumper.WasPressed && !dashInCD)
            {
                dashReadyEffect.SetActive(false);
                dashInCD = true;
                lockMovementDash = true;
                hookScript.BladeChoice(speed);
                StartCoroutine("DoDash");
            }
        }
    }

    IEnumerator DoDash()
    {
        Invoke("StopDash", dashTime);
        Invoke("UnlockMovementDash", inDashStatusTime);
        if (lockMovement)
            yield break;
        rigid.velocity = Vector3.zero;
        rigid.gravityScale = 0;
        yield return new WaitForSeconds(.05f);
        rigid.gravityScale = gravity;
        Invoke("ResetDashCD", dashCDTime);
        if (lockMovement)
            yield break;
        rigid.AddForce((shootPos.transform.position - transform.position).normalized * dashForce, ForceMode2D.Impulse);
        InvokeRepeating("DashEffect", 0, 0.04f);
        Invoke("CancelDashEffect", dashTime * 3.5f);
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
                case State.grounded:
                    rigid.AddForce(Vector3.right * playerInputDevice.LeftStickX.Value * groundedControlForce);
                    break;
                case State.hooked:
                    if (speed < 10)
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
        dashReadyEffect.SetActive(true);
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

    public enum State
    {
        inAir, hooked, grounded,
    }

	IEnumerator CancelVibration(float delay)
	{
		yield return new WaitForSeconds (delay);
		playerInputDevice.StopVibration ();
	}
}