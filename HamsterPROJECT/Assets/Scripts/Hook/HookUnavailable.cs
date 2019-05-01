using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookUnavailable : MonoBehaviour {
	
	[SerializeField] GameObject ball;

	// Update is called once per frame
	void OnEnable () 
	{
		//S'assure que la position de la fléche et toujours aligné sur celle du player
        transform.position = new Vector3(ball.transform.position.x + 1.5f, ball.transform.position.y + 1.5f);
	}

	void Update () 
	{
		//S'assure que la position de la fléche et toujours aligné sur celle du player
        transform.position = new Vector3(ball.transform.position.x + 1.5f, ball.transform.position.y + 1.5f);
	}
}
