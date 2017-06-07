using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CreativeSpore.SuperTilemapEditor;

public class MovableObject : MonoBehaviour {

	// Constant Properties
    protected const float kMinimumMoveDistance = 0.001f;
    protected const float kShellRadius = 0.01f;
    protected const int kMaxCollisions = 8;

	// Inspector Properties
	[Header("Movement")]
	public float baseAcceleration = 1.0f;
	public float baseFriction = 0.1f;
	public float gravityModifier = 1.0f;
    // Internal modifier used for player jumping.
    protected float jumpGravityModifier = 1.0f;
	public bool breaksBlocks = true;

	[Header("Air Movement")]
	public float airAccelerationModifier = 0.7f;
	public float airFrictionModifier = 0.25f;

	[Header("Ice")]
	public bool affectedByIce = true;
	public float iceAccelerationModifier = 0.35f;
	public float iceFrictionModifier = 0.07f;

	[Header("Audio Sets")]
	public TileAudioSet tileAudioSet;

	// Grounded Properties
	private bool _isGrounded;
	public bool IsGrounded {
		get { return _isGrounded; }
		private set {
			if (_isGrounded == value) return;

			_isGrounded = value;

			if (_isGrounded) {
				SendMessage("OnLanded");
			}
		}
	}

	[HideInInspector]
	public SceneTileType currentGroundType = SceneTileType.None;
	[HideInInspector]
	public SceneTileType lastGroundType = SceneTileType.None;

    public bool IsOnJumpThroughTile {
        get { return (currentGroundType & SceneTileType.JumpThrough) != 0; }
    }

	// Velocity Properties
	protected Vector2 targetVelocity;

	[HideInInspector]
	public Vector2 velocity;

	// Collision Detection
    protected Rigidbody2D rb2d;
    protected ContactFilter2D contactFilter;
	private RaycastHit2D[] hitBuffer = new RaycastHit2D[kMaxCollisions];
	private List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(kMaxCollisions);

	/// <summary>
	/// This function is called when the object becomes enabled and active.
	/// </summary>
	private void OnEnable() {
		rb2d = GetComponent<Rigidbody2D>();
	}

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start() {
		contactFilter.useTriggers = false;
		contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
		contactFilter.useLayerMask = true;
	}

	// Movement
		
	protected virtual void ComputeVelocity() { }

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update() {
		targetVelocity = Vector2.zero;
		ComputeVelocity();
	}

	/// <summary>
	/// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
	/// </summary>
	void FixedUpdate() {
		velocity = ComputeUpdatedVelocity(velocity);

		Vector2 deltaPosition = velocity * Time.deltaTime;

		// Move horizontally.
		MoveInDirection(Vector2.right * deltaPosition.x);
		// Move vertically, and update ground type.
		currentGroundType = MoveInDirection(Vector2.up * deltaPosition.y);
		IsGrounded = currentGroundType != SceneTileType.None;
		lastGroundType = IsGrounded ? currentGroundType : lastGroundType;
	}

	private SceneTileType MoveInDirection(Vector2 delta) {
		float distance = delta.magnitude;
		SceneTileType result = SceneTileType.None;

		if (distance > kMinimumMoveDistance) {
			int intersectionCount = rb2d.Cast(delta, contactFilter, hitBuffer, distance + kShellRadius);

			hitBufferList.Clear();
			for (int i = 0; i < intersectionCount; i++) {
                hitBufferList.Add(hitBuffer[i]);
            }

            foreach (RaycastHit2D hit in hitBufferList) {
				if (ShouldIgnoreCollision(hit)) continue;

				SceneTile hitTile = SceneTile.SceneTileFromRaycastHit(hit);

				Vector2 currentNormal = hit.normal;
                if (currentNormal.y > 0.0f) {
					result = hitTile.type;
                }
				else if (currentNormal.y < 0.0f &&
						 hit.transform.gameObject.layer == GameLayers.Breakable && 
						 breaksBlocks) {
					// If the tile is on a breakable layer, perform the tile break.
					BreakSceneTile(hitTile);
				}

                if (hit.transform.gameObject.layer == GameLayers.JumpThrough) {
                    result |= SceneTileType.JumpThrough;
                }

                float projection = Vector2.Dot(velocity, currentNormal);
                if (projection < 0) {
                    velocity = velocity - projection * currentNormal;
                }

                float modifiedDistance = hit.distance - kShellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }

			rb2d.position = rb2d.position + delta.normalized * distance;
		}

		return result;
	}

    private Vector2 ComputeUpdatedVelocity(Vector2 vel) {
		float accelerationFactor;
		float frictionFactor;
		AccelerationAndFrictionForGroundType(out accelerationFactor, out frictionFactor);
		
		// If the object doesn't want to be moving (i.e. friction is our acceleration).
		bool isFriction = Mathf.Abs(targetVelocity.x) < Mathf.Epsilon;

		float acceleration = isFriction ? frictionFactor : accelerationFactor;
		float accelerationDirection = Mathf.Sign(targetVelocity.x - vel.x);

		float deltaAbsVelocity = acceleration * Time.deltaTime;
		float targetVelocityDistance = Mathf.Abs(targetVelocity.x - vel.x);

		deltaAbsVelocity = deltaAbsVelocity > targetVelocityDistance ? targetVelocityDistance : deltaAbsVelocity;

		float previousXVelocity = vel.x;
		vel.x += deltaAbsVelocity * accelerationDirection;

		// If acceleration is friction, check if we've changed directions
		// (Dot product of x vector).
		if (isFriction && previousXVelocity * vel.x <= 0.0f) {
			vel.x = 0.0f;
		}

		// Adjust for gravity
        vel += Physics2D.gravity * gravityModifier * jumpGravityModifier * Time.deltaTime;

		return vel;
	}

	private void AccelerationAndFrictionForGroundType(out float adjustedAcceleration, out float adjustedFriction) {
		if (affectedByIce && (lastGroundType == SceneTileType.Ice || currentGroundType == SceneTileType.Ice)) {
			adjustedAcceleration = baseAcceleration * iceAccelerationModifier;
			adjustedFriction = baseFriction * iceFrictionModifier;
		}
		else if (currentGroundType == SceneTileType.None) {
			adjustedAcceleration = baseAcceleration * airAccelerationModifier;
			adjustedFriction = baseFriction * airFrictionModifier;
		}
		else {
			adjustedAcceleration = baseAcceleration;
			adjustedFriction = baseFriction;
		}
	}

	private bool ShouldIgnoreCollision(RaycastHit2D hit) {
		int layer = hit.transform.gameObject.layer;

        if (layer == GameLayers.Solid || layer == GameLayers.Breakable) {
			return false;
		}
		else if (layer == GameLayers.JumpThrough) {
			// Don't ignore if we've collided with the top and falling down.
			return !(hit.normal.y > 0.0f && velocity.y < 0.0f && hit.distance > 0.0f);
		}
		else if (layer == GameLayers.Player) {
			return false;
		}
		return true;
	}

	private void BreakSceneTile(SceneTile tile) {
		tile.RemoveTile();
		if (tileAudioSet) {
			AudioManager.Shared.PlaySingle(tileAudioSet.breakTileAudioClip);
		}
	}
}
