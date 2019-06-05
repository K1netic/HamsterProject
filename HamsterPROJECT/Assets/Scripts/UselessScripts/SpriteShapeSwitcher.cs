using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SpriteShapeSwitcher : MonoBehaviour {

    [Header("Factory")]
    [SerializeField]
    SpriteShape factoryLinearShape;
    [SerializeField]
    SpriteShape factoryShape;
    [SerializeField]
    SpriteShape factoryFiller;

    [Header("Desert")]
    [SerializeField]
    SpriteShape desertLinearShape;
    [SerializeField]
    SpriteShape desertShape;
    [SerializeField]
    SpriteShape desertFiller;

    [Header("Forest")]
    [SerializeField]
    SpriteShape forestLinearShape;
    [SerializeField]
    SpriteShape forestShape;
    [SerializeField]
    SpriteShape forestFiller;

    [Header("Submarine")]
    [SerializeField]
    SpriteShape submarineLinearShape;
    [SerializeField]
    SpriteShape submarineShape;
    [SerializeField]
    SpriteShape submarineFiller;

    GameObject[] hookableObjects;
    List<GameObject> spriteShapeObjects;
    List<GameObject> dpObjects;

    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag("Background"))
        {
            hookableObjects = GameObject.FindGameObjectsWithTag("Hookable");
            foreach (GameObject item in hookableObjects)
            {
                if (item.GetComponent<SpriteShapeController>())
                {
                    spriteShapeObjects.Add(item);
                }else if (item.GetComponent<Explodable>())
                {
                    dpObjects.Add(item);
                }
                else
                {
                    print(item.name+ " t'es qui toi");
                }
            }
            switch (GameObject.FindGameObjectWithTag("Background").name)
            {
                case "Factory":
                    GameObject.Find("Filler").GetComponent<SpriteShapeController>().spriteShape = factoryFiller;
                    break;
                case "Desert":
                    GameObject.Find("Filler").GetComponent<SpriteShapeController>().spriteShape = desertFiller;
                    break;
                case "Forest":
                    GameObject.Find("Filler").GetComponent<SpriteShapeController>().spriteShape = forestFiller;
                    break;
                case "Submarine":
                    GameObject.Find("Filler").GetComponent<SpriteShapeController>().spriteShape = submarineFiller;
                    break;
                default:
                    break;
            }
        }
        else
        {
            print("Mets un background gros sac");
        }
        //Envoyer un signal aux scripts explodable ?
    }
}
