using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorBehavior : MonoBehaviour {

    [SerializeField]
    GameObject warning;
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

    Vector3 target;

    GameObject warningInst;

	void Start () {
        target = new Vector3(Random.Range(leftBound, rightBound),targetY, 0);
        transform.rotation = Quaternion.FromToRotation(Vector3.right, -(target - transform.position).normalized);
        warningInst = Instantiate(warning, Vector3.Lerp(transform.position, target, .38f), transform.rotation);
    }

	void Update () {
        transform.position = Vector3.MoveTowards(transform.position, target, speed);
        if (transform.position.y < 23)
            Destroy(warningInst);
        if (transform.position.y < targetY)
            Destroy(gameObject);
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(explosionFX, transform.position, transform.rotation);
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
            }else if (collider.gameObject.CompareTag("Hook"))
            {
                collider.gameObject.GetComponent<Projectile>().hook.DisableRope(false);
            }
        }
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, deathRadius);
    }
}
