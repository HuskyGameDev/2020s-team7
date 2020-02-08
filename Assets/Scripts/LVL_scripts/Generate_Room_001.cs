using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate_Room_001 : Generate_Generic_Room {
	override public string getLevelName() {
		return "001";
	}
	override public LevelMap generateRoom() {
		LevelMap demoMap = new LevelMap();

		// simple map for 1st room, one chunk, not strange connections
		Node[,] chunk = LevelEditor_2.createChunk(demoMap, new Color32(255, 255, 255, 255), 3, 3);

		LevelEditor_2.setSource(demoMap, chunk, new LevelEditor_2.TileCoord(0, 1));
		LevelEditor_2.setTarget(demoMap, chunk, new LevelEditor_2.TileCoord(2, 1));

		return demoMap;
	}
}
