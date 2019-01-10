using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    Balancing balanceData;

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

    void Start(){
        //S'il y a une erreur ici s'assurer que le prefab "Balancing" est bien dans la scène
        balanceData = GameObject.Find("Balancing").GetComponent<Balancing>();

        speed = balanceData.speedHookhead;
        hookheadDamage = balanceData.hookheadDamage;

        rigid = GetComponent<Rigidbody2D>();
        /*direction = new Vector3(Input.GetAxis("Horizontal" + playerNumber), Input.GetAxis("Vertical" + playerNumber), 0);
		direction = direction.normalized;
        if(direction == Vector3.zero)
        {
            direction = Vector3.right;
        }*/
	}

	void Update () {
        if (!hooked)
        {
            rigid.AddForce(direction / speed);
            RaycastNoBounce();
        }
	}

    void RaycastNoBounce()
    {
        if (!inDestruction)
        {
            Debug.DrawRay(transform.position, direction, Color.black);
            raycast = Physics2D.Raycast(transform.position, direction, 1f, hook.layerMaskRaycast);
            if (raycast.collider != null)
            {
                //S'accroche si jamais le gameObject à le bon tag
                if (raycast.collider.gameObject.CompareTag("Hookable"))
                {
                    GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                    hooked = true;
                    transform.position = raycast.point;
                }
            }
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
        //Rend le projectile jusqu'à ce qu'il soit détruit pour que la fonction ResetHookCD puisse s'effectuer entièrement
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        Invoke("End",balanceData.timeBtwShots+1);
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Vérifie les collisions uniquement si le projectile n'est pas pas aggripé
        if (!hooked)
        {
            //S'accroche si jamais le gameObject à le bon tag
            /*if (collision.gameObject.CompareTag("Hookable"))
            {
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                hooked = true;
            }
            //Inflige des dégâts et détruit le projectile s'il touche un player
            else*/ if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<PlayerLifeManager>().TakeDamage(hookheadDamage,gameObject,true);
                Destruction();
            }
            
            else if(!collision.gameObject.CompareTag("Hookable"))
            {
                Destruction();
            }
        }
    }
    //Appel la méthode qui permet de couper la corde si le grappin est attaché
    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.CompareTag("Rope")){
            col.gameObject.GetComponent<LineCutter>().CutRope();
        }else if (col.gameObject.CompareTag("Laser")){
            Destruction();
        }
    }


    public void End()
    {
        Destroy(gameObject);
    }
}
