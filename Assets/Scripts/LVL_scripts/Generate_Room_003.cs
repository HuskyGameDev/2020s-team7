using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate_Room_003 : Generate_Generic_Room {
	override public string getLevelName() {
		return "003";
	}
	override public Map generateRoom() {
        
		Map demoMap = new Map();
       

			// This map is a long room, with a walled off walway in the middle.
			// The hallway looks short from the outside, and long from the inside

		List<LevelEditor_2.TileCoord> emptyTiles = new List<LevelEditor_2.TileCoord>();
            emptyTiles.Add(new LevelEditor_2.TileCoord(2, 2));

        List<LevelEditor_2.TileCoord> emptyTiles2 = new List<LevelEditor_2.TileCoord>();
            emptyTiles2.Add(new LevelEditor_2.TileCoord(0, 1));
            emptyTiles2.Add(new LevelEditor_2.TileCoord(0, 2));
            emptyTiles2.Add(new LevelEditor_2.TileCoord(0, 3));
            emptyTiles2.Add(new LevelEditor_2.TileCoord(0, 4));
            emptyTiles2.Add(new LevelEditor_2.TileCoord(1, 1));
            emptyTiles2.Add(new LevelEditor_2.TileCoord(1, 2));
            emptyTiles2.Add(new LevelEditor_2.TileCoord(1, 3));
            emptyTiles2.Add(new LevelEditor_2.TileCoord(1, 4));
            emptyTiles2.Add(new LevelEditor_2.TileCoord(2, 1));
            emptyTiles2.Add(new LevelEditor_2.TileCoord(2, 2));
            emptyTiles2.Add(new LevelEditor_2.TileCoord(2, 3));
            emptyTiles2.Add(new LevelEditor_2.TileCoord(2, 4));
            emptyTiles2.Add(new LevelEditor_2.TileCoord(3, 1));
            emptyTiles2.Add(new LevelEditor_2.TileCoord(3, 2));
            emptyTiles2.Add(new LevelEditor_2.TileCoord(3, 3));
            emptyTiles2.Add(new LevelEditor_2.TileCoord(3, 4));

        Node[,] chunk1 = LevelEditor_2.createChunk(demoMap, new Color32(255, 255, 0, 255), 5, 5, emptyTiles);
		Node[,] chunk2 = LevelEditor_2.createChunk(demoMap, new Color32(0, 155, 255, 255), 5, 5, emptyTiles2);

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
				new LevelEditor_2.TileCoord(2, 3),
				new LevelEditor_2.TileCoord(4, 4),
				LevelEditor_2.Direction.North
				);

        LevelEditor_2.setSource(demoMap, chunk1, new LevelEditor_2.TileCoord(0, 0));
        LevelEditor_2.setTarget(demoMap, chunk2, new LevelEditor_2.TileCoord(4, 0));

        return demoMap;
		}
	}

