using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornsBall : MonoBehaviour {

    public GameObject target;
    public float speed;

    Vector3 dir;
    bool soundPlayed = false;

    void Update () {
        if(Time.timeScale != 0)
        { 
            if (target)
            {
                if (!soundPlayed)
                {
                    //TEST-SON
                    AudioManager.instance.PlaySound("movingThornBall", "thorns");
                    soundPlayed = true;
                }
                if(dir == Vector3.zero)
                    dir = (target.transform.position - transform.position).normalized;
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed);
                if (Vector3.Distance(target.transform.position, transform.position) < .1f)
                {
                    if (dir == Vector3.zero)
                        dir = (target.transform.position - transform.position).normalized;
                    transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed);
                    if (Vector3.Distance(target.transform.position, transform.position) < .1f)
                    {
                        target.GetComponent<Animator>().SetBool("Transition", true);
                        Invoke("Destruction", .1f);
                    }
                }
            }
        }
	}

    void Destruction()
    {
        target.GetComponent<Animator>().SetTrigger("TransitionIn");
        AudioManager.instance.PlaySound("transformation", "thorns2");
        Destroy(gameObject);
    }
}
