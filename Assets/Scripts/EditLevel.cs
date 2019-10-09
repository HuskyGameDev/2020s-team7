using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EditLevel : MonoBehaviour {

	public string nameToSaveLevelWith;
	public bool overwriteLevel;
	//public 

	public Node currentNode;

	public void getCurrentNode() {
		currentNode = GameManager.instance.gameplay.currentPosition.Copy();
		//currentNode = GameManager.instance.gameplay.currentPosition;
	}
	
	public void applyToNode() {
		//GameManager.instance.gameplay.currentPosition = currentNode.Copy;
		GameManager.instance.gameplay.map[GameManager.instance.gameplay.currentPosition.index] = currentNode.Copy();
		GameManager.instance.gameplay.currentPosition = GameManager.instance.gameplay.map[GameManager.instance.gameplay.currentPosition.index];
		GameManager.instance.gameplay.nonEuclidRenderer.HandleRender(GameManager.Direction.East, GameManager.instance.gameplay.currentPosition, false);
	}

	public void saveLevel() {
		Map modMap = GameManager.instance.gameplay.map;
		if (File.Exists(Application.dataPath + "/LevelsEdited/room_" + nameToSaveLevelWith)) {
			if (overwriteLevel) {
                Map.Save(modMap, Application.dataPath + "/LevelsEdited/room_" + (nameToSaveLevelWith) +".json");
				Debug.Log("Overwriting level at: \"" + Application.dataPath + "/LevelsEdited/room_" + nameToSaveLevelWith + "\" ?");
			} else {
				Debug.Log("Are you sure you want to overwrite the level at: \"" + Application.dataPath + "/LevelsEdited/room_" + nameToSaveLevelWith + "\" ?");
			}
		} else {
			Map.Save(modMap, Application.dataPath + "/LevelsEdited/room_" + (nameToSaveLevelWith)+".json");
		}
		
	}

}
