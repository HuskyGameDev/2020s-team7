using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour {

	public float scalingFactor = 740.0f;
	public float min_H_W = 3.0f;

	Camera _camera;
	void Start() {
		_camera = GetComponent<Camera>();
	}

	// Update is called once per frame
	void Update () {
		_camera.orthographicSize = min_H_W * ((float)Screen.height / scalingFactor);
	}
}
