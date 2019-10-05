using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class Map {
	//private static System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

	public int defaultPosition;
	//private int _size = 0;
	//public int size { get { return _size; } set { } }
	//public Node_2 sourceNode = null;
	//public Node_2 targetNode = null;
	public int sourceNodeIndex = -1;
	public int targetNodeIndex = -1;

	// new stuff
	public int size = 0;
	public int arraySize = 20;
	[SerializeField]
	private Node[] nodes = new Node[20];
	public Node this[int i] {
		get {
			if ((i >= size) || (i < 0)) {
				return null;
			} else {
				return nodes[i];
			}
		}
		set {
			if (i >= size)
				size = i + 1;

			while (size > arraySize) {
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

	#region Old code
	/*
	public Dictionary<int, Node_2> nodes = new Dictionary<int, Node_2>();
	public Node_2 this[int i] {
		get {
			if (nodes.ContainsKey(i) == false) {
				Node_2 newNode = new Node_2();
				newNode.index = i;
				nodes.Add(i, newNode);
			}
			return (nodes.ContainsKey(i) == false) ? null : nodes[i];
		}
		set {
			if (i >= _size)
				_size = i + 1;

			if (nodes.ContainsKey(i)) {
				nodes[i] = value;
			} else {
				value.index = i;
				nodes.Add(i, value);
			}
		}
	}
	*/
	#endregion

	public Map Copy() {
		Map newCopy = new Map();
		for (int i = 0; i < size; i++) {
			newCopy[i] = nodes[i].Copy();
		}

		return newCopy;
	}


	public static void Save(Map map, string path) {
		string jsonData = JsonUtility.ToJson(map, true);
		File.WriteAllText(path, jsonData);

	}

	public static Map Load(string path) {
		if (File.Exists(path)) {
			Debug.Log("Success: map file exists at path: " + path);
			string jsonData = File.ReadAllText(path);
			Map map = JsonUtility.FromJson<Map>(jsonData);
			

			if (map == null) {
				Debug.Log("Error: map is null, WTF?");
			}

			if (map.sourceNodeIndex >= 0) {
				Debug.Log("Success: map.sourceNode is not null, is :" + map.sourceNodeIndex);
				GameManager.instance.gameplay.currentPosition = map[map.sourceNodeIndex];
			} else {
				Debug.Log("Error: map.sourceNode is null");
				GameManager.instance.gameplay.currentPosition = map[0];
			}

			return map;
		} else {
			return null;
		}

	}
}
