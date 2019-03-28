using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	public static class Generate_Room_3 {

		public static Map generateRoom() {
			Map demoMap = new Map();

			List<LevelEditor_2.TileCoord> emptyTiles = new List<LevelEditor_2.TileCoord>();
			emptyTiles.Add(new LevelEditor_2.TileCoord(2, 1));

			Node[,] chunk1 = LevelEditor_2.createChunk(demoMap, new Color32(255, 255, 0, 255), 5, 3, emptyTiles);
			Node[,] chunk2 = LevelEditor_2.createChunk(demoMap, new Color32(0, 155, 255, 255), 3, 1);

			LevelEditor_2.createTwoWayLink(
				chunk1,
				chunk2,
				new LevelEditor_2.TileCoord(1, 1, false, true, false, false),
				new LevelEditor_2.TileCoord(0, 0, false, false, false, true)
				);
			LevelEditor_2.createTwoWayLink(
				chunk1,
				chunk2,
				new LevelEditor_2.TileCoord(3, 1, false, false, false, true),
				new LevelEditor_2.TileCoord(2, 0, false, true, false, false)
				);

        LevelEditor_2.setSource(chunk1, new LevelEditor_2.TileCoord(0, 0));
        LevelEditor_2.setTarget(chunk2, new LevelEditor_2.TileCoord(1, 0));

        return demoMap;
		}
	}

