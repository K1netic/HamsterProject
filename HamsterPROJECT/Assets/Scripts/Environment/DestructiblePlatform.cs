using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructiblePlatform : MonoBehaviour {

    [SerializeField]
    int maxHP = 10;
    [SerializeField]
    GameObject destroyedVersion;

    SpriteRenderer myRenderer;
    int currentPV;

    private void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        currentPV = maxHP;
    }

    public void TakeDamage(int damage)
    {
        currentPV -= damage;
        if(currentPV <= 0)
        {
            Instantiate(destroyedVersion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        //Remplace le sprite par le sprite qui correspond à son nombre de PV
        myRenderer.sprite = Resources.Load<Sprite>("Platforms/"+ gameObject.name + "/"+currentPV);
    }

    private void OnMouseDown()
    {
        TakeDamage(1);
    }
}
