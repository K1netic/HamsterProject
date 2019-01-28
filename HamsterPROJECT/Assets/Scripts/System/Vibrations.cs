using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

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
			break;
		case "Dash":
			device.Vibrate (0f, balanceData.mediumVibration);
			duration = balanceData.shortVibrationDuration;
			return duration;
			break;
		// TO DEBUG
		case "Death":
			device.Vibrate (balanceData.heavyVibration);
			duration = 0.08f;
			return duration;
			break;
		// Works for both player to player collision and player to platform collision
		case "CollisionPlayerPlayer":
			device.Vibrate (0f, balanceData.lightVibration);
			duration = balanceData.shortVibrationDuration;
			return duration;
			break;
		case "CollisionArrowPlayer":
			device.Vibrate (0f, balanceData.mediumLightVibration);
			duration = balanceData.mediumVibrationDuration;
			return duration;
			break;
		case "CollisionArrowShield":
			device.Vibrate (balanceData.mediumlightRumble, 0f);
			duration = balanceData.mediumVibrationDuration;
			return duration;
			break;
		case "CollisionArrowArrow":
			device.Vibrate (balanceData.mediumRumble, 0f);
			duration = balanceData.mediumVibrationDuration;
			return duration;
			break;
		case "Laser":
			device.Vibrate (balanceData.mediumRumble, balanceData.mediumVibration);
			duration = balanceData.mediumVibrationDuration;
			return duration;
			break;
		case "HookheadOnPlayer":
			device.Vibrate (0f, balanceData.lightVibration);
			duration = balanceData.mediumVibrationDuration;
			return duration;
			break;
		// TO TEST
		case "HookProjectileDestroyed":
			device.Vibrate (balanceData.lightRumble, 0f);
			duration = balanceData.mediumVibrationDuration;
			return duration;
			break;
		default:
			return 0f;
			break;
		}

		return 0f;

	}
}
