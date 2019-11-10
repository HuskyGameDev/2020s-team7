using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Direction = GameManager.Direction;

[System.Serializable]
public class Node {
	//[ReadOnly]   // nullable variables are not serializable, so could not be saved. Now -1 indicates a non-existant connection
	public int index = -1;
	public Color32 colorF = Color.magenta;	// color for the floor
	public Color32 colorW = Color.magenta;	// color for the walls/corners

	public String floorSprite = null;	// name of sprite for the floor
	public String wallSprite = null;    // name of sprite for the walls
	public String cornerSprite = null;  // name of sprite for the corners
	public String[] debris = new String[9]; // name of sprites for debris 

	public enum TileType { regular, source, target, checkpointon, checkpointoff };
	public TileType type = TileType.regular;	// the type of this tile
	public bool hasSign = false;
	public String signMessage = "";

	// used to get the most current line data
	public LineData data {
		get {
			//return dataStack.Peek();
			return dataList[dataList.Count - 1];
		}
		set {

		}
	}
	// used to get the most current connection data
	public ConnectionSet connections {
		get {
			//return connectionStack.Peek();
			return connectionList[connectionList.Count - 1];
		}
		set {

		}
	}

	
	// had to change these to lists because lists are serializable, but stacks are not.
	[HideInInspector]
	public List<ConnectionSet> connectionList = new List<ConnectionSet>();
	private List<LineData> dataList = new List<LineData>();

	// this is the same as justin left it
	[System.Serializable]
	public class LineData {
		public bool hasEnter = false;
		public bool hasLeave = false;
		public GameManager.Direction enter = Direction.North;
		public GameManager.Direction leave = Direction.North;
		public bool lineActive = false;
	
		public LineData() {

		}

		public LineData(bool hasEnter, bool hasLeave, GameManager.Direction enter, GameManager.Direction leave, bool lineActive) {
			this.hasEnter = hasEnter;
			this.hasLeave = hasLeave;
			this.enter = enter;
			this.leave = leave;
			this.lineActive = lineActive;
		}

		public LineData Copy() {
			return new LineData(hasEnter, hasLeave, enter, leave, lineActive);
		}
	}

	/// <summary>
	/// Adds a connection set to the end of the list
	/// </summary>
	/// <param name="set"></param>
	public void AddToConnectionStack(ConnectionSet set) {
		connectionList.Add(set);
		dataList.Add(new LineData());
	}

	/// <summary>
	/// pops all but the last connection and data sets
	/// </summary>
	public void PopConnectionStack() {
		if (data.hasEnter || data.hasLeave)
			return;
		while (connectionList.Count > 1) {
			connectionList.RemoveAt(connectionList.Count - 1);
			dataList.RemoveAt(dataList.Count - 1);
		}
	}

	/// <summary>
	/// Gets the index that the current nodeis  connected to in the given direction
	/// </summary>
	/// <param name="dir"></param>
	/// <returns></returns>
	public int GetConnectionFromDir(GameManager.Direction dir) {
		switch (dir) {
			case GameManager.Direction.North:
				return connections.north;
			case GameManager.Direction.South:
				return connections.south;
			case GameManager.Direction.East:
				return connections.east;
			case GameManager.Direction.West:
				return connections.west;
		}
		return -1;
	}

	// basic constructor
	public Node() {
		//connectionStack.Push(new ConnectionSet());
		connectionList.Add(new ConnectionSet());
		//dataStack.Push(new LineData());
		dataList.Add(new LineData());
	}

	// constructor that sets up a small amount of information
	public Node(int index, Color32 color, string sprite) {
		//connectionStack.Push(new ConnectionSet());
		connectionList.Add(new ConnectionSet());
		//dataStack.Push(new LineData());
		dataList.Add(new LineData());
		this.index = index;
		this.colorF = color;
		this.colorW = color;
		this.floorSprite = sprite;
		this.wallSprite = "Wall_Default";
		this.cornerSprite = "Corner_Default";
	}

	// detailed constructor, used in the node.copy() method
	public Node(int index, int north, int south, int east, int west, Color32 colorF, Color32 colorW, String floorSprite, String wallSprite, String cornerSprite, String[] debris, TileType type, bool hasSign, String signMessage, List<ConnectionSet> connectionOriginal, List<LineData> dataOriginal) {
		//connectionStack.Push(new ConnectionSet());
		connectionList.Add(new ConnectionSet());
		//dataStack.Push(new LineData());
		dataList.Add(new LineData());
		this.index = index;
		this.connections.north = north;
		this.connections.south = south;
		this.connections.east = east;
		this.connections.west = west;
		this.colorF = colorF;
		this.colorW = colorW;
		this.floorSprite = floorSprite;
		this.wallSprite = wallSprite;
		this.cornerSprite = cornerSprite;
		for (int i = 0; i < 9; i++) {
			this.debris[i] = debris[i];
		}

		this.type = type;
		this.hasSign = hasSign;
		this.signMessage = signMessage;
		foreach (ConnectionSet conn in connectionOriginal) {
			this.connectionList.Add(conn.Copy());
		}
		foreach (LineData data in dataOriginal) {
			this.dataList.Add(data.Copy());
		}
	}

	/// <summary>
	/// Returns a copy of the calling node
	/// </summary>
	/// <returns></returns>
	public Node Copy() {
		/*int index;
		Color32 colorF;
		Color32 colorW;
	
		String floorSprite;
		String wallSprite;
		String cornerSprite;
		String[] debris;
	
		TileType type;
		bool hasSign;
		String signMessage;*/
		Node newNode = new Node(index, connections.north, connections.south, connections.east, connections.west, colorF, colorW, floorSprite, wallSprite, cornerSprite, debris, type, hasSign, signMessage, connectionList, dataList);
		for (int i = 0; i < 9; i++) {
			newNode.debris[i] = this.debris[i];
		}
		return newNode;
	}

	/// <summary>
	/// Returns a list of all nodes that a node is connected to in the given direction
	/// </summary>
	/// <param name="dir"></param>
	/// <returns></returns>
	public List<Node> GetFullStackConnectionsFromDir(Direction dir) {
		List<Node> ns = new List<Node>();
		
		ConnectionSet[] conns = connectionList.ToArray();
		Array.Reverse(conns);
		foreach (ConnectionSet set in conns) {
			//if (set[dir] != null) {
			if (set[dir] != -1) {
					Node n = GameManager.instance.gameplay.map[(int)set[dir]];
				if (ns.Contains(n) == false)
					ns.Add(n);
			}
		}
		return ns;
	}

	/// <summary>
	/// lists the nodes that a node is connected to in each direction
	/// </summary>
	[System.Serializable]
	public class ConnectionSet {
		/*
		public int? north = null;
		public int? south = null;
		public int? east = null;
		public int? west = null;
		*/
		public int north = -1;
		public int south = -1;
		public int east = -1;
		public int west = -1;

		//public int? this[Direction dir] {
		public int this[Direction dir] {
			get {
				switch (dir) {
					case Direction.North:
						return north;
					case Direction.South:
						return south;
					case Direction.West:
						return west;
					case Direction.East:
						return east;
				}
				return east;
			}
			set {
				switch (dir) {
					case Direction.North:
						north = value;
						break;
					case Direction.South:
						south = value;
						break;
					case Direction.West:
						west = value;
						break;
					case Direction.East:
						east = value;
						break;
				}
			}
		}

		public ConnectionSet Copy() {
			ConnectionSet newSet = new ConnectionSet();
			newSet.north = north;
			newSet.south = south;
			newSet.east = east;
			newSet.west = west;
			return newSet;
		}
	}

}
