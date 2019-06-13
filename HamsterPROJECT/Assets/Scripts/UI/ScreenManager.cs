using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class ScreenManager : MonoBehaviour {

	//Screen to open automatically at the start of the Scene
	[HideInInspector] public Animator initiallyOpen;
	[SerializeField] Animator titleScreen;
	[SerializeField] Animator resultsScreen;
	[SerializeField] Animator characterSelectionScreen;
	[SerializeField] GameObject mapSelectionScreen;

	//string to determine whether or not the menu is opened after a series of match or when the game is launched
	public static string screenToInitiallyOpen;

	//Currently Open Screen
	private Animator m_Open;

	//Hash of the parameter used to control the transitions
	private int m_OpenParameterId;

	//GameObject Selected before the current Screen is opened
	//Used when closing a Screen, to go back on the button that opened it
	private GameObject m_PreviouslySelected;

	//Animator State and Transition names that needed to be checked against
	public string k_OpenTransitionName = "Open";
	const string k_ClosedStateName = "Closed";

	public bool closedStateReached = false;

	public void OnEnable()
	{
		//Cache the Hash to the "Open" Parameter to feed to Animator.SetBool
		m_OpenParameterId = Animator.StringToHash (k_OpenTransitionName);

		//Determine which screen should be initially opened when the menu is loaded
		switch (screenToInitiallyOpen)
		{
			case "TitlePanel":
			initiallyOpen = titleScreen;
			break;
			case "ResultsPanel":
			initiallyOpen = resultsScreen;
			break;
			case "CharacterSelectionPanel":
			initiallyOpen = characterSelectionScreen;
			break;
			case "MapSelectionPanel":
			mapSelectionScreen.SetActive(true);
			mapSelectionScreen.gameObject.GetComponent<ActivateInput>().inputOK = true;
			initiallyOpen = null;
			break;
			default:
			initiallyOpen = titleScreen;
			break;
		}

		//If set, opens the initial Screen
		if (initiallyOpen == null)
			return;
		OpenPanel (initiallyOpen);
	}

	//Closes the currently open panel and opens the provided one
	public void OpenPanel(Animator anim)
	{
		if (m_Open == anim)
			return;

		//Activates Screen hierarchy to animate it
		anim.gameObject.SetActive (true);
		//Move the Screen to front
		anim.transform.SetAsLastSibling();

		CloseCurrent ();

		//Sets the new Screen and opens one
		m_Open = anim;
		//Starts the open animation
		m_Open.SetBool(m_OpenParameterId, true);
	}

	//Closes the currently open Screen
	public void CloseCurrent()
	{
		if (m_Open == null)
			return;
		//Starts the close animation
		m_Open.SetBool(m_OpenParameterId, false);

		//Starts Coroutine to disable the hierarchy when closing animation finishes
		StartCoroutine(DisablePanelDelayed(m_Open));
		// No screen open
		m_Open = null;
	}

	//Coroutine that will detect when the Closing animation is finished and it will
	//deactivate the hierarchy
	IEnumerator DisablePanelDelayed(Animator anim)
	{
		closedStateReached = false;
		bool wantToClose = true;
		while( !closedStateReached && wantToClose)
		{
			if (!anim.IsInTransition (0))
			{
				closedStateReached = anim.GetCurrentAnimatorStateInfo (0).IsName (k_ClosedStateName);
			}

			wantToClose = !anim.GetBool (m_OpenParameterId);

			yield return new WaitForEndOfFrame ();
		}

		if (wantToClose)
		{
			anim.gameObject.SetActive (false);
		}
	}
}
