using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LaserSize : MonoBehaviour {

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
    GameObject boxCollider;
    [SerializeField]
    LayerMask layerMaskRaycast;
    
    BoxCollider2D coll;
    RaycastHit2D raycast;
    bool FXinstantiate;
    ParticleSystem instEndLaser;
    ParticleSystem.ShapeModule pShape;
    ParticleSystem.EmissionModule pEmission;
    float startRate;
    float startScale;

    private void Start()
    {
        coll = boxCollider.GetComponent<BoxCollider2D>();
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
            Debug.DrawLine(startPos, raycast.point, Color.red);
            if (!FXinstantiate)
            {
                FXinstantiate = true;
                instEndLaser = Instantiate(endLaser, raycast.point, transform.rotation);
            }
            if(instEndLaser != null)
            {
                instEndLaser.transform.position = raycast.point;
            }
            center.SetPosition(1, transform.InverseTransformPoint(raycast.point));
            glow.SetPosition(1, transform.InverseTransformPoint(raycast.point));

            coll.size = new Vector3(Vector3.Distance(center.GetPosition(0), center.GetPosition(1)), coll.size.y, 0);
            coll.transform.position = transform.TransformPoint((center.GetPosition(0) + center.GetPosition(1)) / 2);
            sparks.transform.position = coll.transform.position;
            float newScale = Vector3.Distance(transform.TransformPoint(center.GetPosition(0)), transform.TransformPoint(center.GetPosition(1)));
            pShape.scale = new Vector2(newScale, 0);
            pEmission.rateOverTime = startRate * newScale / startScale;
        }
        else
        {
            center.SetPosition(1, -start.transform.right * 10);
            glow.SetPosition(1, -start.transform.right * 10);
        }
    }
}
