﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class LevelMap {
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
    public bool winConditions()
    {
        if (!(GameManager.gameplay.currentPosition.type == Node.TileType.target) || !GameManager.gameplay.hasBall)
        {
            return false;
        }
		//Debug.Log("Checking conditions");
		if (countCheckpoints() < checkpoints.Length) return false;
        //Debug.Log(GameManager.instance.gameplay.stringLeft == 0);
        return GameManager.gameplay.stringLeft == 0; //stringleft == 0; //&& GameManager.instance.gameplay.currentPosition.index == GameManager.instance.gameplay.map.targetNodeIndex;
    }
	[SerializeField]
	private Node[] nodes = new Node[20]; // by default, array of tiles has 20 slots.
	public Node this[int i] { // get/set method that will automaticaly make the array of tiles larger when necessary
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
				Node[] newNodes = new Node[arraySize];
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

	public int size {	// getter method. Shouldn't be set from outside
		get{ return _size; }
		set{ }
	}

	public LevelMap() {
		version = 1;
	}

	/// <summary>
	/// Sets the list of nodes for this map to the given array.
	/// Used in the clean up method of LevelEditor_2
	/// </summary>
	/// <param name="newNodes"></param>
	/// <param name="newSize"></param>
	/// <param name="newArraySize"></param>
	public void setNodes(Node[] newNodes) {//, int newSize, int newArraySize) {
		nodes = newNodes;
		//_size = newSize;
		//arraySize = newArraySize;
		_size = newNodes.Length;
		arraySize = newNodes.Length;
	}

	/// <summary>
	/// returns a copy of a map
	/// </summary>
	/// <returns></returns>
	public LevelMap Copy() {
		LevelMap newCopy = new LevelMap();
		for (int i = 0; i < _size; i++) {
			newCopy[i] = nodes[i].Copy();
		}

		return newCopy;
	}

	/// <summary>
	/// Save the given map at the given file path
	/// </summary>
	/// <param name="map"></param>
	/// <param name="path"></param>
	public static void Save(LevelMap map, string path) {
		//for (int i = 0; i < map.size; i++) {
		//	map[i].hasEnter = false;
		//	map[i].hasLeave = false;
		//}
		string jsonData = JsonUtility.ToJson(map, true);
		File.WriteAllText(path, jsonData);
	}

	/// <summary>
	/// Loads the map from the given file path.
	/// Prints some warinings if things don't work right.
	/// Returns the loaded map.
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	public static LevelMap Load(string path) {
		if (File.Exists(path)) {	// can only load the map if the given file exists
			//Debug.Log("Success: map file exists at path: " + path);
			string jsonData = File.ReadAllText(path);
			LevelMap map = JsonUtility.FromJson<LevelMap>(jsonData);
			

			if (map == null) {
				Debug.Log("Error: map is null, WTF?");
			}

			/*if (map.sourceNodeIndex >= 0) {
				GameManager.gameplay.currentPosition = map[map.sourceNodeIndex];
			} else {
				Debug.Log("Error: map.sourceNode is null");
				GameManager.gameplay.currentPosition = map[0];
			}*/
			/*
			for (int i = 0; i < map.arraySize; i++) {
				Node n = map[i];
				if (n.colorF = ) {

				}
			}
			*/
			return map;
		} else {
			return null;
		}

	}

	public int countCheckpoints() {
		int count = 0;
		foreach (int i in checkpoints) {
			if (GameManager.gameplay.map[i].hasEnter/* && GameManager.gameplay.map[i].hasLeave*/) count++;
		}
		return count;
	}

	public bool disjoint(Node node, GameManager.Direction dir) {
		Node other = this[node[(int)dir]];
		GameManager.Direction opposite = dir.inverse();
		if (
			((node.hasEnter && (node.enter == dir)) && ((other.leave != opposite) || !other.hasLeave)) ||
			((node.hasLeave && (node.leave == dir)) && ((other.enter != opposite) || !other.hasEnter)) ||
			((other.hasEnter && (other.enter == opposite)) && ((node.leave != dir) || !node.hasLeave)) ||
			((other.hasLeave && (other.leave == opposite)) && ((node.enter != dir) || !node.hasEnter))
			) {
			return true;
		} else {
			return false;
		}
	}
}