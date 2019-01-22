using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSize : MonoBehaviour {

    [SerializeField]
    GameObject start;
    Vector2 startPos;

    [SerializeField]
    LineRenderer center;
    [SerializeField]
    LineRenderer glow;
    [SerializeField]
    LineRenderer ring;
    [SerializeField]
    ParticleSystem sparks;

    RaycastHit2D raycast;

    private void Start()
    {
        startPos = start.transform.position;
    }


}
