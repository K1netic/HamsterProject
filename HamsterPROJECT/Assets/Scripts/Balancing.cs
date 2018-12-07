using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balancing : MonoBehaviour {

    //ITEM
    public float timeCDItem = 1f;
    public float bonusCroquettes;
    public float timeBonusCroquettes;
    public float gravityWithParachute;
    public float timeWithParachute;

    //HOOK
    public float speedHookhead;
    public float distanceMaxHook = 15f;
    public float retractationStep;
    public float offsetHook;
    public float timeBtwShots;

    //MOVEMENT
    public float maxSpeedPlayer = 100;
    public float checkRadius = 1.0f;
    public LayerMask groundLayer;
    public float fastFallSpeed = 200;
    public float fastFallVerticalThreshold = -0.5f;
    public float fastFallHorizontalThreshold = 0.1f;

    //PLAYER
    public int playerMaxHP = 5;
}
