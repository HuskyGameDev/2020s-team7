using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate_Room_004 : Generate_Generic_Room {
	override public string getLevelName() {
		return "004";
	}
	override public Map generateRoom() {
		Map demoMap = new Map();

		// This room has two hallways that go through the middle.
		// both of the hallways are longer from the inside than the outside,
		// and the hallways are also at 90 degrees to each other, passing through the same space.

		List<LevelEditor_2.TileCoord> emptyTiles = new List<LevelEditor_2.TileCoord>();
		emptyTiles.Add(new LevelEditor_2.TileCoord(1, 1));

		Node[,] chunk1 = LevelEditor_2.createChunk(demoMap, new Color32(255, 0, 0, 255), 3, 3, emptyTiles);
		Node[,] chunk2 = LevelEditor_2.createChunk(demoMap, new Color32(0, 255, 0, 255), 2, 1);
		Node[,] chunk3 = LevelEditor_2.createChunk(demoMap, new Color32(0, 0, 255, 255), 1, 2);

		LevelEditor_2.createTwoWayLink(
			chunk1,
			chunk2,
			new LevelEditor_2.TileCoord(0, 1),
			new LevelEditor_2.TileCoord(0, 0),
				LevelEditor_2.Direction.East
			);

		LevelEditor_2.createTwoWayLink(
			chunk1,
			chunk2,
			new LevelEditor_2.TileCoord(2, 1),
			new LevelEditor_2.TileCoord(1, 0),
				LevelEditor_2.Direction.West
			);

		LevelEditor_2.createTwoWayLink(
			chunk1,
			chunk3,
			new LevelEditor_2.TileCoord(1, 0),
			new LevelEditor_2.TileCoord(0, 0),
				LevelEditor_2.Direction.South
			);

		LevelEditor_2.createTwoWayLink(
			chunk1,
			chunk3,
			new LevelEditor_2.TileCoord(1, 2),
			new LevelEditor_2.TileCoord(0, 1),
				LevelEditor_2.Direction.North
			);

		LevelEditor_2.setSource(demoMap, chunk1, new LevelEditor_2.TileCoord(0, 2));
		LevelEditor_2.setTarget(demoMap, chunk1, new LevelEditor_2.TileCoord(2, 0));

		return demoMap;
	}
}

