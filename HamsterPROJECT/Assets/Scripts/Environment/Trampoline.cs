﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Trampoline : MonoBehaviour {

    Balancing balanceData;
	float knockBackTime;
	float knockBackTrampoline;
	PlayerMovement playerMovement;

	[SerializeField] bool isFlat = false;

	void Start()
	{
		balanceData = GameObject.Find("Balancing").GetComponent<Balancing>();
		knockBackTime = balanceData.knockBackTime;
		knockBackTrampoline = balanceData.knockBackTrampoline;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
			//Point de contact entre le joueur et le trampoline
			Vector3 contact = collision.GetContact(0).point;
			Vector3 center = Vector3.zero;
			//Centre du trampoline
			if (!isFlat)
				center = this.transform.position;
			else if (isFlat)
				center = new Vector3(contact.x, transform.position.y, 0);
			//Rayon incident
			Vector3 incident = collision.transform.position - contact;
			//Normale
			Vector3 normale = contact - center;
			//Rayon réfléchi
			Vector3 reflected = - Vector3.Reflect(incident, normale).normalized;
			Knockback(collision.gameObject, reflected);
		}
	}

	void Knockback(GameObject player, Vector3 direction)
	{
		playerMovement.hookScript.DisableRope(false);
        playerMovement.rigid.AddForce(direction * knockBackTrampoline, ForceMode2D.Impulse);
	}
}
