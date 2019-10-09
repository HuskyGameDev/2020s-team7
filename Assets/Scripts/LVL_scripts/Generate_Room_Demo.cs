using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate_Room_Demo : Generate_Generic_Room {
	override public string getLevelName() {
		return "Demo";
	}
	override public Map generateRoom() {
		Map demoMap = new Map();

		//Demo room map, relativly complex

		List<LevelEditor_2.TileCoord> emptyTiles1 = new List<LevelEditor_2.TileCoord>();
		emptyTiles1.Add(new LevelEditor_2.TileCoord(0, 0));
		emptyTiles1.Add(new LevelEditor_2.TileCoord(2, 0));
		emptyTiles1.Add(new LevelEditor_2.TileCoord(1, 2));

		Node[,] chunk1 = LevelEditor_2.createChunk(demoMap, new Color32(255,200,100,255), 3, 4, emptyTiles1);
		Node[,] chunk2 = LevelEditor_2.createChunk(demoMap, new Color32(100, 100, 255, 255), 3, 4, emptyTiles1);
		
		LevelEditor_2.createOneWayLink(
			chunk1,
			chunk2,
			new LevelEditor_2.TileCoord(0, 3),
			new LevelEditor_2.TileCoord(1, 3),
				GameManager.Direction.East
			);
		LevelEditor_2.createOneWayLink(
			chunk2,
			chunk1,
			new LevelEditor_2.TileCoord(2, 3),
			new LevelEditor_2.TileCoord(1, 3),
				GameManager.Direction.West
			);


		List<LevelEditor_2.TileCoord> emptyTiles2 = new List<LevelEditor_2.TileCoord>();
		emptyTiles2.Add(new LevelEditor_2.TileCoord(1, 1));

		Node[,] chunk3 = LevelEditor_2.createChunk(demoMap, new Color32(255, 0, 0, 255), 3, 3, emptyTiles2);
		Node[,] chunk4 = LevelEditor_2.createChunk(demoMap, new Color32(0, 255, 0, 255), 2, 1);
		Node[,] chunk5 = LevelEditor_2.createChunk(demoMap, new Color32(0, 0, 255, 255), 1, 2);

		LevelEditor_2.createTwoWayLink(
			chunk3,
			chunk4,
			new LevelEditor_2.TileCoord(0, 1),
			new LevelEditor_2.TileCoord(0, 0),
				GameManager.Direction.East
			);

		LevelEditor_2.createTwoWayLink(
			chunk3,
			chunk4,
			new LevelEditor_2.TileCoord(2, 1),
			new LevelEditor_2.TileCoord(1, 0),
				GameManager.Direction.West
			);

		LevelEditor_2.createTwoWayLink(
			chunk3,
			chunk5,
			new LevelEditor_2.TileCoord(1, 0),
			new LevelEditor_2.TileCoord(0, 0),
				GameManager.Direction.South
			);

		LevelEditor_2.createTwoWayLink(
			chunk3,
			chunk5,
			new LevelEditor_2.TileCoord(1, 2),
			new LevelEditor_2.TileCoord(0, 1),
				GameManager.Direction.North
			);

		LevelEditor_2.createTwoWayLink(
			chunk1,
			chunk3,
			new LevelEditor_2.TileCoord(1, 0),
			new LevelEditor_2.TileCoord(1, 2),
				GameManager.Direction.North
			);

		Node[,] chunk6 = LevelEditor_2.createChunk(demoMap, new Color32(50, 50, 50, 255), 1, 1);

		LevelEditor_2.createTwoWayLink(
			chunk3,
			chunk6,
			new LevelEditor_2.TileCoord(0, 0),
			new LevelEditor_2.TileCoord(0, 0),
				GameManager.Direction.West
			);

		LevelEditor_2.setSource(demoMap, chunk2, new LevelEditor_2.TileCoord(1, 0));
		LevelEditor_2.setTarget(demoMap, chunk6, new LevelEditor_2.TileCoord(0, 0));

		return demoMap;
	}
}
