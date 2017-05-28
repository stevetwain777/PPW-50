using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementController : MonoBehaviour {

    [HideInInspector]
    public bool rightIsDown = false;
    [HideInInspector]
    public bool leftIsDown = false;
    [HideInInspector]
    public bool jumpIsDown = false;
    [HideInInspector]
    public bool jumpFirstDown = false;
    [HideInInspector]
    public bool downIsDown = false;
    [HideInInspector]
    public bool upIsDown = false;

	public abstract void UpdateInputs();

}
