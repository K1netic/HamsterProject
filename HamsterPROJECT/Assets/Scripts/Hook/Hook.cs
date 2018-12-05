using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour {

	//AIM
	public float offset;
	public GameObject player;
	private Vector2 screenPoint;
    private LineRenderer line;

	//SHOT
	public GameObject projectile;
	public Transform shotPoint;
	public float timeBtwShots = 1;
    private bool hookInCD;
    GameObject currentProjectile;
    string playerNumber;


	// Use this for initialization
	void Start () {
        playerNumber = player.GetComponent<PlayerMovement>().playerNumber;

        line = new GameObject("Line").AddComponent<LineRenderer>();//instantie un line renderer
        line.positionCount = 2; //le nombre de point pour la ligne
        line.startWidth = .05f;// la largeur de la ligne
        line.endWidth = .05f;
        line.gameObject.SetActive(false);// désactive la ligne
        line.startColor = Color.black;
        line.endColor = Color.black;
        line.GetComponent<Renderer>().material.color = Color.black;// couleur du matérial
    }
	
	// Update is called once per frame
	void Update () {

        transform.position = player.transform.position;
        screenPoint.x = (Input.GetAxis("Horizontal" + playerNumber));
        screenPoint.y = (Input.GetAxis("Vertical" + playerNumber));
        float rotZ = Mathf.Atan2(screenPoint.y, screenPoint.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);

		if (Input.GetButtonDown ("Hook" + playerNumber) && !hookInCD) 
		{
            line.gameObject.SetActive(true);
            line.SetPosition(0, player.transform.position);
            currentProjectile = Instantiate(projectile, shotPoint.position, transform.rotation);
            currentProjectile.GetComponent<Projectile>().playerNumber = playerNumber;
            hookInCD = true;
            Invoke("ResetHookCD", timeBtwShots);
            Debug.Log("Shoot");
		} 

		else if (Input.GetButtonUp ("Hook" + playerNumber) && currentProjectile != null) 
		{
            currentProjectile.GetComponent<Projectile>().Destruction();
            line.gameObject.SetActive(false);
            Debug.Log ("Destroy");
		}		

        if(currentProjectile != null)
        {
            line.SetPosition(0, player.transform.position);
            line.SetPosition(1, currentProjectile.transform.position);
        }
	}

    void ResetHookCD()
    {
        hookInCD = false;
    }
}