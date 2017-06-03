using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedAnimationBlock : MonoBehaviour {

	public Vector2 animationIntervalRange = new Vector2(3.0f, 5.0f);

	private float timeUntilTrigger;
	private Animator animator;

	// Use this for initialization
	void Awake () {
		timeUntilTrigger = Random.Range(animationIntervalRange.x, animationIntervalRange.y);
	}

	void Start() {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		timeUntilTrigger -= Time.deltaTime;
		if (timeUntilTrigger <= 0.0f) {
			animator.SetTrigger("animate");
			timeUntilTrigger = Random.Range(animationIntervalRange.x, animationIntervalRange.y);
		}
	}
}
