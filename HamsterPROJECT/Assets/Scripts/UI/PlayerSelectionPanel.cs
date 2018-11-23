using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectionPanel : MonoBehaviour {

	public string playerSelectionPanelID;
	public enum SelectionPanelState {Deactivated, Activated, Validated};
	public SelectionPanelState state;
	[SerializeField] Color activationColor;

	Image img;

	void Start()
	{
		img = this.GetComponent<Image> ();
		state = SelectionPanelState.Deactivated;
	}

	// Update is called once per frame
	void Update () {
		#region StateManagement
		if (Input.GetButtonDown ("Submit" + playerSelectionPanelID) && state == SelectionPanelState.Deactivated )
		{
			state = SelectionPanelState.Activated;
		}
			
		else if (Input.GetButtonDown ("Submit" + playerSelectionPanelID) && state == SelectionPanelState.Activated)
		{
			state = SelectionPanelState.Validated;
		}

		if (Input.GetButtonDown("Cancel" + playerSelectionPanelID) && state == SelectionPanelState.Activated)
		{
			state = SelectionPanelState.Deactivated;
		}

		else if (Input.GetButtonDown("Cancel" + playerSelectionPanelID) && state == SelectionPanelState.Validated)
		{
			state = SelectionPanelState.Activated;
		}
		#endregion

		#region ActionsToDoForEachState
		switch (state)
		{
		case SelectionPanelState.Deactivated:
			img.color = Color.gray;
			break;
		case SelectionPanelState.Activated:
			img.color = activationColor;
			//Hiding "ready" text
			this.transform.GetChild (0).gameObject.SetActive (false);
			break;
		case SelectionPanelState.Validated:
			this.transform.GetChild (0).gameObject.SetActive (true);
			break;
		}
		#endregion
	}
}
