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
    float knockBackForceArrowPlayer;
    float knockBackForceHookheadPlayer;
    float knockBackForceSpikesPlayer;
    float velocityKnockBackRatio;

    // Use this for initialization
    void Start () {
        //S'il y a une erreur ici s'assurer que le prefab "Balancing" est bien dans la scène
        balanceData = GameObject.Find("Balancing").GetComponent<Balancing>();

        playerHP = balanceData.playerMaxHP;
        recoveryTime = balanceData.recoveryTime;
        flashingRate = balanceData.flashingRate;
        knockBackTime = balanceData.knockBackTime;
        knockBackForceArrowPlayer = balanceData.knockBackForceArrowPlayer;
        knockBackForceHookheadPlayer = balanceData.knockBackForceHookheadPlayer;
        spikesDamage = balanceData.spikesDamage;
        knockBackForceSpikesPlayer= balanceData.knockBackForceSpikesPlayer;
        velocityKnockBackRatio = balanceData.velocityKnockBackRatio;

        sprite = GetComponent<SpriteRenderer>();
        playerMovement = GetComponent<PlayerMovement>();

        lifeUI = GameObject.Find("HPBar"+playerMovement.playerNumber).GetComponent<Slider>();
        lifeUI.transform.GetChild(1).GetComponentInChildren<Image>().color = GetComponent<SpriteRenderer>().color;

        UpdateLifeUI();
    }
	
	// Update is called once per frame
	void Update () {
        if (playerHP <= 0)
        {
            Dead();
        }
    }

    public void TakeDamage(float damage, GameObject attacker, bool knockBack)
    {
        if (!inRecovery)
        {
            inRecovery = true;
            if (knockBack)
            {
                playerMovement.lockMovementKnockBack = true;
                Vector2 directionKnockBack = (attacker.transform.position - transform.position).normalized;
                playerMovement.rigid.velocity = Vector3.zero;
                Invoke("UnlockMovement", knockBackTime);
                switch (attacker.tag)
                {
                    case "Arrow":
                        playerMovement.rigid.AddForce(-directionKnockBack * (knockBackForceArrowPlayer/*
                        + attacker.GetComponent<Hook>().playerMovement.rigid.velocity.magnitude * velocityKnockBackRatio*/), ForceMode2D.Impulse);
                        break;
                    case "Hook":
                        playerMovement.rigid.AddForce(-directionKnockBack * knockBackForceHookheadPlayer, ForceMode2D.Impulse);
                        break;
                    case "Spikes":
                        playerMovement.rigid.AddForce(-directionKnockBack * knockBackForceSpikesPlayer, ForceMode2D.Impulse);
                        break;
                    default:
                        break;
                }
                Invoke("UnlockMovement", knockBackTime);
            }
            playerHP -= damage;
            UpdateLifeUI();
            Invoke("ResetRecovery", recoveryTime);
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
