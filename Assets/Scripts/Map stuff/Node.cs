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

	public enum TileType { regular, source, target, checkpoint, unwalkable};
	public TileType type = TileType.regular;	// the type of this tile
	public bool hasSign = false;
	public String signMessage = "";

	public int[] defaultConn = { -1, -1, -1, -1};	// default connections, returns to using these connection when temp connections are out of sight
	public int[] tempConn = { -1, -1, -1, -1 };	// temp connection, is used instead of default if the value is non-zero.
	public bool[] drawCorner = { false, false, false, false };	// indicates that corners should be drawn, starts at NE and goes clockwise.

	// removed stack for line-direction data. Makes level-saving less glitchy.
	public bool hasEnter = false;
	public bool hasLeave = false;
	public GameManager.Direction enter = Direction.North;
	public GameManager.Direction leave = Direction.North;

	/// <summary>
	/// Gets the index that the current node is connected to in the given direction.
	/// </summary>
	/// <param name="dir"></param>
	/// <returns></returns>
	public int this[int dir] {
		get {
			if (dir < 0 || dir > 3) {	// only return value for valid indexes
				return -1;
			} else if (tempConn[dir] >= 0) {	// return temp connection if it is valid
				return tempConn[dir];
			} else {
				return defaultConn[dir];	// else return default connection/wall
			}
		}
		set {
			if (dir >= 0 || dir <= 3) {	// set the temp connection. Setting the default requires setting directly.
				tempConn[dir] = value;
			}
		}
	}

	/// <summary>
	/// returns true if there is a temp connection in that direction
	/// </summary>
	/// <param name="dir"></param>
	/// <returns></returns>
	public bool hasTempConn(int dir) {
		if (tempConn[dir] >= 0) {
			return true;
		} else {
			return false;
		}
	}

	/// <summary>
	/// pops all but the last connection and data sets
	/// </summary>
	public void PopConnectionStack() {
		for (int i = 0; i < 4; i ++) {	// remove temp connections that do not have a string going through them.
			if (!((hasEnter && ((int)enter == i)) || (hasLeave && ((int)leave == i)))) {
				tempConn[i] = -1;
			}
		}
	}

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

}
