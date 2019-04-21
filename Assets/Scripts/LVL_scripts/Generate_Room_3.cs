using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	public static class Generate_Room_3 {

		public static Map generateRoom() {
			Map demoMap = new Map();

			// This map is a long room, with a walled off walway in the middle.
			// The hallway looks short from the outside, and long from the inside

			List<LevelEditor_2.TileCoord> emptyTiles = new List<LevelEditor_2.TileCoord>();
			emptyTiles.Add(new LevelEditor_2.TileCoord(2,2));

			Node[,] chunk1 = LevelEditor_2.createChunk(demoMap, new Color32(255, 255, 0, 255), 5, 5, emptyTiles);
			Node[,] chunk2 = LevelEditor_2.createChunk(demoMap, new Color32(0, 155, 255, 255), 5, 1);

			LevelEditor_2.createTwoWayLink(
				chunk1,
				chunk2,
				new LevelEditor_2.TileCoord(1, 2),
				new LevelEditor_2.TileCoord(0, 0),
				LevelEditor_2.Direction.East
				);
			LevelEditor_2.createTwoWayLink(
				chunk1,
				chunk2,
				new LevelEditor_2.TileCoord(3, 2),
				new LevelEditor_2.TileCoord(4, 0),
				LevelEditor_2.Direction.West
				);

        LevelEditor_2.setSource(chunk1, new LevelEditor_2.TileCoord(0, 0));
        LevelEditor_2.setTarget(chunk2, new LevelEditor_2.TileCoord(2, 0));

        return demoMap;
		}
	}

