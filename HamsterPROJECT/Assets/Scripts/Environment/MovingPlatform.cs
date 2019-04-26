using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform : MonoBehaviour {

    [SerializeField]
    bool waitBeforeStart;
    [SerializeField]
    float timeBeforeStart;
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

    float timer;

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
            if (waitBeforeStart)
            {
                timer += Time.deltaTime;
                if(timer > timeBeforeStart)
                    waitBeforeStart = false;
            }
            else
            {
                if (fullVerticalMovement)
                {
                    if (transform.position.y > node1pos.y && transform.position.y < node2pos.y && startMoving)
                        startMoving = false;
                    if (transform.position.y <= node1pos.y && !startMoving && !waiting)
                        StartCoroutine(NewTarget(node2pos));
                    else if (transform.position.y >= node2pos.y && !startMoving && !waiting)
                        StartCoroutine(NewTarget(node1pos));
                }
                else
                {
                    if (transform.position.x > node1pos.x && transform.position.x < node2pos.x && startMoving)
                        startMoving = false;
                    if (transform.position.x <= node1pos.x && !startMoving && !waiting)
                        StartCoroutine(NewTarget(node2pos));
                    else if (transform.position.x >= node2pos.x && !startMoving && !waiting)
                        StartCoroutine(NewTarget(node1pos));
                }
                if (!waiting && rigid.velocity == Vector2.zero)
                    rigid.velocity = moveSpeed * target.normalized;
            }
        }
    }

    IEnumerator NewTarget(Vector2 targetPos)
    {
        waiting = true;
        rigid.velocity = Vector2.zero;
        target = targetPos - (Vector2)transform.position;
        yield return new WaitForSeconds(waitingTime);
        startMoving = true;
        waiting = false;
    }
}

