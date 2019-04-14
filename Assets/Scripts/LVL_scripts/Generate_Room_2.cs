using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Generate_Room_2 {

	public static Map generateRoom() {
		Map demoMap = new Map();

		// still a simple map for room 2, still one chunk but a bit larger
		Node[,] chunk = LevelEditor_2.createChunk(demoMap, new Color32(255, 200, 100, 255), 5, 5);

		LevelEditor_2.setSource(chunk, new LevelEditor_2.TileCoord(0, 1));
		LevelEditor_2.setTarget(chunk, new LevelEditor_2.TileCoord(4, 3));

		return demoMap;
	}
}
