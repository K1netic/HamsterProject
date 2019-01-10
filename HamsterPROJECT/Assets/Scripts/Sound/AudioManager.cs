using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	[SerializeField] float pitchMin;
	[SerializeField] float pitchMax;

	#region IG
	[SerializeField] AudioClip towing;
	[Range(0f,1f)] [SerializeField] float towingVolume;
	[SerializeField] AudioClip untowing;
	[Range(0f,1f)] [SerializeField] float untowingVolume;
	[SerializeField] AudioClip hit;
	[Range(0f,1f)] [SerializeField] float hitVolume;
	[SerializeField] AudioClip speed;
	[Range(0f,1f)] [SerializeField] float speedVolume;
	[SerializeField] AudioClip cancel;
	[Range(0f,1f)] [SerializeField] float cancelVolume;
	[SerializeField] AudioClip grip;
	[Range(0f,1f)] [SerializeField] float gripVolume;
	[SerializeField] AudioClip gripCancel;
	[Range(0f,1f)] [SerializeField] float gripCancelVolume;
	[SerializeField] AudioClip gripLoss;
	[Range(0f,1f)] [SerializeField] float gripLossVolume;
	[SerializeField] AudioClip doubleGripCancel;
	[Range(0f,1f)] [SerializeField] float doubleGripCancelVolume;
	[SerializeField] AudioClip pick;
	[Range(0f,1f)] [SerializeField] float pickVolume;
	[SerializeField] AudioClip lightPick;
	[Range(0f,1f)] [SerializeField] float lightPickVolume;
	[SerializeField] AudioClip thuddyPick;
	[Range(0f,1f)] [SerializeField] float thuddyPickVolume;
	[SerializeField] AudioClip lightThud;
	[Range(0f,1f)] [SerializeField] float lightThudVolume;
	[SerializeField] AudioClip playerDeath;
	[Range(0f,1f)] [SerializeField] float playerDeathVolume;
	[SerializeField] AudioClip crack;
	[Range(0f,1f)] [SerializeField] float crackVolume;
	[SerializeField] AudioClip breakPlatform;
	[Range(0f,1f)] [SerializeField] float breakPlatformVolume;
	#endregion

	#region UI
	[SerializeField] AudioClip UI_highlight;
	[Range(0f,1f)] [SerializeField] float UI_highlightVolume;
	[SerializeField] AudioClip UI_validate;
	[Range(0f,1f)] [SerializeField] float UI_validateVolume;
	[SerializeField] AudioClip UI_cancel;
	[Range(0f,1f)] [SerializeField] float UI_cancelVolume;
	[SerializeField] AudioClip UI_pick;
	[Range(0f,1f)] [SerializeField] float UI_pickVolume;
	[SerializeField] AudioClip UI_highlightPlus;
	[Range(0f,1f)] [SerializeField] float UI_highlightPlusVolume;
	[SerializeField] AudioClip UI_validatePlus;
	[Range(0f,1f)] [SerializeField] float UI_validatePlusVolume;
	[SerializeField] AudioClip UI_matchEnd;
	[Range(0f,1f)] [SerializeField] float UI_matchEndVolume;
	[SerializeField] AudioClip UI_matchEndValidation;
	[Range(0f,1f)] [SerializeField] float UI_matchEndValidationVolume;
	[SerializeField] AudioClip UI_gameLaunch;
	[Range(0f,1f)] [SerializeField] float UI_gameLaunchVolume;
	[SerializeField] AudioClip UI_pauseMenuEnabled;
	[Range(0f,1f)] [SerializeField] float UI_pauseMenuEnabledVolume;
	[SerializeField] AudioClip UI_pauseMenuDisabled;
	[Range(0f,1f)] [SerializeField] float UI_pauseMenuDisabledVolume;
	[SerializeField] AudioClip UI_panelActivation;
	[Range(0f,1f)] [SerializeField] float UI_panelActivationVolume;
	[SerializeField] AudioClip UI_titleScreenValidation;
	[Range(0f,1f)] [SerializeField] float UI_titleScreenValidationVolume;
	#endregion

	public static AudioManager instance = null;

	public AudioSource P1source;
	public AudioSource P2source;
	public AudioSource P3source;
	public AudioSource P4source;
	public AudioSource UIsource;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
		else 
		{
			Destroy (gameObject);
		}
	}

	public void PlaySound(string audioName, AudioSource source)
	{
//		source.pitch = Random.Range (pitchMin, pitchMax);
		switch(audioName)
		{
		case "towing":
			source.volume = towingVolume;
			source.PlayOneShot (towing);
			break;
		case "untowing":
			source.volume = untowingVolume;
			source.PlayOneShot (untowing);
			break;
		case "hit":
			source.volume = hitVolume;
			source.PlayOneShot (hit);
			break;
		case "speed":
			source.volume = speedVolume;
			source.PlayOneShot (speed);
			break;
		case "cancel":
			source.pitch = 0.5f;
			source.volume = cancelVolume;
			source.PlayOneShot (cancel);
			break;
		case "grip":
			source.volume = gripVolume;
			source.PlayOneShot (grip);
			break;
		case "gripCancel":
			source.volume = gripCancelVolume;
			source.PlayOneShot (gripCancel);
			break;
		case "gripLoss":
			source.volume = gripLossVolume;
			source.PlayOneShot (gripLoss);
			break;
		case "doubleGripCancel":
			source.volume = doubleGripCancelVolume;
			source.PlayOneShot (doubleGripCancel);
			break;
		case "pick":
			source.volume = pickVolume;
			source.PlayOneShot (pick);
			break;
		case "lightPick":
			source.volume = lightPickVolume;
			source.PlayOneShot (lightPick);
			break;
		case "thuddyPick":
			source.volume = thuddyPickVolume;
			source.PlayOneShot (thuddyPick);
			break;
		case "playerDeath":
			source.volume = playerDeathVolume;
			source.PlayOneShot (playerDeath);
			break;
		case "crack":
			source.volume = crackVolume;
			source.PlayOneShot (crack);
			break;
		case "breakPlatform":
			source.volume = breakPlatformVolume;
			source.PlayOneShot (breakPlatform);
			break;
		case "UI_highlight":
			source.volume = UI_highlightVolume;
			source.PlayOneShot (UI_highlight);
			break;
		case "UI_validate":
			source.volume = UI_validateVolume;
			source.PlayOneShot (UI_validate);
			break;
		case "UI_cancel":
			source.volume = UI_cancelVolume;
			source.PlayOneShot (UI_cancel);
			break;
		case "UI_pick":
			source.volume = UI_pickVolume;
			source.PlayOneShot (UI_pick);
			break;
		case "UI_highlightPlus":
			source.volume = UI_highlightPlusVolume;
			source.PlayOneShot (UI_highlightPlus);
			break;
		case "UI_validatePlus":
			source.volume = UI_validatePlusVolume;
			source.PlayOneShot (UI_validatePlus);
			break;
		case "UI_matchEnd":
			source.volume = UI_matchEndVolume;
			source.PlayOneShot (UI_matchEnd);
			break;
		case "UI_matchEndValidation":
			source.volume = UI_matchEndValidationVolume;
			source.PlayOneShot (UI_matchEndValidation);
			break;
		case "UI_gameLaunch":
			source.volume = UI_gameLaunchVolume;
			source.PlayOneShot (UI_gameLaunch);
			break;
		case "UI_pauseMenuEnabled":
			source.volume = UI_pauseMenuEnabledVolume;
			source.PlayOneShot (UI_pauseMenuEnabled);
			break;
		case "UI_pauseMenuDisabled":
			source.volume = UI_pauseMenuDisabledVolume;
			source.PlayOneShot (UI_pauseMenuDisabled);
			break;
		case "UI_panelActivation":
			source.volume = UI_panelActivationVolume;
			source.PlayOneShot (UI_panelActivation);
			break;
		case "UI_titleScreenValidation":
			source.pitch = 1.0f;
			source.volume = UI_titleScreenValidationVolume;
			source.PlayOneShot (UI_titleScreenValidation);
			break;
		default:
			break;
		}
	}
}
