﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    [SerializeField]
    GameObject node1;
    [SerializeField]
    GameObject node2;
    [SerializeField]
    public float moveSpeed;

    public Vector2 target;
    public Vector2 node1pos;
    public Vector2 node2pos;

    public bool isDuplicated;

    private void Start()
    {
        if (!isDuplicated)
        {
            node1pos = (Vector2)node1.transform.position;
            node2pos = (Vector2)node2.transform.position;

            target = node1pos;
        }
    }

    private void FixedUpdate()
    {
        if ((Vector2)this.transform.position == node1pos)
            target = node2pos;
        else if ((Vector2)this.transform.position == node2pos)
            target = node1pos;
        this.transform.position = Vector2.MoveTowards(this.transform.position, target, moveSpeed * Time.fixedDeltaTime);
    }
}
