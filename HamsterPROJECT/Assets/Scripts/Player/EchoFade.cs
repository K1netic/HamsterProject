using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoFade : MonoBehaviour {

	SpriteRenderer Sprite;
    [SerializeField]
    Color ColorEnd;
	Color Color1;
    [SerializeField]
	float timeRemaining = 1;
	float t;
    public Sprite playerSprite;

	// Use this for initialization
	void Start () {
		Sprite = GetComponent<SpriteRenderer>();
        Sprite.sprite = playerSprite;
		Color1 = Sprite.color;
	}
	
	// Update is called once per frame
	void Update () {
		timeRemaining -= Time.deltaTime;
		t += (Time.deltaTime / timeRemaining);
		Color1 = Color.Lerp(Color1, ColorEnd,t);
		Sprite.color = Color1;
		if (t>= 1) {
			Destroy (gameObject);
		}
	}
		
}
