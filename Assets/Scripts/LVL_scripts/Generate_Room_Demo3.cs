using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate_Room_Demo3 : Generate_Generic_Room {
	override public string getLevelName() {
		return "Demo3";
	}
	override public Map generateRoom() {
		Map demoMap = new Map();

		//Demo room map, relativly complex

		Node[,] chunk1 = LevelEditor_2.createChunk(demoMap, new Color32(255, 200, 100, 255), 3, 3);
		Node[,] chunk2 = LevelEditor_2.createChunk(demoMap, new Color32(100, 100, 255, 255), 3, 3);
		
		LevelEditor_2.createTwoWayLink(
			chunk1,
			chunk2,
			new LevelEditor_2.TileCoord(1, 0),
			new LevelEditor_2.TileCoord(1, 2),
				LevelEditor_2.Direction.North
			);
		LevelEditor_2.createTwoWayLink(
			chunk1,
			chunk2,
			new LevelEditor_2.TileCoord(0, 1),
			new LevelEditor_2.TileCoord(2, 1),
				LevelEditor_2.Direction.West
			);
		LevelEditor_2.createTwoWayLink(
			chunk2,
			chunk1,
			new LevelEditor_2.TileCoord(1, 0),
			new LevelEditor_2.TileCoord(1, 2),
				LevelEditor_2.Direction.North
			);
		LevelEditor_2.createTwoWayLink(
			chunk2,
			chunk1,
			new LevelEditor_2.TileCoord(0, 1),
			new LevelEditor_2.TileCoord(2, 1),
				LevelEditor_2.Direction.West
			);

		LevelEditor_2.setSource(demoMap, chunk1, new LevelEditor_2.TileCoord(1, 1));
		LevelEditor_2.setTarget(demoMap, chunk2, new LevelEditor_2.TileCoord(1, 1));

		return demoMap;
	}
}
