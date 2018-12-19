using UnityEngine;
using System.Collections;

public class ExplodeOnClick : MonoBehaviour {

	float speed;
	bool grosZizi;


	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (!grosZizi) {
			if (collision.gameObject.CompareTag ("Player")) {
				print (speed);
				speed = collision.gameObject.GetComponent<Rigidbody2D> ().velocity.magnitude;
				if (speed > 10f) {
					GameObject.FindObjectOfType<ExplosionForce> ().doExplosion (transform.position, speed);
					grosZizi = true;
				}
			}
		}
	}
		
}
