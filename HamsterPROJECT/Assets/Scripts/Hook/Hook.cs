using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour {

    //JOINT
    DistanceJoint2D joint;
    public float distanceMax = 10f; // distance of the hook
    bool jointNotCreated = true;
    PlayerMovement playerMovement;
    public float retractationStep;
    int layerMask;

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
        layerMask = ~(1 << 8);

        playerNumber = player.GetComponent<PlayerMovement>().playerNumber;
        playerMovement = player.GetComponent<PlayerMovement>();

        joint = player.AddComponent<DistanceJoint2D>();
        joint.enabled = false;

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

        if (currentProjectile != null)
        {
            line.SetPosition(0, player.transform.position);
            line.SetPosition(1, currentProjectile.transform.position);

            if(Vector3.Distance(currentProjectile.transform.position,player.transform.position) > distanceMax)
            {
                currentProjectile.GetComponent<Projectile>().Destruction();
                line.gameObject.SetActive(false);
            }

            if (currentProjectile.GetComponent<Projectile>().hooked)
            {
                playerMovement.StateHooked();
                if (jointNotCreated)
                {
                    joint.enabled = true;
                    joint.connectedBody = currentProjectile.GetComponent<Rigidbody2D>();
                    joint.distance = Vector3.Distance(currentProjectile.transform.position, player.transform.position);
                    joint.maxDistanceOnly = true;
                    jointNotCreated = false;
                }

                Vector3 jointDirection = (currentProjectile.transform.position - player.transform.position).normalized;
                RaycastHit2D checkToJoint = Physics2D.Raycast(player.transform.position, jointDirection, .75f, layerMask);
                RaycastHit2D checkOppositeToJoint = Physics2D.Raycast(player.transform.position, -jointDirection, .75f, layerMask);
                
                if(Input.GetAxisRaw("RT"+ playerNumber) < 0 && checkToJoint.collider == null)
                {
                    joint.distance -= retractationStep;
                }

                if (Input.GetAxisRaw("LT" + playerNumber) > 0 && checkOppositeToJoint.collider == null)
                {
                    if(joint.distance < distanceMax - retractationStep)
                    {
                        joint.distance += retractationStep;
                        joint.maxDistanceOnly = false;
                    }
                }
                else
                {
                    joint.maxDistanceOnly = true;
                }
            }
        }

        if (Input.GetButtonDown("Hook" + playerNumber) && !hookInCD)
        {
            line.gameObject.SetActive(true);
            line.SetPosition(0, player.transform.position);
            currentProjectile = Instantiate(projectile, shotPoint.position, transform.rotation);
            currentProjectile.GetComponent<Projectile>().playerNumber = playerNumber;
            currentProjectile.GetComponent<Projectile>().line = line;
            line.SetPosition(1, currentProjectile.transform.position);
            hookInCD = true;
            
        }

        else if (Input.GetButtonUp("Hook" + playerNumber) && currentProjectile != null)
        {
            Invoke("ResetHookCD", timeBtwShots);
            playerMovement.StateNotHooked();
            joint.enabled = false;
            currentProjectile.GetComponent<Projectile>().Destruction();
            line.gameObject.SetActive(false);
            jointNotCreated = true;
        }
    }

    void ResetHookCD()
    {
        hookInCD = false;
    }
}