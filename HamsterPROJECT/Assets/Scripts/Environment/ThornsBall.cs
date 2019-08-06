using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornsBall : MonoBehaviour {

    public GameObject target;
    public float speed;

    Vector3 dir;
    bool soundPlayed = false;

    AudioSource newSource;

    [SerializeField] AudioClip movingThornBall;

    void Update () {
        if(Time.timeScale != 0)
        { 
            if (target)
            {
                if (!soundPlayed)
                {
                    newSource = gameObject.AddComponent<AudioSource>();
                    newSource.pitch = Random.Range (0.8f, 1.0f);
			        newSource.volume = 0.15f;
                    newSource.PlayOneShot(movingThornBall);
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
        Destroy(gameObject);
    }
}
