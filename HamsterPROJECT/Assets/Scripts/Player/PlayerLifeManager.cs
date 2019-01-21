using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;


public class PlayerLifeManager : MonoBehaviour {

    [SerializeField]
    ParticleSystem deathParticle;
    [SerializeField]
    ParticleSystem hitLittle;
    [SerializeField]
    ParticleSystem hitHard;

    Balancing balanceData;

    Image lifeBackground;
    Image lifeImage;

    PlayerMovement playerMovement;
    float playerHP;
    [HideInInspector]
    public bool inRecovery;
    float recoveryTime;
    float flashingRate;
    SpriteRenderer sprite;
    float knockBackTime;
    float knockBackPlayerHit;
    float knockBackLaser;
    float laserDamage;
	TrailRenderer trail;
	PlayerIndex plyrIndex;

	// Makes sure Dead function is only called once at a time
	bool deadLimiter = false;
    private bool doubleFXprotection;
    private float criticalSpeed;

    // Use this for initialization
    void Start () {
        //S'il y a une erreur ici s'assurer que le prefab "Balancing" est bien dans la scène
        balanceData = GameObject.Find("Balancing").GetComponent<Balancing>();

        playerHP = balanceData.playerMaxHP;
        recoveryTime = balanceData.recoveryTime;
        flashingRate = balanceData.flashingRate;
        knockBackTime = balanceData.knockBackTime;
        knockBackPlayerHit = balanceData.knockBackPlayerHit;
        knockBackLaser= balanceData.knockBackLaser;
        laserDamage= balanceData.laserDamage;
        criticalSpeed = balanceData.criticalSpeed;

        sprite = GetComponent<SpriteRenderer>();
        playerMovement = GetComponent<PlayerMovement>();

        trail = GetComponent<TrailRenderer>();

        lifeBackground = GameObject.Find("HPBar" + playerMovement.playerNumber).GetComponent<Image>();
        lifeImage = GameObject.Find("HP" + playerMovement.playerNumber).GetComponent<Image>();

        switch (GetComponent<SpriteRenderer>().sprite.name)
        {
            case "Perso1":
                lifeBackground.sprite = Resources.Load<Sprite>("UISprites/LifeBar1");
                break;
            case "Perso2":
                lifeBackground.sprite = Resources.Load<Sprite>("UISprites/LifeBar2");
                break;
            case "Perso3":
                lifeBackground.sprite = Resources.Load<Sprite>("UISprites/LifeBar3");
                break;
            case "Perso4":
                lifeBackground.sprite = Resources.Load<Sprite>("UISprites/LifeBar4");
                break;
            case "Perso5":
                lifeBackground.sprite = Resources.Load<Sprite>("UISprites/LifeBar5");
                break;
            default:
                print("Default case switch start PlayerLifeManager.cs");
                break;
        }

        UpdateLifeUI();

		// Set PlayerIndex for vibrations
		switch(playerMovement.playerNumber)
		{
		case "_P1":
			plyrIndex = PlayerIndex.One;
			break;
		case "_P2":
			plyrIndex = PlayerIndex.Two;
			break;
		case "_P3":
			plyrIndex = PlayerIndex.Three;
			break;
		case "_P4":
			plyrIndex = PlayerIndex.Four;
			break;
		default:
			break;
		}
    }
	
