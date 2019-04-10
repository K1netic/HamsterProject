using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LaserSize : MonoBehaviour {

    [SerializeField]
    bool FxNotNeeded;
    [SerializeField]
    GameObject start;
    Vector2 startPos;

    [SerializeField]
    LineRenderer center;
    [SerializeField]
    LineRenderer glow;
    [SerializeField]
    ParticleSystem sparks;
    [SerializeField]
    ParticleSystem endLaser;
    [SerializeField]
    GameObject capsCollider;
    [SerializeField]
    GameObject capsColliderTop;
    [SerializeField]
    LayerMask layerMaskRaycast;
    
    CapsuleCollider2D colliderBot;
    CapsuleCollider2D colliderTop;
    RaycastHit2D raycast;
    bool FXinstantiate;
    ParticleSystem instEndLaser;
    ParticleSystem.ShapeModule pShape;
    ParticleSystem.EmissionModule pEmission;
    float startRate;
    float startScale;

    [HideInInspector]
    public Vector2 laserDirection;

    private void Start()
    {
        if(gameObject.CompareTag("LaserEdge"))
            colliderBot = capsCollider.GetComponent<CapsuleCollider2D>();
        else
        {
            colliderBot = capsCollider.GetComponent<CapsuleCollider2D>();
            colliderTop = capsColliderTop.GetComponent<CapsuleCollider2D>();
        }
        startPos = start.transform.position;
        center.SetPosition(0, transform.InverseTransformPoint(startPos));
        glow.SetPosition(0, transform.InverseTransformPoint(startPos));
        pShape = sparks.GetComponent<ParticleSystem>().shape;
        pEmission = sparks.GetComponent<ParticleSystem>().emission;
        startRate = pEmission.rateOverTime.constantMax;
        startScale = pShape.scale.x; 
    }

    private void Update()
    {
        Raycasting();
        
    }

    void Raycasting()
    {
        startPos = start.transform.position;
        raycast = Physics2D.Raycast(startPos, -start.transform.right, 500 , layerMaskRaycast);
        
        if(raycast.collider != null)
        {
            if (!FxNotNeeded)
            {
                if (instEndLaser != null)
                {
                    instEndLaser.transform.position = raycast.point;
                }
            }
            Debug.DrawLine(startPos, raycast.point, Color.red);
            if (!FxNotNeeded)
            {
                if (!FXinstantiate)
                {
                    FXinstantiate = true;
                    instEndLaser = Instantiate(endLaser, raycast.point, transform.rotation);
                }
            }

            center.SetPosition(1, transform.InverseTransformPoint(raycast.point));
            glow.SetPosition(1, transform.InverseTransformPoint(raycast.point));

            if (gameObject.CompareTag("LaserEdge"))
            {
                colliderBot.size = new Vector3(Vector3.Distance(center.GetPosition(0), center.GetPosition(1)) - .2f, colliderBot.size.y, 0);
                colliderBot.transform.position = transform.TransformPoint((center.GetPosition(0) + center.GetPosition(1)) / 2);
                sparks.transform.position = colliderBot.transform.position;
            }
            else
            {
                colliderBot.size = new Vector3(Vector3.Distance(center.GetPosition(0), center.GetPosition(1)) - .2f, colliderBot.size.y, 0);
                colliderTop.size = new Vector3(Vector3.Distance(center.GetPosition(0), center.GetPosition(1)) - .2f, colliderTop.size.y, 0);
                colliderBot.transform.position = transform.TransformPoint((center.GetPosition(0) + center.GetPosition(1)) / 2);
                colliderTop.transform.position = transform.TransformPoint((center.GetPosition(0) + center.GetPosition(1)) / 2);
                sparks.transform.position = colliderBot.transform.position;
            }
            
            float newScale = Vector3.Distance(transform.TransformPoint(center.GetPosition(0)), transform.TransformPoint(center.GetPosition(1)));
            pShape.scale = new Vector2(newScale, 0);
            pEmission.rateOverTime = startRate * newScale / startScale;

            laserDirection = raycast.point - (Vector2)start.transform.position;
        }
        else
        {
            center.SetPosition(1, -start.transform.right * 10);
            glow.SetPosition(1, -start.transform.right * 10);
        }
    }
}
