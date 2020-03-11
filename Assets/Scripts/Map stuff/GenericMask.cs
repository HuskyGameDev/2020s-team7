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

	public SpriteMask GetTileMask { set { } get { return maskTile; }}
	public SpriteMask GetAltTileMask { set { } get { return altMaskTile; } }

	/// <summary>
	/// Set the visiblity of this corner mask. True = visible, false = not visible.
	/// For corner masks on the diagonal, has two parts of the mask, one for each half 
	/// of the mask that might be needed if the tiles shown on each side do not have 
	/// the same lack/presence of a corner.
	/// </summary>
	/// <param name="set"></param>
	/// <param name="dir"></param>
	public void setCornerFromDir(bool set, Direction dir) {
		if ((dir == Direction.North) || (dir == Direction.South)) {	// if accessed from vertical direction, set vertical part of mask
			foreach (SpriteMask mask in maskVertical) {
				if (mask != null) mask.gameObject.SetActive(!set);
			}
			vertActive = !set;
		} else {    // if accessed from horizontal direction, set horizontal part of mask
			foreach (SpriteMask mask in maskHorizontal) {
				if (mask != null) mask.gameObject.SetActive(!set);
			}
			horiActive = !set;
		}
		// The "line of sight" mask needs to be set visible if either the verical/horizontal corner is set visible, or visa-versa
		// If the sets of masks for horizontal/vertical was deleted because it was not needed, it does not need to be considered.
		if (((maskVertical.Length == 0) || vertActive) && ((maskHorizontal.Length == 0) || horiActive)) {
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
