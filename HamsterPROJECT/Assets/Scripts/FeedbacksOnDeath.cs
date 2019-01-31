using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class FeedbacksOnDeath : MonoBehaviour {

	// Color Blink
	GameObject overlay;
	SpriteRenderer overlaySprite;
	Color newColor;
	Animator colorBlinkAnimator;

	void Start()
	{
		overlay = GameObject.Find("BlinkOverlay");
		overlaySprite = overlay.GetComponent<SpriteRenderer> ();
		colorBlinkAnimator = overlay.GetComponent<Animator> ();
	}

	public void SendFeedbacks(InputDevice device, string characterID)
	{
		switch (characterID)
		{
		// orange
		case "0":
			newColor = new Color (1f, 0.5f, 0f);
			break;
		// violet
		case "1":
			newColor = new Color (0.9f, 0f, 1f);
			break;
		// vert
		case "2":
			newColor = new Color (0f, 0.5f, 0f);
			break;
		// jaune
		case "3":
			newColor = new Color (1f, 0.85f, 0f);
			break;
		// bleu
		case "4":
			newColor = new Color (0f, 0.3f, 1f);
			break;
		}
		overlaySprite.color = newColor;

		StartCoroutine(CancelVibration (Vibrations.PlayVibration("Death", device), device));
		StartCoroutine (ColorBlink ());
	}

	public IEnumerator CancelVibration(float delay, InputDevice device)
	{
		yield return new WaitForSeconds (delay);
		device.StopVibration ();
	}

	IEnumerator ColorBlink()
	{
		colorBlinkAnimator.SetBool ("colorblink", true);
		yield return new WaitForSeconds (0.1f);
		colorBlinkAnimator.SetBool ("colorblink", false);
	}
}
