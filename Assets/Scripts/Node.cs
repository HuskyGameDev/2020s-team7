using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Direction = GameManager.Direction;

[System.Serializable]
public class Node {
	public int index;
	// Use this for initialization
	public Color32 color = Color.magenta;
	//public Sprite floorSprite;
	public String floorSprite = null;
	public enum TileType { regular, source, target };
	public TileType type = TileType.regular;

	public LineData data {
		get {
			//return dataStack.Peek();
			return dataList[dataList.Count - 1];
		}
		set {

		}
	}
	public ConnectionSet connections {
		get {
			//return connectionStack.Peek();
			return connectionList[connectionList.Count - 1];
		}
		set {

		}
	}

	
	[SerializeField]
	private List<ConnectionSet> connectionList = new List<ConnectionSet>();
	private List<LineData> dataList = new List<LineData>();


	//[SerializeField]
	//private Stack<ConnectionSet> connectionStack = new Stack<ConnectionSet>();
	//[SerializeField]
	//private Stack<LineData> dataStack = new Stack<LineData>();

	[System.Serializable]
	public class LineData {
		public bool hasEnter = false;
		public bool hasLeave = false;
		public GameManager.Direction enter = Direction.North;
		public GameManager.Direction leave = Direction.North;
		public bool lineActive = false;

		public enum TileType { regular, source, target, checkpointon, checkpointoff };

		public TileType type = TileType.regular;
	
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

	public void AddToConnectionStack(ConnectionSet set) {
		//connectionStack.Push(set);
		connectionList.Add(set);
		//dataStack.Push(new LineData());
		dataList.Add(new LineData());
	}

	public void PopConnectionStack() {
		if (data.hasEnter || data.hasLeave)
			return;
		//while (connectionStack.Count > 1) {
		while (connectionList.Count > 1) {
			//connectionStack.Pop();
			connectionList.RemoveAt(connectionList.Count - 1);
			//dataStack.Pop();
			dataList.RemoveAt(dataList.Count - 1);
		}
	}

	//public int? GetConnectionFromDir(GameManager.Direction dir) {
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
		//return null;
		return -1;
	}

	public Node() {
		//connectionStack.Push(new ConnectionSet());
		connectionList.Add(new ConnectionSet());
		//dataStack.Push(new LineData());
		dataList.Add(new LineData());
	}

	//public Node(int index, Color32 color, Sprite sprite) {
	public Node(int index, Color32 color, string sprite) {
		//connectionStack.Push(new ConnectionSet());
		connectionList.Add(new ConnectionSet());
		//dataStack.Push(new LineData());
		dataList.Add(new LineData());
		this.index = index;
		this.color = color;
		this.floorSprite = sprite;
	}

	//public Node_2(int? north, int? south, int? east, int? west, int index, Color32 color) {
	public Node(int north, int south, int east, int west, int index, Color32 color, String floorSprite, List<ConnectionSet> connectionOriginal, List<LineData> dataOriginal) {
		//connectionStack.Push(new ConnectionSet());
		connectionList.Add(new ConnectionSet());
		//dataStack.Push(new LineData());
		dataList.Add(new LineData());
		this.connections.north = north;
		this.connections.south = south;
		this.connections.east = east;
		this.connections.west = west;
		this.index = index;
		this.color = color;
		this.floorSprite = floorSprite;
		foreach (ConnectionSet conn in connectionOriginal) {
			this.connectionList.Add(conn.Copy());
		}
		foreach (LineData data in dataOriginal) {
			this.dataList.Add(data.Copy());
		}
	}

	public Node Copy() {
		return new Node(connections.north, connections.south, connections.east, connections.west, index, color, floorSprite, connectionList, dataList);
	}

	public List<Node> GetFullStackConnectionsFromDir(Direction dir) {
		List<Node> ns = new List<Node>();
		//ConnectionSet[] conns = connectionStack.ToArray();
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
