using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovableObject : MovableObject {

	[Header("Velocity")]
	public float maxXVelocity = 2.5f;
	public float jumpVelocity = 4.5f;

	// Movement controller
	[Header("Controller")]
	public MovementController controller = null;

	protected bool jumping = false;
	protected SpriteRenderer spriteRenderer;
	protected Animator animator;

	void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
	}

	// Movement
	protected override void ComputeVelocity() {
		if (controller == null) return;
		controller.UpdateInputs();

		if (controller.leftDown && !controller.rightDown) {
			targetVelocity.x = -maxXVelocity;
			spriteRenderer.flipX = true;
		}
		else if (controller.rightDown && !controller.leftDown) {
			targetVelocity.x = maxXVelocity;
			spriteRenderer.flipX = false;
		}
		else {
			targetVelocity.x = 0.0f;
		}

		if (controller.jumpDown && isGrounded) {
			velocity.y = jumpVelocity;
			jumping = true;
		}

		//animator.SetBool("grounded", isGrounded);
		animator.SetFloat("velX", Mathf.Abs(velocity.x));
		animator.SetFloat("tvelX", Mathf.Abs(targetVelocity.x));
		animator.SetBool("skidding", (targetVelocity.x * velocity.x) < 0.0f);
		animator.SetBool("jump", jumping);
	}

	protected void OnLanded() {
		jumping = false;
	}
}
