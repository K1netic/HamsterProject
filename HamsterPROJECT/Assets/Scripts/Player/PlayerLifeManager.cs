using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLifeManager : MonoBehaviour {

    Balancing balanceData;

    //Player's informations
    [HideInInspector]
    public SpriteRenderer spriteArrow;
    [HideInInspector]
    public Hook hookScript;
    [HideInInspector]
    public LayerMask playerLayer;
    PlayerMovement playerMovement;
    SpriteRenderer sprite;

    //Combat system
    [HideInInspector]
    public float playerHP;
    [HideInInspector]
    public bool inRecovery;
    float recoveryTime;
    float flashingRate;
    float knockBackTime;
    float knockBackLaser;
    float knockBackBlade1;
    float knockBackBlade2;
    float knockBackBlade3;
    float knockBackMeteor;
    float laserDamage;
    float criticalSpeed;
    float arrowDamage;

    //Last attacker
    public string lastAttacker = null;
    float lastAttackerDuration;

    //FX
    [SerializeField]
    GameObject lifeParticlesManager;
    LifeParticlesManager lifeParticlesManagerScript;
    ParticleSystem deathParticle;
    ParticleSystem hitLittle;
    ParticleSystem hitHard;
    ParticleSystem hitLaser;
    bool doubleFXprotection;
    bool doubleFXprotectionLaser;

    //Wounded animation
    Color startColor = new Color(1, 1, 1, 1);
    Color woundedColor = new Color(1, 1, 1, 0.49f);
    bool wounded;
    Gradient woundedGradient = new Gradient();
    Material startMaterial;
    Material woundedMaterial;

    //Trail
    TrailRenderer trail;
    Gradient trailGradient = new Gradient();
    GradientColorKey[] colorTrail = new GradientColorKey[2];
    GradientAlphaKey[] alphaTrail = new GradientAlphaKey[2];

    //Death
    bool deadLimiter = false;// Makes sure Dead function is only called once at a time
    FeedbacksOnDeath FbOnDeath;

    // Use this for initialization
    void Start () {
        //S'il y a une erreur ici s'assurer que le prefab "Balancing" est bien dans la scène
        balanceData = GameObject.Find("Balancing").GetComponent<Balancing>();

        playerHP = balanceData.playerMaxHP;
        recoveryTime = balanceData.recoveryTime;
        flashingRate = balanceData.flashingRate;
        knockBackTime = balanceData.knockBackTime;
        knockBackLaser= balanceData.knockBackLaser;
        knockBackBlade1 = balanceData.knockBackBlade1;
        knockBackBlade2 = balanceData.knockBackBlade2;
        knockBackBlade3 = balanceData.knockBackBlade3;
        knockBackMeteor = balanceData.knockBackMeteor;
        laserDamage = balanceData.laserDamage;
        criticalSpeed = balanceData.criticalSpeed;
        arrowDamage = balanceData.arrowDamage;
        lastAttackerDuration = balanceData.lastAttackerDuration;

        playerMovement = GetComponent<PlayerMovement>();
        trail = GetComponent<TrailRenderer>();
		lifeParticlesManagerScript = lifeParticlesManager.GetComponent<LifeParticlesManager>();        
        sprite = GetComponent<SpriteRenderer>();        
        startMaterial = sprite.material;

        FbOnDeath = GameObject.Find ("LevelScripts").GetComponent<FeedbacksOnDeath> ();

        woundedMaterial = Resources.Load<Material>("Material/SpriteBlink");

        //Switch gérant la couleur de la trail et la couleur des particules
        switch (sprite.sprite.name)
        {
            case "0":
                colorTrail[0].color = new Color(1, 0.5529412f, 0);
                colorTrail[1].color = new Color(1, 0,0);
                deathParticle = Resources.Load<ParticleSystem>("Particles/Nuke/NukeOrange");
                hitLittle = Resources.Load<ParticleSystem>("Particles/HitLittle/HitLittleOrange");
                hitHard = Resources.Load<ParticleSystem>("Particles/HitHard/HitHardOrange");
                hitLaser = Resources.Load<ParticleSystem>("Particles/LaserHitPlayer/LaserHitPlayerOrange");
                break;
            case "1":
                colorTrail[0].color = new Color(1,0,1);
                colorTrail[1].color = new Color(0.3254902f,0,1);
                deathParticle = Resources.Load<ParticleSystem>("Particles/Nuke/NukePink");
                hitLittle = Resources.Load<ParticleSystem>("Particles/HitLittle/HitLittlePink");
                hitHard = Resources.Load<ParticleSystem>("Particles/HitHard/HitHardPink");
                hitLaser = Resources.Load<ParticleSystem>("Particles/LaserHitPlayer/LaserHitPlayerPink");
                break;
            case "2":
                colorTrail[0].color = new Color(0.201914f, 0.5660378f, 0.1361694f);
                colorTrail[1].color = new Color(0.09819563f, 0.4245283f, 0.04205233f);
                deathParticle = Resources.Load<ParticleSystem>("Particles/Nuke/NukeGreen");
                hitLittle = Resources.Load<ParticleSystem>("Particles/HitLittle/HitLittleGreen");
                hitHard = Resources.Load<ParticleSystem>("Particles/HitHard/HitHardGreen");
                hitLaser = Resources.Load<ParticleSystem>("Particles/LaserHitPlayer/LaserHitPlayerGreen");
                break;
            case "3":
                colorTrail[0].color = new Color(1,1,0);
                colorTrail[1].color = new Color(1, 0.5568628f,0);
                deathParticle = Resources.Load<ParticleSystem>("Particles/Nuke/NukeYellow");
                hitLittle = Resources.Load<ParticleSystem>("Particles/HitLittle/HitLittleYellow");
                hitHard = Resources.Load<ParticleSystem>("Particles/HitHard/HitHardYellow");
                hitLaser = Resources.Load<ParticleSystem>("Particles/LaserHitPlayer/LaserHitPlayerYellow");
                break;
            case "4":
                colorTrail[0].color = new Color(0, 1, 1);
                colorTrail[1].color = new Color(0, 0.2901961f, 1);
                deathParticle = Resources.Load<ParticleSystem>("Particles/Nuke/NukeBlue");
                hitLittle = Resources.Load<ParticleSystem>("Particles/HitLittle/HitLittlePink");
                hitHard = Resources.Load<ParticleSystem>("Particles/HitHard/HitHardYellow");
                hitLaser = Resources.Load<ParticleSystem>("Particles/LaserHitPlayer/LaserHitPlayerGreen");
                break;
            default:
                print("Default case switch start PlayerLifeManager.cs");
                break;
        }
        alphaTrail[0].alpha = 1;
        alphaTrail[0].time = 0;
        alphaTrail[1].alpha = 0;
        alphaTrail[1].time = 1;
        colorTrail[0].time = 0;
        colorTrail[1].time = 1;
        trailGradient.SetKeys(colorTrail, alphaTrail);
        trail.colorGradient = trailGradient;

        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0].alpha = .157f;
        alphaKeys[0].time = 0;
        alphaKeys[1] = trailGradient.alphaKeys[1];
        woundedGradient.SetKeys(trailGradient.colorKeys, alphaKeys);
    }

    // Update is called once per frame
    void Update () {

		lifeParticlesManagerScript.playerHP = playerHP;

        //Vérifie si le player à toujours des PV sinon appelle la fonction Dead()
		if (playerHP <= 0 && !deadLimiter)
        {
			deadLimiter = true;
            Dead();
        }
    }

    //Fonction qui applique des dégâts au joueur
    //1er arg : les dégats qui vont être appliqués
    //2eme arg : le game object qui est à l'origine des dégâts, utilisé pour trouver la direction du knockback
    //3eme arg : bool qui sert à savoir s'il on doit appliquer un knockback
    //4 et 5 servent pour le knockback avec les lasers
    public void TakeDamage(float damage, GameObject attacker, bool knockBack, Vector3 contactPoint = default(Vector3), bool inverseDir = default(bool))
    {
        //Vérifie si le joueur n'est pas en recovery
        if (!inRecovery)
        {
            if (hookScript.hooked)
            {
                hookScript.DisableRope(false);
            }
            inRecovery = true;
            if (knockBack)
            {
                //Bloque le mouvement du joueur pour ne pas override le knockback
                playerMovement.lockMovement = true;
                //Calcul la direction du knockback
                Vector2 directionKnockBack = -(attacker.transform.position - transform.position).normalized;
                //Passe la vitesse à 0 pour que le knockback soit correctement appliqué
                playerMovement.rigid.velocity = Vector3.zero;
                Invoke("UnlockMovement", knockBackTime);
                //Switch qui test la nature de l'attaquant pour savoir quel knockback effectué
                //ForceMode2D.Impulse est essentiel pour que le knockback soit efficace
                switch (attacker.tag)
                {
                    //Si c'est la flèche d'un autre joueur qui est à l'origine des dégâts on applique un knockback dépendant de sa lame
                    case "Arrow":
                        switch (attacker.GetComponent<Hook>().currentBlade)
                        {
                            case Hook.CurrentBlade.none:
                                print("You are not suppossed to be there ! How did you come ?!");
                                break;
                            case Hook.CurrentBlade.blade1:
                                playerMovement.rigid.AddForce(directionKnockBack * knockBackBlade1, ForceMode2D.Impulse);
                                break;
                            case Hook.CurrentBlade.blade2:
                                playerMovement.rigid.AddForce(directionKnockBack * knockBackBlade2, ForceMode2D.Impulse);
                                break;
                            case Hook.CurrentBlade.blade3:
                                playerMovement.rigid.AddForce(directionKnockBack * knockBackBlade3, ForceMode2D.Impulse);
                                break;
                            default:
                                print("Impossible, you just CAN'T be there !");
                                break;
                        }
                        break;
                    case "LaserEdge":
                        LaserColliderDetection laserScript = attacker.GetComponent<LaserColliderDetection>();
                        switch (laserScript.side)
                        {
                            case LaserColliderDetection.LaserSide.bot:
                                playerMovement.rigid.AddForce(Vector3.up * knockBackLaser, ForceMode2D.Impulse);
                                break;
                            case LaserColliderDetection.LaserSide.top:
                                playerMovement.rigid.AddForce(Vector3.down * knockBackLaser, ForceMode2D.Impulse);
                                break;
                            case LaserColliderDetection.LaserSide.right:
                                playerMovement.rigid.AddForce(Vector3.left * knockBackLaser, ForceMode2D.Impulse);
                                break;
                            case LaserColliderDetection.LaserSide.left:
                                playerMovement.rigid.AddForce(Vector3.right * knockBackLaser, ForceMode2D.Impulse);
                                break;
                            default:
                                break;
                        }
                        break;
                    case "Meteor":
                        playerMovement.rigid.AddForce(directionKnockBack * knockBackMeteor, ForceMode2D.Impulse);
                        break;
                    case "Laser":
                        if(inverseDir)
                            playerMovement.rigid.AddForce(-Vector2.Perpendicular(attacker.GetComponent<LaserSize>().laserDirection).normalized * knockBackLaser, ForceMode2D.Impulse);
                        else
                            playerMovement.rigid.AddForce(Vector2.Perpendicular(attacker.GetComponent<LaserSize>().laserDirection).normalized * knockBackLaser, ForceMode2D.Impulse);
                        break;
                    default:
                        break;
                }
            }

            //Attribution des dégâts
            CancelCleanLastAttacker();
            if (attacker.CompareTag("Arrow"))
            {
                lastAttacker = attacker.GetComponent<Hook>().playerMovement.playerNumber;
                playerHP -= damage;
                woundedMaterial.color = Color.Lerp(Color.red, Color.white , playerHP / 100);
                if(attacker.GetComponent<Hook>().currentBlade == Hook.CurrentBlade.blade3)
                    AudioManager.instance.PlaySound("criticalDamage", playerMovement.playerNumber);
                else
                    AudioManager.instance.PlaySound("damage", playerMovement.playerNumber);
            }
            else
            {
                playerHP -= damage;
                woundedMaterial.color = Color.Lerp(Color.red, Color.white, playerHP / 100);
                AudioManager.instance.PlaySound("playerHitLaser", playerMovement.playerNumber);
            }
            CleanLastAttacker();

            //Invoke le changement de layer après 0.1 secondes pour que la collision repousse un peu l'attaquant
            Invoke("RecoveryLayer", .1f);
            //Rend le player invulnérable pendant recoveryTime secondes
            Invoke("ResetRecovery", recoveryTime);
            //Fait clignoter le joueur tant qu'il est invulnérable
            InvokeRepeating("Flashing", 0, flashingRate);

			// Player metrics setting
			if (playerHP <= 0)
			{
				if (attacker.tag == "Arrow" || attacker.tag == "Hook")
				{
					GameManager.playersKills[int.Parse(attacker.transform.parent.GetChild(0).GetComponent<PlayerMovement>().playerNumber.Substring (2, 1)) - 1] += 1; 
					// Counting kills for the player score if GameMode is set to Kills
					if (GameManager.gameModeType == GameManager.gameModes.Deathmatch)
					{
						GameManager.playersScores[int.Parse(attacker.transform.parent.GetChild(0).GetComponent<PlayerMovement>().playerNumber.Substring (2, 1)) - 1] += 1; 
					}
				}

				else if (attacker.tag == "LaserEdge" || attacker.tag == "Laser" || attacker.tag == "Bombe" ||attacker.tag == "Meteor")
				{
                    if (lastAttacker == null)
                    {
                        GameManager.playersSelfDestructs[int.Parse((this.GetComponent<PlayerMovement>().playerNumber.Substring(2, 1))) - 1] += 1;
                    }
                    else if (GameManager.gameModeType == GameManager.gameModes.Deathmatch)
                    {
                        GameManager.playersKills[int.Parse(lastAttacker.Substring(2, 1)) - 1] += 1;
                        GameManager.playersScores[int.Parse(lastAttacker.Substring(2, 1)) - 1] += 1;
                    }
                    else
                        GameManager.playersKills[int.Parse(lastAttacker.Substring(2, 1)) - 1] += 1;
                }
                else
                {
                    print("Who the fking hell kills you ?!");
                }
			}

			// DAMAGE TAKEN VIBRATION
			// Apply a lighter/heavier vibration depending on the damage taken
			playerMovement.playerInputDevice.Vibrate(0f, balanceData.lightVibration * (damage / balanceData.damageToVibrationDivisor));
			StartCoroutine(CancelVibration (balanceData.mediumVibrationDuration));
        }
    }

    void RecoveryLayer()
    {
        //Passe le player et la flèche en layer "Recovery" pour qu'ils traversent les autres joueurs
        gameObject.layer = LayerMask.NameToLayer("Recovery");
        hookScript.gameObject.layer = LayerMask.NameToLayer("Recovery");
    }

    public void CleanLastAttacker()
    {
        Invoke("CleanLastAttackerDone", lastAttackerDuration);
    }

    void CleanLastAttackerDone()
    {
        lastAttacker = null;
    }

    public void CancelCleanLastAttacker()
    {
        CancelInvoke("CleanLastAttackerDone");
    }

    public void HitFX(Vector3 position, float speed)
    {
        if (!doubleFXprotection && !inRecovery)
        {
            doubleFXprotection = true;
            if (speed > criticalSpeed)
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

    void OnCollisionEnter2D(Collision2D col){
        switch (col.gameObject.tag)
        {
            case "LaserEdge":
                LaserHitFX(col.GetContact(0).point);
                if (!inRecovery)
                    TakeDamage(laserDamage, col.gameObject, true);
                else
                {//Le joueur retouche le LaserEdge alors qu'il est encore en recovery
                    if (hookScript.hooked)
                    {
                        hookScript.DisableRope(false);
                    }
                    //Bloque le mouvement du joueur pour ne pas override le knockback
                    playerMovement.lockMovement = true;
                    //Passe la vitesse à 0 pour que le knockback soit correctement appliqué
                    playerMovement.rigid.velocity = Vector3.zero;
                    Invoke("UnlockMovement", knockBackTime);
                    LaserColliderDetection laserScript = col.gameObject.GetComponent<LaserColliderDetection>();
                    switch (laserScript.side)
                    {
                        case LaserColliderDetection.LaserSide.bot:
                            playerMovement.rigid.AddForce(Vector3.up * knockBackLaser, ForceMode2D.Impulse);
                            break;
                        case LaserColliderDetection.LaserSide.top:
                            playerMovement.rigid.AddForce(Vector3.down * knockBackLaser, ForceMode2D.Impulse);
                            break;
                        case LaserColliderDetection.LaserSide.right:
                            playerMovement.rigid.AddForce(Vector3.left * knockBackLaser, ForceMode2D.Impulse);
                            break;
                        case LaserColliderDetection.LaserSide.left:
                            playerMovement.rigid.AddForce(Vector3.right * knockBackLaser, ForceMode2D.Impulse);
                            break;
                        default:
                            break;
                    }
                    AudioManager.instance.PlaySound("playerHitLaser", playerMovement.playerNumber);
                }
                break;
            case "Laser":
                LaserHitFX(col.GetContact(0).point);
                if (!inRecovery)
                {
                    switch (col.collider.name)
                    {
                        case "ColliderBot":
                            TakeDamage(laserDamage, col.gameObject, true, col.GetContact(0).point);
                            break;
                        case "ColliderTop":
                            TakeDamage(laserDamage, col.gameObject, true, col.GetContact(0).point, true);
                            break;
                        default:
                            break;
                    }
                }
                else
                {//Le joueur retouche le LaserEdge alors qu'il est encore en recovery
                    if (hookScript.hooked)
                    {
                        hookScript.DisableRope(false);
                    }
                    //Bloque le mouvement du joueur pour ne pas override le knockback
                    playerMovement.lockMovement = true;
                    //Passe la vitesse à 0 pour que le knockback soit correctement appliqué
                    playerMovement.rigid.velocity = Vector3.zero;
                    Invoke("UnlockMovement", knockBackTime);
                    switch (col.gameObject.name)
                    {
                        case "ColliderBot":
                            playerMovement.rigid.AddForce(Vector2.Perpendicular(col.transform.parent.gameObject.GetComponent<LaserSize>().laserDirection).normalized * knockBackLaser, ForceMode2D.Impulse);
                            break;
                        case "ColliderTop":
                            playerMovement.rigid.AddForce(-Vector2.Perpendicular(col.transform.parent.gameObject.GetComponent<LaserSize>().laserDirection).normalized * knockBackLaser, ForceMode2D.Impulse);
                            break;
                        default:
                            break;
                    }
                    AudioManager.instance.PlaySound("playerHitLaser", playerMovement.playerNumber);
                }
                break;
            default:
                break;
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        //Normalement ceci ne peux pas se produire, mais je le rajoute par sécurité
        switch (col.gameObject.tag)
        {
            case "LaserEdge":
                if (hookScript.hooked)
                {
                    hookScript.DisableRope(false);
                }
                LaserHitFX(col.GetContact(0).point);
                TakeDamage(laserDamage, col.gameObject, true);
                StartCoroutine(CancelVibration(Vibrations.PlayVibration("LaserEdge", playerMovement.playerInputDevice)));
                break;
            case "Laser":
                if (hookScript.hooked)
                {
                    hookScript.DisableRope(false);
                }
                LaserHitFX(col.GetContact(0).point);
                switch (col.gameObject.name)
                {
                    case "ColliderBot":
                        TakeDamage(laserDamage, col.gameObject, true, col.GetContact(0).point);
                        break;
                    case "ColliderTop":
                        TakeDamage(laserDamage, col.gameObject, true, col.GetContact(0).point, true);
                        break;
                    default:
                        break;
                }
                StartCoroutine(CancelVibration(Vibrations.PlayVibration("LaserEdge", playerMovement.playerInputDevice)));
                break;
            default:
                break;
        }
    }

    void LaserHitFX(Vector3 position)
    {
        if (!doubleFXprotectionLaser)
        {
            doubleFXprotectionLaser = true;
            Instantiate(hitLaser, position, transform.rotation);
            Invoke("CancelFXProtectionLaser", .1f);
        }
    }

    void CancelFXProtectionLaser()
    {
        doubleFXprotectionLaser = false;
    }

    void UnlockMovement()
    {
        playerMovement.lockMovement = false ;
    }

    void Flashing()
	{ 
        if (wounded)
        {
            sprite.material = startMaterial;
            spriteArrow.material = startMaterial;
            wounded = false;
        }
        else
        {
            sprite.material = woundedMaterial;
            spriteArrow.material = woundedMaterial;
            wounded = true;
        }
    }

    void ResetRecovery()
    {
        //Annule le InvokeRepeating pour le clignotement de l'invulnérabilité et remet un layer physique correspondant au player
        CancelInvoke("Flashing");
        inRecovery = false;
        gameObject.layer = playerLayer;
        hookScript.gameObject.layer = hookScript.arrowLayer;
        sprite.material = startMaterial;
        spriteArrow.material = startMaterial;
    }

    void Dead()
    {
        AudioManager.instance.PlaySound("death", playerMovement.playerNumber);
		// Set player as dead in the game manager
        GameManager.playersAlive [int.Parse((this.GetComponent<PlayerMovement> ().playerNumber.Substring (2,1))) - 1] = false;
		// Add a death in metrics
		GameManager.playersDeaths [int.Parse ((this.GetComponent<PlayerMovement> ().playerNumber.Substring (2, 1))) - 1] += 1;

        //Nuke
        Instantiate(deathParticle, transform.position, transform.rotation);

		FbOnDeath.SendFeedbacks (playerMovement.playerInputDevice, transform.parent.name.Substring(0, 1));

        Destroy(transform.parent.gameObject);
    }

	IEnumerator CancelVibration(float delay)
	{
		yield return new WaitForSeconds (delay);
		playerMovement.playerInputDevice.StopVibration ();
	}
}
