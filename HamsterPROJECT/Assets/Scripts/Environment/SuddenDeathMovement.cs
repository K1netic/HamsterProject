using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuddenDeathMovement : MonoBehaviour {

    Balancing balanceData;

    [SerializeField]
    GameObject factoryWarning;
    [SerializeField]
    GameObject desertWarning;
    [SerializeField]
    GameObject forestWarning;
    [SerializeField]
    GameObject submarineWarning;
    [SerializeField]
    [Range(24f, 36f)]
    float remainingSpace;

    GameObject[] childs = new GameObject[4];
    float suddenDeathSpeed;
    float suddenDeathTime;
    float timeRemainingWhenTwoPlayersLeft;
    bool launchSuddenDeath;
    bool warningDone;
    Camera cam;
    float counter;

    void Start () {
        balanceData = GameObject.Find("Balancing").GetComponent<Balancing>();

        suddenDeathSpeed = balanceData.suddenDeathSpeed;
        suddenDeathTime = balanceData.suddenDeathTime;
        timeRemainingWhenTwoPlayersLeft = balanceData.timeRemainingWhenTwoPlayersLeft;

        cam = FindObjectOfType<Camera>();

        for (int i = 0; i < childs.Length; i++)
        {
            childs[i] = gameObject.transform.GetChild(i).gameObject;
        }
        counter = 0;
        foreach (GameObject item in childs)
        {
            item.SetActive(false);
        }
    }
	
	void Update () {
        if (MatchStart.gameHasStarted && !launchSuddenDeath && counter < suddenDeathTime)
        {
            if (GameManager.HowManyPlayersPlaying() > 2 && GameManager.HowManyPlayersAlive() == 2)
            {
                if (suddenDeathTime - counter > timeRemainingWhenTwoPlayersLeft)
                    counter = suddenDeathTime - timeRemainingWhenTwoPlayersLeft;
            }

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
        foreach (GameObject item in childs)
        {
            item.SetActive(true);
        }
        warningDone = true;
        switch (GameObject.FindGameObjectWithTag("Background").name)
        {
            case "Factory":
                Instantiate(factoryWarning);
                break;
            case "Desert":
                Instantiate(desertWarning);
                break;
            case "Forest":
                Instantiate(forestWarning);
                break;
            case "Submarine":
                Instantiate(submarineWarning);
                break;
            default:
                print("Mets un fond si tu veux un warning");
                break;
        }
        AudioManager.instance.PlaySound("UI_alarmSuddenDeath", "UI");
    }
}
