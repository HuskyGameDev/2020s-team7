using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateRenderGrid : LevelGenerator {

	public RenderingHandler handler;
	public int dim;
	public GameObject tilePrefab;

	public override Map GenerateLevel() {

		//Delete all old tiles
		GameObject[] oldRender = GameObject.FindGameObjectsWithTag("RenderTile");
		for (int i = oldRender.Length - 1; i >= 0 ; i--) {
			DestroyImmediate(oldRender[i].gameObject);
		}


		handler.renderMap = new RenderingHandler.RenderMap(dim * dim);
		handler.altRenderMap = new RenderingHandler.RenderMap(dim * dim);
		handler.renderMap.dim = dim;
		handler.altRenderMap.dim = dim;

		for (int x = 0; x < dim; x++)
		{
			for (int y = 0; y < dim; y++) {
				handler.renderMap[x,y] = GetPrefab(x, y).GetComponent<RenderTile>();
				handler.altRenderMap[x,y] = GetPrefab(x, y, "alt").GetComponent<RenderTile>();
				//Change the masking on the alt render map
				RenderTile t = handler.altRenderMap[x, y];
				foreach (SpriteRenderer spr in t.GetAllSprites) {
					spr.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
					spr.sortingOrder += 10;
				}
			}
		}

		return null;
	}


	public GameObject GetPrefab(int x, int y, string nameAppend = "") {
		GameObject newObj = Instantiate(tilePrefab);
		newObj.transform.position = new Vector3((-(dim)/2.0f)+0.5f, (-(dim)/2.0f) + 0.5f,0.0f) + new Vector3(x,y,0.0f);
		newObj.name = "RenderTile("+x+","+y+")" +((nameAppend == "") ? "" : " " + nameAppend);
		newObj.transform.parent = this.transform;
		return newObj;
	}
}
