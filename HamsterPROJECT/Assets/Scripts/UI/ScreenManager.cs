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
	[SerializeField] Animator mapSelectionScreen;

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
			initiallyOpen = mapSelectionScreen;
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
	//Sets the new Selected element
	public void OpenPanel(Animator anim)
	{
		if (m_Open == anim)
			return;

		//Activates Screen hierarchy to animate it
		anim.gameObject.SetActive (true);
		//Saves the currently Selected button that was used to open this Screen
		var newPreviouslySelected = EventSystem.current.currentSelectedGameObject;
		//Move the Screen to front
		anim.transform.SetAsLastSibling();

		CloseCurrent ();

		m_PreviouslySelected = newPreviouslySelected;

		//Sets the new Screen and opens one
		m_Open = anim;
		//Starts the open animation
		m_Open.SetBool(m_OpenParameterId, true);

		// //Sets an element in the new screen as the new Selected one
		// GameObject go = FindFirstEnabledSelectable(anim.gameObject);
		// SetSelected (go);
	}

	//Finds the first Selectable element in the provided hierarchy
	// static GameObject FindFirstEnabledSelectable(GameObject gameObject)
	// {
	// 	GameObject go = null;
	// 	var selectables = gameObject.GetComponentsInChildren<Selectable> (true);
	// 	foreach(var selectable in selectables)
	// 	{
	// 		if (selectable.IsActive() && selectable.IsInteractable())
	// 		{
	// 			go = selectable.gameObject;
	// 			break;
	// 		}
	// 	}
	// 	return go;
	// }

	//Closes the currently open Screen
	//Reverts selection to the Selectable used before opening the current screen
	public void CloseCurrent()
	{
		if (m_Open == null)
			return;
		//Starts the close animation
		m_Open.SetBool(m_OpenParameterId, false);

		//Reverts selection to the Selectable used before opening the current screen
		// SetSelected(m_PreviouslySelected);
		//Starts Coroutine to disable the hierarchy when closing animation finishes
		StartCoroutine(DisablePanelDelayed(m_Open));
		// No screen open
		m_Open = null;
	}

	//Coroutine that will detect when the Closing animation is finished and it will
	//deactivate the hierarchy
	IEnumerator DisablePanelDelayed(Animator anim)
	{
		bool closedStateReached = false;
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

	//Makes the provided GameObject Selected
	//When using the mouse/touch we actually want to set it as the previously selected and
	//set nothing as selected for now
	// private void SetSelected(GameObject go)
	// {
	// 	//Select the GameObject
	// 	EventSystem.current.SetSelectedGameObject(go);

	// 	var standaloneInputModule = EventSystem.current.currentInputModule as StandaloneInputModule;
	// 	if (standaloneInputModule != null)
	// 		return;

	// 	EventSystem.current.SetSelectedGameObject (null);
	// }
}
