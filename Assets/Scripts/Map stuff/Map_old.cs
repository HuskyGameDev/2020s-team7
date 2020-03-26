using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class LevelMap_old {
	//private static System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
	public int version = 0;
	public int defaultPosition;
	public int sourceNodeIndex = -1; // had to change these. Nullable variables are not serializable, and coud not be saved
	public int targetNodeIndex = -1;

	// new stuff
	[ReadOnly] // would like to be able to see this in the editor, but doen't want people accidentaly changing it
	[SerializeField]
	private int _size = 0;	// number of tiles in the map
	[ReadOnly] // would like to be able to see this in the editor, but doen't want people accidentaly changing it
	[SerializeField]

	private int arraySize = 20;
    public int[] checkpoints;
    public int stringleft = 21;
	[SerializeField]
	private Node_old[] nodes = new Node_old[20]; // by default, array of tiles has 20 slots.
	public Node_old this[int i] { // get/set method that will automaticaly make the array of tiles larger when necessary
		get {
			if ((i >= _size) || (i < 0)) { // can't return nodes that can't be in the arrray
				return null;
			} else {
				return nodes[i];
			}
		}
		set {
			if (i >= _size)
				_size = i + 1;

			while (_size > arraySize) { // make the array larger if it needs to be
				int oldSize = arraySize;
				arraySize += 10;
				Node_old[] newNodes = new Node_old[arraySize];
				for (int j = 0; j < oldSize; j++) {
					newNodes[j] = nodes[j];
				}
				nodes = newNodes;
			}

			if (nodes[i] != null) {
				nodes[i] = value;
			} else {
				value.index = i; 
				nodes[i] = value;
			}
		}
	}

	public int size {   // getter method. Shouldn't be set from outside
		get { return _size; }
		set { }
	}

	/// <summary>
	/// Loads the map from the given file path.
	/// Prints some warinings if things don't work right.
	/// Returns the loaded map.
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	public static LevelMap_old Load(string path) {
		if (File.Exists(path)) {	// can only load the map if the given file exists
			//Debug.Log("Success: map file exists at path: " + path);
			string jsonData = File.ReadAllText(path);
			LevelMap_old map = JsonUtility.FromJson<LevelMap_old>(jsonData);
			if (map == null) {
				Debug.Log("Error: map is null, WTF?");
				return null;
			}
			return map;
		} else {
			return null;
		}
	}

	/// <summary>
	/// Copy relevant data to new map, that uses new node type
	/// </summary>
	/// <param name="oldMap"></param>
	/// <returns></returns>
	public static LevelMap ConvertToNew(LevelMap_old oldMap) {
		if (oldMap.version != 0) {
			Debug.Log("Error: map is not version 0, cannot convert to version 1");
			return null;
		}
		LevelMap newMap = new LevelMap();
		newMap.version = 1;
		newMap.defaultPosition = oldMap.defaultPosition;	// copy map info
		newMap.sourceNodeIndex = oldMap.sourceNodeIndex;
		newMap.targetNodeIndex = oldMap.targetNodeIndex;
		newMap.checkpoints = new int[oldMap.checkpoints.Length];
		for (int i = 0; i < oldMap.checkpoints.Length; i++) newMap.checkpoints[i] = oldMap.checkpoints[i];
		newMap.stringleft = oldMap.stringleft;

		Node[] newNodes = new Node[oldMap.size];
		for (int i = 0; i < oldMap.size; i++) {
			newNodes[i] = oldMap[i].ConvertToNew();	// create matching nodes of the new type
		}

		newMap.setNodes(newNodes);

		//set corner-drawing.

		return newMap;
	}
}
