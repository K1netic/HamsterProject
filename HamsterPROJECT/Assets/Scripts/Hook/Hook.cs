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


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        transform.position = Player.transform.position;
        screenPoint.x = (Input.GetAxis("Horizontal"));
        screenPoint.y = (Input.GetAxis("Vertical"));
        float rotZ = Mathf.Atan2(screenPoint.y, screenPoint.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);

		if (Input.GetButtonDown ("Hook") && !hookInCD) 
		{
            currentProjectile = Instantiate(projectile, shotPoint.position, transform.rotation);
            hookInCD = true;
            Invoke("ResetHookCD", timeBtwShots);
            Debug.Log("Shoot");
		} 

		else if (Input.GetButtonUp ("Hook") && currentProjectile != null) 
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