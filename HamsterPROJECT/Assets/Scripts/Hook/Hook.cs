using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Hook : MonoBehaviour {

    [SerializeField]
    bool manualSwitchOn;
    [SerializeField]
    ParticleSystem hitLittle;
    [SerializeField]
    ParticleSystem hitHard;

    Balancing balanceData;

    //JOINT
    DistanceJoint2D joint;
    bool jointNotCreated = true;
    [HideInInspector]
    public PlayerMovement playerMovement;
    float retractationStep;
    [SerializeField]
    public LayerMask layerMaskRaycast;//Layer qui bloque le changement de la distance max du joint
    //[SerializeField]
    //public LayerMask layerMaskArrow;//Layer qui gère les collisions de la flèche dans DontGoThroughThings (anciennement utilisé pour le raycast de la flèche)
	float timeHooked;
	float timeRemaining;
    Vector3 jointDirection;
    RaycastHit2D checkOppositeToJoint;
    RaycastHit2D checkToJoint ;
    public LayerMask layerMaskLineCast;
    float initialDistance;
    [HideInInspector]
    public bool hooked;
    Projectile stockedProjectileScript;
    float timeBeforeDestroy;

    //AIM
    //float offset;
    public GameObject player;
	private Vector2 screenPoint;
    [HideInInspector]
    public LineRenderer line;
    BoxCollider2D lineCollider;
    Vector3 shootPos;

	//SHOT
    [SerializeField]
	GameObject projectile;
    float timeBtwShots;
    private bool hookInCD;
    [HideInInspector]
    public GameObject currentProjectile;
    string playerNumber;
    Projectile projectileScript;
    Vector3 startPos;
    Vector3 endPos;

    //ARROW 
    float knockBackTime;
    float knockBackShieldHit;
    float arrowDamage;
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
    Sprite hookSprite;
    SpriteRenderer spriteRenderer;
    bool switchingState;
    [SerializeField]
    public HookState currentState;
    PolygonCollider2D[] colliders;
    PolygonCollider2D arrowCollider;
    PolygonCollider2D shieldCollider ;
    Texture rope;
    float criticalSpeed;
    float freezeFrameDuration;
    [HideInInspector]
    public bool cantAttack;

    //COLOR
    Color colorRope;
	float t;

	// Pause menu
	public bool isFrozen = false;
    private bool doubleFXprotection;

    private void Awake()
    {
        //Ajoute le joint au player dans l'awake pour être sur de pouvoir y accéder dans le start des autres scripts
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

		timeHooked = balanceData.TimeHooked;
        retractationStep = balanceData.retractationStep;
        //offset = balanceData.offsetHook;
        timeBtwShots = balanceData.timeBtwShots;
        knockBackTime = balanceData.knockBackTime;
        knockBackShieldHit = balanceData.knockBackShieldHit;
        knockBackPlayerHit = balanceData.knockBackPlayerHit;
        arrowDamage = balanceData.arrowDamage;
        criticalSpeed = balanceData.criticalSpeed;
        timeBeforeDestroy = balanceData.timeRopeCut;

        timeRemaining = timeHooked;

        playerNumber = player.GetComponent<PlayerMovement>().playerNumber;
        playerMovement = player.GetComponent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player.GetComponent<PlayerLifeManager>().spriteArrow = spriteRenderer;
        player.GetComponent<PlayerLifeManager>().hookScript = this;

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
        rope = Resources.Load<Texture>("ArrowSprites/Rope");
        line.material.SetTexture("_MainTex", rope);
        
        //Ajoutet un collider à la corde ainsi que le script qui permet de la couper
        line.gameObject.AddComponent<BoxCollider2D>();
        line.gameObject.AddComponent<LineCutter>();
        line.gameObject.GetComponent<LineCutter>().hook = this;
        lineCollider = line.GetComponent<BoxCollider2D>();
        lineCollider.isTrigger = true;   
        line.gameObject.tag = "Rope";
        line.sortingOrder = 2;

        //Charge les sprites en fonction du personnage sélectionné
        switch (player.GetComponent<SpriteRenderer>().sprite.name)
        {
            case "Perso1":
                arrowSprite = Resources.Load<Sprite>("ArrowSprites/Peak1");
                shieldSprite = Resources.Load<Sprite>("ArrowSprites/Shield1");
                hookSprite = Resources.Load<Sprite>("ArrowSprites/hook1");
                colorRope = new Color(.784f, .451f, .173f);
                break;
            case "Perso2":
                arrowSprite = Resources.Load<Sprite>("ArrowSprites/Peak2");
                shieldSprite = Resources.Load<Sprite>("ArrowSprites/Shield2");
                hookSprite = Resources.Load<Sprite>("ArrowSprites/hook2");
                colorRope = new Color(.596f, .31f,.624f);
                break;
            case "Perso3":
                arrowSprite = Resources.Load<Sprite>("ArrowSprites/Peak3");
                shieldSprite = Resources.Load<Sprite>("ArrowSprites/Shield3");
                hookSprite = Resources.Load<Sprite>("ArrowSprites/hook3");
                colorRope = new Color(0.310f, 0.624f, 0.318f);
                break;
            case "Perso4":
                arrowSprite = Resources.Load<Sprite>("ArrowSprites/Peak4");
                shieldSprite = Resources.Load<Sprite>("ArrowSprites/Shield4");
                hookSprite = Resources.Load<Sprite>("ArrowSprites/hook4");
                colorRope = new Color(.847f, .761f, .271f);
                break;
            case "Perso5":
                arrowSprite = Resources.Load<Sprite>("ArrowSprites/Peak5");
                shieldSprite = Resources.Load<Sprite>("ArrowSprites/Shield5");
                hookSprite = Resources.Load<Sprite>("ArrowSprites/hook5");
                colorRope = new Color(.216f, .384f, .529f);
                break;
            default:
                print("Default case switch start Hook.cs");
            break;
        }
        //Initialise le joueur avec la flèche
        spriteRenderer.sprite = arrowSprite;
        arrowCollider.enabled = true;
        currentState = HookState.Arrow;

        //Fixe les différents layer en fonction du numéro du joueur
        switch  (playerNumber){
            case "_P1":
                line.gameObject.layer = 17;
                gameObject.layer = 17;
                //layerMaskArrow = (1 << 9) | (1<< 10) | (1<<11) | (1 << 18) | (1 << 19) | (1 << 20) | (1 << 22) | (1 << 23) | (1 << 24);
                layerMaskLineCast = (1 << 18) | (1 << 19) | (1 << 20) | (1 << 22) | (1 << 23) | (1 << 24);
                break;
            case "_P2":
                line.gameObject.layer = 18;
                gameObject.layer = 18;
                //layerMaskArrow = (1 << 8) | (1 << 10) | (1 << 11) | (1 << 17) | (1 << 19) | (1 << 20) | (1 << 21) | (1 << 23) | (1 << 24);
                layerMaskLineCast = (1 << 17) | (1 << 19) | (1 << 20) | (1 << 21) | (1 << 23) | (1 << 24);
                break;
            case "_P3":
                line.gameObject.layer = 19;
                gameObject.layer = 19;
                //layerMaskArrow = (1 << 8) | (1 << 9) | (1 << 11) | (1 << 17) | (1 << 18) | (1 << 20) | (1 << 21) | (1 << 22) | (1 << 24);
                layerMaskLineCast = (1 << 17) | (1 << 18) | (1 << 20) | (1 << 21) | (1 << 22) | (1 << 24);
                break;
            case "_P4":
                line.gameObject.layer = 20;
                gameObject.layer = 20;
                //layerMaskArrow = (1 << 8) | (1 << 9) | (1 << 10) | (1 << 17) | (1 << 18) | (1 << 19) | (1 << 21) | (1 << 22) | (1 << 23);
                layerMaskLineCast = (1 << 17) | (1 << 18) | (1 << 19) | (1 << 21) | (1 << 22) | (1 << 23);
                break;
            default:
                print("Default case switch start Hook.cs");
            break;
        }
        /*GetComponent<ShieldCollider>().layerMaskRaycast = layerMaskArrow;
        GetComponent<DontGoThroughThings>().layerMask = layerMaskArrow;*/
    }
	
	// Update is called once per frame
	void Update () {
        //S'assure que le joint ne peut pas être actif sans rigidbody connecté
        if (joint.connectedBody == null && joint.enabled){
            joint.enabled = false;
            jointNotCreated = true;
        }

        //Test si le jeu est en pause
		if (!isFrozen)
		{
            if (!playerMovement.lockMovement)
                //Change entre la flèche et le bouclier
		        if ((playerMovement.playerInputDevice.LeftBumper.WasPressed && !switchingState) || manualSwitchOn){
			        ArrowState();
		        }
                UpdateArrow();
		}

        //Vérifie qu'il y a bien un projectile de créé avant d'y accéder
        if (currentProjectile != null)
        {
            UpdateRope();
            hooked = projectileScript.hooked;

            //Test si le grappin est aggripé
            if (projectileScript.hooked)
            {
                //Change l'état du joueur
                playerMovement.StateHooked();

                //Active le joint s'il ne l'est pas encore
                if (jointNotCreated)
                {
                    CreateJoint();
                }

                TimerRope();

                //Si le temps est négatif le grappin est alors détruit
                if (timeRemaining <= 0) 
                {
					StartCoroutine (CancelVibration (Vibrations.PlayVibration("HookDestruction", playerMovement.playerInputDevice)));
					AudioManager.instance.PlaySound("destructionHook", playerMovement.playerNumber + "Hook");
                    DisableRope(false);
                }

                RaycastingDistanceJoint();

                //Permet de s'approcher du joint uniquement s'il n'y a pas de plateforme directement devant le joueur
                if (playerMovement.playerInputDevice.RightTrigger.Value > 0 && checkToJoint.collider == null)
                {
                    joint.distance -= retractationStep * playerMovement.playerInputDevice.RightTrigger.Value;
                    //AudioManager.instance.PlaySound("towing", playerNumber + "Hook");
                }

                //Permet de s'éloigner du joint uniquement s'il n'y a pas de plateforme juste derrière le joueur et que la distance max n'est pas atteinte
				if (playerMovement.playerInputDevice.LeftTrigger.Value > 0 && checkOppositeToJoint.collider == null)
                {
                    joint.distance += retractationStep * playerMovement.playerInputDevice.LeftTrigger.Value;
                    //permet de faire reculer le joueur avec le changement de distance max
                    joint.maxDistanceOnly = false;
                    //AudioManager.instance.PlaySound("untowing", playerNumber + "Hook");
                }
                else
                {
                    joint.maxDistanceOnly = true;
                }   
            }  
        }
        else{
            playerMovement.StateNotHooked();
            hooked = false;
        }

        //Test si le joueur appuye sur le bouton du grappin et que le grappin n'est pas en CD

		if ((playerMovement.playerInputDevice.Action1.WasPressed 
			|| playerMovement.playerInputDevice.Action2.WasPressed
			|| playerMovement.playerInputDevice.Action3.WasPressed
			|| playerMovement.playerInputDevice.Action4.WasPressed)
			&& !hookInCD && !isFrozen && MatchStart.gameHasStarted)
        {
            ThrowHookhead();
        }
        //Si le joueur relache le bouton on désactive le grappin
		else if ((playerMovement.playerInputDevice.Action1.WasReleased 
			|| playerMovement.playerInputDevice.Action2.WasReleased
			|| playerMovement.playerInputDevice.Action3.WasReleased
			|| playerMovement.playerInputDevice.Action4.WasReleased)
			&& currentProjectile != null)
        {
            DisableRope(false);
        }

		else if ((playerMovement.playerInputDevice.Action1.WasReleased 
			|| playerMovement.playerInputDevice.Action2.WasReleased
			|| playerMovement.playerInputDevice.Action3.WasReleased
			|| playerMovement.playerInputDevice.Action4.WasReleased)
			&& currentProjectile != null && isFrozen)
		{
			StartCoroutine(Unfrozen());
		}
    }

    void RaycastingDistanceJoint(){
        //Calcule la direction du joint pour les raycast
        jointDirection = (currentProjectile.transform.position - player.transform.position).normalized;

        playerMovement.jointDirection = jointDirection;

        //Ces raycast permettent de bloquer le changement de distance du joint s'il y a un obstacle
        checkToJoint = Physics2D.Raycast(player.transform.position, jointDirection, 1.1f, layerMaskRaycast);
        checkOppositeToJoint = Physics2D.Raycast(player.transform.position, -jointDirection, 1.1f, layerMaskRaycast);
    }

    void UpdateArrow(){
        //S'assure que la position de la fléche et toujours aligné sur celle du player
        transform.position = player.transform.position;

        /*screenPoint.x = (Input.GetAxis("Horizontal" + playerNumber));
        screenPoint.y = (Input.GetAxis("Vertical" + playerNumber));
        float rotZ = Mathf.Atan2(screenPoint.y, screenPoint.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);*/

        //Gère l'orientation de la flèche en fonction de l'input du player
		if(playerMovement.playerInputDevice.LeftStickX.Value != 0 || playerMovement.playerInputDevice.LeftStickY.Value != 0){
            transform.rotation = Quaternion.FromToRotation(Vector3.right,
				new Vector3(playerMovement.playerInputDevice.LeftStickX.Value, playerMovement.playerInputDevice.LeftStickY.Value));
        }     
    }

    void ArrowState(){
        manualSwitchOn = false;
        switchingState = true;
        switch (currentState){
            case HookState.Arrow:
                spriteRenderer.sprite = shieldSprite;
                currentState = HookState.Shield;
                arrowCollider.enabled = false;
                shieldCollider.enabled = true;
                switch  (playerNumber){
                    case "_P1":
                    gameObject.layer = 21;
                    break;
                    case "_P2":
                    gameObject.layer = 22;
                    break;
                    case "_P3":
                    gameObject.layer = 23;
                    break;
                    case "_P4":
                    gameObject.layer = 24;
                    break;
                    default:
                    break;
                }
                break;
            case HookState.Shield:
                spriteRenderer.sprite = arrowSprite;
                currentState = HookState.Arrow;
                arrowCollider.enabled = true;
                shieldCollider.enabled = false;
                switch  (playerNumber){
                    case "_P1":
                    gameObject.layer = 17;
                    break;
                    case "_P2":
                    gameObject.layer = 18;
                    break;
                    case "_P3":
                    gameObject.layer = 19;
                    break;
                    case "_P4":
                    gameObject.layer = 20;
                    break;
                    default:
                    break;
                }
                break;
            default:
                break;
            }
        AudioManager.instance.PlaySound("switchWeapon", playerNumber+"Arrow");
        //Empeche l'input d'être répété trop vite
        Invoke("ResetCDSwitch",0.1f);
    }

    void UpdateRope(){
        //Aligne la position de la corde sur le player et la tete de grappin
        line.SetPosition(0, player.transform.position);
        //line.SetPosition(1, projectileScript.pivot);
        startPos = line.GetPosition(0);
        endPos = line.GetPosition(1);

        //Aligne le trigger de la corde sur la corde
        lineCollider.size = new Vector3(Vector3.Distance(startPos, endPos), balanceData.lineWidth, 0);
        lineCollider.transform.position = (startPos + endPos) / 2;
        lineCollider.transform.rotation = Quaternion.FromToRotation(Vector3.right, (endPos - startPos).normalized);

        //Gère la déformation de la texture selon la taille de la corde
        float scaleX = Vector3.Distance(line.GetPosition(0), line.GetPosition(1));
        line.GetComponent<LineRenderer>().material.mainTextureScale = new Vector2(scaleX, 1f);
    }

    void CreateJoint(){
        if (playerMovement.dashRecoveryWithHook)
            playerMovement.ResetDashCD();
        t = 0;
        //player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        joint.enabled = true;
        joint.connectedBody = currentProjectile.GetComponent<Rigidbody2D>();
        joint.distance = Vector3.Distance(currentProjectile.transform.position, player.transform.position);
        joint.maxDistanceOnly = true;
        jointNotCreated = false;
    }

    void TimerRope(){
        //Gère le timer du grappin et le changement de couleur de la corde
        timeRemaining -= Time.deltaTime;
        if(timeRemaining <= timeHooked/2)
            t += (Time.deltaTime / timeRemaining)/2;
        line.startColor = Color.Lerp(colorRope, Color.black,t);
        line.endColor = Color.Lerp(colorRope, Color.black,t);
    }

    private void ThrowHookhead(){
        //Active la corde (line renderer) et instancie une tête de grappin
        line.startColor = colorRope;
        line.endColor = colorRope;
        line.gameObject.SetActive(true);
        line.SetPosition(0, player.transform.position);
        currentProjectile = Instantiate(projectile, transform.position, transform.rotation);
        currentProjectile.GetComponent<SpriteRenderer>().sprite = hookSprite;
        projectileScript = currentProjectile.GetComponent<Projectile>();
        shootPos = transform.GetChild(0).GetComponent<Transform>().position;
        projectileScript.direction = (shootPos - transform.position).normalized;
        line.SetPosition(1, currentProjectile.transform.position);
        currentProjectile.transform.parent = gameObject.transform.parent;
        projectileScript.playerNumber = playerNumber;
        projectileScript.hook = this;

        startPos = line.GetPosition(0);
        endPos = line.GetPosition(1);

        //Aligne le collider avec la corde, fait une première fois ici pour être sur qu'il n'y ait pas de frame de retard
        lineCollider.size = new Vector3(Vector3.Distance(startPos,endPos),balanceData.lineWidth,0);
        lineCollider.transform.position = (startPos + endPos) / 2;
        lineCollider.transform.rotation = Quaternion.FromToRotation(Vector3.right, (endPos - startPos).normalized);

        hookInCD = true;

        line.gameObject.GetComponent<LineCutter>().projectile = currentProjectile;

        AudioManager.instance.PlaySound("throwHook", playerNumber + "Hook");
    }

    //Désactive tout ce qui est relatif au grappin
    public void DisableRope(bool cut){
        StartCoroutine("ResetHookCD");
        playerMovement.StateNotHooked();
        //player.GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezeRotation;
        joint.enabled = false;
        if (!cut)
        {
            if(projectileScript)
                projectileScript.End();
        } 
        else
        {
            stockedProjectileScript = projectileScript;
            projectileScript = null;
            currentProjectile = null;
            Invoke("DestroyProjectile", timeBeforeDestroy);
        }
        line.gameObject.SetActive(false);
        jointNotCreated = true;
        timeRemaining = timeHooked;
        line.startColor = colorRope;
        line.endColor = colorRope;
    }

    void DestroyProjectile()
    {
        stockedProjectileScript.End();
    }

    public IEnumerator ResetHookCD()
    {
        yield return new WaitForSeconds(timeBtwShots);
        hookInCD = false;
    }

	IEnumerator Unfrozen()
	{
		yield return new WaitUntil(() => !isFrozen);

		// Disable rope if button is not held at the moment the game is unpaused/unfrozen
		if (!(playerMovement.playerInputDevice.Action1.IsPressed
			|| playerMovement.playerInputDevice.Action2.IsPressed
			|| playerMovement.playerInputDevice.Action3.IsPressed
			|| playerMovement.playerInputDevice.Action4.IsPressed))
		{
			DisableRope (false);
		}
	}

    void OnCollisionEnter2D(Collision2D collision){
        //Arrow - Arrow
        if(currentState == HookState.Arrow)
        {
            if (!cantAttack)
            {
                //Si c'est une fleche qui est touché on applique un knockback dépendant de la nature de la flèche (arrow ou shield)
                if (collision.gameObject.CompareTag("Arrow"))
                {
                    GameObject.Find("SlowMo").GetComponent<SlowMotion>().DoSlowmotion();
                    ArrowHit(collision);
                    HitFX(collision.GetContact(0).point, collision.gameObject);
                }
                //Si le joueur est touché des dégâts lui sont appliqués en les modifiant selon la vitesse de l'attaquant
                else if (collision.gameObject.CompareTag("Player"))
                {
                    RaycastHit2D collisionFail = Physics2D.Linecast(transform.position, collision.gameObject.transform.position, layerMaskLineCast);
                    if (collisionFail.collider != null)
                    {
                        if (collisionFail.collider.gameObject.CompareTag("Arrow"))
                        {
                            GameObject.Find("SlowMo").GetComponent<SlowMotion>().DoSlowmotion();
                            ArrowHit(collisionFail.collider);
                            HitFX(collisionFail.point, collisionFail.collider.gameObject);
                        }
                    }
                    else
                    {
                        PlayerLifeManager foeScript = collision.gameObject.GetComponent<PlayerLifeManager>();
                        //Appelle la méthode du fx avant celle des dégâts pour qu'elle ne soit pas bloqué par le recovery
                        GameObject.Find("SlowMo").GetComponent<SlowMotion>().DoSlowmotion();
                        foeScript.HitFX(collision.GetContact(0).point, playerMovement.speed);
                        foeScript.TakeDamage(arrowDamage + playerMovement.speed, gameObject, true);
                        StartCoroutine(CancelVibration(Vibrations.PlayVibration("CollisionArrowPlayer", playerMovement.playerInputDevice)));
                    }
                }
            }
        }
        else
        {//Shield - Shield
            if (collision.gameObject.CompareTag("Arrow"))
            {
                if(collision.gameObject.GetComponent<Hook>().currentState == HookState.Shield)
                {
                    DoubleShieldCollide(collision);
                    HitFX(collision.GetContact(0).point, collision.gameObject);
                    AudioManager.instance.PlaySound("doubleShieldContact", playerNumber + "Arrow");
                }
            }
        }
        
    }

    public IEnumerator ResetBoolAttack()
    {
        yield return new WaitForSeconds(.5f);
        cantAttack = false;
    }

    void HitFX(Vector3 position, GameObject hook)
    {
        if (!doubleFXprotection && !hook.GetComponent<Hook>().player.GetComponent<PlayerLifeManager>().inRecovery)
        {
            doubleFXprotection = true;
            if (playerMovement.speed > criticalSpeed)
            {
                Instantiate(hitHard, position, transform.rotation);
            }
            else
            {
                Instantiate(hitLittle, position, transform.rotation);
            }
            Invoke("CancelFXProtection", .1f);
        }
           
    }

    void CancelFXProtection()
    {
        doubleFXprotection = false;
    }

    void DoubleShieldCollide(Collision2D collision)
    {
        playerMovement.lockMovement = true;
        Vector2 directionKnockBack = (collision.gameObject.transform.position - transform.position).normalized;
        playerMovement.rigid.velocity = Vector3.zero;
        playerMovement.rigid.AddForce(-directionKnockBack * knockBackPlayerHit, ForceMode2D.Impulse);
        StartCoroutine(CancelVibration(Vibrations.PlayVibration("CollisionArrowShield", playerMovement.playerInputDevice)));
        Invoke("UnlockMovement", knockBackTime);
    }

    void ArrowHit(Collision2D collision)
    {
        playerMovement.lockMovement = true;
        Vector2 directionKnockBack = (collision.gameObject.transform.position - transform.position).normalized;
        playerMovement.rigid.velocity = Vector3.zero;
        switch (collision.gameObject.GetComponent<Hook>().currentState)
        {
            case HookState.Arrow:
                // Collision Arrow - Arrow 
                playerMovement.rigid.AddForce(-directionKnockBack * knockBackPlayerHit, ForceMode2D.Impulse);
                StartCoroutine(CancelVibration(Vibrations.PlayVibration("CollisionArrowArrow", playerMovement.playerInputDevice)));
                AudioManager.instance.PlaySound("arrowHitArrow", playerNumber+"Arrow");
                break;
            case HookState.Shield:
                // Collision Arrow - Shield 
                playerMovement.rigid.AddForce(-directionKnockBack * knockBackShieldHit, ForceMode2D.Impulse);
                StartCoroutine(CancelVibration(Vibrations.PlayVibration("CollisionArrowShield", playerMovement.playerInputDevice)));
                AudioManager.instance.PlaySound("arrowHitShield", playerNumber + "Arrow");
                break;
            default:
                break;
        }
        Invoke("UnlockMovement", knockBackTime);
    }

    void ArrowHit(Collider2D collision)
    {
        playerMovement.lockMovement = true;
        Vector2 directionKnockBack = (collision.gameObject.transform.position - transform.position).normalized;
        playerMovement.rigid.velocity = Vector3.zero;
        switch (collision.gameObject.GetComponent<Hook>().currentState)
        {
            case HookState.Arrow:
                // Collision Arrow - Arrow 
                playerMovement.rigid.AddForce(-directionKnockBack * knockBackPlayerHit, ForceMode2D.Impulse);
                StartCoroutine(CancelVibration(Vibrations.PlayVibration("CollisionArrowArrow", playerMovement.playerInputDevice)));
                AudioManager.instance.PlaySound("arrowHitArrow", playerNumber + "Arrow");
                break;
            case HookState.Shield:
                // Collision Arrow - Shield 
                playerMovement.rigid.AddForce(-directionKnockBack * knockBackShieldHit, ForceMode2D.Impulse);
                StartCoroutine(CancelVibration(Vibrations.PlayVibration("CollisionArrowShield", playerMovement.playerInputDevice)));
                AudioManager.instance.PlaySound("arrowHitShield", playerNumber + "Arrow");
                break;
            default:
                break;
        }
        Invoke("UnlockMovement", knockBackTime);
    }

    void UnlockMovement()
    {
        playerMovement.lockMovement = false;
    }

    void ResetCDSwitch(){
        switchingState = false;
    }

	public void VibrationOnProjectileDestroyed()
	{
		StartCoroutine (CancelVibration (Vibrations.PlayVibration("HookProjectileDestroyed", playerMovement.playerInputDevice)));
	}

	public void VibrationOnTouchingPlayerWithHookhead()
	{
		StartCoroutine (CancelVibration (Vibrations.PlayVibration("HookheadOnPlayer", playerMovement.playerInputDevice)));
	}

	IEnumerator CancelVibration(float delay)
	{
		yield return new WaitForSeconds (delay);
		playerMovement.playerInputDevice.StopVibration ();
	}
}
