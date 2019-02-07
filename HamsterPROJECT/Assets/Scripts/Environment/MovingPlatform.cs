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
    [SerializeField]
    bool inverseMovement;
    [SerializeField]
    bool fullVerticalMovement;

    Vector2 target;

    Vector2 node1pos;
    Vector2 node2pos;

    Rigidbody2D rigid;

    private void Start()
    {
        if (fullVerticalMovement)
        {
            if (node1.transform.position.y < node2.transform.position.y)
            {
                node1pos = (Vector2)node1.transform.position;
                node2pos = (Vector2)node2.transform.position;
            }
            else
            {
                node1pos = (Vector2)node2.transform.position;
                node2pos = (Vector2)node1.transform.position;
            }
        }
        else
        {
            if (node1.transform.position.x < node2.transform.position.x)
            {
                node1pos = (Vector2)node1.transform.position;
                node2pos = (Vector2)node2.transform.position;
            }
            else
            {
                node1pos = (Vector2)node2.transform.position;
                node2pos = (Vector2)node1.transform.position;
            }
        }

        if (!inverseMovement)
            target = node1pos - (Vector2)transform.position;
        else
            target = node2pos - (Vector2)transform.position;

        rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (fullVerticalMovement)
        {
            if (transform.position.y <= node1pos.y)
            {
                target = node2pos - (Vector2)transform.position;
            }
            else if (transform.position.y >= node2pos.y)
            {
                target = node1pos - (Vector2)transform.position;
            }
        }
        else
        {
            if (transform.position.x <= node1pos.x)
            {
                target = node2pos - (Vector2)transform.position;
            }
            else if (transform.position.x >= node2pos.x)
            {
                target = node1pos - (Vector2)transform.position;
            }
        }

        rigid.velocity = moveSpeed * target.normalized;
        //transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.FromToRotation(-Vector3.right, target),Time.deltaTime);
    }
}

