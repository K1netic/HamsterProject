using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TractInversionGuid : MonoBehaviour {

	Image point;
	Text txt;
	PlayerSelectionPanel panel;

	// Use this for initialization
	void Start () {
		point = transform.GetChild(0).GetComponent<Image>();
		txt = transform.GetChild(1).GetComponent<Text>();
		panel = transform.parent.GetComponent<PlayerSelectionPanel>();
	}
	
	// Update is called once per frame
	void Update () {
		if (panel.tract)
		{
			txt.color = new Color (0, 0, 255);
			point.color = new Color (0, 0, 255);
		}
		else
		{
			txt.color = new Color (0,0,0);
			point.color = new Color(0,0,0);
		}
	}
}
