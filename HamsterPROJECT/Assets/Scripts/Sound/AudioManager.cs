using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	[SerializeField] float pitchMin;
	[SerializeField] float pitchMax;

    [Header("In game sounds")]
    #region IG
    [SerializeField] AudioClip criticalDamage;
	[Range(0f,1f)] [SerializeField] float criticalDamageVolume;
	[SerializeField] AudioClip damage;
	[Range(0f,1f)] [SerializeField] float damageVolume;
	[SerializeField] AudioClip dash;
	[Range(0f,1f)] [SerializeField] float dashVolume;
	[SerializeField] AudioClip destructionHook;
	[Range(0f,1f)] [SerializeField] float destructionHookVolume;
	// [SerializeField] AudioClip doubleHookContact;
	// [Range(0f,1f)] [SerializeField] float doubleHookContactVolume;
	// [SerializeField] AudioClip hookContactScraps;
	// [Range(0f,1f)] [SerializeField] float hookContactScrapsVolume;
	[SerializeField] AudioClip playerHitLaser;
	[Range(0f,1f)] [SerializeField] float playerHitLaserVolume;
	[SerializeField] AudioClip playerHitPlayer;
	[Range(0f,1f)] [SerializeField] float playerHitPlayerVolume;
	// [SerializeField] AudioClip throwHook;
	// [Range(0f,1f)] [SerializeField] float throwHookVolume;
	// [SerializeField] AudioClip arrowHitShield;
	// [Range(0f,1f)] [SerializeField] float arrowHitShieldVolume;
	[SerializeField] AudioClip arrowHitArrow;
	[Range(0f,1f)] [SerializeField] float arrowHitArrowVolume;
	// [SerializeField] AudioClip switchWeapon;
	// [Range(0f,1f)] [SerializeField] float switchWeaponVolume;
	// [SerializeField] AudioClip towing;
	// [Range(0f,1f)] [SerializeField] float towingVolume;
	// [SerializeField] AudioClip untowing;
	// [Range(0f,1f)] [SerializeField] float untowingVolume;
    [SerializeField] AudioClip death;
    [Range(0f, 1f)] [SerializeField] float deathVolume;
    [SerializeField] AudioClip dashRecovery;
    [Range(0f, 1f)] [SerializeField] float dashRecoveryVolume;
    // [SerializeField] AudioClip doubleShieldContact;
    // [Range(0f, 1f)] [SerializeField] float doubleShieldContactVolume;
	[SerializeField] AudioClip destructingPlatform;
	[Range(0f, 1f)] [SerializeField] float destructingPlatformVolume;
	[SerializeField] AudioClip ropeCut;
	[Range(0f, 1f)] [SerializeField] float ropeCutVolume;
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
            default:
                break;
        }
			
        switch(audioName)
		{
        // case "doubleShieldContact":
        //     source.pitch = .75f;
        //     source.volume = doubleShieldContactVolume;
        //         source.PlayOneShot(doubleShieldContact);
        //     break;
		// case "towing":
		// 	source.pitch = 1.0f;
		// 	source.volume = towingVolume;
		// 	source.PlayOneShot (towing);
		// 	break;
		// case "untowing":
		// 	source.pitch = 1.0f;
		// 	source.volume = untowingVolume;
		// 	source.PlayOneShot (untowing);
		// 	break;
		case "damage":
			source.pitch = Random.Range (pitchMin, pitchMax);
			source.volume = damageVolume;
			source.PlayOneShot (damage);
			break;
		case "criticalDamage":
			source.pitch = Random.Range (pitchMin, pitchMax);
			source.volume = criticalDamageVolume;
			source.PlayOneShot (criticalDamage);
			break;
		case "dash":
			source.pitch = Random.Range (pitchMin, pitchMax);
			source.volume = dashVolume;
			source.PlayOneShot (dash);
			break;
		// case "doubleHookContact":
		// 	source.pitch = Random.Range (pitchMin, pitchMax);
		// 	source.volume = doubleHookContactVolume;
		// 	source.PlayOneShot (doubleHookContact);
		// 	break;
		case "destructionHook":
			source.pitch = 1.0f;
			source.volume = destructionHookVolume;
			source.PlayOneShot (destructionHook);
			break;
		// case "hookContactScraps":
		// 	source.pitch = Random.Range (pitchMin, pitchMax);
		// 	source.volume = hookContactScrapsVolume;
		// 	source.PlayOneShot (hookContactScraps);
		// 	break;
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
		// case "throwHook":
		// 	source.pitch = Random.Range (pitchMin, pitchMax);
		// 	source.volume = throwHookVolume;
		// 	source.PlayOneShot (throwHook);
		// 	break;
		// case "arrowHitShield":
		// 	source.pitch = 0.5f;
		// 	source.volume = arrowHitShieldVolume;
		// 	source.PlayOneShot (arrowHitShield);
		// 	break;
		case "arrowHitArrow":
			source.pitch = Random.Range (pitchMin, pitchMax);
			source.volume = arrowHitArrowVolume;
			source.PlayOneShot (arrowHitArrow);
			break;
		// case "switchWeapon":
		// 	source.pitch = Random.Range (pitchMin, pitchMax);
		// 	source.volume = switchWeaponVolume;
		// 	source.PlayOneShot (switchWeapon);
		// 	break;
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
		default:
			break;
		}
	}
}
