using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Generate_Room_1{
	public static Map generateRoom() {
		Map demoMap = new Map();

		Node[,] chunk = LevelEditor_2.createChunk(demoMap, new Color32(255, 255, 255, 255), 3, 3);

		return demoMap;
	}
}
