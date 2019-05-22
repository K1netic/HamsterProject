using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;


public class Thorns : MonoBehaviour {

    [SerializeField]
    SpriteShape thornsShape;

    SpriteShapeController[] controllers;
    public List<GameObject> allPlatforms = new List<GameObject>();
    public List<GameObject> targetPlatforms = new List<GameObject>();

    SpriteShape stockedShape;
    GameObject currentPlatform;

	void Start () {
        controllers = GameObject.FindObjectsOfType<SpriteShapeController>();
        foreach (SpriteShapeController item in controllers)
        {
            if (item.gameObject.layer == 16 && !item.gameObject.GetComponent<Trampoline>())
                allPlatforms.Add(item.gameObject);
        }
        FillTargetsPlatforms();

    }
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.L))
            Infest();
	}

    void Infest()
    {
        if(currentPlatform)
            currentPlatform.GetComponent<SpriteShapeController>().spriteShape = stockedShape;
        currentPlatform = targetPlatforms[Random.Range(0, targetPlatforms.Count)];
        FillTargetsPlatforms();
        targetPlatforms.Remove(currentPlatform);
        //Anim && wait
        SwitchSpriteShape();
    }

    void SwitchSpriteShape()
    {
        stockedShape = currentPlatform.GetComponent<SpriteShapeController>().spriteShape;
        currentPlatform.GetComponent<SpriteShapeController>().spriteShape = thornsShape;
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
