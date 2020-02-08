using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate_Room_002 : Generate_Generic_Room {
	override public string getLevelName() {
		return "002";
	}
	override public LevelMap generateRoom() {
		LevelMap demoMap = new LevelMap();

		// still a simple map for room 2, still one chunk but a bit larger
		Node[,] chunk = LevelEditor_2.createChunk(demoMap, new Color32(255, 200, 100, 255), 5, 5);

		LevelEditor_2.setSource(demoMap, chunk, new LevelEditor_2.TileCoord(0, 1));
		LevelEditor_2.setTarget(demoMap, chunk, new LevelEditor_2.TileCoord(4, 3));

		return demoMap;
	}
}
