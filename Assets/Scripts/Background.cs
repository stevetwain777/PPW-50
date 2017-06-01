using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {

	private PixelPerfectCamera sceneCamera;
	private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
		sceneCamera = FindObjectOfType<PixelPerfectCamera>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		ResizeToCamera();

		transform.position = new Vector3(sceneCamera.transform.position.x,
										 sceneCamera.transform.position.y,
										 transform.position.z);
	}

	private void ResizeToCamera() {
		float ppu = sceneCamera.currentPixelsPerUnit;
		Sprite sprite = spriteRenderer.sprite;
		Vector2 size = sprite.bounds.size * ppu;

		float xScale = Mathf.Max(Screen.width / size.x, 1.0f);
		float yScale = Mathf.Max(Screen.height / size.y, 1.0f);

		float scale = Mathf.Max(xScale, yScale);
		transform.localScale = new Vector3(scale, scale, 1.0f);
	}
}
