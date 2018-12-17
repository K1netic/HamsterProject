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
	public float TimeHooked = 8f;
    public float lineWidth = 0.05f;
    public Sprite arrowSprite;
    public Sprite shieldSprite;

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
    public float playerMaxHP = 100;
    public float hookheadDamage = 5;
    public float recoveryTime = 1.5f;
    public float flashingRate = .1f;
    public float arrowDamage = 10;
    public float knockBackTime = 0.5f;
    public float knockBackPlayerHit = 5;
    public float knockBackShieldHit = 10;
    public float knockBackSpikes = 5;
    public float spikesDamage = 10;
    public float velocityArrowDamageRatio = 2;
}
