using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftMovingObject : MovableObject {

	public bool go = true;
	public bool right = false;

	protected override void ComputeVelocity() { 
		if (go) {
			targetVelocity.x = right ? 3.0f : -3.0f;
		} else {
			targetVelocity.x = 0.0f;
		}
	}
	
}
