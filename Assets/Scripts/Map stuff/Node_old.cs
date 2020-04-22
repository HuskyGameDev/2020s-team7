using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Direction = GameManager.Direction;

[System.Serializable]
public class Node_old {
	//[ReadOnly]   // nullable variables are not serializable, so could not be saved. Now -1 indicates a non-existant connection
	public int index = -1;
	public Color32 colorF = Color.magenta;	// color for the floor
	public Color32 colorW = Color.magenta;	// color for the walls/corners

	public String floorSprite = null;	// name of sprite for the floor
	public String wallSprite = null;    // name of sprite for the walls
	//public String cornerSprite = null;  // name of sprite for the corners
	public String[] debris = new String[9]; // name of sprites for debris 

	//public enum TileType { regular, source, target, checkpoint };
	public Node.TileType type = Node.TileType.regular;	// the type of this tile
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
	//[HideInInspector]
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

	// basic constructor
	public Node_old() {
		//connectionStack.Push(new ConnectionSet());
		connectionList.Add(new ConnectionSet());
		//dataStack.Push(new LineData());
		dataList.Add(new LineData());
	}

	// constructor that sets up a small amount of information
	public Node_old(int index, Color32 color, string sprite) {
		//connectionStack.Push(new ConnectionSet());
		connectionList.Add(new ConnectionSet());
		//dataStack.Push(new LineData());
		dataList.Add(new LineData());
		this.index = index;
		this.colorF = color;
		this.colorW = color;
		this.floorSprite = sprite;
		this.wallSprite = "Default";
		//this.cornerSprite = "Corner_Default";
	}

	// detailed constructor, used in the node.copy() method
	public Node_old(int index, 
			int north, int south, int east, int west, 
			Color32 colorF, Color32 colorW, 
			String floorSprite, String wallSprite, /*String cornerSprite,*/ 
			String[] debris, 
			Node.TileType type, bool hasSign, String signMessage, 
			List<ConnectionSet> connectionOriginal, List<LineData> dataOriginal) {

		this.index = index;
		this.colorF = colorF;
		this.colorW = colorW;
		this.floorSprite = floorSprite;
		this.wallSprite = wallSprite;
		//this.cornerSprite = cornerSprite;
		for (int i = 0; i < 9; i++) {
			this.debris[i] = debris[i];
		}
		this.type = type;
		this.hasSign = hasSign;
		this.signMessage = signMessage;

		//this.connectionList = connectionOriginal.
		foreach (ConnectionSet conn in connectionOriginal) {	// does this actually do anything?
			this.connectionList.Add(conn.Copy());
		}
		foreach (LineData data in dataOriginal) {
			this.dataList.Add(data.Copy());
		}
	}

	/// <summary>
	/// create copy of this node using the new node type
	/// </summary>
	/// <returns></returns>
	public Node ConvertToNew() {
		Node newNode = new Node();
		newNode.index = this.index;
		newNode.colorF = this.colorF;  // color for the floor
		newNode.colorW = this.colorW;  // color for the walls/corners

		newNode.floorSprite = this.floorSprite;   // name of sprite for the floor
		newNode.wallSprite = this.wallSprite;    // name of sprite for the walls
		newNode.debris = this.debris; // name of sprites for debris 

		newNode.type = this.type;    // the type of this tile
		//newNode.hasSign = this.hasSign;
		newNode.signMessage = this.signMessage;

		//newNode.defaultConn = { -1, -1, -1, -1 };
		newNode.defaultConn[0] = this.connectionList[0].north;	// default connections
		newNode.defaultConn[1] = this.connectionList[0].east;
		newNode.defaultConn[2] = this.connectionList[0].south;
		newNode.defaultConn[3] = this.connectionList[0].west;

		return newNode;
	}


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
	}

}
