using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour {

    [SerializeField]
    GameObject meteorPrefab;
    [SerializeField]
    float timeBeforeFirstSpawn;
    [SerializeField]
    int howManySpawn;
    [SerializeField]
    float timeBetweenEachSpawn;
    [Range(-35f, 35f)]
    [SerializeField]
    float leftBound = -35; //Ces valeurs correpsondent à une arène avec la caméra à 22.5
    [Range(-35f, 35f)]
    [SerializeField]
    float rightBound = 35;
    float targetY = -30;
    float timer;

	void Start()
    {
        transform.position = new Vector3(transform.position.x, 50, 0);//Nécessaire pour que le warning soit placé au bon endroit
    }

	void Update () {
        if(MatchStart.gameHasStarted)
            timer += Time.deltaTime;
        if(timer > timeBeforeFirstSpawn)
        {
            GameObject meteor = Instantiate(meteorPrefab, transform.position, transform.rotation);
            meteor.GetComponent<MeteorBehavior>().leftBound = leftBound;
            meteor.GetComponent<MeteorBehavior>().rightBound = rightBound;
            meteor.GetComponent<MeteorBehavior>().targetY = targetY;
            howManySpawn--;
            if (howManySpawn > 0)
            {
                timer -= timeBetweenEachSpawn;
            }
            else
            {
                Destroy(gameObject);
            }
        }
	}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector3(leftBound, targetY, 0), Vector3.one);
        Gizmos.DrawWireCube(new Vector3(rightBound, targetY, 0), Vector3.one);
    }
}
