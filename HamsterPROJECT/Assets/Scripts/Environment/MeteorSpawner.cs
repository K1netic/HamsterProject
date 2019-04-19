using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour {

    [SerializeField]
    GameObject meteorPrefab;
    [SerializeField]
    float timeBeforeSpawn;
    [Range(-35f, 35f)]
    [SerializeField]
    float leftBound = -35; //Ces valeurs correpsondent à une arène avec la caméra à 22.5
    [Range(-35f, 35f)]
    [SerializeField]
    float rightBound = 35;
    float targetY = -30;
    float timer;

	// Update is called once per frame
	void Update () {
        if(MatchStart.gameHasStarted)
            timer += Time.deltaTime;
        if(timer > timeBeforeSpawn)
        {
            timer = 0;
            GameObject meteor = Instantiate(meteorPrefab, transform.position, transform.rotation);
            meteor.GetComponent<MeteorBehavior>().leftBound = leftBound;
            meteor.GetComponent<MeteorBehavior>().rightBound = rightBound;
            meteor.GetComponent<MeteorBehavior>().targetY = targetY;
        }
	}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector3(0, targetY, 0), new Vector3(Vector3.Distance(new Vector3(leftBound,targetY,0), new Vector3(rightBound, targetY, 0)),1,0));
    }
}
