using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassWallSize : MonoBehaviour {

    [SerializeField]
    GameObject start;
    Vector2 startPos;

    [SerializeField]
    LineRenderer center;
    [SerializeField]
    LineRenderer glow;
    [SerializeField]
    LineRenderer ring;
    [SerializeField]
    GameObject capsCollider;
    [SerializeField]
    LayerMask layerMaskRaycast;

    CapsuleCollider2D caps;
    RaycastHit2D raycast;

    [HideInInspector]
    public Vector2 laserDirection;

    private void Start()
    {
        caps = capsCollider.GetComponent<CapsuleCollider2D>();
        startPos = start.transform.position;
        center.SetPosition(0, transform.InverseTransformPoint(startPos));
        glow.SetPosition(0, transform.InverseTransformPoint(startPos));
        ring.SetPosition(0, transform.InverseTransformPoint(startPos));
    }

    private void Update()
    {
        Raycasting();
    }

    void Raycasting()
    {
        startPos = start.transform.position;
        raycast = Physics2D.Raycast(startPos, -start.transform.right, 500, layerMaskRaycast);

        if (raycast.collider != null)
        {
            center.SetPosition(1, transform.InverseTransformPoint(raycast.point));
            glow.SetPosition(1, transform.InverseTransformPoint(raycast.point));
            ring.SetPosition(1, transform.InverseTransformPoint(raycast.point));

            caps.size = new Vector3(Vector3.Distance(center.GetPosition(0), center.GetPosition(1)) - .2f, caps.size.y, 0);
            caps.transform.position = transform.TransformPoint((center.GetPosition(0) + center.GetPosition(1)) / 2);

            laserDirection = raycast.point - (Vector2)start.transform.position;
        }
        else
        {
            center.SetPosition(1, -start.transform.right * 10);
            glow.SetPosition(1, -start.transform.right * 10);
        }
    }
}
