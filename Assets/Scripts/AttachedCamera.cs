using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachedCamera : MonoBehaviour {

	public GameObject attachedObject;

	private Camera targetCamera;

	// Use this for initialization
	void Start () {
		targetCamera = GetComponent<Camera>();
	}

	private float FloorValue(float value) {
		return Mathf.Floor(value * 32.0f) / 32.0f;
	}

	// Update is called once per frame
	void Update () {
		if (attachedObject == null) return;

		targetCamera.transform.position = new Vector3(attachedObject.transform.position.x, 
													  attachedObject.transform.position.y, 
													  targetCamera.transform.position.z);
	}
}
