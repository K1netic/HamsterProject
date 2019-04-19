using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorBehavior : MonoBehaviour {

    [SerializeField]
    float speed = .25f;
    [SerializeField]
    float triggerDistance = .5f;
    [SerializeField]
    ParticleSystem explosionFX;
    [SerializeField]
    float deathRadius;
    [SerializeField]
    LayerMask layerMaskDeath;
    Collider2D[] deathOverlap;

    public float leftBound;
    public float rightBound;
    public float targetY;

    //GameObject[] potentialTargets;
    Vector3 target;

	void Start () {
        /*potentialTargets = GameObject.FindGameObjectsWithTag("Hookable");
        target = potentialTargets[Random.Range(0, potentialTargets.Length)];
        while (target.gameObject.name == "Contour")
        {
            target = potentialTargets[Random.Range(0, potentialTargets.Length)];
        }*/

        target = new Vector3(Random.Range(leftBound, rightBound),targetY, 0);
        transform.rotation = Quaternion.FromToRotation(Vector3.right, -(target - transform.position).normalized);
    }

	void Update () {
        transform.position = Vector3.MoveTowards(transform.position, target, speed);
        if (transform.position == target)
            Destroy(gameObject);
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print(collision.gameObject.name);
            /*Instantiate(explosionFX, transform.position, transform.rotation);
            deathOverlap = Physics2D.OverlapCircleAll(transform.position, deathRadius, layerMaskDeath);
            foreach (Collider2D collider in deathOverlap)
            {
                if (collider.gameObject.CompareTag("Player"))
                {
                    collider.gameObject.GetComponent<PlayerLifeManager>().TakeDamage(50, gameObject, true);
                }
                else if (collider.gameObject.GetComponent<ExplodeOnClick>())
                {
                    collider.gameObject.GetComponent<ExplodeOnClick>().Explosion(collider.gameObject.transform.position, speed);
                }
            }
            Destroy(gameObject);*/
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, deathRadius);
    }
}
