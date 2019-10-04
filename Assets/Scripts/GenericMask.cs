using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Direction = GameManager.Direction;

public class GenericMask : MonoBehaviour {
	public SpriteMask maskHorizontal = null;
	public SpriteMask maskVertical = null;
	public SpriteMask maskLineOfSight = null;
	public SpriteMask maskTile = null;
	public bool corner = false;
	public Direction dir;

	public SpriteMask[] GetAllMasks {
		set { }
		get {
			if (corner) {
				return new SpriteMask[] {maskHorizontal, maskVertical, maskLineOfSight};
			} else {
				return new SpriteMask[] {maskTile};
			}
		}
	}

	public void setCornerFromDir(bool set) {
		if ((dir == Direction.North) || (dir == Direction.North)) {
			if (maskVertical != null) maskVertical.gameObject.SetActive(!set);
		} else {
			if (maskHorizontal != null) maskHorizontal.gameObject.SetActive(!set);
		}
		if (((maskVertical == null) || maskVertical.gameObject.activeInHierarchy) && ((maskHorizontal == null) || maskHorizontal.gameObject.activeInHierarchy)) {
			if (maskLineOfSight != null) maskLineOfSight.gameObject.SetActive(false);
		} else {
			if (maskLineOfSight != null) maskLineOfSight.gameObject.SetActive(true);
		}
		
	}
}
