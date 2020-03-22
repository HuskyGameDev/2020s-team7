using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour {
	private float endTime;
	private float time;
	private bool stop;
		
	// Use this for initialization
	void Start () {
		//endTime = 70.0f;
		//time = 0.0f;
		SendMessage("Play");
	}
	
	// Update is called once per frame
	void Update () {
		//time += Time.deltaTime;
		
		//if (time > endTime) {
			//SendMessage("Stop");
			//Destroy(gameObject);
		//}
	}
}
