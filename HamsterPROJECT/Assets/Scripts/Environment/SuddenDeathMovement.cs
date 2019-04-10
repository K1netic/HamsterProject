using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuddenDeathMovement : MonoBehaviour {

    Balancing balanceData;

    [SerializeField]
    [Range(24f, 34f)]
    float remainingSpace;

    GameObject[] childs = new GameObject[4];
    float suddenDeathSpeed;
    bool launchSuddenDeath;
    Camera cam;

    void Start () {
        balanceData = GameObject.Find("Balancing").GetComponent<Balancing>();

        suddenDeathSpeed = balanceData.suddenDeathSpeed;

        cam = FindObjectOfType<Camera>();

        for (int i = 0; i < childs.Length; i++)
        {
            childs[i] = gameObject.transform.GetChild(i).gameObject;
        }
    }
	
	void Update () {
        if (launchSuddenDeath)
        {
            for (int i = 0; i < childs.Length; i++)
            {
                childs[i].transform.position = Vector3.MoveTowards(childs[i].transform.position, Vector3.zero, suddenDeathSpeed);
            }
            if(childs[0].transform.position.x <= remainingSpace)
            {
                launchSuddenDeath = false;
            }

            if(childs[0].transform.position.x <= 38)
                cam.orthographicSize -= suddenDeathSpeed/2;
        }
        if (Input.GetKeyDown(KeyCode.I))
            LaunchSuddenDeath();
	}

    public void LaunchSuddenDeath()
    {
        launchSuddenDeath = true;
    }
}
