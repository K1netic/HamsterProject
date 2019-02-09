using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotion : MonoBehaviour {

	[SerializeField]
    float slowdownFactor = 0.5f;
	[SerializeField]
    float slowdownLength = 0.18f;
	float TimeEx;

	void Start ()
	{
		TimeEx = Time.fixedDeltaTime;
	}

    public void DoSlowmotion()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;
		StartCoroutine (UndoSlowmotion());
    }

	IEnumerator UndoSlowmotion()
	{
		yield return new WaitForSecondsRealtime (0.18f);
		Time.fixedDeltaTime = TimeEx;
		Time.timeScale = 1;
	}

}
