using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashText : MonoBehaviour {
    public int counter = 0;
    public int countLimit = 240;
	// Use this for initialization
	
	// Update is called once per frame
	void Update () {
        counter++;
        if (counter >= countLimit) this.gameObject.SetActive(false);
	}
}
