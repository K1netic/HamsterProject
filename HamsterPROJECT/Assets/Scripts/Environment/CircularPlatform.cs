﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CircularPlatform : MonoBehaviour {

	[SerializeField]
	float moveSpeed;
    [SerializeField]
    bool clockWise = true;
    [SerializeField]
    bool randomDirection;

    Rigidbody2D rigid;
    Vector3 pivotPos;
    Vector3 vectorToTarget;
    [SerializeField]
    float startPositionInDegree; //à choisir entre 0° et 360° 
    float radius;
    float angle;
    float t;

    [SerializeField] bool peanut;

    void Awake () {
        t = (12.565f * startPositionInDegree) / 180; //(NB pour Ben : 25.13 pour un tour complet. 0 en haut, 12.565 en bas, 6.2825 à droite et 18.8475 à gauche)
        pivotPos = transform.parent.transform.position;
        rigid = GetComponent<Rigidbody2D>();
        radius = Vector2.Distance(pivotPos, transform.position);

        if (!clockWise)
            moveSpeed = -moveSpeed;

        if (randomDirection)
        {
            if (Random.Range(0, 2) == 1)
                moveSpeed = -moveSpeed;
        }

        transform.position = pivotPos + new Vector3(Mathf.Sin(t * moveSpeed) * radius, Mathf.Cos(t * moveSpeed) * radius, 0);
        vectorToTarget = transform.position - pivotPos;
        angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        if (peanut) angle -= 90f;
        rigid.MoveRotation(angle);
    }

    void FixedUpdate () {
        if (MatchStart.gameHasStarted)
        {
            t += Time.deltaTime;
            rigid.MovePosition(pivotPos + new Vector3(Mathf.Sin(t * moveSpeed) * radius, Mathf.Cos(t * moveSpeed) * radius, 0));
            vectorToTarget = transform.position - pivotPos;
            angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            if (peanut) angle -= 90;
            rigid.MoveRotation(angle);
        }
    }
}


