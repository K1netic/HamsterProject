using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balancing : MonoBehaviour {

    [Header("ITEM")]
    public float timeCDItem = 1f;
    public float bonusCroquettes;
    public float timeBonusCroquettes;
    public float gravityWithParachute;
    public float timeWithParachute;

    [Header("HOOK")]
    public float speedHookhead;
    public float distanceMaxHook = 15f;
    public float retractationStep;
    public float offsetHook;
    public float timeBtwShots;

    [Header("PLAYER MOVEMENT")]
    public float maxSpeedPlayer = 100;
    public float checkRadius = 1.0f;
    public LayerMask groundLayer;
    /*public float fastFallSpeed = 200;
    public float fastFallVerticalThreshold = -0.5f;
    public float fastFallHorizontalThreshold = 0.1f;*/
    public float airControlForce = 10;
    public float hookMovementForce = 25;

    [Header("DAMAGE & LIFE")]
    public int playerMaxHP = 5;
    public int hookheadDamage = 1;
    public float recoveryTime = .5f;
    public float flashingRate = .1f;
    public int arrowDamage = 1;
    public float knockBackTime = 0.25f;
    public float knockBackForceTwoArrows = 300;
    public float knockBackForceArrowPlayer = 300;
    public float knockBackForceHookheadPlayer = 400;
}
