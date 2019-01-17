using UnityEngine;
using System.Collections;

public class ExplodeOnClick : MonoBehaviour {

    Balancing balanceData;

	float speed;
    float threshold;
	bool explosionDone;

    private void Start()
    {
        //S'il y a une erreur ici s'assurer que le prefab "Balancing" est bien dans la scène
        balanceData = GameObject.Find("Balancing").GetComponent<Balancing>();

        threshold = balanceData.speedThresholdDestruction;
    }

    private void OnCollisionEnter2D(Collision2D collision)
	{
        if (collision.gameObject.CompareTag("Player"))
        {
            speed = collision.gameObject.GetComponent<PlayerMovement>().speed;
            if (!explosionDone)
                {
                if (speed > threshold) {
					GameObject.FindObjectOfType<ExplosionForce> ().doExplosion (transform.position, speed);
                    explosionDone = true;
				}
			}
		}
	}

    private void OnMouseDown()
    {
        GameObject.FindObjectOfType<ExplosionForce>().doExplosion(transform.position, speed);
    }

}
