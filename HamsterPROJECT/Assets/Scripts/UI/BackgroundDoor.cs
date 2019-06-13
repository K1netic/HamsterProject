using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundDoor : MonoBehaviour {
	
	public bool closeOver = false;
	public bool openOver = false;
	// Use this for initialization
	public void CloseAnimationOver()
	{
		closeOver = true;
		openOver = false;
	}

	public void OpenAnimationOver()
	{
		closeOver = false;
		openOver = true;
	}
}
