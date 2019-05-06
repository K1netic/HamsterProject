using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balancing : MonoBehaviour {

    [Header("HOOK")]
    public float speedHookhead;
    public float retractationStep;
    public float timeBtwShots;
	public float TimeHooked = 8f;
    public float lineWidth = 0.05f;
    public float timeRopeCut = 1.5f;
    public float cutRopeSpeed = .25f;

    [Header("PLAYER MOVEMENT")]
    public float groundedControlForce = 15;
    public float airControlForce = 10;
    public float hookMovementForce = 25;
    public float speedThresholdDestruction = 20f;

    [Header("DAMAGE & LIFE")]
    public float playerMaxHP = 100;
    public float recoveryTime = 1.5f;
    public float flashingRate = .1f;
    public float blade1Damage = 20;
    public float blade2Damage = 35;
    public float blade3Damage = 50;
    public float laserDamage = 10;
    public float hookRecuperationSpeed = 20;
    public float criticalSpeed = 40;
    public float lastAttackerDuration = 3f;
    public float freezeFrameDuration = .1f;

    [Header("KNOCKBACK")]
    public float knockBackTime = 0.5f;
    public float knockBackBlade1 = 10;
    public float knockBackBlade2 = 15;
    public float knockBackBlade3 = 20;
    public float knockBackLaser = 20;
    public float knockBackMeteor = 20;

    [Header("ATTACK")]
    public float attackTime;
    public float dashTime = 0.05f;
    public float dashForce;
    public float attackCDTime;
    public float dragEndOfDash = 20;
    public float inDashStatusTime = .5f;

    [Header("VIBRATIONS")]
	public float lightRumble = 0.1f;
	public float mediumlightRumble = 0.25f;
	public float mediumRumble = 0.5f;
	public float heavyRumble = 0.9f;
	public float lightVibration = 0.1f;
	public float mediumLightVibration = 0.25f;
	public float mediumVibration = 0.5f;
	public float heavyVibration = 0.9f;
	public float damageToVibrationDivisor = 10.0f;
	public float shortVibrationDuration = 0.1f;
	public float mediumVibrationDuration = 0.2f;
	public float longVibrationDuration = 0.5f;

	[Header("SCREENSHAKE")]
	public float lightMagnitude = 1f;
	public float heavyMagnitude = 3f;
	public float roughness = 40f;
	public float fadeIn = 0.1f;
	public float fadeOut = 0.4f;

    [Header("SUDDEN DEATH")]
    public float suddenDeathSpeed = .02f;
    public float suddenDeathTime = 90;

}
