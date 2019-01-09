using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldCollider : MonoBehaviour {

    PolygonCollider2D coll;

    Vector2[] raycastOrigins;

    int x;

    void Start()
    {
        coll = GetComponent<PolygonCollider2D>();
        raycastOrigins = new Vector2[coll.points.Length];        
    }

    private void Update()
    {
        UpdateOrigins();
    }

    void UpdateOrigins()
    {
        for (int i = 0; i < coll.points.Length; i++)
        {
            x = i + 1;
            if (x > coll.points.Length - 1)
                x = 0;
            print(x);
            raycastOrigins[i] = (Vector2)coll.transform.TransformPoint(coll.points[i]);
            Debug.DrawRay(raycastOrigins[i], raycastOrigins[i], Color.green);
            Debug.DrawRay((raycastOrigins[i] + raycastOrigins[x]) / 2, (raycastOrigins[i] + raycastOrigins[x]) / 2, Color.green);
        }
    }
    
}
