using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Hook : MonoBehaviour {

    public enum CurrentBlade
    {
        none, blade1, blade2, blade3,
    }

    Balancing balanceData;

    //PLAYER
    [HideInInspector]
    public PlayerMovement playerMovement;
    PlayerLifeManager lifeManager;
    string playerNumber;    

    //JOINT
    [SerializeField]
    public LayerMask layerMaskRaycast;//Layer qui bloque le changement de la distance max du joint
    [HideInInspector]
    public bool hooked;
    DistanceJoint2D joint;
    bool jointNotCreated = true;
    float retractationStep;
	float timeHooked;
	float timeRemaining;
    float timeBeforeDestroy;
    Vector3 jointDirection;
    RaycastHit2D checkOppositeToJoint;
    RaycastHit2D checkToJoint ;

    //ROPE
    [HideInInspector]
    public LineRenderer line;
    [HideInInspector]
    public bool ropeCut;
    BoxCollider2D lineCollider;
    Vector3 startPos;
    Vector3 endPos;
    Texture rope;
    //COLOR
    Color colorRope;
    float t;

    //AIM
    public GameObject player;
	Vector2 screenPoint;
    Vector3 shootPos;

	//SHOT
    [SerializeField]
	GameObject projectile;
    [HideInInspector]
    public GameObject currentProjectile;
    float timeBtwShots;
    bool hookInCD;
    Sprite hookheadSprite;
    Projectile projectileScript;
    Projectile stockedProjectileScript;

    //ARROW
    Sprite arrowSprite;
    SpriteRenderer spriteRenderer;

    //COMBAT SYSTEM
    [HideInInspector]
    public LayerMask layerMaskLineCast;//Layer qui gère la détection des autres flèches
    [HideInInspector]
    public bool cantAttack;
    [HideInInspector]
    public CurrentBlade currentBlade;
    Sprite blade1Sprite;
    Sprite blade2Sprite;
    Sprite blade3Sprite;
    PolygonCollider2D[] bladeColliders;
    PolygonCollider2D blade1Collider;
    PolygonCollider2D blade2Collider;
    PolygonCollider2D blade3Collider;
    SpriteRenderer bladeRenderer;
    float attackTime;
    float criticalSpeed;
    float knockBackTime;
    float arrowDamage;
    float knockBackBlade1;
    float knockBackBlade2;
    float knockBackBlade3;
    bool doubleFXprotection;

    //PARTICLE
    ParticleSystem hitLittle;
    ParticleSystem hitHard;

	//PAUSE MENU
	public bool isFrozen = false;

    private void Awake()
    {
        //Ajoute le joint au player dans l'awake pour être sur de pouvoir y accéder dans le start des autres scripts
        joint = player.AddComponent<DistanceJoint2D>();
        joint.enabled = false;
    } 

    // Use this for initialization
    void Start () {
        //S'il y a une erreur ici s'assurer que le prefab "Balancing" est bien dans la scène
        balanceData = GameObject.Find("Balancing").GetComponent<Balancing>();
        timeHooked = balanceData.TimeHooked;
        retractationStep = balanceData.retractationStep;
        timeBtwShots = balanceData.timeBtwShots;
        knockBackTime = balanceData.knockBackTime;
        knockBackBlade1 = balanceData.knockBackBlade1;
        knockBackBlade2 = balanceData.knockBackBlade2;
        knockBackBlade3 = balanceData.knockBackBlade3;
        arrowDamage = balanceData.arrowDamage;
        criticalSpeed = balanceData.criticalSpeed;
        timeBeforeDestroy = balanceData.timeRopeCut;
        attackTime = balanceData.attackTime;

        timeRemaining = timeHooked;

        //Récupère tout les éléments du gameObject
        bladeColliders = GetComponents<PolygonCollider2D>();
        blade1Collider = bladeColliders[2];
        blade2Collider = bladeColliders[1];
        blade3Collider = bladeColliders[0];
        bladeRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();
        playerMovement = player.GetComponent<PlayerMovement>();
        playerNumber = playerMovement.playerNumber;
        playerMovement.hookScript = this;
        spriteRenderer = GetComponent<SpriteRenderer>();
        player.GetComponent<PlayerLifeManager>().spriteArrow = spriteRenderer;
        player.GetComponent<PlayerLifeManager>().hookScript = this;

        //Crée le line renderer et le configure
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
        
        //Ajoute un collider à la corde ainsi que le script qui permet de la couper
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
            case "0":
                arrowSprite = Resources.Load<Sprite>("ArrowSprites/Peak1");
                blade1Sprite = Resources.Load<Sprite>("ArrowSprites/Blade1_1");
                blade2Sprite = Resources.Load<Sprite>("ArrowSprites/Blade1_2");
                blade3Sprite = Resources.Load<Sprite>("ArrowSprites/Blade1_3");
                hookheadSprite = Resources.Load<Sprite>("ArrowSprites/hook1");
                colorRope = new Color(.784f, .451f, .173f);
                hitLittle = Resources.Load<ParticleSystem>("Particles/HitLittle/HitLittleOrange");
                hitHard = Resources.Load<ParticleSystem>("Particles/HitHard/HitHardOrange");
                break;
            case "1":
                arrowSprite = Resources.Load<Sprite>("ArrowSprites/Peak1");
                blade1Sprite = Resources.Load<Sprite>("ArrowSprites/Blade1_1");
                blade2Sprite = Resources.Load<Sprite>("ArrowSprites/Blade1_2");
                blade3Sprite = Resources.Load<Sprite>("ArrowSprites/Blade1_3");
                hookheadSprite = Resources.Load<Sprite>("ArrowSprites/hook2");
                colorRope = new Color(.596f, .31f,.624f);
                hitLittle = Resources.Load<ParticleSystem>("Particles/HitLittle/HitLittlePink");
                hitHard = Resources.Load<ParticleSystem>("Particles/HitHard/HitHardPink");
                break;
            case "2":
                arrowSprite = Resources.Load<Sprite>("ArrowSprites/Peak1");
                blade1Sprite = Resources.Load<Sprite>("ArrowSprites/Blade1_1");
                blade2Sprite = Resources.Load<Sprite>("ArrowSprites/Blade1_2");
                blade3Sprite = Resources.Load<Sprite>("ArrowSprites/Blade1_3");
                hookheadSprite = Resources.Load<Sprite>("ArrowSprites/hook3");
                colorRope = new Color(0.310f, 0.624f, 0.318f);
                hitLittle = Resources.Load<ParticleSystem>("Particles/HitLittle/HitLittleGreen");
                hitHard = Resources.Load<ParticleSystem>("Particles/HitHard/HitHardGreen");
                break;
            case "3":
                arrowSprite = Resources.Load<Sprite>("ArrowSprites/Peak1");
                blade1Sprite = Resources.Load<Sprite>("ArrowSprites/Blade1_1");
                blade2Sprite = Resources.Load<Sprite>("ArrowSprites/Blade1_2");
                blade3Sprite = Resources.Load<Sprite>("ArrowSprites/Blade1_3");
                hookheadSprite = Resources.Load<Sprite>("ArrowSprites/hook4");
                colorRope = new Color(.847f, .761f, .271f);
                hitLittle = Resources.Load<ParticleSystem>("Particles/HitLittle/HitLittleYellow");
                hitHard = Resources.Load<ParticleSystem>("Particles/HitHard/HitHardYellow");
                break;
            case "4":
                arrowSprite = Resources.Load<Sprite>("ArrowSprites/Peak1");
                blade1Sprite = Resources.Load<Sprite>("ArrowSprites/Blade1_1");
                blade2Sprite = Resources.Load<Sprite>("ArrowSprites/Blade1_2");
                blade3Sprite = Resources.Load<Sprite>("ArrowSprites/Blade1_3");
                hookheadSprite = Resources.Load<Sprite>("ArrowSprites/hook5");
                colorRope = new Color(.216f, .384f, .529f);
                hitLittle = Resources.Load<ParticleSystem>("Particles/HitLittle/HitLittlePink");
                hitHard = Resources.Load<ParticleSystem>("Particles/HitHard/HitHardYellow");
                break;
            default:
                print("Default case switch start Hook.cs");
            break;
        }
        //Initialise le joueur avec la flèche et sans lame
        spriteRenderer.sprite = arrowSprite;
        Sheathe();

        //Fixe les différents layer en fonction du numéro du joueur
        switch  (playerNumber){
            case "_P1":
                line.gameObject.layer = 17;
                gameObject.layer = 17;
                layerMaskLineCast = (1 << 18) | (1 << 19) | (1 << 20) | (1 << 22) | (1 << 23) | (1 << 24);
                break;
            case "_P2":
                line.gameObject.layer = 18;
                gameObject.layer = 18;
                layerMaskLineCast = (1 << 17) | (1 << 19) | (1 << 20) | (1 << 21) | (1 << 23) | (1 << 24);
                break;
            case "_P3":
                line.gameObject.layer = 19;
                gameObject.layer = 19;
                layerMaskLineCast = (1 << 17) | (1 << 18) | (1 << 20) | (1 << 21) | (1 << 22) | (1 << 24);
                break;
            case "_P4":
                line.gameObject.layer = 20;
                gameObject.layer = 20;
                layerMaskLineCast = (1 << 17) | (1 << 18) | (1 << 19) | (1 << 21) | (1 << 22) | (1 << 23);
                break;
            default:
                print("Default case switch start Hook.cs");
            break;
        }
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
            UpdateArrow();
		}

        //Vérifie qu'il y a bien un projectile de créé avant d'y accéder
        if (currentProjectile != null)
        {
            UpdateRope();
            //hooked = projectileScript.hooked; VERIF SI PAS CASSE

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
			&& !hookInCD && !isFrozen && MatchStart.gameHasStarted && !ropeCut)
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

    //Déploie la lame qui correspond à la vitesse au début de l'attaque
    public void BladeChoice(float speed)
    {
        if(speed <= 20)
        {
            bladeRenderer.sprite = blade1Sprite;
            blade1Collider.enabled = true;
            currentBlade = CurrentBlade.blade1;
        }
        else if(speed <= 40)
        {
            bladeRenderer.sprite = blade2Sprite;
            blade2Collider.enabled = true;
            currentBlade = CurrentBlade.blade2;
        }
        else
        {
            bladeRenderer.sprite = blade3Sprite;
            blade3Collider.enabled = true;
            currentBlade = CurrentBlade.blade1;
        }
        Invoke("Sheathe", attackTime);
    }

    void Sheathe()
    {
        currentBlade = CurrentBlade.none;
        bladeRenderer.sprite = null;
        foreach (PolygonCollider2D collider in bladeColliders)
        {
            collider.enabled = false;
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

        //Gère l'orientation de la flèche en fonction de l'input du player
		if(playerMovement.playerInputDevice.LeftStickX.Value != 0 || playerMovement.playerInputDevice.LeftStickY.Value != 0){
            transform.rotation = Quaternion.FromToRotation(Vector3.right,
				new Vector3(playerMovement.playerInputDevice.LeftStickX.Value, playerMovement.playerInputDevice.LeftStickY.Value));
        }     
    }

    void UpdateRope(){
        //Aligne la position de la corde sur le player et la tete de grappin
        line.SetPosition(0, player.transform.position);
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

    //Active le grappin
    void CreateJoint(){
        if (playerMovement.dashRecoveryWithHook)
            playerMovement.ResetDashCD();
        t = 0;
        joint.enabled = true;
        joint.connectedBody = currentProjectile.GetComponent<Rigidbody2D>();
        joint.distance = Vector3.Distance(currentProjectile.transform.position, player.transform.position);
        joint.maxDistanceOnly = true;
        jointNotCreated = false;
    }

    //Gère le timer du grappin et le changement de couleur de la corde
    void TimerRope(){
        timeRemaining -= Time.deltaTime;
        if(timeRemaining <= timeHooked/2)
            t += (Time.deltaTime / timeRemaining)/2;
        line.startColor = Color.Lerp(colorRope, Color.black,t);
        line.endColor = Color.Lerp(colorRope, Color.black,t);
    }

    //Active la corde (line renderer) et instancie une tête de grappin
    private void ThrowHookhead(){
        line.startColor = colorRope;
        line.endColor = colorRope;
        line.gameObject.SetActive(true);
        line.SetPosition(0, player.transform.position);

        currentProjectile = Instantiate(projectile, transform.position, transform.rotation);
        currentProjectile.GetComponent<SpriteRenderer>().sprite = hookheadSprite;
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
        if (!cantAttack)
        {
            //Si c'est une fleche qui est touché on applique un knockback
            if (collision.gameObject.CompareTag("Arrow"))
            {
                GameObject.Find("SlowMo").GetComponent<SlowMotion>().DoSlowmotion();
                ArrowHit(collision);
                HitFX(collision.GetContact(0).point, collision.gameObject);
            }
            //Si le joueur est touché des dégâts lui sont appliqués
            //Mais avant on vérifie si la collision n'a pas eu de problème en envoyant un raycast entre les deux joueurs pour voir s'il n'y a pas la flèche du défenseur qui était censé bloquer
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
                    foeScript.TakeDamage(arrowDamage, gameObject, true);
                    StartCoroutine(CancelVibration(Vibrations.PlayVibration("CollisionArrowPlayer", playerMovement.playerInputDevice)));
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

    //Deux flèches se touche, on applique un knockback en fonction de la flèche de l'autre joueur
    void ArrowHit(Collision2D collision)
    {
        playerMovement.lockMovement = true;
        Vector2 directionKnockBack = (collision.gameObject.transform.position - transform.position).normalized;
        playerMovement.rigid.velocity = Vector3.zero;
        AddLastAttacker(collision.gameObject.GetComponent<Hook>().playerNumber);
        switch (collision.gameObject.GetComponent<Hook>().currentBlade)
        {
            case CurrentBlade.none:
                print("You are not suppossed to be there ! How do you came ?!");
                break;
            case CurrentBlade.blade1:
                playerMovement.rigid.AddForce(-directionKnockBack * knockBackBlade1, ForceMode2D.Impulse);
                break;
            case CurrentBlade.blade2:
                playerMovement.rigid.AddForce(-directionKnockBack * knockBackBlade2, ForceMode2D.Impulse);
                break;
            case CurrentBlade.blade3:
                playerMovement.rigid.AddForce(-directionKnockBack * knockBackBlade3, ForceMode2D.Impulse);
                break;
            default:
                print("Impossible, you just CAN'T be there !");
                break;
        }
        StartCoroutine(CancelVibration(Vibrations.PlayVibration("CollisionArrowArrow", playerMovement.playerInputDevice)));
        AudioManager.instance.PlaySound("arrowHitArrow", playerNumber+"Arrow");
        Invoke("UnlockMovement", knockBackTime);
    }
    void ArrowHit(Collider2D collision)
    {
        playerMovement.lockMovement = true;
        Vector2 directionKnockBack = (collision.gameObject.transform.position - transform.position).normalized;
        playerMovement.rigid.velocity = Vector3.zero;
        AddLastAttacker(collision.gameObject.GetComponent<Hook>().playerNumber);
        switch (collision.gameObject.GetComponent<Hook>().currentBlade)
        {
            case CurrentBlade.none:
                print("You are not suppossed to be there ! How do you came ?!");
                break;
            case CurrentBlade.blade1:
                playerMovement.rigid.AddForce(-directionKnockBack * knockBackBlade1, ForceMode2D.Impulse);
                break;
            case CurrentBlade.blade2:
                playerMovement.rigid.AddForce(-directionKnockBack * knockBackBlade2, ForceMode2D.Impulse);
                break;
            case CurrentBlade.blade3:
                playerMovement.rigid.AddForce(-directionKnockBack * knockBackBlade3, ForceMode2D.Impulse);
                break;
            default:
                print("Impossible, you just CAN'T be there !");
                break;
        }
        StartCoroutine(CancelVibration(Vibrations.PlayVibration("CollisionArrowArrow", playerMovement.playerInputDevice)));
        AudioManager.instance.PlaySound("arrowHitArrow", playerNumber + "Arrow");
        Invoke("UnlockMovement", knockBackTime);
    }

    void AddLastAttacker(string attacker)
    {
        lifeManager = player.GetComponent<PlayerLifeManager>();
        lifeManager.CancelCleanLastAttacker();
        lifeManager.lastAttacker = attacker;
        lifeManager.CleanLastAttacker();
    }

    void UnlockMovement()
    {
        playerMovement.lockMovement = false;
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

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Rope"))
        {
            col.gameObject.GetComponent<LineCutter>().CutRope(transform.GetChild(0).transform.position, playerNumber);
        }
    }
}
