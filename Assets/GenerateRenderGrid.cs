using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateRenderGrid : LevelGenerator {

	public RenderingHandler handler;
	public int dim;
	public GameObject tilePrefab;

	public override Map GenerateLevel() {
		handler.renderMap = new RenderingHandler.RenderMap(dim * dim);
		handler.renderMap.dim = dim;

		for (int x = 0; x < dim; x++)
		{
			for (int y = 0; y < dim; y++) {
				handler.renderMap[x,y] = GetPrefab(x, y).GetComponent<RenderTile>();
			}
		}

		return null;
	}


	public GameObject GetPrefab(int x, int y) {
		GameObject newObj = Instantiate(tilePrefab);
		newObj.transform.position = new Vector3((-(dim)/2.0f)+0.5f, (-(dim)/2.0f) + 0.5f,0.0f) + new Vector3(x,y,0.0f);
		newObj.name = "RenderTile("+x+","+y+")";
		newObj.transform.parent = this.transform;
		return newObj;
	}
}
