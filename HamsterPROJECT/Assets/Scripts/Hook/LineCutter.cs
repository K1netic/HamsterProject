using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCutter : MonoBehaviour {

	[HideInInspector]
	public Hook hook;
    [HideInInspector]
    public GameObject projectile;

    public void CutRope(Vector3 cuttingPos){
        if (hook.currentProjectile.GetComponent<Projectile>().hooked)
        {

            GameObject cutRope = new GameObject();

            cutRope.AddComponent<LineRenderer>();
            LineRenderer cutLine = cutRope.GetComponent<LineRenderer>();

            cutRope.AddComponent<RopeScript>();
            RopeScript script = cutRope.GetComponent<RopeScript>();
            script.color = hook.line.startColor;
            script.startPosition = projectile.GetComponent<Projectile>().pivot;
            script.endPosition = cuttingPos;
            script.projectile = projectile;

            Instantiate(Resources.Load<ParticleSystem>("Prefabs/CutHook/Cut"), cuttingPos, transform.rotation);

            hook.DisableRope(true);
			// fais autrement
			//StartCoroutine (CancelVibration (Vibrations.PlayVibration("RopeCut", hook.playerMovement.playerInputDevice)));
        }

    }

	IEnumerator CancelVibration(float delay)
	{
		yield return new WaitForSeconds (delay);
		hook.GetComponent<PlayerMovement>().playerInputDevice.StopVibration ();
	}
}
