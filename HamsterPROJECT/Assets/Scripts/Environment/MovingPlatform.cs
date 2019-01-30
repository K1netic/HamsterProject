using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform : MonoBehaviour {

    [SerializeField]
    GameObject node1;
    [SerializeField]
    GameObject node2;
    [SerializeField]
    float moveSpeed;

    Vector2 target;

    Vector2 node1pos;
    Vector2 node2pos;

    Rigidbody2D rigid;

    private void Start()
    {
        if(node1.transform.position.x < node2.transform.position.x)
        {
            node1pos = (Vector2)node1.transform.position;
            node2pos = (Vector2)node2.transform.position;
        }
        else
        {
            node1pos = (Vector2)node2.transform.position;
            node2pos = (Vector2)node1.transform.position;
        }

        target = node1pos - (Vector2)transform.position;

        rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (transform.position.x <= node1pos.x)
        {
            target = node2pos - (Vector2)transform.position;
        }
        else if (transform.position.x >= node2pos.x)
        {
            target = node1pos - (Vector2)transform.position;
        }

        rigid.velocity = moveSpeed * target.normalized;
    }
}

