using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCutter : MonoBehaviour {

	[HideInInspector]
	public Hook hook;
    [HideInInspector]
    public GameObject projectile;

    PlayerLifeManager lifeManager;

    public void CutRope(Vector3 cuttingPos, string cutter){
        if (hook.currentProjectile.GetComponent<Projectile>().hooked && hook.playerMovement.playerNumber != cutter)
        {
            print("zizi");
            projectile.GetComponent<Projectile>().cut = true;

            lifeManager = hook.player.GetComponent<PlayerLifeManager>();
            lifeManager.CancelCleanLastAttacker();
            lifeManager.lastAttacker = cutter;
            lifeManager.CleanLastAttacker();

            //Rope to projectile
            GameObject cutRope = new GameObject();
            cutRope.AddComponent<LineRenderer>();
            cutRope.AddComponent<RopeScript>();
            RopeScript script = cutRope.GetComponent<RopeScript>();
            script.color = hook.line.startColor;
            script.startPosition = projectile.GetComponent<Projectile>().pivot;
            script.endPosition = cuttingPos;
            script.firstConnectedBody = projectile;

            Instantiate(Resources.Load<ParticleSystem>("Prefabs/CutHook/Cut"), cuttingPos, transform.rotation);

            //Rope to player
            hook.ropeCut = true;
            GameObject cutRope2 = new GameObject();
            cutRope2.AddComponent<LineRenderer>();
            cutRope2.AddComponent<RopeScript>();
            RopeScript script2 = cutRope2.GetComponent<RopeScript>();
            script2.connectedToPlayer = true;
            script2.color = hook.line.startColor;
            script2.startPosition = hook.player.transform.position;
            script2.endPosition = cuttingPos;
            script2.firstConnectedBody = hook.player;
            script2.hookScript = hook;

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
