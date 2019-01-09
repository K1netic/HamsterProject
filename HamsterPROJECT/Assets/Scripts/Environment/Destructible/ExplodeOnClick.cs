using UnityEngine;
using System.Collections;

public class ExplodeOnClick : MonoBehaviour {

	float speed;
	bool explosionDone;


	private void OnCollisionEnter2D(Collision2D collision)
	{
        if (collision.gameObject.CompareTag("Player"))
        {
            speed = collision.gameObject.GetComponent<PlayerMovement>().speed;
            if (!explosionDone)
                {
                if (speed > 10f) {
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
