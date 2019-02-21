using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingLaser : MonoBehaviour {

    [SerializeField]
    GameObject objectPosition;

    private void Start()
    {
        objectPosition.transform.rotation = transform.rotation;
    }

    void Update () {
        transform.position = objectPosition.transform.position;
        transform.rotation = objectPosition.transform.rotation;

    }
}
