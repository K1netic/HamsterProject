﻿using System.Collections;
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

    [Header("PLAYER MOVEMENT")]
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
    public float spikesDamage = 10;
    public float velocityArrowDamageRatio = 2;
    public float laserDamage = 10;

    [Header("Knockback")]
    public float knockBackTime = 0.5f;
    public float knockBackPlayerHit = 5;
    public float knockBackShieldHit = 10;
    public float knockBackSpikes = 5;
    public float knockBackLaser = 10;

	[Header("VIBRATION & SCREENSHAKE")]
	public float lightScreeShakeMagnitude = 4f;
	public float mediumScreenShakeMagnitude = 8f;
	public float heavyScreenShakeMagnitude = 12f;
	public float screenShakeRoughness = 4f;
	public float screenShakeFadeIn = 0.1f;
	public float screenShakeFadeOut = 1f;
	public float lightRumble = 0.1f;
	public float mediumRumble = 0.5f;
	public float heavyRumble = 1f;
	public float lightVibration = 0.1f;
	public float mediumVibration = 0.5f;
	public float heavyVibration = 1.0f;

}
