using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Direction = GameManager.Direction;

public class Node {

    public int index;
    // Use this for initialization
	public Color32 color = Color.magenta;
	public LineData data {
        get {
            return dataStack.Peek();
        }
        set {

        }
    }
    public ConnectionSet connections {
        get {
            return connectionStack.Peek();
        }
        set {
            
        }
    }

    private Stack<ConnectionSet> connectionStack = new Stack<ConnectionSet>();
    private Stack<LineData> dataStack = new Stack<LineData>();

	public class LineData {
        public bool hasEnter = false;
        public bool hasLeave = false;
		public GameManager.Direction enter = Direction.North;
		public GameManager.Direction leave = Direction.North;
		public bool lineActive = false;
		public enum TileType {regular, source, target};
		public TileType type = TileType.regular;
	}

	public void AddToConnectionStack(ConnectionSet set) {
        connectionStack.Push(set);
        dataStack.Push(new LineData());
    }

    public void PopConnectionStack() {
        if (data.hasEnter || data.hasLeave)
            return;
        while(connectionStack.Count > 1) {
            connectionStack.Pop();
            dataStack.Pop();
        }
    }

    public int? GetConnectionFromDir(GameManager.Direction dir) {
        switch (dir)
        {
            case GameManager.Direction.North:
                return connections.north;
                break;
            case GameManager.Direction.South:
                return connections.south;
                break;
            case GameManager.Direction.East:
                return connections.east;
                break;
            case GameManager.Direction.West:
                return connections.west;
                break;
        }
        return null;
    }

    public Node() {
        connectionStack.Push(new ConnectionSet());
		dataStack.Push(new LineData());
	}

	public Node(int index, Color32 color) {
        connectionStack.Push(new ConnectionSet());
		dataStack.Push(new LineData());
		this.index = index;
		this.color = color;
	}

	public Node(int? north, int? south, int? east, int? west, int index, Color32 color) {
        connectionStack.Push(new ConnectionSet());
		dataStack.Push(new LineData());
        this.connections.north = north;
        this.connections.south = south;
        this.connections.east = east;
        this.connections.west = west;
    }

    public Node Copy() {
        return new Node(connections.north, connections.south, connections.east, connections.west, index, color);
    }

    public List<Node> GetFullStackConnectionsFromDir(Direction dir) {
        List<Node> ns = new List<Node>();
        ConnectionSet[] conns = connectionStack.ToArray();
        foreach (ConnectionSet set in conns) {
            if (set[dir] != null) {
                Node n = GameManager.instance.map[(int)set[dir]]; 
                if (ns.Contains(n) == false)
                    ns.Add(n);
            }
        }
        return ns;
    }


    public class ConnectionSet {
        public int? north = null;
        public int? south = null;
        public int? east = null;
        public int? west = null;

        public int? this[Direction dir] {
            get { 
                switch (dir) {
                    case Direction.North:
                    return north;
                    break;
                    case Direction.South:
                    return south;                
                    break;
                    case Direction.West:
                    return west;                
                    break;
                    case Direction.East:
                    return east;                
                    break;
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
