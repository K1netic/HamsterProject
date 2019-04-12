using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balancing : MonoBehaviour {

    /*[Header("ITEM")]
    public float timeCDItem = 1f;
    public float bonusCroquettes;
    public float timeBonusCroquettes;
    public float gravityWithParachute;
    public float timeWithParachute;*/

    [Header("HOOK")]
    public float speedHookhead;
    public float retractationStep;
    public float offsetHook;
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
    public float hookheadDamage = 5;
    public float recoveryTime = 1.5f;
    public float flashingRate = .1f;
    public float arrowDamage = 10;
    public float laserDamage = 10;
    public float criticalSpeed = 35;
    public float deathRadius = 2.5f;
    public float dashDamage = 50;
    public float lastAttackerDuration = 3f;

    [Header("Knockback")]
    public float knockBackTime = 0.5f;
    public float knockBackPlayerHit = 5;
    public float maxKnockBackPlayerHit = 40f;
    public float knockBackShieldHit = 10;
    public float knockBackLaser = 10;
    public float knockBackNuke = 15;

    [Header("ATTACK")]
    public float attackTime;
    public float dashTime = 0.05f;
    public float dashForce;
    public float attackCDTime;
    public float dragEndOfDash = 20;
    public bool attackRecoveryWithHook;
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
