using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate_Room_Demo2 : Generate_Generic_Room {
	override public string getLevelName() {
		return "Demo2";
	}
	override public Map generateRoom() {
		Map demoMap = new Map();

		//Demo room map, relativly complex

		List<LevelEditor_2.TileCoord> emptyTiles1 = new List<LevelEditor_2.TileCoord>();
		emptyTiles1.Add(new LevelEditor_2.TileCoord(1, 1));
		//emptyTiles1.Add(new LevelEditor_2.TileCoord(2, 0));
		//emptyTiles1.Add(new LevelEditor_2.TileCoord(1, 2));

		Node[,] chunk1 = LevelEditor_2.createChunk(demoMap, new Color32(150, 150, 255, 255), 3, 3, emptyTiles1);
		Node[,] chunk2 = LevelEditor_2.createChunk(demoMap, new Color32(150, 150, 255, 255), 3, 3, emptyTiles1);
		
		LevelEditor_2.createTwoWayLink(
			chunk1,
			chunk2,
			new LevelEditor_2.TileCoord(2, 2),
			new LevelEditor_2.TileCoord(1, 0),
				GameManager.Direction.West
			);

		LevelEditor_2.createTwoWayLink(
			chunk1,
			chunk2,
			new LevelEditor_2.TileCoord(1, 2),
			new LevelEditor_2.TileCoord(2, 0),
				GameManager.Direction.East
			);

		LevelEditor_2.createOneWayLink(
			chunk1,
			chunk1,
			new LevelEditor_2.TileCoord(1, 2),
			new LevelEditor_2.TileCoord(2, 2),
				GameManager.Direction.East
			);

		LevelEditor_2.setSource(demoMap, chunk1, new LevelEditor_2.TileCoord(1, 0));
		//LevelEditor_2.setTarget(demoMap, chunk6, new LevelEditor_2.TileCoord(0, 0));

		return demoMap;
	}
}
