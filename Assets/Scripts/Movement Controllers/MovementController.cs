using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementController : MonoBehaviour {

	[HideInInspector]
	public bool rightDown = false;
	[HideInInspector]
	public bool leftDown = false;
	[HideInInspector]
	public bool jumpDown = false;

	public abstract void UpdateInputs();

}
