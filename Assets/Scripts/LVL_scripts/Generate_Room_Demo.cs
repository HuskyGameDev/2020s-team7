using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Generate_Room_Demo {

	public static Map generateRoom() {
		Map demoMap = new Map();

		List<LevelEditor_2.TileCoord> emptyTiles1 = new List<LevelEditor_2.TileCoord>();
		emptyTiles1.Add(new LevelEditor_2.TileCoord(0, 0));
		emptyTiles1.Add(new LevelEditor_2.TileCoord(2, 0));
		emptyTiles1.Add(new LevelEditor_2.TileCoord(1, 2));

		Node[,] chunk1 = LevelEditor_2.createChunk(demoMap, new Color32(255,200,100,255), 3, 4, emptyTiles1);
		Node[,] chunk2 = LevelEditor_2.createChunk(demoMap, new Color32(100, 100, 255, 255), 3, 4, emptyTiles1);
		
		LevelEditor_2.createOneWayLink(
			chunk1,
			chunk2,
			new LevelEditor_2.TileCoord(0, 3, false, true, false, false),
			new LevelEditor_2.TileCoord(1, 3)
			);
		LevelEditor_2.createOneWayLink(
			chunk2,
			chunk1,
			new LevelEditor_2.TileCoord(2, 3, false, false, false, true),
			new LevelEditor_2.TileCoord(1, 3)
			);


		List<LevelEditor_2.TileCoord> emptyTiles2 = new List<LevelEditor_2.TileCoord>();
		emptyTiles2.Add(new LevelEditor_2.TileCoord(1, 1));

		Node[,] chunk3 = LevelEditor_2.createChunk(demoMap, new Color32(255, 0, 0, 255), 3, 3, emptyTiles2);
		Node[,] chunk4 = LevelEditor_2.createChunk(demoMap, new Color32(0, 255, 0, 255), 2, 1);
		Node[,] chunk5 = LevelEditor_2.createChunk(demoMap, new Color32(0, 0, 255, 255), 1, 2);

		LevelEditor_2.createTwoWayLink(
			chunk3,
			chunk4,
			new LevelEditor_2.TileCoord(0, 1, false, true, false, false),
			new LevelEditor_2.TileCoord(0, 0, false, false, false, true)
			);

		LevelEditor_2.createTwoWayLink(
			chunk3,
			chunk4,
			new LevelEditor_2.TileCoord(2, 1, false, false, false, true),
			new LevelEditor_2.TileCoord(1, 0, false, true, false, false)
			);

		LevelEditor_2.createTwoWayLink(
			chunk3,
			chunk5,
			new LevelEditor_2.TileCoord(1, 0, false, false, true, false),
			new LevelEditor_2.TileCoord(0, 0, true, false, false, false)
			);

		LevelEditor_2.createTwoWayLink(
			chunk3,
			chunk5,
			new LevelEditor_2.TileCoord(1, 2, true, false, false, false),
			new LevelEditor_2.TileCoord(0, 1, false, false, true, false)
			);

		LevelEditor_2.createTwoWayLink(
			chunk1,
			chunk3,
			new LevelEditor_2.TileCoord(1, 0, true, false, false, false),
			new LevelEditor_2.TileCoord(1, 2, false, false, true, false)
			);

		Node[,] chunk6 = LevelEditor_2.createChunk(demoMap, new Color32(50, 50, 50, 255), 1, 1);

		LevelEditor_2.createTwoWayLink(
			chunk3,
			chunk6,
			new LevelEditor_2.TileCoord(0, 0, false, false, false, true),
			new LevelEditor_2.TileCoord(0, 0, false, true, false, false)
			);

		LevelEditor_2.setSource(chunk2, new LevelEditor_2.TileCoord(1, 0));
		LevelEditor_2.setTarget(chunk6, new LevelEditor_2.TileCoord(0, 0));

		return demoMap;
	}
}
