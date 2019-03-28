using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Generate_Room_2 {

	public static Map generateRoom() {
		Map demoMap = new Map();

		Node[,] chunk = LevelEditor_2.createChunk(demoMap, new Color32(255, 200, 100, 255), 5, 5);

		return demoMap;
	}
}
