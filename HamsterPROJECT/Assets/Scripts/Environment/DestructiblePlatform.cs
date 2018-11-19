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
    string gameObjectName;
    string carbage = " (UnityEngine.Sprite)";

    private void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        gameObjectName = myRenderer.sprite.name;
        //La ligne en dessous sert à enlever " (UnityEngine.Sprite)" du string, car quand on fait sprite.name on récupère son nom mais avec " (UnityEngine.Sprite)" à la fin......
        gameObjectName = gameObjectName.Replace(carbage, "");
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
        myRenderer.sprite = Resources.Load<Sprite>("Platforms/"+ gameObjectName+"/"+currentPV);
    }

    private void OnMouseDown()
    {
        TakeDamage(1);
    }
}
