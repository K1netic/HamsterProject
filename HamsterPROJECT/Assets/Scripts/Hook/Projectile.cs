using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    Balancing balanceData;

    float speed;
	private Vector3 direction;

    [HideInInspector]
    public string playerNumber;
    [HideInInspector]
    public bool hooked;
    [HideInInspector]
    public Hook hook;

    void Start(){
        //S'il y a une erreur ici s'assurer que le prefab "Balancing" est bien dans la scène
        balanceData = GameObject.Find("Balancing").GetComponent<Balancing>();

        speed = balanceData.speedHookhead;

        direction = new Vector3(Input.GetAxis("Horizontal" + playerNumber), Input.GetAxis("Vertical" + playerNumber), 0);
		direction = direction.normalized;
        if(direction == Vector3.zero)
        {
            direction = Vector3.right;
        }
	}

	void Update () {
        if (!hooked)
        {
            transform.position += direction / speed;
        } 
	}

	public void Destruction(){
        if (hook.line.gameObject.activeSelf)
        {
            hook.line.gameObject.SetActive(false);
        }
        StartCoroutine(hook.ResetHookCD());
        Destroy (gameObject);
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hooked)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Destruction();
            }
            if (collision.gameObject.CompareTag("Hookable"))
            {
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                hooked = true;
            }
            else
            {
                Destruction();
            }
        }
    }
}
