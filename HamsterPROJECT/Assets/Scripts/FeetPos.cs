﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetPos : MonoBehaviour {

    [SerializeField]
    GameObject player;
	
	// Update is called once per frame
	void Update () {
        transform.position = player.transform.position - new Vector3(0,0.6f,0);
    }
}