using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour {

    Balancing balanceData;

    //JOINT
    DistanceJoint2D joint;
    float distanceMax; // distance of the hook
    bool jointNotCreated = true;
    PlayerMovement playerMovement;
    float retractationStep;
    int layerMask;

    //AIM
    float offset;
	public GameObject player;
	private Vector2 screenPoint;
    [HideInInspector]
    public LineRenderer line;

	//SHOT
    [SerializeField]
	GameObject projectile;
	public Transform shotPoint;
    float timeBtwShots;
    private bool hookInCD;
    [HideInInspector]
    public GameObject currentProjectile;
    string playerNumber;

    private void Awake()
    {
        joint = player.AddComponent<DistanceJoint2D>();
        joint.enabled = false;
    }

    // Use this for initialization
    void Start () {
        //S'il y a une erreur ici s'assurer que le prefab "Balancing" est bien dans la scène
        balanceData = GameObject.Find("Balancing").GetComponent<Balancing>();

        distanceMax = balanceData.distanceMaxHook;
        retractationStep = balanceData.retractationStep;
        offset = balanceData.offsetHook;
        timeBtwShots = balanceData.timeBtwShots;

        switch (gameObject.layer)
        {
            case 12:
                layerMask = ~(1 << 8);
                break;
            case 13:
                layerMask = ~(1 << 9);
                break;
            case 14:
                layerMask = ~(1 << 10);
                break;
            case 15:
                layerMask = ~(1 << 11);
                break;
            default:
                break;
        }

        playerNumber = player.GetComponent<PlayerMovement>().playerNumber;
        playerMovement = player.GetComponent<PlayerMovement>();

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
                StartCoroutine("ResetHookCD");
                currentProjectile.GetComponent<Projectile>().End();
                line.gameObject.SetActive(false);
            }

            if (currentProjectile.GetComponent<Projectile>().hooked)
            {
                playerMovement.StateHooked();
                
                if (jointNotCreated)
                {
                    player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                    joint.enabled = true;
                    joint.connectedBody = currentProjectile.GetComponent<Rigidbody2D>();
                    joint.distance = Vector3.Distance(currentProjectile.transform.position, player.transform.position);
                    joint.maxDistanceOnly = true;
                    jointNotCreated = false;
                }

                Vector3 jointDirection = (currentProjectile.transform.position - player.transform.position).normalized;

                playerMovement.jointDirection = jointDirection;

                RaycastHit2D checkToJoint = Physics2D.Raycast(player.transform.position, jointDirection, .75f, layerMask);
                RaycastHit2D checkOppositeToJoint = Physics2D.Raycast(player.transform.position, -jointDirection, .75f, layerMask);

                Debug.DrawRay(player.transform.position, jointDirection, Color.red, 5);

                if(Input.GetAxisRaw("RT"+ playerNumber) < 0 && checkToJoint.collider == null)
                {
                    joint.distance -= retractationStep;
                }

                if (Input.GetAxisRaw("LT" + playerNumber) > 0 && checkOppositeToJoint.collider == null)
                {
                    if (joint.distance < distanceMax - retractationStep)
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
            currentProjectile.GetComponent<Projectile>().hook = this;
            line.SetPosition(1, currentProjectile.transform.position);
            hookInCD = true;
            
        }

        else if (Input.GetButtonUp("Hook" + playerNumber) && currentProjectile != null)
        {
            StartCoroutine("ResetHookCD");
            playerMovement.StateNotHooked();
            player.GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezeRotation;
            joint.enabled = false;
            currentProjectile.GetComponent<Projectile>().End();
            line.gameObject.SetActive(false);
            jointNotCreated = true;
        }
    }

    public IEnumerator ResetHookCD()
    {
        yield return new WaitForSeconds(timeBtwShots);
        hookInCD = false;
    }
}