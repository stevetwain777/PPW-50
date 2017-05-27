using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardMovementController : MovementController {

	private const float horizontalThreshold = 0.01f;

	// Update is called once per frame
	public override void UpdateInputs() {
		leftDown = false;
		rightDown = false;

		float horizAxis = Input.GetAxis("Horizontal");
		if (horizAxis < -horizontalThreshold) {
			leftDown = true;
		}
		else if (horizAxis > horizontalThreshold) {
			rightDown = true;
		}

		jumpDown = Input.GetButtonDown("Jump");
	}
}
