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
	void Update () {    // for some reason, it scales too much when Screen.height changes, this reverses it.
		_camera.orthographicSize = min_H_W * ((float)Screen.height / scalingFactor);
		// took me way to feaking long to figure out exactly what needed to be reveresed.
		// I suspect that there is a better way to do this, but I don't know what it is.
	}
}
