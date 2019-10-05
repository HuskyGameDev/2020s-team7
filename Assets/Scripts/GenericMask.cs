using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Direction = GameManager.Direction;

public class GenericMask : MonoBehaviour {
	/*
	public SpriteMask maskHorizontal = null;
	public SpriteMask altMaskHorizontal = null;
	public SpriteMask maskVertical = null;
	public SpriteMask altMaskVertical = null;
	*/
	public SpriteMask[] maskHorizontal;
	public SpriteMask[] maskVertical;
	private bool horiActive = false;
	private bool vertActive = false;

	public SpriteMask maskLineOfSight = null;

	public SpriteMask maskTile = null;
	public SpriteMask altMaskTile = null;

	public bool corner = false;
	public Direction dir;

	/*
	public SpriteMask[] GetHorizontalMasks {
		set { }
		//get {return new SpriteMask[] { maskHorizontal, maskVertical, maskLineOfSight};}
		get {
			return maskHorizontal;*/
			/*
			SpriteMask[] maskList = new SpriteMask[maskHorizontal.Length + maskVertical.Length];
			int i = 0;
			foreach (SpriteMask mask in maskHorizontal) {
				maskList[i] = mask;
				i++;
			}
			foreach (SpriteMask mask in maskVertical) {
				maskList[i] = mask;
				i++;
			}
			return maskList;
			*/
			/*
		}
	}
	public SpriteMask[] GetVerticalMasks {
		set { }
		//get {return new SpriteMask[] { maskHorizontal, maskVertical, maskLineOfSight};}
		get { return maskVertical; }
	}
	*/

	public SpriteMask GetTileMask { set { } get { return maskTile; }}
	public SpriteMask GetAltTileMask { set { } get { return altMaskTile; } }

	public void setCornerFromDir(bool set) {
		if ((dir == Direction.North) || (dir == Direction.North)) {
			foreach (SpriteMask mask in maskVertical) {
				if (mask != null) mask.gameObject.SetActive(!set);
			}
			vertActive = !set;
		} else {
			foreach (SpriteMask mask in maskHorizontal) {
				if (mask != null) mask.gameObject.SetActive(!set);
			}
			horiActive = !set;
		}
		if (((maskVertical == null) || vertActive) && ((maskHorizontal == null) || horiActive)) {
			if (maskLineOfSight != null) {
				maskLineOfSight.gameObject.SetActive(false);
			}
		} else {
			if (maskLineOfSight != null) {
				maskLineOfSight.gameObject.SetActive(true);
			}
		}
		
	}
}
