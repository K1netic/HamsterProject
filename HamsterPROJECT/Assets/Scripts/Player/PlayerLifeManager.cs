using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerLifeManager : MonoBehaviour {

    Balancing balanceData;

    Slider lifeUI;

    PlayerMovement playerMovement;
    float playerHP;
    bool inRecovery;
    float recoveryTime;
    float flashingRate;
    SpriteRenderer sprite;
    float spikesDamage;
    float knockBackTime;
    float knockBackPlayerHit;
    float knockBackSpikes;

    // Use this for initialization
    void Start () {
        //S'il y a une erreur ici s'assurer que le prefab "Balancing" est bien dans la scène
        balanceData = GameObject.Find("Balancing").GetComponent<Balancing>();

        playerHP = balanceData.playerMaxHP;
        recoveryTime = balanceData.recoveryTime;
        flashingRate = balanceData.flashingRate;
        knockBackTime = balanceData.knockBackTime;
        knockBackPlayerHit = balanceData.knockBackPlayerHit;
        knockBackSpikes = balanceData.knockBackSpikes;
        spikesDamage = balanceData.spikesDamage;

        sprite = GetComponent<SpriteRenderer>();
        playerMovement = GetComponent<PlayerMovement>();

        //Récupère le slider qui correspond au player et applique la couleur du joueur sur la zone de remplissage
        lifeUI = GameObject.Find("HPBar"+playerMovement.playerNumber).GetComponent<Slider>();
        lifeUI.transform.GetChild(1).GetComponentInChildren<Image>().color = GetComponent<SpriteRenderer>().color;

        UpdateLifeUI();
    }
	
	// Update is called once per frame
	void Update () {
        //Vérifie si le player à toujours des PV sinon appelle la fonction Dead()
        if (playerHP <= 0)
        {
            Dead();
        }
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
                playerMovement.lockMovementKnockBack = true;
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
                        /*+ attacker.GetComponent<Hook>().playerMovement.rigid.velocity.magnitude * velocityKnockBackRatio*/), ForceMode2D.Impulse);
                        break;
                    case "Hook":
                        playerMovement.rigid.AddForce(-directionKnockBack * knockBackPlayerHit, ForceMode2D.Impulse);
                        break;
                    case "Spikes":
                        playerMovement.rigid.AddForce(-directionKnockBack * knockBackSpikes, ForceMode2D.Impulse);
                        break;
                    default:
                        break;
                }
                Invoke("UnlockMovement", knockBackTime);
            }
            playerHP -= damage;
            UpdateLifeUI();
            //Rend le player invulnérable pendant recoveryTime secondes
            Invoke("ResetRecovery", recoveryTime);
            //Fait clignoter le joueur tant qu'il est invulnérable
            InvokeRepeating("Flashing", 0, flashingRate);

			// Counting kills for the player score
			if (GameManager.gameModeType == GameManager.gameModes.Kills && playerHP <= 0 && (attacker.tag == "Arrow" || attacker.tag == "Hook"))
			{
				GameManager.playersScores[(int.Parse(attacker.gameObject.transform.parent.name.Substring (2, 1))) - 1] += 1; 
			}
        }
        
    }

    void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.CompareTag("Spikes")){
            TakeDamage(spikesDamage,col.gameObject,true);
        }
    }

    void UnlockMovement()
    {
        playerMovement.lockMovementKnockBack = false ;
    }

    void Flashing()
    {
        sprite.enabled = !sprite.enabled;
    }

    void ResetRecovery()
    {
        //Annule le InvokeRepeating pour le clignotement de l'invulnérabilité
        CancelInvoke();
        inRecovery = false;
        sprite.enabled = true;
    }

    void UpdateLifeUI()
    {
        lifeUI.value = playerHP;
    }

    void Dead()
    {
		// Set player as dead in the game manager
        lifeUI.value = 0;
		GameManager.playersAlive [int.Parse((this.GetComponent<PlayerMovement> ().playerNumber.Substring (2,1))) - 1] = false; 
        Destroy(transform.parent.gameObject, 0.05f);
    }
}
