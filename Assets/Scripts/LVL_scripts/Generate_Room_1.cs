using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Generate_Room_1{
	public static Map generateRoom() {
		Map demoMap = new Map();

		// simple map for 1st room, one chunk, not strange connections
		Node[,] chunk = LevelEditor_2.createChunk(demoMap, new Color32(255, 255, 255, 255), 3, 3);

		LevelEditor_2.setSource(chunk, new LevelEditor_2.TileCoord(0, 1));
		LevelEditor_2.setTarget(chunk, new LevelEditor_2.TileCoord(2, 1));

		return demoMap;
	}
}
