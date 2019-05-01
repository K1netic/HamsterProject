using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuddenDeathMovement : MonoBehaviour {

    Balancing balanceData;

    [SerializeField]
    GameObject warning;
    [SerializeField]
    [Range(24f, 34f)]
    float remainingSpace;

    GameObject[] childs = new GameObject[4];
    float suddenDeathSpeed;
    float suddenDeathTime;
    bool launchSuddenDeath;
    bool warningDone;
    Camera cam;
    float counter;

    void Start () {
        balanceData = GameObject.Find("Balancing").GetComponent<Balancing>();

        suddenDeathSpeed = balanceData.suddenDeathSpeed;
        suddenDeathTime = balanceData.suddenDeathTime;

        cam = FindObjectOfType<Camera>();

        for (int i = 0; i < childs.Length; i++)
        {
            childs[i] = gameObject.transform.GetChild(i).gameObject;
        }
        counter = 0;
    }
	
	void Update () {
        if (MatchStart.gameHasStarted && !launchSuddenDeath && counter < suddenDeathTime)
        {
            counter += Time.deltaTime;
            if (counter > suddenDeathTime)
            {
                launchSuddenDeath = true;
            }else if(counter > suddenDeathTime - 1)
            {
                if (!warningDone)
                {
                    LaunchWarning();
                }
            }
                
        }
        if (launchSuddenDeath)
        {
            if (!warningDone)
            {
                LaunchWarning();
            }
            for (int i = 0; i < childs.Length; i++)
            {
                childs[i].transform.position = Vector3.MoveTowards(childs[i].transform.position, Vector3.zero, suddenDeathSpeed);
            }
            if(childs[0].transform.position.x <= remainingSpace)
            {
                launchSuddenDeath = false;
            }

            if(childs[0].transform.position.x <= 38.5)
                cam.orthographicSize -= suddenDeathSpeed/2;
        }
        if (Input.GetKeyDown(KeyCode.I))
            launchSuddenDeath = true;
    }

    void LaunchWarning()
    {
        warningDone = true;
        Instantiate(warning);
    }
}
