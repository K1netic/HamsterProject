using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotativePlatform : MonoBehaviour {

    [SerializeField]
    float speed;
    [SerializeField]
    bool clockwise = true;

    private void Start()
    {
        if (clockwise)
            speed = -speed;
    }

    private void Update()
    {
        if (MatchStart.gameHasStarted)
        {
            transform.Rotate(new Vector3(0, 0, speed));
        }
    }
}
