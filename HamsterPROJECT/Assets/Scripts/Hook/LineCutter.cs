using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCutter : MonoBehaviour {

	[HideInInspector]
	public Hook line;

	public void CutRope(){
		line.DisableRope();
	}
}
