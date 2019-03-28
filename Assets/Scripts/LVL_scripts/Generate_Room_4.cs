using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Generate_Room_4 {

	public static Map generateRoom() {
		Map demoMap = new Map();

		List<LevelEditor_2.TileCoord> emptyTiles = new List<LevelEditor_2.TileCoord>();
		emptyTiles.Add(new LevelEditor_2.TileCoord(1, 1));

		Node[,] chunk1 = LevelEditor_2.createChunk(demoMap, new Color32(255, 0, 0, 255), 3, 3, emptyTiles);
		Node[,] chunk2 = LevelEditor_2.createChunk(demoMap, new Color32(0, 255, 0, 255), 2, 1);
		Node[,] chunk3 = LevelEditor_2.createChunk(demoMap, new Color32(0, 0, 255, 255), 1, 2);

		LevelEditor_2.createTwoWayLink(
			chunk1,
			chunk2,
			new LevelEditor_2.TileCoord(0, 1, false, true, false, false),
			new LevelEditor_2.TileCoord(0, 0, false, false, false, true)
			);

		LevelEditor_2.createTwoWayLink(
			chunk1,
			chunk2,
			new LevelEditor_2.TileCoord(2, 1, false, false, false, true),
			new LevelEditor_2.TileCoord(1, 0, false, true, false, false)
			);

		LevelEditor_2.createTwoWayLink(
			chunk1,
			chunk3,
			new LevelEditor_2.TileCoord(1, 0, false, false, true, false),
			new LevelEditor_2.TileCoord(0, 0, true, false, false, false)
			);

		LevelEditor_2.createTwoWayLink(
			chunk1,
			chunk3,
			new LevelEditor_2.TileCoord(1, 2, true, false, false, false),
			new LevelEditor_2.TileCoord(0, 1, false, false, true, false)
			);



		return demoMap;
	}
}

