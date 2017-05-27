using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LayerManager : MonoBehaviour {

	public static LayerManager shared = null;

	public int playerLayer;
	public int solidLayer;
	public int jumpThroughLayer;

	// Use this for initialization
	void Awake () {
		if (shared == null) {
			shared = this;
			InitializeLevel();
		}
		else if (shared != this) {
			Destroy(gameObject);
		}
	}
	
	void OnDestroy() {
		// For safety, check.
		if (shared == this) {
			shared = null;
		}
	}

	private void InitializeLevel() {
		playerLayer = LayerMask.NameToLayer("Player");
		solidLayer = LayerMask.NameToLayer("Solid");
		jumpThroughLayer = LayerMask.NameToLayer("JumpThrough");
	}
	
}
