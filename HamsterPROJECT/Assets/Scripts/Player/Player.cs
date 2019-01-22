
using InControl;
using UnityEngine;


// This is just a simple "player" script that rotates and colors a cube
// based on input read from the device on its inputDevice field.
//
// See comments in PlayerManager.cs for more details.
//
public class Player : MonoBehaviour
{
	public InputDevice Device { get; set; }
}