	// Update is called once per frame
	void Update () {

//		for (int i = 0; i < Input.GetJoystickNames ().Length; i++)
//		{
//			if (Input.GetAxis ("Horizontal_P" + (i + 1).ToString()) > 0 || Input.GetAxis ("Vertical_P" + (i + 1).ToString()) > 0)
//			{
//				Debug.Log (i.ToString());
//			}
//		}
	
        //Vérifie si le player à toujours des PV sinon appelle la fonction Dead()
		if (playerHP <= 0 && !deadLimiter)
        {
			deadLimiter = true;
            Dead();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print('i');
    }

    //Fonction qui applique des dégâts au joueur
    //1er arg : les dégats qui vont être appliqués
    //2eme arg : le game object qui est à l'origine des dégâts, utilisé pour trouver la direction du knockback
    //3eme arg : bool qui sert à savoir s'il on doit appliquer un knockback
    public void TakeDamage(float damage, GameObject attacker, bool knockBack)
    {
        //Vérifie si le joueur n'est pas en recovery
        if (!inRecovery)
        {
            inRecovery = true;
            if (knockBack)
            {
                //Bloque le mouvement du joueur pour ne pas override le knockback
                playerMovement.lockMovement = true;
                //Calcul la direction du knockback
                Vector2 directionKnockBack = (attacker.transform.position - transform.position).normalized;
                //Passe la vitesse à 0 pour que le knockback soit correctement appliqué
                playerMovement.rigid.velocity = Vector3.zero;
                Invoke("UnlockMovement", knockBackTime);
                //Switch qui test la nature de l'attaquant pour savoir quel knockback effectué
                //ForceMode2D.Impulse est essentiel pour que le knockback soit efficace
                switch (attacker.tag)
                {
                    //Si c'est la flèche d'un autre joueur qui est à l'origine des dégâts il faut prendre en compte la vitesse de l'attaquant pour moduler la force du knockback
                    case "Arrow":
                        playerMovement.rigid.AddForce(-directionKnockBack * (knockBackPlayerHit
                        + attacker.GetComponent<Hook>().playerMovement.speed /2), ForceMode2D.Impulse);
                        break;
                    case "Hook":
                        playerMovement.rigid.AddForce(-directionKnockBack * knockBackPlayerHit, ForceMode2D.Impulse);
                        break;
                    case "Laser":
                        playerMovement.rigid.AddForce(Vector3.up * knockBackLaser, ForceMode2D.Impulse);
                        break;
                    default:
                        break;
                }
            }
            playerHP -= damage;
            UpdateLifeUI();
            //Rend le player invulnérable pendant recoveryTime secondes
            Invoke("ResetRecovery", recoveryTime);
            //Fait clignoter le joueur tant qu'il est invulnérable
            InvokeRepeating("Flashing", 0, flashingRate);

			// Player metrics setting
			if (playerHP <= 0 && (attacker.tag == "Arrow" || attacker.tag == "Hook"))
			{
				GameManager.playersKills[int.Parse(attacker.transform.parent.GetChild(0).GetComponent<PlayerMovement>().playerNumber.Substring (2, 1)) - 1] += 1; 
				// Counting kills for the player score if GameMode is set to Kills
				if (GameManager.gameModeType == GameManager.gameModes.Kills)
				{
					GameManager.playersScores[int.Parse(attacker.transform.parent.GetChild(0).GetComponent<PlayerMovement>().playerNumber.Substring (2, 1)) - 1] += 1; 
				}
			}

			//Vibrations
			// Apply a lighter/heavier vibration depending on the damage taken
			Debug.Log("vibrating on : " + plyrIndex.ToString());
			GamePad.SetVibration (plyrIndex, 0f, balanceData.lightVibration * (damage / balanceData.damageToVibrationDivisor));
			StartCoroutine(CancelVibration (0.2f));
        }
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
        if (col.gameObject.CompareTag("Laser"))
        {
            if(!inRecovery)
                TakeDamage(laserDamage, col.gameObject, true);
            else
            {
                //Bloque le mouvement du joueur pour ne pas override le knockback
                playerMovement.lockMovement = true;
                //Passe la vitesse à 0 pour que le knockback soit correctement appliqué
                playerMovement.rigid.velocity = Vector3.zero;
                Invoke("UnlockMovement", knockBackTime);
                playerMovement.rigid.AddForce(Vector3.up * knockBackLaser, ForceMode2D.Impulse);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Laser"))
        {
            TakeDamage(laserDamage, col.gameObject, true);
        }
    }

    void UnlockMovement()
    {
        playerMovement.lockMovement = false ;
    }

    void Flashing()
	{ 
		trail.enabled = !trail.enabled;
        sprite.enabled = !sprite.enabled;
    }

    void ResetRecovery()
    {
        //Annule le InvokeRepeating pour le clignotement de l'invulnérabilité
        CancelInvoke();
        inRecovery = false;
		sprite.enabled = true;
		trail.enabled = true;
    }

    void UpdateLifeUI()
    {
        lifeImage.fillAmount = playerHP/100;
    }

    void Dead()
    {
        lifeImage.fillAmount = 0;
		// Set player as dead in the game manager
        GameManager.playersAlive [int.Parse((this.GetComponent<PlayerMovement> ().playerNumber.Substring (2,1))) - 1] = false; 
		// Add a death in metrics
		GameManager.playersDeaths [int.Parse ((this.GetComponent<PlayerMovement> ().playerNumber.Substring (2, 1))) - 1] += 1; 

		// Vibration on death
//		GamePad.SetVibration (plyrIndex, 0f, balanceData.heavyRumble);
//		StartCoroutine(CancelVibration (0.05f));

        Destroy(transform.parent.gameObject, 0.05f);
        Instantiate(deathParticle, transform.position, transform.rotation);
    }
		
	IEnumerator CancelVibration(float delay)
	{
		yield return new WaitForSeconds (delay);
		GamePad.SetVibration (plyrIndex, 0f, 0f);
	}
}
