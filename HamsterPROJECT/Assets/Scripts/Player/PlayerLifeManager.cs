using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerLifeManager : MonoBehaviour {

    Balancing balanceData;

    [SerializeField]
    Text lifeText;
    int playerHP;

    // Use this for initialization
    void Start () {
        //S'il y a une erreur ici s'assurer que le prefab "Balancing" est bien dans la scène
        balanceData = GameObject.Find("Balancing").GetComponent<Balancing>();

        playerHP = balanceData.playerMaxHP;

        UpdateLifeUI();
    }
	
	// Update is called once per frame
	void Update () {
        if (playerHP <= 0)
        {
            Dead();
        }
    }

    public void TakeDamage(int damage)
    {
        playerHP -= damage;
        UpdateLifeUI();
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
