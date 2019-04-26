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
			newColor = new Color(.9215686f, 0.7294118f, 0.345098f);
			break;
		// rose
		case "1":
			newColor = new Color(0.9960784f, 0.5686275f, 0.7568628f);
			break;
		// bleu
		case "2":
			newColor = new Color(0.2313726f, 0.572549f, 0.9882353f);
			break;
		// vert
		case "3":
			newColor = new Color(0.4627451f, 0.7372549f, 0.2862745f);
			break;
		// rouge
		case "4":
			newColor = new Color(0.9098039f, 0.1176471f, 0.3176471f);
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
