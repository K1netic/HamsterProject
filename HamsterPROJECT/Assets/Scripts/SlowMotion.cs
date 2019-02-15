using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotion : MonoBehaviour {

	[SerializeField]
    float slowdownFactor;
    [SerializeField]
    float slowdownLength;
	float startFixedDeltaTime;

	void Start ()
	{
        startFixedDeltaTime = Time.fixedDeltaTime;
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            DoSlowmotion();
        if(Time.timeScale != 1)
        {
            Time.timeScale += 1 / slowdownLength * Time.unscaledDeltaTime;
            Time.fixedDeltaTime = Mathf.Lerp(slowdownFactor * .02f, startFixedDeltaTime, Time.timeScale);
        }
        if(Time.timeScale > 1)
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = startFixedDeltaTime;
        }
    }

    public void DoSlowmotion()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;
		//StartCoroutine (UndoSlowmotion());
    }

	IEnumerator UndoSlowmotion()
	{
		yield return new WaitForSecondsRealtime (0.18f);
		Time.fixedDeltaTime = startFixedDeltaTime;
		Time.timeScale = 1;
	}

}
