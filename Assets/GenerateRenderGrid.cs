using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateRenderGrid : LevelGenerator {

	public bool regenerateMasks = false;
	public RenderingHandler handler;
	public int dim;
	public GameObject tilePrefab;
	public GameObject cornerMaskPrefab;
	public GameObject tileMaskPrefab;
	public Universe universe;

	public override Map GenerateLevel() {

		//Delete all old tiles
		/*
		foreach (RenderTile tile in handler.renderMap) {
			DestroyImmediate(tile.gameObject);
		}
		foreach (RenderTile tile in handler.altRenderMap) {
			DestroyImmediate(tile.gameObject);
		}*/
		
		GameObject[] oldRender = GameObject.FindGameObjectsWithTag("RenderTile");
		foreach(GameObject rt in oldRender) {
			DestroyImmediate(rt.gameObject);
		}
		/*for (int i = oldRender.Length - 1; i >= 0 ; i--) {
			DestroyImmediate(oldRender[i].gameObject);
		}*/

		//5555
		handler.renderMap = new RenderingHandler.RenderMap(dim);
		handler.altRenderMap = new RenderingHandler.RenderMap(dim);
		/*
		handler.renderMap = new RenderingHandler.RenderMap(dim * dim);
		handler.altRenderMap = new RenderingHandler.RenderMap(dim * dim);
		handler.cornerMap = new RenderingHandler.CornerMap((dim + 1) * (dim + 1));
		handler.renderMap.dim = dim;
		handler.altRenderMap.dim = dim;
		handler.cornerMap.dim = dim + 1;
		*/

		for (int x = 0; x < dim; x++)
		{
			for (int y = 0; y < dim; y++) {
				handler.renderMap[x,y] = GetPrefab(x, y).GetComponent<RenderTile>();
				handler.altRenderMap[x,y] = GetPrefab(x, y, "alt").GetComponent<RenderTile>();
				//Change the masking on the alt render map
				RenderTile t = handler.altRenderMap[x, y];
				foreach (SpriteRenderer spr in t.GetAllSprites) {
					//spr.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
					spr.sortingOrder += 20;
				}
			}
		}

		if (this.regenerateMasks) {
			
			GameObject[] oldMask = GameObject.FindGameObjectsWithTag("MaskTile");
			foreach (GameObject mask in oldMask) {
				DestroyImmediate(mask.gameObject);
			}
			/*
			for (int i = oldMask.Length - 1; i >= 0; i--) {
				DestroyImmediate(oldRender[i].gameObject);
			}*/
			/*
			foreach (GenericMask mask in handler.cornerMaskMap) {
				DestroyImmediate(mask.gameObject);
			}
			foreach (GenericMask mask in handler.altCornerMaskMap) {
				DestroyImmediate(mask.gameObject);
			}*/

			handler.cornerMaskMap = new RenderingHandler.MaskMap(dim + 1);
			handler.altCornerMaskMap = new RenderingHandler.MaskMap(dim + 1);

			for (int x = 0; x <= dim; x++) {
				for (int y = 0; y <= dim; y++) {
					handler.cornerMaskMap[x, y] = GetPrefabCornerMask(x, y).GetComponent<GenericMask>();
					handler.altCornerMaskMap[x, y] = GetPrefabCornerMask(x, y, "alt").GetComponent<GenericMask>();
					GenericMask t = handler.altCornerMaskMap[x, y];
					foreach (SpriteMask mask in t.GetAllMasks) {
						//mask.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
						//mask.sortingOrder += 20;
						mask.backSortingOrder += 20;
						mask.frontSortingOrder += 20;
					}
				}
			}

			handler.tileMaskMap = new RenderingHandler.MaskMap(dim);
			handler.altTileMaskMap = new RenderingHandler.MaskMap(dim);

			for (int x = 0; x < dim; x++) {
				for (int y = 0; y < dim; y++) {
					handler.tileMaskMap[x, y] = GetPrefabTileMask(x, y).GetComponent<GenericMask>();
					handler.altTileMaskMap[x, y] = GetPrefabTileMask(x, y, "alt").GetComponent<GenericMask>();
					GenericMask t = handler.altTileMaskMap[x, y];
					foreach (SpriteMask mask in t.GetAllMasks) {
						//mask.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
						//mask.sortingOrder += 20;
						mask.backSortingOrder += 20;
						mask.frontSortingOrder += 20;
					}
				}
			}
		}

		return null;
	}


	public GameObject GetPrefab(int x, int y, string nameAppend = "") {
		GameObject newObj = Instantiate(tilePrefab);
		newObj.transform.position = new Vector3((-(dim)/2.0f)+0.5f, (-(dim)/2.0f) + 0.5f,0.0f) + new Vector3(x,y,0.0f);
		newObj.name = "RenderTile("+x+","+y+")" +((nameAppend == "") ? "" : " " + nameAppend);
		//newObj.transform.parent = this.transform;
		newObj.transform.parent = universe.renderTilesObject.transform;
		return newObj;
	}

	public GameObject GetPrefabCornerMask(int x, int y, string nameAppend = "") {
		GameObject newObj = Instantiate(cornerMaskPrefab);
		newObj.transform.position = new Vector3((-(dim) / 2.0f), (-(dim) / 2.0f), 0.0f) + new Vector3(x, y, 0.0f);
		newObj.name = "CornerMask(" + x + "," + y + ")" + ((nameAppend == "") ? "" : " " + nameAppend);
		//newObj.transform.parent = this.transform;
		newObj.transform.parent = universe.cornerMasksObject.transform;
		return newObj;
	}

	public GameObject GetPrefabTileMask(int x, int y, string nameAppend = "") {
		GameObject newObj = Instantiate(tileMaskPrefab);
		newObj.transform.position = new Vector3((-(dim) / 2.0f), (-(dim) / 2.0f), 0.0f) + new Vector3(x, y, 0.0f);
		newObj.name = "TileMask(" + x + "," + y + ")" + ((nameAppend == "") ? "" : " " + nameAppend);
		//newObj.transform.parent = this.transform;
		newObj.transform.parent = universe.tileMasksObject.transform;
		return newObj;
	}
}
