using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour {

	bool startExplode = false;
	bool oneAnim = true;
	float timer = 20;
	[SerializeField]
	Color startColor;
	[SerializeField]
	Color endColor;
	[SerializeField]
	GameObject bomb;
	[SerializeField]
	SpriteRenderer bombSprite;
	[SerializeField]
	Animator animatorBomb;
	[SerializeField]
	GameObject size;
	public float speedAnim = 1;
	[SerializeField]
	ParticleSystem particles;
	Collider2D[] deathOverlap = new Collider2D[4];
	[SerializeField]
	float deathRadius;
	public LayerMask layerMaskDeath;

	// Update is called once per frame
	void Update () {
		if (startExplode) {
			Explode ();
		}
	}

	void Explode(){
		if (oneAnim == true) {
			animatorBomb.SetBool ("StartAnim", true);
			oneAnim = false;
		}

		bombSprite.color = Color.Lerp (startColor, endColor, 1/timer);
		speedAnim += 0.001f;
		animatorBomb.speed = speedAnim;
		timer = timer - Time.deltaTime;
		if (timer < 0) {
			Instantiate (particles, transform.position, transform.rotation);
			deathOverlap = Physics2D.OverlapCircleAll (transform.position, deathRadius, layerMaskDeath);
			foreach (Collider2D player in deathOverlap) {
				if (player.gameObject.CompareTag ("Player")) {
					player.gameObject.GetComponent<PlayerLifeManager> ().TakeDamage (15000f,bomb,false);
				}
			}
			Destroy (bomb);
		}
		size.transform.localScale =new Vector3 (speedAnim,speedAnim,1);
	}

	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.tag == "Player") {
			startExplode = true;
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere (transform.position, deathRadius);
	}

}
