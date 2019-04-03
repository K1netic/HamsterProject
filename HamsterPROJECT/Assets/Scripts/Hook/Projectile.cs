﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    Balancing balanceData;

    [Range(0f, 1f)]
    [SerializeField]
    float raycastRange = 1;

    [SerializeField]
    GameObject child;
    [SerializeField]
    ParticleSystem hitHookOrange;
    [SerializeField]
    ParticleSystem hitHookPink;
    [SerializeField]
    ParticleSystem hitHookGreen;
    [SerializeField]
    ParticleSystem hitHookYellow;
    [SerializeField]
    ParticleSystem hitHookBlue;
    ParticleSystem hitHook;

    float speed;
    [HideInInspector]
	public Vector3 direction;
    float hookheadDamage;
    Rigidbody2D rigid;
    bool inDestruction;
    [HideInInspector]
    public string playerNumber;
    [HideInInspector]
    public bool hooked;
    [HideInInspector]
    public Hook hook;
    RaycastHit2D raycast;
    RaycastHit2D raycastLeft;
    RaycastHit2D raycastRight;
    RaycastHit2D raycastRope;
    RaycastHit2D raycastBackRope;
    PolygonCollider2D coll;
    [HideInInspector]
    public Vector2 pivot;

    GameObject hookedObject;

    private void Awake()
    {
        pivot = pivot = child.transform.position;
    }

    void Start(){
        //S'il y a une erreur ici s'assurer que le prefab "Balancing" est bien dans la scène
        balanceData = GameObject.Find("Balancing").GetComponent<Balancing>();

        speed = balanceData.speedHookhead;
        hookheadDamage = balanceData.hookheadDamage;

        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<PolygonCollider2D>();

        switch (playerNumber)
        {
            case "_P1":
                gameObject.layer = 12;
                break;
            case "_P2":
                gameObject.layer = 13;
                break;
            case "_P3":
                gameObject.layer = 14;
                break;
            case "_P4":
                gameObject.layer = 15;
                break;
            default:
                print("Default case switch start Projectile.cs");
                break;
        }

        switch (hook.player.GetComponent<SpriteRenderer>().sprite.name)
        {
            case "Perso1":
                hitHook = hitHookOrange;
                break;
            case "Perso2":
                hitHook = hitHookPink;
                break;
            case "Perso3":
                hitHook = hitHookGreen;
                break;
            case "Perso4":
                hitHook = hitHookYellow;
                break;
            case "Perso5":
                hitHook = hitHookBlue;
                break;
            default:
                break;
        }

        /*direction = new Vector3(Input.GetAxis("Horizontal" + playerNumber), Input.GetAxis("Vertical" + playerNumber), 0);
		direction = direction.normalized;
        if(direction == Vector3.zero)
        {
            direction = Vector3.right;
        }*/
    }

	void Update () {
        pivot =child.transform.position;
        switch (hooked)
        {
            case true:
                if (hookedObject.CompareTag("Untagged"))
                {
                    hook.DisableRope(false);
                }
                break;
            case false:
                rigid.AddForce(direction / speed);
                RaycastNoBounce();
                RaycastRope();
                break;
            default:
                break;
        }
        hook.line.SetPosition(1, pivot);
    }

    void RaycastNoBounce()
    {
        if (!inDestruction)
        {
            raycast = Physics2D.Raycast(transform.position, direction, raycastRange, hook.layerMaskRaycast);
            raycastLeft = Physics2D.Raycast((Vector2)coll.transform.TransformPoint(coll.points[13]), direction, raycastRange, hook.layerMaskRaycast);
            raycastRight = Physics2D.Raycast((Vector2)coll.transform.TransformPoint(coll.points[37]), direction, raycastRange, hook.layerMaskRaycast);
            if (raycast.collider != null)
            {
                //S'accroche si jamais le gameObject à le bon tag
                if (raycast.collider.gameObject.CompareTag("Hookable"))
                {
                    GetHooked(raycast.point, raycast.collider.gameObject);
                }
                else if (raycastLeft.collider.gameObject.CompareTag("Hookable"))
                {
                    GetHooked(raycastLeft.point, raycastLeft.collider.gameObject);
                }
                else if (raycastRight.collider.gameObject.CompareTag("Hookable"))
                {
                    GetHooked(raycastRight.point, raycastRight.collider.gameObject);
                }
            }
        }
    }

    void RaycastRope()
    {
        raycastRope = Physics2D.Raycast(transform.position, direction, raycastRange, hook.layerMaskLineCast);
        raycastBackRope = Physics2D.Raycast(transform.position, -direction, raycastRange * 2.25f, hook.layerMaskLineCast);
        if (raycastRope.collider)
        {
            if (raycastRope.collider.gameObject.CompareTag("Rope"))
            {
                raycastRope.collider.gameObject.GetComponent<LineCutter>().CutRope(transform.position, playerNumber);
            }      
        }else if (raycastBackRope.collider)
            if (raycastBackRope.collider.gameObject.CompareTag("Rope"))
            {
                raycastBackRope.collider.gameObject.GetComponent<LineCutter>().CutRope(transform.position, playerNumber);
            }
    }

    void GetHooked(Vector2 position, GameObject platform)
    {
        rigid.bodyType = RigidbodyType2D.Static;
        hooked = true;
        transform.position = position;
        hookedObject = platform;
        Instantiate(hitHook, transform.position, transform.rotation);
        gameObject.transform.parent = hookedObject.transform;
        if (hookedObject.GetComponent<Rigidbody2D>())
        {
            gameObject.layer = 27;
            rigid.bodyType = RigidbodyType2D.Kinematic;
            rigid.interpolation = RigidbodyInterpolation2D.None;
        }
    }

	void Destruction(){
        inDestruction = true;
        //Désactive la corde
        if (hook.line.gameObject.activeSelf)
        {
            hook.line.gameObject.SetActive(false);
        }
        StartCoroutine(hook.ResetHookCD());
        //Rend le projectile invisible jusqu'à ce qu'il soit détruit pour que la fonction ResetHookCD puisse s'effectuer entièrement
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        Invoke("End",balanceData.timeBtwShots+1);
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Vérifie les collisions uniquement si le projectile n'est pas pas aggripé
        if (!hooked)
        {
            switch (collision.gameObject.tag)
            {
                case "Player":
                    //Inflige des dégâts et détruit le projectile s'il touche un player
                    if (!hook.cantAttack)
                    {
                        GameObject.Find("SlowMo").GetComponent<SlowMotion>().DoSlowmotion();
                        PlayerLifeManager foeScript = collision.gameObject.GetComponent<PlayerLifeManager>();
                        Instantiate(hitHook, transform.position, transform.rotation);
                        //Appelle la méthode du fx avant celle des dégâts pour qu'elle ne soit pas bloqué par le recovery
                        foeScript.HitFX(collision.GetContact(0).point, 0);
                        foeScript.TakeDamage(hookheadDamage, gameObject, true);
                        hook.VibrationOnTouchingPlayerWithHookhead();
                        Destruction();
                    }
                    else
                    {
                        Instantiate(hitHook, transform.position, transform.rotation);
                        hook.VibrationOnProjectileDestroyed();
                        Destruction();
                    }
                    break;
                case "Hook":
                    Instantiate(hitHook, transform.position, transform.rotation);
                    hook.VibrationOnProjectileDestroyed();
                    AudioManager.instance.PlaySound("doubleHookContact", hook.playerMovement.playerNumber + "Hook");
                    Destruction();
                    break;
                case "Hookable":
                    GetHooked(collision.GetContact(0).point, collision.gameObject);
                    AudioManager.instance.PlaySound("hookContactScraps", hook.playerMovement.playerNumber + "Hook");
                    break;
                default:
                    if(collision.gameObject.layer != 25)//scraps
                    {
                        Instantiate(hitHook, transform.position, transform.rotation);
                        hook.VibrationOnProjectileDestroyed();
                        Destruction();
                    }
                    else
                    {
                        Instantiate(hitHook, transform.position, transform.rotation);
                        hook.VibrationOnProjectileDestroyed();
                        Destruction();
                    }
                    break;
            }
        }
    }

    //Appelle la méthode qui permet de couper la corde si le grappin est attaché
    void OnTriggerEnter2D(Collider2D col){
        if (col.gameObject.CompareTag("Rope"))
        {
            col.gameObject.GetComponent<LineCutter>().CutRope(transform.position, playerNumber);
        }
    }

    public void End()
    {
        if(gameObject)
            Destroy(gameObject);
    }
}
