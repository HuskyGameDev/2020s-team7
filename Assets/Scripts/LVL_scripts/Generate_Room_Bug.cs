using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Generate_Room_Bug {

	public static Map generateRoom() {
		Map demoMap = new Map();

		// This map demonstrates the ways that connects can be very wierd
		// Demonstrates cunfusing, undesired behavior

		List<LevelEditor_2.TileCoord> emptyTiles = new List<LevelEditor_2.TileCoord>();
		emptyTiles.Add(new LevelEditor_2.TileCoord(0, 0));
		emptyTiles.Add(new LevelEditor_2.TileCoord(0, 2));
		emptyTiles.Add(new LevelEditor_2.TileCoord(2, 0));
		emptyTiles.Add(new LevelEditor_2.TileCoord(2, 2));

		Node[,] chunk = LevelEditor_2.createChunk(demoMap, new Color32(255, 255, 200, 255), 3, 3, emptyTiles);

		LevelEditor_2.createOneWayLink(
			chunk,
			chunk,
			new LevelEditor_2.TileCoord(1, 0),
			new LevelEditor_2.TileCoord(0, 1),
				LevelEditor_2.Direction.North
			);

		LevelEditor_2.createOneWayLink(
			chunk,
			chunk,
			new LevelEditor_2.TileCoord(0, 1),
			new LevelEditor_2.TileCoord(1, 0),
				LevelEditor_2.Direction.West
			);

		LevelEditor_2.createTwoWayLink(
			chunk,
			chunk,
			new LevelEditor_2.TileCoord(1, 2),
			new LevelEditor_2.TileCoord(2, 1),
				LevelEditor_2.Direction.South
			);

		LevelEditor_2.createTwoWayLink(
			chunk,
			chunk,
			new LevelEditor_2.TileCoord(2, 1),
			new LevelEditor_2.TileCoord(1, 2),
				LevelEditor_2.Direction.East
			);

		return demoMap;
	}
}

