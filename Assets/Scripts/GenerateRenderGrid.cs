using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateRenderGrid : LevelGenerator {

	
	public bool regenerateMasks = false;	// indicates wether the masks should be deeted and re-generated alongside the render-tiles
	public RenderingHandler handler;	// references to stuff used to generate the render grid and masks
	public int dim = 5;
	public GameObject tilePrefab;	// render tile prefab
	public GameObject cornerMaskPrefab;	// corner mask prefab
	public GameObject tileMaskPrefab;	// tile mask prefab
	public Universe universe;	// refernce to the GameObject that the render-tiles and masks are placed under

	public Sprite cornerHalfMask;	// masks that are needed besides the prefab defaults
	public Sprite tile18Mask;
	public Sprite tile72Mask;
	public Sprite lineMaskLow;

	/// <summary>
	/// Deletes the render map, and optionally the masks
	/// </summary>
	public override void DeleteRenderMap() {
		foreach (RenderTile tile in handler.renderMap) {
			if (tile != null) DestroyImmediate(tile.gameObject);
		}
		foreach (RenderTile tile in handler.altRenderMap) {
			if (tile != null) DestroyImmediate(tile.gameObject);
		}
		/*
		try {
			foreach (RenderTile tile in handler.renderMap) {
				DestroyImmediate(tile.gameObject);
			}
		} catch (System.NullReferenceException) { }
		*/
		if (regenerateMasks) {
			foreach (GenericMask mask in handler.cornerMaskMap) {
				if (mask != null) DestroyImmediate(mask.gameObject);
			}
			foreach (GenericMask mask in handler.tileMaskMap) {
				if (mask != null) DestroyImmediate(mask.gameObject);
			}
		}
	}

	/// <summary>
	/// Calls DeleteRenderMap(), then re-generates the render-tiles and masks, and applies settings to the tiles and masks based on position
	/// </summary>
	/// <returns></returns>
	public override Map GenerateLevel() {
		// only works correctly if these prefabs/references are set
		if ((tilePrefab == null) ||
			(cornerMaskPrefab == null) ||
			(tileMaskPrefab == null) ||
			(universe == null) ||
			(cornerHalfMask == null) ||
			(tile18Mask == null) ||
			(tile72Mask == null) ||
			(lineMaskLow == null)) {
			Debug.Log("Error: Prefabs, Universe, or mask Sprites not set");
			return new Map();
		}

		// get the layer ids, so don't have to call NameToID() numerous times
		int[] layerID = new int[4];
		layerID[0] = SortingLayer.NameToID("Layer_0");
		layerID[1] = SortingLayer.NameToID("Layer_1");
		layerID[2] = SortingLayer.NameToID("Layer_2");
		layerID[3] = SortingLayer.NameToID("Layer_3");

		//Delete all old tiles and masks
		DeleteRenderMap();

		// create render & altrender tile maps
		handler.renderMap = new RenderingHandler.RenderMap(dim);
		handler.altRenderMap = new RenderingHandler.RenderMap(dim);

		for (int x = 0; x < dim; x++){
			for (int y = 0; y < dim; y++) {
				handler.renderMap[x,y] = GetPrefab(x, y).GetComponent<RenderTile>();
				handler.altRenderMap[x,y] = GetPrefab(x, y, "alt").GetComponent<RenderTile>();

				handler.renderMap[x, y].gameObject.SetActive(true);
				handler.altRenderMap[x, y].gameObject.SetActive(true);

				RenderTile t = handler.renderMap[x, y];
				foreach (SpriteRenderer spr in t.GetAllSprites) {
					spr.sortingLayerID = layerID[(x % 2) + (2 * (y % 2))]; // set tile to be on layer Layer_0/1/2/3 based on (x,y), so that masks on neigboring tiles do not interfere
				}

				t = handler.altRenderMap[x, y];
				foreach (SpriteRenderer spr in t.GetAllSprites) {
					spr.sortingOrder += 20; // set altRenderMap to have higher height, so that masks do not interfere between render and altrender maps
					spr.sortingLayerID = layerID[(x % 2) + (2 * (y % 2))]; // set tile to be on layer Layer_0/1/2/3 based on (x,y), so that masks on neigboring tiles do not interfere
				}

			}
		}

		// only generate these if regenerateMasks is set
		if (this.regenerateMasks) {
			handler.cornerMaskMap = new RenderingHandler.MaskMap(dim + 1);

			// Generate corner masks
			#region Generate corner masks
			for (int x = 0; x <= dim; x++) {
				for (int y = 0; y <= dim; y++) {
					handler.cornerMaskMap[x, y] = GetPrefabCornerMask(x, y).GetComponent<GenericMask>();
					//handler.altCornerMaskMap[x, y] = GetPrefabCornerMask(x, y, "alt").GetComponent<GenericMask>();

					handler.cornerMaskMap[x, y].gameObject.SetActive(true);

					// certain corner masks are only neededd based on which direction a tile is visited/rendered from
					// only tiles on the diagonal can have corner masks be rendered from both directions
					if (((y < x) && ((y+x) > dim)) || ((y > x) && ((y + x) < dim))) {
						// only need horizontal masks
						foreach (SpriteMask mask in handler.cornerMaskMap[x, y].maskVertical) {
							if (mask != null) DestroyImmediate(mask.gameObject);
						}
						handler.cornerMaskMap[x, y].maskVertical = new SpriteMask[0];

						handler.cornerMaskMap[x, y].maskLineOfSight.sprite = lineMaskLow;
						handler.cornerMaskMap[x, y].maskLineOfSight.transform.localPosition = new Vector3(-0.5f, -0.5f, 0.0f);
						float xScale = handler.cornerMaskMap[x, y].maskLineOfSight.transform.localScale.x;
						float yScale = handler.cornerMaskMap[x, y].maskLineOfSight.transform.localScale.y;
						float zScale = handler.cornerMaskMap[x, y].maskLineOfSight.transform.localScale.z;
						handler.cornerMaskMap[x, y].maskLineOfSight.transform.localScale = new Vector3(xScale, yScale * -1, zScale);
						handler.cornerMaskMap[x, y].maskLineOfSight.transform.eulerAngles = new Vector3(0.0f, 0.0f, 90.0f);
					} else if (((y < x) && ((y + x) < dim)) || ((y > x) && ((y + x) > dim))) {
						// only need vertical masks
						foreach (SpriteMask mask in handler.cornerMaskMap[x, y].maskHorizontal) {
							if (mask != null) DestroyImmediate(mask.gameObject);
						}
						handler.cornerMaskMap[x, y].maskHorizontal = new SpriteMask[0];

						handler.cornerMaskMap[x, y].maskLineOfSight.sprite = lineMaskLow;
						handler.cornerMaskMap[x, y].maskLineOfSight.transform.localPosition = new Vector3(-0.5f, -0.5f, 0.0f);
					} else if (((x == (dim / 2)) || (x == (dim / 2) + 1)) && ((y == (dim / 2)) || (y == (dim / 2) + 1))) {
						foreach (SpriteMask mask in handler.cornerMaskMap[x, y].maskHorizontal) {
							if (mask != null) DestroyImmediate(mask.gameObject);
						}
						handler.cornerMaskMap[x, y].maskHorizontal = new SpriteMask[0];
					}

					//outer-edge corner masks do not need line mask
					if ((x == 0) || (y == 0) || (x == dim) || (y == dim)) {
						SpriteMask line = handler.cornerMaskMap[x, y].maskLineOfSight;
						if (line != null) DestroyImmediate(line.gameObject);
						line = null;
					}


					// set 45 angle corner SpriteMasks to use CornerMask_Half
					// do not do this for corners immediately around the center tile
					if (((x == y) || ((x + y) == dim)) && !((x == (dim/2)) || (x == (dim/2) + 1))) {
						foreach (SpriteMask mask in handler.cornerMaskMap[x, y].maskVertical) {
							mask.sprite = cornerHalfMask;
						}
						foreach (SpriteMask mask in handler.cornerMaskMap[x, y].maskHorizontal) {
							mask.sprite = cornerHalfMask;
						}
					}

					if ((x > (dim / 2)) && (y > (dim / 2))) {
						handler.cornerMaskMap[x, y].gameObject.transform.localScale = new Vector3(-1.0f, -1.0f, 1.0f);
					} else if (x > (dim / 2)) {
						handler.cornerMaskMap[x, y].gameObject.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
					} else if (y > (dim / 2)) {
						handler.cornerMaskMap[x, y].gameObject.transform.localScale = new Vector3(1.0f, -1.0f, 1.0f);
					}
				}
			}
			#endregion

			handler.tileMaskMap = new RenderingHandler.MaskMap(dim);
			//handler.altTileMaskMap = new RenderingHandler.MaskMap(dim);

			// Generate Tile masks
			#region Generate Tile masks
			for (int x = 0; x < dim; x++) {
				for (int y = 0; y < dim; y++) {
					if ((x != (dim / 2)) && (y != (dim / 2))) {
						handler.tileMaskMap[x, y] = GetPrefabTileMask(x, y).GetComponent<GenericMask>();
						//handler.altTileMaskMap[x, y] = GetPrefabTileMask(x, y, "alt").GetComponent<GenericMask>();

						handler.tileMaskMap[x, y].gameObject.SetActive(true);

						SpriteMask mask = handler.tileMaskMap[x, y].GetTileMask;
						SpriteMask maskAlt = handler.tileMaskMap[x, y].GetAltTileMask;
						mask.frontSortingLayerID = layerID[(x % 2) + (2 * (y % 2))]; // set mask to be on layer Layer_0/1/2/3 based on (x,y), so that does not interfere with neighboring tiles
						mask.backSortingLayerID = layerID[(x % 2) + (2 * (y % 2))];

						maskAlt.frontSortingLayerID = layerID[(x % 2) + (2 * (y % 2))]; // set mask to be on layer Layer_0/1/2/3 based on (x,y), so that does not interfere with neighboring tiles
						maskAlt.backSortingLayerID = layerID[(x % 2) + (2 * (y % 2))];

						// alt-tile-masks along the diagonals need to be rotated by 180 degrees, so only 1 sprite needs to be used
						if ((x == y) || (x + y == (dim - 1))) {
							maskAlt.transform.eulerAngles = new Vector3(0.0f, 0.0f, 180.0f);
						}

						//set tiles (0, 1), (0, 3), (4, 1), (4, 3) to be rotated & flipped, and to use different sprites
						if (((y < x) && ((y + x) > (dim -1))) || ((y > x) && ((y + x) < (dim - 1)))) {
							float xScale = mask.transform.localScale.x;
							float yScale = mask.transform.localScale.y;
							float zScale = mask.transform.localScale.z;
							mask.transform.localScale = new Vector3(xScale, yScale * -1, zScale);
							mask.transform.eulerAngles = new Vector3(0.0f, 0.0f, 90.0f);
							maskAlt.transform.localScale = new Vector3(xScale, yScale * -1, zScale);
							maskAlt.transform.eulerAngles = new Vector3(0.0f, 0.0f, 90.0f);

							mask.sprite = tile72Mask;
							maskAlt.sprite = tile18Mask;
						}
						if (((y < x) && ((y + x) < (dim - 1))) || ((y > x) && ((y + x) > (dim - 1)))) {
							mask.sprite = tile18Mask;
							maskAlt.sprite = tile72Mask;
						}

						if ((x > (dim / 2)) && (y > (dim / 2))) {
							handler.tileMaskMap[x, y].gameObject.transform.localScale = new Vector3(-1.0f, -1.0f, 1.0f);
						} else if (x > (dim / 2)) {
							handler.tileMaskMap[x, y].gameObject.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
						} else if (y > (dim / 2)) {
							handler.tileMaskMap[x, y].gameObject.transform.localScale = new Vector3(1.0f, -1.0f, 1.0f);
						}
					}
				}
			}
			#endregion
		}

		return null;
	}

	/// <summary>
	/// Get renderTile prefab
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="nameAppend"></param>
	/// <returns></returns>
	public GameObject GetPrefab(int x, int y, string nameAppend = "") {
		GameObject newObj = Instantiate(tilePrefab);
		newObj.name = "RenderTile("+x+","+y+")" +((nameAppend == "") ? "" : " " + nameAppend);
		//newObj.transform.parent = this.transform;
		newObj.transform.parent = universe.renderTilesObject.transform;
		newObj.transform.localPosition = new Vector3((-(dim) / 2.0f) + 0.5f, (-(dim) / 2.0f) + 0.5f, 0.0f) + new Vector3(x, y, 0.0f);
		return newObj;
	}

	/// <summary>
	/// get prefab of corner mask
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="nameAppend"></param>
	/// <returns></returns>
	public GameObject GetPrefabCornerMask(int x, int y, string nameAppend = "") {
		GameObject newObj = Instantiate(cornerMaskPrefab);
		newObj.name = "CornerMask(" + x + "," + y + ")" + ((nameAppend == "") ? "" : " " + nameAppend);
		//newObj.transform.parent = this.transform;
		newObj.transform.parent = universe.cornerMasksObject.transform;
		newObj.transform.localPosition = new Vector3((-(dim) / 2.0f), (-(dim) / 2.0f), 0.0f) + new Vector3(x, y, 0.0f);
		return newObj;
	}

	/// <summary>
	/// Get prefab of tile mask
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="nameAppend"></param>
	/// <returns></returns>
	public GameObject GetPrefabTileMask(int x, int y, string nameAppend = "") {
		GameObject newObj = Instantiate(tileMaskPrefab);
		newObj.name = "TileMask(" + x + "," + y + ")" + ((nameAppend == "") ? "" : " " + nameAppend);
		//newObj.transform.parent = this.transform;
		newObj.transform.parent = universe.tileMasksObject.transform;
		newObj.transform.localPosition = new Vector3((-(dim) / 2.0f) + 0.5f, (-(dim) / 2.0f) + 0.5f, 0.0f) + new Vector3(x, y, 0.0f);
		return newObj;
	}
}
