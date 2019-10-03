using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Direction = GameManager.Direction;

public class CornerMask : MonoBehaviour {
	public SpriteMask mask_0;
	public SpriteMask mask_1;
	public SpriteMask mask_2;

	public SpriteMask[] GetAllMasks { set { } get { return new SpriteMask[] { mask_0, mask_1, mask_2 }; } }


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
