using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour {

    Balancing balanceData;

    //JOINT
    DistanceJoint2D joint;
    float distanceMax; // distance of the hook
    bool jointNotCreated = true;
    [HideInInspector]
    public PlayerMovement playerMovement;
    float retractationStep;
    [SerializeField]
    LayerMask layerMaskRaycast;
    [SerializeField]
    LayerMask layerMaskArrow;
	float timeHooked;
	float timeRemaining;

    //AIM
    //float offset;
	public GameObject player;
	private Vector2 screenPoint;
    [HideInInspector]
    public LineRenderer line;
    BoxCollider2D lineCollider;

	//SHOT
    [SerializeField]
	GameObject projectile;
    float timeBtwShots;
    private bool hookInCD;
    [HideInInspector]
    public GameObject currentProjectile;
    string playerNumber;

    //ARROW 
    float knockBackTime;
    float knockBackShieldHit;
    float arrowDamage;
    float velocityArrowDamageRatio;
    Vector2 start1;
    Vector2 start2;
    Vector2 end;
    RaycastHit2D arrowEdge1;
    RaycastHit2D arrowEdge2;
    bool damageAlreadyApplied;
    float knockBackForceTwoArrows;
    float knockBackPlayerHit;
    Sprite arrowSprite;
    Sprite shieldSprite;
    SpriteRenderer spriteRenderer;
    bool switchingState;
    [HideInInspector]
    public HookState currentState;
    PolygonCollider2D[] colliders;
    PolygonCollider2D arrowCollider;
    PolygonCollider2D shieldCollider ;

	//COLOR
	Color colorRope;
	float t;

	// Pause menu
	public bool isFrozen = false;

    private void Awake()
    {
        joint = player.AddComponent<DistanceJoint2D>();
        joint.enabled = false;
    }

    public enum HookState
    {
        Arrow, Shield
    }   

    // Use this for initialization
    void Start () {
        //S'il y a une erreur ici s'assurer que le prefab "Balancing" est bien dans la scène
        balanceData = GameObject.Find("Balancing").GetComponent<Balancing>();

        colliders = GetComponents<PolygonCollider2D>();
        arrowCollider = colliders[0];
        shieldCollider = colliders[1];
        arrowCollider.enabled = true;

		timeHooked = balanceData.TimeHooked;
        distanceMax = balanceData.distanceMaxHook;
        retractationStep = balanceData.retractationStep;
        //offset = balanceData.offsetHook;
        timeBtwShots = balanceData.timeBtwShots;
        knockBackTime = balanceData.knockBackTime;
        knockBackShieldHit = balanceData.knockBackShieldHit;
        knockBackPlayerHit = balanceData.knockBackPlayerHit;
        arrowDamage = balanceData.arrowDamage;
        velocityArrowDamageRatio = balanceData.velocityArrowDamageRatio;
        arrowSprite = balanceData.arrowSprite;
        shieldSprite = balanceData.shieldSprite;

        timeRemaining = timeHooked;

        playerNumber = player.GetComponent<PlayerMovement>().playerNumber;
        playerMovement = player.GetComponent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = arrowSprite;
		colorRope = projectile.GetComponent<SpriteRenderer> ().color;

        line = new GameObject("Line").AddComponent<LineRenderer>();//instantie un line renderer
        line.positionCount = 2; //le nombre de point pour la ligne
        line.startWidth = balanceData.lineWidth;// la largeur de la ligne
        line.endWidth = balanceData.lineWidth;
        line.gameObject.SetActive(false);// désactive la ligne
		line.startColor = colorRope;
		line.endColor = colorRope;
		line.GetComponent<Renderer>().material.shader = Shader.Find("Particles/Alpha Blended");
        line.GetComponent<Renderer>().material.color = Color.black;// couleur du matérial
        line.transform.parent = gameObject.transform.parent;
        
        line.gameObject.AddComponent<BoxCollider2D>();
        line.gameObject.AddComponent<LineCutter>();
        line.gameObject.GetComponent<LineCutter>().line = this;
        lineCollider = line.GetComponent<BoxCollider2D>();
        lineCollider.isTrigger = true;   

        line.gameObject.tag = "Rope";
        switch  (playerNumber){
            case "_P1":
            line.gameObject.layer = 17;
            break;
            case "_P2":
            line.gameObject.layer = 18;
            break;
            case "_P3":
            line.gameObject.layer = 19;
            break;
            case "_P4":
            line.gameObject.layer = 20;
            break;
            default:
            print("Default case switch start Hook.cs");
            break;
        }     
    }
	
	// Update is called once per frame
	void Update () {

		if (!isFrozen)
		{
			if(Input.GetButtonDown("Item"+playerNumber) && !switchingState){
				switchingState = true;
				switch (currentState){
				case HookState.Arrow:
					spriteRenderer.sprite = shieldSprite;
					currentState = HookState.Shield;
					arrowCollider.enabled = false;
					shieldCollider.enabled = true;
					break;
				case HookState.Shield:
					spriteRenderer.sprite = arrowSprite;
					currentState = HookState.Arrow;
					arrowCollider.enabled = true;
					shieldCollider.enabled = false;
					break;
				default:
					break;
				}
				Invoke("ResetCDSwitch",0.1f);
			}

			transform.position = player.transform.position;
			/*screenPoint.x = (Input.GetAxis("Horizontal" + playerNumber));
        screenPoint.y = (Input.GetAxis("Vertical" + playerNumber));
        float rotZ = Mathf.Atan2(screenPoint.y, screenPoint.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);*/
			//lineCollider.transform.rotation = Quaternion.FromToRotation(Vector3.right, (endPos - startPos).normalized);
			if(Input.GetAxisRaw("Horizontal"+playerNumber) != 0 || Input.GetAxisRaw("Vertical"+playerNumber) != 0)
				transform.rotation = Quaternion.FromToRotation(Vector3.right,new Vector3(Input.GetAxis("Horizontal"+playerNumber),Input.GetAxis("Vertical"+playerNumber)));

			/*start1 = transform.GetChild(0).GetComponent<Transform>().position;
        start2 = transform.GetChild(1).GetComponent<Transform>().position;
        end = transform.GetChild(2).GetComponent<Transform>().position;

			Debug.DrawLine(start1,end,Color.red);
			Debug.DrawLine(start2,end,Color.red);

			arrowEdge1 = Physics2D.Linecast(start1,end,layerMaskArrow);
			arrowEdge2 = Physics2D.Linecast(start2,end,layerMaskArrow);

			damageAlreadyApplied = false;

        if(arrowEdge1.collider != null){
            if(arrowEdge1.collider.gameObject.CompareTag("Player")){
                arrowEdge1.collider.gameObject.GetComponent<PlayerLifeManager>().
                TakeDamage(arrowDamage + 
                playerMovement.rigid.velocity.magnitude / velocityArrowDamageRatio,gameObject, true);
                damageAlreadyApplied = true;
            } else if (arrowEdge1.collider.gameObject.CompareTag("Arrow")){
                Vector2 directionKnockBack = (arrowEdge1.collider.gameObject.transform.position - transform.position).normalized;
                playerMovement.rigid.AddForce(-directionKnockBack * knockBackForceTwoArrows, ForceMode2D.Impulse);
                damageAlreadyApplied = true;
            }
        }
        if(arrowEdge2.collider != null && !damageAlreadyApplied){
            if(arrowEdge2.collider.gameObject.CompareTag("Player")){
                arrowEdge2.collider.gameObject.GetComponent<PlayerLifeManager>().
                TakeDamage(arrowDamage + 
                playerMovement.rigid.velocity.magnitude / velocityArrowDamageRatio,gameObject, true);
            } else if(arrowEdge2.collider.gameObject.CompareTag("Arrow")){
                Vector2 directionKnockBack = (arrowEdge2.collider.gameObject.transform.position - transform.position).normalized;
                playerMovement.rigid.AddForce(-directionKnockBack * knockBackForceTwoArrows, ForceMode2D.Impulse);
                damageAlreadyApplied = true;
            }
        }*/



			if (currentProjectile != null)
			{
				line.SetPosition(0, player.transform.position);
				line.SetPosition(1, currentProjectile.transform.position);

				Vector3 startPos = line.GetPosition(0);
				Vector3 endPos = line.GetPosition(1);

				lineCollider.size = new Vector3(Vector3.Distance(startPos,endPos),balanceData.lineWidth,0);
				lineCollider.transform.position = (startPos + endPos) / 2;
				lineCollider.transform.rotation = Quaternion.FromToRotation(Vector3.right, (endPos - startPos).normalized);


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
						t = 0;
						player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
						joint.enabled = true;
						joint.connectedBody = currentProjectile.GetComponent<Rigidbody2D>();
						joint.distance = Vector3.Distance(currentProjectile.transform.position, player.transform.position);
						joint.maxDistanceOnly = true;
						jointNotCreated = false;
					}

					timeRemaining -= Time.deltaTime;
					if(timeRemaining <= timeHooked/2)
						t += (Time.deltaTime / timeRemaining)/2;
					line.startColor = Color.Lerp(colorRope, Color.black,t);
					line.endColor = Color.Lerp(colorRope, Color.black,t);

					if (timeRemaining <= 0) 
					{
						StartCoroutine("ResetHookCD");
						playerMovement.StateNotHooked();
						player.GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezeRotation;
						joint.enabled = false;
						currentProjectile.GetComponent<Projectile>().End();
						line.gameObject.SetActive(false);
						jointNotCreated = true;
						timeRemaining = timeHooked;
						line.startColor = colorRope;
						line.endColor = colorRope;
					}


					Vector3 jointDirection = (currentProjectile.transform.position - player.transform.position).normalized;

					playerMovement.jointDirection = jointDirection;

					RaycastHit2D checkToJoint = Physics2D.Raycast(player.transform.position, jointDirection, .85f, layerMaskRaycast);
					RaycastHit2D checkOppositeToJoint = Physics2D.Raycast(player.transform.position, -jointDirection, .85f, layerMaskRaycast);

					Debug.DrawRay(player.transform.position, -jointDirection * .85f, Color.red, 5);

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
				line.startColor = colorRope;
				line.endColor = colorRope;
				line.gameObject.SetActive(true);
				line.SetPosition(0, player.transform.position);
				currentProjectile = Instantiate(projectile, transform.position, transform.rotation);
				currentProjectile.transform.parent = gameObject.transform.parent;
				currentProjectile.GetComponent<Projectile>().playerNumber = playerNumber;
				currentProjectile.GetComponent<Projectile>().hook = this;
				line.SetPosition(1, currentProjectile.transform.position);

				Vector3 startPos = line.GetPosition(0);
				Vector3 endPos = line.GetPosition(1);

				lineCollider.size = new Vector3(Vector3.Distance(startPos,endPos),balanceData.lineWidth,0);
				lineCollider.transform.position = (startPos + endPos) / 2;
				lineCollider.transform.rotation = Quaternion.FromToRotation(Vector3.right, (endPos - startPos).normalized);

				hookInCD = true;
			}

			else if (Input.GetButtonUp("Hook" + playerNumber) && currentProjectile != null)
			{
				DisableRope();
			}
		}
    }

    public void DisableRope(){
        StartCoroutine("ResetHookCD");
        playerMovement.StateNotHooked();
        player.GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezeRotation;
        joint.enabled = false;
        currentProjectile.GetComponent<Projectile>().End();
        line.gameObject.SetActive(false);
        jointNotCreated = true;
        timeRemaining = timeHooked;
        line.startColor = colorRope;
        line.endColor = colorRope;
    }

    public IEnumerator ResetHookCD()
    {
        yield return new WaitForSeconds(timeBtwShots);
        hookInCD = false;
    }

    void OnCollisionEnter2D(Collision2D collision){
        if(currentState == HookState.Arrow){
            if(collision.gameObject.CompareTag("Player")){
                collision.gameObject.GetComponent<PlayerLifeManager>().TakeDamage(arrowDamage + 
                playerMovement.rigid.velocity.magnitude / velocityArrowDamageRatio,gameObject, true);
            } 
            else if (collision.gameObject.CompareTag("Arrow"))
            {
                playerMovement.lockMovementKnockBack = true;
                Vector2 directionKnockBack = (collision.gameObject.transform.position - transform.position).normalized;
                playerMovement.rigid.velocity = Vector3.zero;
                switch (collision.gameObject.GetComponent<Hook>().currentState){
                    case HookState.Arrow:
                        playerMovement.rigid.AddForce(-directionKnockBack * knockBackPlayerHit, ForceMode2D.Impulse);
                        
                    break;
                    case HookState.Shield:
                        playerMovement.rigid.AddForce(-directionKnockBack * knockBackShieldHit, ForceMode2D.Impulse);
                    break;
                    default:
                    break;
                }
                Invoke("UnlockMovement", knockBackTime);
            }
        }
        
    }

    void UnlockMovement()
    {
        playerMovement.lockMovementKnockBack = false;
    }

    void ResetCDSwitch(){
        switchingState = false;
    }
}