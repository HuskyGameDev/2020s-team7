using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {
	private float endTime;
	private float time;
	public Object BGM;

	// Use this for initialization
	void Start () {
		endTime = 65.0f;
		time = 0.0f;
		Instantiate(BGM);
	}
	
	// Update is called once per frame
	void Update () {
		//time += Time.deltaTime;
		
		//if (time > endTime) {
			//time -= endTime;
			//Instantiate(BGM);
		//}
	}
}
