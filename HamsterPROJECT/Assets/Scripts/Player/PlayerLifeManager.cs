using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerLifeManager : MonoBehaviour {

    Balancing balanceData;

    [SerializeField]
    Text lifeText;

    PlayerMovement playerMovement;
    int playerHP;
    bool inRecovery;
    float recoveryTime;
    float flashingRate;
    SpriteRenderer sprite;
    int arrowDamage;
    float knockBackTime;
    float knockBackForceArrowPlayer;
    float knockBackForceHookheadPlayer;

    // Use this for initialization
    void Start () {
        //S'il y a une erreur ici s'assurer que le prefab "Balancing" est bien dans la scène
        balanceData = GameObject.Find("Balancing").GetComponent<Balancing>();

        playerHP = balanceData.playerMaxHP;
        recoveryTime = balanceData.recoveryTime;
        flashingRate = balanceData.flashingRate;
        arrowDamage = balanceData.arrowDamage;
        knockBackTime = balanceData.knockBackTime;
        knockBackForceArrowPlayer = balanceData.knockBackForceArrowPlayer;
        knockBackForceHookheadPlayer = balanceData.knockBackForceHookheadPlayer;

        sprite = GetComponent<SpriteRenderer>();
        playerMovement = GetComponent<PlayerMovement>();

        UpdateLifeUI();
    }
	
	// Update is called once per frame
	void Update () {
        if (playerHP <= 0)
        {
            Dead();
        }
    }

    public void TakeDamage(int damage, GameObject attacker, bool knockBack)
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
                        playerMovement.rigid.AddForce(-directionKnockBack * knockBackForceArrowPlayer);
                        break;
                    case "Hook":
                        playerMovement.rigid.AddForce(-directionKnockBack * knockBackForceHookheadPlayer);
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
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            TakeDamage(arrowDamage,collision.gameObject, true);
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
        lifeText.text = "Current HP : " + playerHP;
    }

    void Dead()
    {
        Destroy(transform.parent.gameObject);
    }
}
