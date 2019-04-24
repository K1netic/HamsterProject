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
    float waitingTime;
    [SerializeField]
    bool inverseMovement;
    [SerializeField]
    bool fullVerticalMovement;

    Vector2 target;

    Vector2 node1pos;
    Vector2 node2pos;

    Rigidbody2D rigid;

    bool waiting;
    bool startMoving;

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
        if (MatchStart.gameHasStarted)
        {
            if (fullVerticalMovement)
            {
                if (transform.position.y <= node1pos.y && !startMoving)
                {
                    StartCoroutine(Wait());
                    target = node2pos - (Vector2)transform.position;
                }
                else if (transform.position.y >= node2pos.y && !startMoving)
                {
                    StartCoroutine(Wait());
                    target = node1pos - (Vector2)transform.position;
                }
            }
            else
            {
                if (transform.position.x <= node1pos.x && !startMoving)
                {
                    StartCoroutine(Wait());
                    target = node2pos - (Vector2)transform.position;
                }
                else if (transform.position.x >= node2pos.x && !startMoving)
                {
                    StartCoroutine(Wait());
                    target = node1pos - (Vector2)transform.position;
                }
            }

            if (!waiting)
            {
                rigid.velocity = moveSpeed * target.normalized;
            }
        }
        
    }

    IEnumerator Wait()
    {
        waiting = true;
        rigid.velocity = Vector3.zero;
        yield return new WaitForSeconds(waitingTime);
        startMoving = true;
        waiting = false;
        yield return new WaitForSeconds(.5f); //Attend 0.5 secondes avant de changer le booléan pour etre sur que la plateforme ait franchir la limite
        startMoving = false;
    }
}

