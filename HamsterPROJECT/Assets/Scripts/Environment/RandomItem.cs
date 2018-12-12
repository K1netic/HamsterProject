using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomItem : MonoBehaviour {

    /* Liste des items 
     * 
     * Corde en acier
     * Boule à pics
     * Grappin explosif
     * Parachute 
     * Coque de protection
     * Protection miracle
     * Croquettes cocaïnées
     * Crotte explosive
    */

    //Répartir les minimums et maximums entre 1 et 100, pour obtenir des %. Par exemple pour la SteelRope, min = 1 et max = 20, elle aura 20% de chance de sortir.

    [SerializeField]
    float repopTime = 10;
    [SerializeField]
    int minSteelRope;
    [SerializeField]
    int maxSteelRope;
    [SerializeField]
    int minPeakBall;
    [SerializeField]
    int maxPeakBall;
    [SerializeField]
    int minExplosiveHook;
    [SerializeField]
    int maxExplosiveHook;
    [SerializeField]
    int minParachute;
    [SerializeField]
    int maxParachute;
    [SerializeField]
    int minShield;
    [SerializeField]
    int maxShield;
    [SerializeField]
    int minMiracle;
    [SerializeField]
    int maxMiracle;
    [SerializeField]
    int minCocaineCroquettes;
    [SerializeField]
    int maxCocaineCroquettes;
    [SerializeField]
    int minExplosivePoop;
    [SerializeField]
    int maxExplosivePoop;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            int n = Random.Range(1, 100);

            if(n >= minSteelRope && n <= maxSteelRope)
            {
                Text item = collision.gameObject.GetComponent<Inventory>().item;
                item.text = "SteelRope";
                collision.gameObject.GetComponent<Inventory>().currentItem = "SteelRope";
//                print("SteelRope");
            }else if(n >= minPeakBall && n <= maxPeakBall)
            {
                Text item = collision.gameObject.GetComponent<Inventory>().item;
                item.text = "PeakBall";
                collision.gameObject.GetComponent<Inventory>().currentItem = "PeakBall";
//                print("PeakBall");
            }else if(n >= minExplosiveHook && n <= maxExplosiveHook)
            {
                Text item = collision.gameObject.GetComponent<Inventory>().item;
                item.text = "ExplosiveHook";
                collision.gameObject.GetComponent<Inventory>().currentItem = "ExplosiveHook";
//                print("ExplosiveHook");
            }else if(n >= minParachute && n <= maxParachute)
            {
                Text item = collision.gameObject.GetComponent<Inventory>().item;
                item.text = "Parachute";
                collision.gameObject.GetComponent<Inventory>().currentItem = "Parachute";
//                print("Parachute");
            }else if(n >= minShield && n <= maxShield)
            {
                Text item = collision.gameObject.GetComponent<Inventory>().item;
                item.text = "Shield";
                collision.gameObject.GetComponent<Inventory>().currentItem = "Shield";
//                print("Shield");
            }else if(n >= minMiracle && n <= maxMiracle)
            {
                Text item = collision.gameObject.GetComponent<Inventory>().item;
                item.text = "Miracle";
                collision.gameObject.GetComponent<Inventory>().currentItem = "Miracle";
//                print("Miracle");
            }else if(n >= minCocaineCroquettes && n <= maxCocaineCroquettes)
            {
                Text item = collision.gameObject.GetComponent<Inventory>().item;
                item.text = "CocaineCroquettes";
                collision.gameObject.GetComponent<Inventory>().currentItem = "CocaineCroquettes";
//                print("CocaineCroquettes");
            }
            else if(n >= minExplosivePoop && n <= maxExplosivePoop)
            {
                Text item = collision.gameObject.GetComponent<Inventory>().item;
                item.text = "ExplosivePoop";
                collision.gameObject.GetComponent<Inventory>().currentItem = "ExplosivePoop";
//                print("ExplosivePoop");
            }

            gameObject.SetActive(false);
            Invoke("Reactivate", repopTime);

            /*switch (random)
            {
                case int n when (n >= minSteelRope && n <= maxSteelRope):
                    print("SteelRope");
                    break;
                case int n when (n >= minPeakBall && n <= maxPeakBall):
                    print("PeakBall");
                    break;
                case int n when (n >= minExplosiveHook && n <= maxExplosiveHook):
                    print("ExplosiveHook");
                    break;
                case int n when (n >= minParachute && n <= maxParachute):
                    print("Parachute");
                    break;
                case int n when (n >= minShield && n <= maxShield):
                    print("Shield");
                    break;
                case int n when (n >= minMiracle && n <= maxMiracle):
                    print("Miracle");
                    break;
                case int n when (n >= minCocaineCroquettes && n <= maxCocaineCroquettes):
                    print("CocaineCroquettes");
                    break;
                case int n when (n >= minExplosivePoop && n <= maxExplosivePoop):
                    print("ExplosivePoop");
                    break;
                default:
                    break;
            }*/
        }
    }

    void Reactivate()
    {
        gameObject.SetActive(true);
    }
}
