using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornsBall : MonoBehaviour {

    public GameObject target;
    public float speed;
    public Thorns thornsSystem;

    Vector3 dir;

    void Update () {
        if (target)
        {
            if(dir == Vector3.zero)
                dir = (target.transform.position - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed);
            if (Vector3.Distance(target.transform.position, transform.position) < .1f)
            {
                thornsSystem.SwitchSpriteShape();
                Destroy(gameObject);
            }
        }
	}
}
