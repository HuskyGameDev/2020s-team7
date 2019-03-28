using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Map {

	public int defaultPosition;
	private int _size = 0;
	public int size {get {return _size;} set{}}

	[SerializeField]
	public Dictionary<int, Node> nodes = new Dictionary<int, Node>();

	public Node this[int i] { 
		get 
		{
			if (nodes.ContainsKey(i) == false) {
				Node newNode = new Node();
				newNode.index = i;
				nodes.Add(i, newNode);
			}
			return (nodes.ContainsKey(i) == false) ? null : nodes[i]; 
		} 
		set 
		{ 
			if (i >= _size)
				_size = i + 1;
			
			if (nodes.ContainsKey(i)) {
				nodes[i] = value;
			}
			else
			{
				value.index = i;
				nodes.Add(i,value);
			}
		}
	}


	public Map Copy() {
		Map newCopy = new Map();
		for (int i = 0; i < size; i++)
		{
			newCopy[i] = nodes[i].Copy();
		}

		return newCopy;
	}


	public static void Save(Map map, string path) {
		string jsonData = JsonUtility.ToJson(map,true);
		File.WriteAllText(Application.dataPath + path, jsonData);
	}

	public static Map Load(string path) {
		if (File.Exists(path)) {
			string jsonData = File.ReadAllText(path);
			return JsonUtility.FromJson<Map>(jsonData);
		}
		else {
			return null;
		}
	}
}
