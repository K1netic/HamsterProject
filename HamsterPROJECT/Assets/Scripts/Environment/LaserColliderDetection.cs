using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserColliderDetection : MonoBehaviour {

    [SerializeField]
    public LaserSide side;
    [SerializeField]
    ParticleSystem particle;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Scraps")
        {
            Destroy(collision.gameObject);
            Instantiate(particle, collision.GetContact(0).point, particle.transform.rotation);
        }
    }

    public enum LaserSide
    {
        bot, top, right, left
    }
}
