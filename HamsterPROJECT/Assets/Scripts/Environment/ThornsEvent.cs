using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornsEvent : MonoBehaviour {

    [SerializeField]
    string thornsName = "ThornsSystem";

    AudioSource newSource;
    [SerializeField] AudioClip transformation;

	void TransitionIn()
    {
        GameObject.Find(thornsName).GetComponent<Thorns>().SwitchSpriteShape();
        if (newSource == null)
        {
            newSource = gameObject.AddComponent<AudioSource>();
        }
        newSource.pitch = Random.Range (0.8f, 1.0f);
        newSource.volume = 0.3f;
        newSource.PlayOneShot(transformation);
    }

    void TransitionOut()
    {
        GameObject.Find(thornsName).GetComponent<Thorns>().LaunchThornsBall();
    }
}
