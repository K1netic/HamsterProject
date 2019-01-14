using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldCollider : MonoBehaviour {

    Balancing balanceData;

    [Range(0f, 1f)]
    [SerializeField]
    float raycastRange = 1;

    [HideInInspector]
    public LayerMask layerMaskRaycast;

    PolygonCollider2D coll;

    Vector2[] raycastOrigins;
    Vector2[] raycastDirections;
    Vector2[] halfwayRaycastOrigins;
    Vector2[] halfwayRaycastDirections;
    RaycastHit2D[] raycasts;
    RaycastHit2D[] halfwayRaycasts;

    Hook arrow;
    Hook opponentArrow;

    int x;
    float knockBackShieldHit;
    float knockBackTime;

    void Start()
    {
        //S'il y a une erreur ici s'assurer que le prefab "Balancing" est bien dans la scène
        balanceData = GameObject.Find("Balancing").GetComponent<Balancing>();

        knockBackShieldHit = balanceData.knockBackShieldHit;
        knockBackTime = balanceData.knockBackTime;

        //Récupère le collider du shield
        coll = GetComponents<PolygonCollider2D>()[1];
        raycastOrigins = new Vector2[coll.points.Length];
        raycastDirections = new Vector2[coll.points.Length];
        halfwayRaycastOrigins = new Vector2[coll.points.Length];
        halfwayRaycastDirections = new Vector2[coll.points.Length];
        raycasts = new RaycastHit2D[coll.points.Length];
        halfwayRaycasts = new RaycastHit2D[coll.points.Length];
        arrow = GetComponent<Hook>();
    }

    private void Update()
    {
        if(arrow.currentState == Hook.HookState.Shield)
        {
            UpdateOrigins();
            Raycasting();
            CheckRaycast();
        }    
    }

    //Rempli les tableaux avec les coordonnées du collider transcrite dans le world space
    void UpdateOrigins()
    {
        for (int i = 0; i < coll.points.Length; i++)
        {
            x = i + 1;
            if (x > coll.points.Length - 1)
                x = 0;
            raycastOrigins[i] = (Vector2)coll.transform.TransformPoint(coll.points[i]);
            raycastDirections[i] = (raycastOrigins[i] - (Vector2)transform.position).normalized;
            //halfway correspond à des points qui sont pile entre deux points du collider, ils sont la pour doubler le nombre de raycast effectués
            halfwayRaycastOrigins[i] = (raycastOrigins[i] + (Vector2)coll.transform.TransformPoint(coll.points[x])) / 2;
            halfwayRaycastDirections[i] = (((raycastOrigins[i] + (Vector2)coll.transform.TransformPoint(coll.points[x])) / 2 - (Vector2)transform.position).normalized);
        }
    }
    
    void Raycasting()
    {
        for (int i = 0; i < coll.points.Length; i++)
        {
            raycasts[i] = Physics2D.Raycast(raycastOrigins[i], raycastDirections[i], raycastRange, layerMaskRaycast);
            halfwayRaycasts[i] = Physics2D.Raycast(halfwayRaycastOrigins[i], halfwayRaycastDirections[i], raycastRange, layerMaskRaycast);
            Debug.DrawRay(raycastOrigins[i], raycastDirections[i] * raycastRange, Color.blue);
            Debug.DrawRay(halfwayRaycastOrigins[i], halfwayRaycastDirections[i] * raycastRange, Color.blue);
        }
    }

    //Parcourt les tableaux de raycast pour vérifier si l'un d'eux ne touche pas une flèche adverse
    void CheckRaycast()
    {
        int i = 0;
        while(i < coll.points.Length)
        {
            if(raycasts[i].collider != null)
            {
                print(raycasts[i].collider.gameObject.name);
                if (raycasts[i].collider.gameObject.CompareTag("Arrow"))
                {
                    print("arrow");
                    opponentArrow = raycasts[i].collider.gameObject.GetComponent<Hook>();
                    if (opponentArrow.currentState == Hook.HookState.Arrow)
                    {
                        KnockBack(opponentArrow, raycasts[i].collider.gameObject);
                        break;
                    }
                }
            }else if (halfwayRaycasts[i].collider != null)
            {
                if (halfwayRaycasts[i].collider.gameObject.CompareTag("Arrow"))
                {
                    opponentArrow = halfwayRaycasts[i].collider.gameObject.GetComponent<Hook>();
                    if (opponentArrow.currentState == Hook.HookState.Arrow)
                    {
                        KnockBack(opponentArrow, halfwayRaycasts[i].collider.gameObject);
                        break;
                    }
                }
            }
            i++;
        }
    }

    //Applique un knockback sur l'adversaire
    void KnockBack(Hook opponentArrow, GameObject opponent)
    {
        opponentArrow.playerMovement.lockMovementKnockBack = true;
        Vector2 directionKnockBack = -(opponent.transform.position - transform.position).normalized;
        opponentArrow.playerMovement.rigid.velocity = Vector3.zero;
        opponentArrow.playerMovement.rigid.AddForce(-directionKnockBack * knockBackShieldHit, ForceMode2D.Impulse);
        StartCoroutine(UnlockMovement(opponentArrow.playerMovement));
    }

    IEnumerator UnlockMovement(PlayerMovement p)
    {
        yield return new WaitForSeconds(knockBackTime);
        opponentArrow.playerMovement.lockMovementKnockBack = false;
    }
}
