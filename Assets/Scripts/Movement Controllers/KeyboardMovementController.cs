using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardMovementController : MovementController {

	private const float horizontalThreshold = 0.01f;
    private const float verticalThreshold = 0.01f;

	// Update is called once per frame
	public override void UpdateInputs() {
		leftIsDown = false;
		rightIsDown = false;
        downIsDown = false;
        upIsDown = false;

		float horizAxis = Input.GetAxis("Horizontal");
		if (horizAxis < -horizontalThreshold) {
			leftIsDown = true;
		}
		else if (horizAxis > horizontalThreshold) {
			rightIsDown = true;
		}

        float vertAxis = Input.GetAxis("Vertical");
        if (vertAxis < -verticalThreshold) {
            downIsDown = true;
        }
        else if (vertAxis > verticalThreshold) {
            upIsDown = true;
        }

        jumpIsDown = Input.GetButton("Jump");
        jumpFirstDown = Input.GetButtonDown("Jump");
	}
}
