using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour {

	//AIM
	public float offset;
	public GameObject Player;
	private Vector2 screenPoint;

	//SHOT
	public GameObject projectile;
	public Transform shotPoint;
	public float timeBtwShots = 1;
    private bool hookInCD;
    GameObject currentProjectile;
    string playerNumber;


	// Use this for initialization
	void Start () {
        playerNumber = Player.GetComponent<PlayerMovement>().playerNumber;
    }
	
	// Update is called once per frame
	void Update () {

        transform.position = Player.transform.position;
        screenPoint.x = (Input.GetAxis("Horizontal" + playerNumber));
        screenPoint.y = (Input.GetAxis("Vertical" + playerNumber));
        float rotZ = Mathf.Atan2(screenPoint.y, screenPoint.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);

		if (Input.GetButtonDown ("Hook" + playerNumber) && !hookInCD) 
		{
            currentProjectile = Instantiate(projectile, shotPoint.position, transform.rotation);
            currentProjectile.GetComponent<Projectile>().playerNumber = playerNumber;
            hookInCD = true;
            Invoke("ResetHookCD", timeBtwShots);
            Debug.Log("Shoot");
		} 

		else if (Input.GetButtonUp ("Hook" + playerNumber) && currentProjectile != null) 
		{
            currentProjectile.GetComponent<Projectile>().Destruction();
			Debug.Log ("Destroy");
		}		
	}

    void ResetHookCD()
    {
        hookInCD = false;
    }
}