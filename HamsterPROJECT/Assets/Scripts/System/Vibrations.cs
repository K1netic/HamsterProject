using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using EZCameraShake;

public class Vibrations : MonoBehaviour {

	public static Balancing balanceData;

	// Use this for initialization
	void Start () {
		balanceData = GameObject.Find("Balancing").GetComponent<Balancing>();
	}

	public static float PlayVibration(string situationName, InputDevice device)
	{
		float duration = 0f;
		switch (situationName)
		{
		case "HookDestruction":
			device.Vibrate (balanceData.mediumRumble, 0f);
			duration = balanceData.mediumVibrationDuration;
			return duration;
		case "Dash":
			device.Vibrate (0f, balanceData.mediumVibration);
			duration = balanceData.shortVibrationDuration;
			return duration;
		// TO DEBUG
		case "Death":
			CameraShaker.Instance.ShakeOnce (balanceData.heavyMagnitude, balanceData.roughness, balanceData.fadeIn, balanceData.fadeOut * 2f);
			device.Vibrate (balanceData.heavyVibration);
			duration = balanceData.longVibrationDuration;
			return duration;
		// Works for both player to player collision and player to platform collision
		case "CollisionPlayerPlayer":
			device.Vibrate (0f, balanceData.lightVibration);
			duration = balanceData.shortVibrationDuration;
			return duration;
		case "CollisionArrowPlayer":
			CameraShaker.Instance.ShakeOnce (balanceData.lightMagnitude, balanceData.roughness, balanceData.fadeIn, balanceData.fadeOut);
			device.Vibrate (0f, balanceData.mediumLightVibration);
			duration = balanceData.mediumVibrationDuration;
			return duration;
		case "CollisionArrowShield":
			device.Vibrate (balanceData.mediumlightRumble, 0f);
			duration = balanceData.mediumVibrationDuration;
			return duration;
		case "CollisionArrowArrow":
			CameraShaker.Instance.ShakeOnce (balanceData.lightMagnitude, balanceData.roughness, balanceData.fadeIn, balanceData.fadeOut);
			device.Vibrate (balanceData.mediumRumble, 0f);
			duration = balanceData.mediumVibrationDuration;
			return duration;
		case "Laser":
			CameraShaker.Instance.ShakeOnce (balanceData.lightMagnitude, balanceData.roughness, balanceData.fadeIn, balanceData.fadeOut / 2f);
			device.Vibrate (balanceData.mediumRumble, balanceData.mediumVibration);
			duration = balanceData.mediumVibrationDuration;
			return duration;
		case "HookheadOnPlayer":
			device.Vibrate (0f, balanceData.lightVibration);
			duration = balanceData.mediumVibrationDuration;
			return duration;
		// TO TEST
		case "HookProjectileDestroyed":
			device.Vibrate (balanceData.lightRumble, 0f);
			duration = balanceData.mediumVibrationDuration;
			return duration;
		default:
			return 0f;
		}

		return 0f;

	}
}
