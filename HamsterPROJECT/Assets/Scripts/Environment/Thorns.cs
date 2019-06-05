using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;


public class Thorns : MonoBehaviour {

    Balancing balanceData;

    [Header("Options")]
    [SerializeField]
    bool manualPlatformSelection;
    [SerializeField]
    float timeBeforeSpawn;
    [Range(3, 20)]
    [SerializeField]
    float timeBetweenEachInfest;
    [Header("Targets")]
    [SerializeField]
    List<GameObject> allPlatforms = new List<GameObject>();
    [Header("Required Objects")]
    [SerializeField]
    SpriteShape thornsShape;
    [SerializeField]
    SpriteShape thornsLinearShape;
    [SerializeField]
    GameObject thornsBall;


    SpriteShapeController[] controllers;
    
    List<GameObject> targetPlatforms = new List<GameObject>();

    SpriteShape stockedShape;
    GameObject currentPlatform;
    GameObject ball;
    ThornsBall ballScript;
    bool infestCall;
    float thornsBallSpeed;

    void Start () {
        //S'il y a une erreur ici s'assurer que le prefab "Balancing" est bien dans la scène
        balanceData = GameObject.Find("Balancing").GetComponent<Balancing>();

        thornsBallSpeed = balanceData.thornsBallSpeed;

        controllers = GameObject.FindObjectsOfType<SpriteShapeController>();
        if (!manualPlatformSelection)
        {
            foreach (SpriteShapeController item in controllers)
            {
                if (item.gameObject.layer == 16 && !item.gameObject.GetComponent<Trampoline>())
                    allPlatforms.Add(item.gameObject);
            }
        }
        FillTargetsPlatforms();

    }
	
	void Update () {
        if (MatchStart.gameHasStarted && !infestCall)
        {
            Invoke("Infest", timeBeforeSpawn);
            infestCall = true;
        }
            if (Input.GetKeyDown(KeyCode.L))
            Infest();
	}

    void Infest()
    {
        if (currentPlatform)
        {
            currentPlatform.GetComponent<SpriteShapeController>().spriteShape = stockedShape;
            currentPlatform.tag = "Hookable";
            ball = Instantiate(thornsBall, currentPlatform.transform.position, currentPlatform.transform.rotation);
        }
        else
        {
            ball = Instantiate(thornsBall, new Vector3(0,30,0), Quaternion.identity);
        }
        ballScript = ball.GetComponent<ThornsBall>();
        currentPlatform = targetPlatforms[Random.Range(0, targetPlatforms.Count)];
        ballScript.target = currentPlatform;
        ballScript.speed = thornsBallSpeed;
        ballScript.thornsSystem = this;
        FillTargetsPlatforms();
        targetPlatforms.Remove(currentPlatform);
    }

    public void SwitchSpriteShape()
    {
        stockedShape = currentPlatform.GetComponent<SpriteShapeController>().spriteShape;
        if(stockedShape.name.Length - "LinearPlatform".Length > 0)
        {
            
            if(stockedShape.name.Substring(stockedShape.name.Length - "LinearPlatform".Length) == "LinearPlatform")
                currentPlatform.GetComponent<SpriteShapeController>().spriteShape = thornsLinearShape;
            else
                currentPlatform.GetComponent<SpriteShapeController>().spriteShape = thornsShape;
        }
        else
            currentPlatform.GetComponent<SpriteShapeController>().spriteShape = thornsShape;
        currentPlatform.tag = "Thorns";
        Invoke("Infest", timeBetweenEachInfest);
    }

    void FillTargetsPlatforms()
    {
        targetPlatforms.Clear();
        foreach (GameObject item in allPlatforms)
        {
            targetPlatforms.Add(item);
        }
    }
}
