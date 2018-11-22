using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectionPanel : MonoBehaviour {

	public string playerSelectionPanelID;
	bool activated = false;
	bool validated = false;
	Image img;

	void Start()
	{
		img = this.GetComponent<Image> ();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Submit" + playerSelectionPanelID) && !activated)
		{
			activated = true;
			ActivatePanel ();
		}
			
		else if (Input.GetButtonDown ("Submit" + playerSelectionPanelID) && !validated)
		{
			validated = true;
			ValidatePanel ();
		}
			
	}

	void ActivatePanel()
	{
		switch (playerSelectionPanelID)
		{
		case "_P1":
			img.color = Color.blue;
			break;
		case "_P2":
			img.color = Color.red;
			break;
		case "_P3":
			img.color = Color.yellow;
			break;
		case "_P4":
			img.color = Color.green;
			break;
		}
	}

	void ValidatePanel()
	{
		this.transform.GetChild (0).gameObject.SetActive (true);
	}
}
