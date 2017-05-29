using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovableObject : MovableObject {

    private const float kFallThroughOffset = 0.01f;
    private const float kJumpGravityModifierInterval = 0.25f;
    private const float kJumpGravityModifier = 0.4f;

	[Header("Velocity")]
	public float maxXVelocity = 2.5f;
	public float jumpVelocity = 4.5f;

	// Movement controller
	[Header("Controller")]
	public MovementController controller = null;

    [HideInInspector]
    public float jumpGravityModifierTimeLeft;
    protected bool isJumping = false;

	protected SpriteRenderer spriteRenderer;
	protected Animator animator;
    protected PlayerAudioSet audioSet;

	void Start() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
        audioSet = GetComponent<PlayerAudioSet>();
	}

	// Movement
	protected override void ComputeVelocity() {
		if (controller == null) return;
		controller.UpdateInputs();

		if (controller.leftIsDown && !controller.rightIsDown) {
			targetVelocity.x = -maxXVelocity;
			spriteRenderer.flipX = true;
		}
		else if (controller.rightIsDown && !controller.leftIsDown) {
			targetVelocity.x = maxXVelocity;
			spriteRenderer.flipX = false;
		}
		else {
			targetVelocity.x = 0.0f;
		}

        if (controller.jumpFirstDown && IsGrounded && !isJumping) {
            // If we're on a jumpthrough platform and down is pressed, fall through.
            if (IsOnJumpThroughTile && controller.downIsDown) {
                rb2d.position += Vector2.down * kFallThroughOffset;
            }
            else {
                Jump();
            }
            isJumping = true;
		}

        // Handle jump impulse.
        if (isJumping && jumpGravityModifierTimeLeft > Mathf.Epsilon) {
            //velocity.y = jumpVelocity;

            jumpGravityModifierTimeLeft -= Time.deltaTime;
            if (!controller.jumpIsDown) {
                jumpGravityModifierTimeLeft = 0.0f;
            }

            if (jumpGravityModifierTimeLeft <= Mathf.Epsilon) {
                jumpGravityModifier = 1.0f;
            }
        }

		animator.SetFloat("velX", Mathf.Abs(velocity.x));
		animator.SetFloat("tvelX", Mathf.Abs(targetVelocity.x));
		animator.SetBool("skidding", (targetVelocity.x * velocity.x) < 0.0f);
		animator.SetBool("jump", isJumping);
	}

	protected void OnLanded() {
		isJumping = false;
	}

    protected void Jump() {
        velocity.y = jumpVelocity;
        jumpGravityModifier = kJumpGravityModifier;
        jumpGravityModifierTimeLeft = kJumpGravityModifierInterval;

        AudioManager.Shared.PlaySingle(audioSet.jumpAudioClip);
    }
}
