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
	public String[] debris = new String[9]; // name of sprites for debris 

	public enum TileType { regular, source, target, checkpoint };
	public TileType type = TileType.regular;	// the type of this tile
	public bool hasSign = false;
	public String signMessage = "";

	public int[] defaultConn = { -1, -1, -1, -1};
	public int[] tempConn = { -1, -1, -1, -1 };
	public bool[] drawCorner = { false, false, false, false };

	public bool hasEnter = false;
	public bool hasLeave = false;
	public GameManager.Direction enter = Direction.North;
	public GameManager.Direction leave = Direction.North;

	/// <summary>
	/// Gets the index that the current nodeis  connected to in the given direction
	/// </summary>
	/// <param name="dir"></param>
	/// <returns></returns>
	public int this[int dir] {
		get {
			if (dir < 0 || dir > 3) {
				return -1;
			} else if (tempConn[dir] >= 0) {
				return tempConn[dir];
			} else {
				return defaultConn[dir];
			}
		}
		set {
			if (dir >= 0 || dir <= 3) {
				tempConn[dir] = value;
			}
		}
	}

	public bool hasTempConn(int dir) {
		if (tempConn[dir] >= 0) {
			return true;
		} else {
			return false;
		}
	}

	/*
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
	}*/


	// had to change these to lists because lists are serializable, but stacks are not.
	//[HideInInspector]
	//public List<ConnectionSet> connectionList = new List<ConnectionSet>();
	//private List<LineData> dataList = new List<LineData>();

	// this is the same as justin left it
	/*[System.Serializable]
	public class LineData {
		public bool hasEnter = false;
		public bool hasLeave = false;
		public GameManager.Direction enter = Direction.North;
		public GameManager.Direction leave = Direction.North;
		//public bool lineActive = false;
	
		public LineData() {

		}

		//public LineData(bool hasEnter, bool hasLeave, GameManager.Direction enter, GameManager.Direction leave, bool lineActive) {
		public LineData(bool hasEnter, bool hasLeave, GameManager.Direction enter, GameManager.Direction leave) {
			this.hasEnter = hasEnter;
			this.hasLeave = hasLeave;
			this.enter = enter;
			this.leave = leave;
			//this.lineActive = lineActive;
		}

		public LineData Copy() {
			//return new LineData(hasEnter, hasLeave, enter, leave, lineActive);
			return new LineData(hasEnter, hasLeave, enter, leave);
		}
	}*/
	/*
	/// <summary>
	/// Adds a connection set to the end of the list
	/// </summary>
	/// <param name="set"></param>
	public void AddToConnectionStack(ConnectionSet set) {
		connectionList.Add(set);
		dataList.Add(new LineData());
	}*/

	/// <summary>
	/// pops all but the last connection and data sets
	/// </summary>
	public void PopConnectionStack() {
		for (int i = 0; i < 4; i ++) {
			if (!((hasEnter && ((int)enter == i)) || (hasLeave && ((int)leave == i)))) {
				tempConn[i] = -1;
			}
		}
	}

	/// <summary>
	/// Gets the index that the current nodeis  connected to in the given direction
	/// </summary>
	/// <param name="dir"></param>
	/// <returns></returns>
	/*public int GetConnectionFromDir(int dir) {
		if (tempConn[dir] >= 0) {
			return tempConn[dir];
		} else {
			return defaultConn[dir];
		}
	}*/

	// basic constructor
	public Node() {
	}

	// constructor that sets up a small amount of information
	public Node(int index, Color32 color, string sprite) {
		this.index = index;
		this.colorF = color;
		this.colorW = color;
		this.floorSprite = sprite;
		this.wallSprite = "Default";
	}

	// detailed constructor, used in the node.copy() method
	public Node(int index, 
			Color32 colorF, Color32 colorW, 
			String floorSprite, String wallSprite,
			String[] debris, 
			TileType type, bool hasSign, String signMessage, 
			int[] defaultConn, int[] tempConn, bool[] drawCorner,
			bool hasEnter, bool hasLeave, Direction enter, Direction leave
		) {

		this.index = index;
		this.colorF = colorF;
		this.colorW = colorW;
		this.floorSprite = floorSprite;
		this.wallSprite = wallSprite;
		for (int i = 0; i < 9; i++) {
			this.debris[i] = debris[i];
		}
		this.type = type;
		this.hasSign = hasSign;
		this.signMessage = signMessage;
		for (int i = 0; i < 4; i++) {
			this.defaultConn[i] = defaultConn[i];
			this.tempConn[i] = tempConn[i];
			this.drawCorner[i] = drawCorner[i];
		}
		this.hasEnter = hasEnter;
		this.hasLeave = hasLeave;
		this.enter = enter;
		this.leave = leave;
	}

	/// <summary>
	/// Returns a copy of the calling node
	/// </summary>
	/// <returns></returns>
	public Node Copy() {
		Node newNode = new Node(index, colorF, colorW, 
			floorSprite, wallSprite, 
			debris, type, hasSign, signMessage, 
			defaultConn, tempConn, drawCorner,
			hasEnter, hasLeave, enter, leave);
		return newNode;
	}

	/*
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
					Node n = GameManager.gameplay.map[(int)set[dir]];
				if (ns.Contains(n) == false)
					ns.Add(n);
			}
		}
		return ns;
	}*/
	/*
	/// <summary>
	/// lists the nodes that a node is connected to in each direction
	/// </summary>
	[System.Serializable]
	public class ConnectionSet {
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
	}*/

}
