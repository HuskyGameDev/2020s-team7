using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

	public string levelSavePath = "level.json";

	public void CreateAndSaveLevel() {
		LevelMap.Save(GenerateLevel(), "/" + levelSavePath+".json");
		//UnityEditor.AssetDatabase.Refresh();
	}
	public virtual LevelMap GenerateLevel() { return new LevelMap();}
	public virtual void DeleteRenderMap() { }
}
