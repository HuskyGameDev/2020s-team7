using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EditLevel : MonoBehaviour {

	public string levelName = "test";
	public string levelPath = "/LevelsEdited";
	public bool overwriteLevel;

	public GameManager.Direction methodDirection;
	public int linkX = -1;
	public int linkY = -1;
	public int linkToIndex = -1;
	public enum LinkDirectionality { Oneway, Twoway};
	public LinkDirectionality linkDirectionality;

	public byte newChunkColorR = 255;
	public byte newChunkColorG = 255;
	public byte newChunkColorB = 255;
	private byte newChunkColorA = 255;
	public int newChunkWidth;
	public int newChunkHeight;
	
	public Node[,] chunkToLink;
	public Node[][,] chunks = new Node[0][,];

	public Node currentNode;
	public Map currentMap;

	public void getNewMap() {
		Map tempMap = new Map();
		tempMap[0] = new Node(0, new Color32(newChunkColorR, newChunkColorG, newChunkColorB, newChunkColorA), null);
		LevelEditor_2.setSource(tempMap, 0);
		currentMap = tempMap;
		GameManager.instance.gameplay.map = tempMap;
		GameManager.instance.gameplay.resetLevelAssets();
	}

	public void getCurrentMap() {
		//Debug.Log("getCurrentMap() does not do anything right now");
		currentMap = GameManager.instance.gameplay.map;
	}

	public void saveLevel() {
		getCurrentMap();
		LevelEditor_2.cleanUpMap(currentMap);
		if (File.Exists(Application.dataPath + levelPath + "/room_" + levelName)) {
			if (overwriteLevel) {
				Map.Save(currentMap, Application.dataPath + levelPath + "/room_" + levelName);
				Debug.Log("Overwriting level at: \"" + Application.dataPath + levelPath + "/room_" + levelName + "\" ?");
			} else {
				Debug.Log("Are you sure you want to overwrite the level at: \"" + Application.dataPath + levelPath + "/room_" + levelName + "\" ?");
			}
		} else {
			Map.Save(currentMap, Application.dataPath + levelPath + "/room_" + levelName);
		}

	}

	public void loadLevelByName() {
		if (File.Exists(Application.dataPath + levelPath + "/room_" + levelName)) {
			currentMap = Map.Load(Application.dataPath + levelPath + "/room_" + levelName);
			GameManager.instance.gameplay.map = currentMap;
			GameManager.instance.gameplay.resetLevelAssets();
		} else {
			Debug.Log("Error: Map file does not exist at path \"" + Application.dataPath + "/Levels/room_" + levelName + "\"");
		}
	}

	public void getCurrentNode() {
		currentNode = GameManager.instance.gameplay.currentPosition.Copy();
		//currentNode = GameManager.instance.gameplay.currentPosition;
	}
	
	public void applyToNode() {
		getCurrentMap();
		//GameManager.instance.gameplay.currentPosition = currentNode.Copy;
		currentMap[GameManager.instance.gameplay.currentPosition.index] = currentNode.Copy();
		GameManager.instance.gameplay.currentPosition = currentMap[GameManager.instance.gameplay.currentPosition.index];
		GameManager.instance.gameplay.nonEuclidRenderer.HandleRender(GameManager.Direction.East, GameManager.instance.gameplay.currentPosition, false);
	}

	public void createTileChunk() {
		getCurrentMap();
		Node[][,] temp = new Node[chunks.Length + 1][,];
		int i;
		for (i = 0; i < chunks.Length; i++) {
			temp[i] = chunks[i];
		}
		chunks = temp;
		chunks[i] = LevelEditor_2.createChunk(currentMap, new Color32(newChunkColorR, newChunkColorG, newChunkColorB, newChunkColorA), newChunkWidth, newChunkHeight);
		chunkToLink = chunks[i];
	}

	public void createLink() {
		getCurrentNode();
		getCurrentMap();
		if ((linkX >= 0) && (linkY >= 0) && (chunkToLink != null)) {
			int toIndex = chunkToLink[(int)linkX, (int)linkY].index;
			if (linkDirectionality == LinkDirectionality.Twoway) {
				LevelEditor_2.createTwoWayLink(currentMap, currentNode.index, toIndex, methodDirection);
			} else {
				LevelEditor_2.createOneWayLink(currentMap, currentNode.index, toIndex, methodDirection);
			}
		} else if (linkToIndex >= 0) {
			if (linkDirectionality == LinkDirectionality.Twoway) {
				LevelEditor_2.createTwoWayLink(currentMap, currentNode.index, (int)linkToIndex, methodDirection);
			} else {
				LevelEditor_2.createOneWayLink(currentMap, currentNode.index, (int)linkToIndex, methodDirection);
			}
		} else {
			Debug.Log("Both link-coordinates (link x & link y) & linkToIndex are invalid, cannot create link");
		}
		GameManager.instance.gameplay.nonEuclidRenderer.HandleRender(GameManager.Direction.East, currentNode, false);
	}

	public void setSource() {
		getCurrentNode();
		getCurrentMap();
		LevelEditor_2.setSource(currentMap, currentNode.index);
		GameManager.instance.gameplay.nonEuclidRenderer.HandleRender(GameManager.Direction.East, currentNode, false);
	}

	public void setTarget() {
		getCurrentNode();
		getCurrentMap();
		LevelEditor_2.setTarget(currentMap, currentNode.index);
		GameManager.instance.gameplay.nonEuclidRenderer.HandleRender(GameManager.Direction.East, currentNode, false);
	}

	/*
	public void cleanUpMap() {
		Debug.Log("cleanUpMap() does not do anything right now");
		Map tempMap = new Map();
		for (int i = 0; i < currentMap.arraySize; i++) {

		}
	}*/


	public void deleteTile() {
		//Debug.Log("deleteTile() does not do anything right now");
		getCurrentNode();
		getCurrentMap();
		int tempIndex = currentNode.GetConnectionFromDir(methodDirection);
		if (tempIndex >= 0) {
			LevelEditor_2.deleteTile(currentMap, tempIndex);
			LevelEditor_2.cleanUpMap(currentMap);
		} else {
			Debug.Log("Error: There is no node the " + methodDirection.ToString() + "-ern direction");
		}
	}
}
