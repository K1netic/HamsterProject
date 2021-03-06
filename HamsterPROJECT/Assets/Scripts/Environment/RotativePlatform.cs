﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RotativePlatform : MonoBehaviour {

    [SerializeField]
    float speed = 20;
    [SerializeField]
    bool clockwise = true;
    [SerializeField]
    bool randomDirection;

    Rigidbody2D rigid;
    float t;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        if (rigid.bodyType != RigidbodyType2D.Kinematic)
            rigid.bodyType = RigidbodyType2D.Kinematic;
        if (clockwise)
            speed = -speed;
        if (randomDirection)
        {
            if (Random.Range(0, 2) == 1)
                speed = -speed;      
        }
    }

    private void Update()
    {
        if (MatchStart.gameHasStarted)
        {
            t += Time.deltaTime;
            rigid.MoveRotation(speed*t);
        }
    }
}
