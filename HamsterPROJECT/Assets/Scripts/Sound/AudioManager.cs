using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	[SerializeField] float pitchMin;
	[SerializeField] float pitchMax;

    [Header("In game sounds")]
    #region IG
    [SerializeField] AudioClip damageLV3;
	[Range(0f,1f)] [SerializeField] float damageLV3Volume;
	[SerializeField] AudioClip damageLV2;
	[Range(0f,1f)] [SerializeField] float damageLV2Volume;
	[SerializeField] AudioClip damageLV1;
	[Range(0f,1f)] [SerializeField] float damageLV1Volume;
	[SerializeField] AudioClip dash;
	[Range(0f,1f)] [SerializeField] float dashVolume;
	[SerializeField] AudioClip destructionHook;
	[Range(0f,1f)] [SerializeField] float destructionHookVolume;
	[SerializeField] AudioClip playerHitLaser;
	[Range(0f,1f)] [SerializeField] float playerHitLaserVolume;
	[SerializeField] AudioClip playerHitPlayer;
	[Range(0f,1f)] [SerializeField] float playerHitPlayerVolume;
	[SerializeField] AudioClip playerHitThorns;
	[Range(0f,1f)] [SerializeField] float playerHitThornsVolume;
	[SerializeField] AudioClip arrowHitArrow;
	[Range(0f,1f)] [SerializeField] float arrowHitArrowVolume;
    [SerializeField] AudioClip death;
    [Range(0f, 1f)] [SerializeField] float deathVolume;
    [SerializeField] AudioClip dashRecovery;
    [Range(0f, 1f)] [SerializeField] float dashRecoveryVolume;
	[SerializeField] AudioClip destructingPlatform;
	[Range(0f, 1f)] [SerializeField] float destructingPlatformVolume;
	[SerializeField] AudioClip ropeCut;
	[Range(0f, 1f)] [SerializeField] float ropeCutVolume;
	[SerializeField] AudioClip movingItem;
	[Range(0f, 1f)] [SerializeField] float movingItemVolume;
	[SerializeField] AudioClip movingBomb;
	[Range(0f, 1f)] [SerializeField] float movingBombVolume;
	[SerializeField] AudioClip trampoline;
	[Range(0f, 1f)] [SerializeField] float trampolineVolume;
	[SerializeField] AudioClip transformation;
	[Range(0f, 1f)] [SerializeField] float transformationVolume;
    #endregion

    [Header("UI sounds")]
    #region UI
    [SerializeField] AudioClip UI_highlight;
	[Range(0f,1f)] [SerializeField] float UI_highlightVolume;
	[SerializeField] AudioClip UI_validate;
	[Range(0f,1f)] [SerializeField] float UI_validateVolume;
	[SerializeField] AudioClip UI_cancel;
	[Range(0f,1f)] [SerializeField] float UI_cancelVolume;
	[SerializeField] AudioClip UI_pick;
	[Range(0f,1f)] [SerializeField] float UI_pickVolume;
	[SerializeField] AudioClip UI_gameLaunch;
	[Range(0f,1f)] [SerializeField] float UI_gameLaunchVolume;
	[SerializeField] AudioClip UI_pauseMenuEnabled;
	[Range(0f,1f)] [SerializeField] float UI_pauseMenuEnabledVolume;
	[SerializeField] AudioClip UI_pauseMenuDisabled;
	[Range(0f,1f)] [SerializeField] float UI_pauseMenuDisabledVolume;
	[SerializeField] AudioClip UI_characterPanelActivation;
	[Range(0f,1f)] [SerializeField] float UI_characterPanelActivationVolume;
	[SerializeField] AudioClip UI_titleScreenValidation;
	[Range(0f,1f)] [SerializeField] float UI_titleScreenValidationVolume;
	[SerializeField] AudioClip UI_resultsScreen;
	[Range(0f,1f)] [SerializeField] float UI_resultsScreenVolume;
	[SerializeField] AudioClip UI_scoreDisplay;
	[Range(0f,1f)] [SerializeField] float UI_scoreDisplayVolume;
	[SerializeField] AudioClip UI_titleJingle;
	[Range(0f,1f)] [SerializeField] float UI_titleJingleVolume;
	[SerializeField] AudioClip UI_readyFight;
	[Range(0f,1f)] [SerializeField] float UI_readyFightVolume;
	[SerializeField] AudioClip UI_alarmSuddenDeath;
	[Range(0f,1f)] [SerializeField] float UI_alarmSuddenDeathVolume;
	#endregion

	public static AudioManager instance = null;

	public AudioSource _P1Source;
	public AudioSource _P2Source;
	public AudioSource _P3Source;
	public AudioSource _P4Source;
    public AudioSource _P1HookSource;
    public AudioSource _P2HookSource;
    public AudioSource _P3HookSource;
    public AudioSource _P4HookSource;
    public AudioSource _P1ArrowSource;
    public AudioSource _P2ArrowSource;
    public AudioSource _P3ArrowSource;
    public AudioSource _P4ArrowSource;
    public AudioSource UIsource;
	public AudioSource enviroSource;
	public AudioSource meteorSource;
	public AudioSource bombSource;
	//used for thorn movements
	public AudioSource thornsSource;
	//used for thorn transformations
	public AudioSource thornsSource2;
	public AudioSource trampolineSource;

    AudioSource source;

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

	public void PlaySound(string audioName, string sourceName)
	{
        switch (sourceName)
        {
            case "_P1":
                source = _P1Source;
                break;
            case "_P2":
                source = _P2Source;
                break;
            case "_P3":
                source = _P3Source;
                break;
            case "_P4":
                source = _P4Source;
                break;
            case "_P1Hook":
                source = _P1HookSource;
                break;
            case "_P2Hook":
                source = _P2HookSource;
                break;
            case "_P3Hook":
                source = _P3HookSource;
                break;
            case "_P4Hook":
                source = _P4HookSource;
                break;
            case "_P1Arrow":
                source = _P1ArrowSource;
                break;
            case "_P2Arrow":
                source = _P2ArrowSource;
                break;
            case "_P3Arrow":
                source = _P3ArrowSource;
                break;
            case "_P4Arrow":
                source = _P4ArrowSource;
                break;
			case "UI":
				source = UIsource;
				break;
			case "enviro":
				source = enviroSource;
				break;
			case "meteor":
				source = meteorSource;
				break;
			case "bomb":
				source = bombSource;
				break;
			case "thorns":
				source = thornsSource;
				break;
			case "thorns2":
				source = thornsSource;
				break;
			case "trampoline":
				source = trampolineSource;
				break;
            default:
                break;
        }
			
        switch(audioName)
		{
		case "damageLV1":
			source.pitch = Random.Range (pitchMin, pitchMax);
			source.volume = damageLV1Volume;
			source.PlayOneShot (damageLV1);
			break;
		case "damageLV2":
			source.pitch = Random.Range (pitchMin, pitchMax);
			source.volume = damageLV2Volume;
			source.PlayOneShot (damageLV2);
			break;
		case "damageLV3":
			source.pitch = Random.Range (pitchMin, pitchMax);
			source.volume = damageLV3Volume;
			source.PlayOneShot (damageLV3);
			break;
		case "dash":
			source.pitch = Random.Range (pitchMin, pitchMax);
			source.volume = dashVolume;
			source.PlayOneShot (dash);
			break;
		case "destructionHook":
			source.pitch = 1.0f;
			source.volume = destructionHookVolume;
			source.PlayOneShot (destructionHook);
			break;
		case "playerHitLaser":
			source.pitch = Random.Range (pitchMin, pitchMax);
			source.volume = playerHitLaserVolume;
			source.PlayOneShot (playerHitLaser);
			break;
		case "playerHitPlayer":
			source.pitch = Random.Range (pitchMin, pitchMax);
			source.volume = playerHitPlayerVolume;
			source.PlayOneShot (playerHitPlayer);
			break;
		case "playerHitThorns":
			source.pitch = Random.Range (pitchMin, pitchMax);
			source.volume = playerHitThornsVolume;
			source.PlayOneShot (playerHitThorns);
			break;
		case "arrowHitArrow":
			source.pitch = Random.Range (pitchMin, pitchMax);
			source.volume = arrowHitArrowVolume;
			source.PlayOneShot (arrowHitArrow);
			break;
		case "death":
			source.pitch = Random.Range (pitchMin, pitchMax);
			source.volume = deathVolume;
			source.PlayOneShot (death);
			break;
		case "dashRecovery":
			source.pitch = 1.0f;
            source.volume = dashRecoveryVolume;
            source.PlayOneShot(dashRecovery);
            break;
		case "destructingPlatform":
			source.pitch = Random.Range (pitchMin, pitchMax);
			source.volume = destructingPlatformVolume;
			source.PlayOneShot (destructingPlatform);
			break;
		case "ropeCut":
			source.pitch = Random.Range (pitchMin, pitchMax);
			source.volume = ropeCutVolume;
			source.PlayOneShot (ropeCut);
			break;
		case "movingBomb":
			source.pitch = Random.Range (pitchMin, pitchMax);
			source.volume = movingBombVolume;
			source.PlayOneShot (movingBomb);
			break;
		case "movingThornBall":
			source.pitch = Random.Range (pitchMin, pitchMax);
			source.volume = movingItemVolume / 4.0f;
			source.PlayOneShot (movingItem);
			break;
		case "trampoline":
			source.pitch = Random.Range (pitchMin-0.1f, pitchMax+0.1f);
			source.volume = trampolineVolume;
			source.PlayOneShot (trampoline);
			break;
		case "transformation":
			source.pitch = Random.Range (pitchMin, pitchMax);
			source.volume = transformationVolume;
			source.PlayOneShot (transformation);
			break;
		case "meteorExplosion":
			source.pitch = Random.Range (pitchMin -0.4f, pitchMax +0.4f);
			source.volume = 0.3f;
			source.PlayOneShot (death);
			break;
		// UI SOUNDS
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
			source.volume = UI_characterPanelActivationVolume;
			source.PlayOneShot (UI_characterPanelActivation);
			break;
		case "UI_titleScreenValidation":
			source.volume = UI_titleScreenValidationVolume;
			source.PlayOneShot (UI_titleScreenValidation);
			break;
		case "UI_resultsScreen":
			source.volume = UI_resultsScreenVolume;
			source.PlayOneShot (UI_resultsScreen);
			break;
		case "UI_scoreDisplay":
			source.volume = UI_scoreDisplayVolume;
			source.PlayOneShot (UI_scoreDisplay);
			break;
		case "UI_titleJingle":
			source.volume = UI_titleJingleVolume;
			source.PlayOneShot (UI_titleJingle);
			break;
		case "UI_readyFight":
			source.volume = UI_readyFightVolume;
			source.PlayOneShot (UI_readyFight);
			break;
		case "UI_alarmSuddenDeath":
			source.volume = UI_alarmSuddenDeathVolume;
			source.PlayOneShot(UI_alarmSuddenDeath);
			break;
		default:
			break;
		}
	}
}
