using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class LayerManager : MonoBehaviour {

    private static LayerManager _shared = null;
    public static LayerManager shared {
        get {
            if (_shared == null) {
                _shared = FindObjectOfType<LayerManager>();
            }
            return _shared;
        }
    }

	public int playerLayer;
	public int solidLayer;
	public int jumpThroughLayer;

	// Use this for initialization
	void Awake () {
		if (_shared == null) {
			_shared = this;
			InitializeLevel();
		}
		else if (_shared != this) {
			Destroy(gameObject);
		}
	}
	
	void OnDestroy() {
		// For safety, check.
		if (_shared == this) {
			_shared = null;
		}
	}

	private void InitializeLevel() {
		playerLayer = LayerMask.NameToLayer("Player");
		solidLayer = LayerMask.NameToLayer("Solid");
		jumpThroughLayer = LayerMask.NameToLayer("JumpThrough");
	}
	
}
