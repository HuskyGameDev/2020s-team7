using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelEditor_2 {

	/// <summary>
	/// Used to hold information about tile coordinates and which 
	/// edges should have either walls or a connection to another node
	/// </summary>
	public class TileCoord {

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public TileCoord (int x, int y) {
			this.x = x;
			this.y = y;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="north"></param>
		/// <param name="east"></param>
		/// <param name="south"></param>
		/// <param name="west"></param>
		public TileCoord(int x, int y, bool north, bool east, bool south, bool west) {
			this.x = x;
			this.y = y;
			this.North = north;
			this.East = east;
			this.South = south;
			this.West = west;
		}

		// data this class is meant to hold
		public int x;
		public int y;
		public bool North;
		public bool East;
		public bool South;
		public bool West;

		// object that this tile represents
        public GameObject myObject;
	}
	//public enum Direction {North, East, South, West};

		/// <summary>
		/// This method creates a "chunk": a simple cartesian grid of connected tiles.
		/// It takes a map/room that these new tiles should be within, 
		/// a color for all of these tiles to be,
		/// a height and width for the grid to be, 
		/// a list of which tiles should not actually be created within the grid, 
		/// and a list of tiles that should have walls along with which sides of the tile those walls should be on.
		/// </summary>
		/// <param name="room"></param>
		/// <param name="color"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="emptyTiles"></param>
		/// <param name="tileWalls"></param>
		/// <returns></returns>
	public static Node[,] createChunk(LevelMap room, Color32 color, int width, int height, List<TileCoord> emptyTiles = null, List<TileCoord> tileWalls = null) {
		Node[,] chunk = new Node[width,height]; // 2D array of tiles for the new chunk, allows easy accessing based on coordinates

		// do stuff to create chunk with empty tiles, only needs to be done if there is a list of tiles that should be empty
		if (null != emptyTiles) {	
			IEnumerator<TileCoord> emptyIterator = emptyTiles.GetEnumerator();	// iterator for list of empty tiles
			
			// create tiles only where tiles are not indicated as supossed to be empty
			//Debug.Log("Empty tile list exists");
			for (int i = 0; i < width; i++) {
				for (int j = 0; j < height; j++) {
					bool valid = true;  // if a tile should be created for this square
					bool notDone = true;   // indicates that not all listed empty tiles have been checked to see if the match the coordinates of the current tile

					while (valid && notDone) {	// check each tile in the empty tile list, to see if it matches the current coordinates
						notDone = emptyIterator.MoveNext();
						//Debug.Log("emptyIterator.current =  [" + emptyIterator.Current.x + ", " + emptyIterator.Current.y + "]");
						if ((emptyIterator.Current.x == i) && (emptyIterator.Current.y == j)) { //check if coordinate 
							valid = false;
							//Debug.Log("Tile [" + i + ", " + j + "] should not be created");
						}
					}
					emptyIterator.Reset();

					if (valid) {    // if none of the given empty tile coords were found to match current tile coords, create this tile
						int index = room.size;
						//Debug.Log("Creating tile \"" + index + "\" [" + i + ", " + j + "]");
						chunk[i, j] = new Node(index, color, "Floor_Rock"); //GameManager.instance.spriteBook[0]);
						room[index] = chunk[i, j];
					}
				}
			}
		} else {    // create chunk with no empty tiles
			//Debug.Log("No empty tile list");
			for (int i = 0; i < width; i++) {
				for (int j = 0; j < height; j++) {
					int index = room.size;
					//Debug.Log("Creating tile \"" + index + "\" [" + i + ", " + j + "]");
					chunk[i, j] = new Node(index, color, "Floor_Rock"); //GameManager.instance.spriteBook[0]);
					room[index] = chunk[i, j];
				}
			}
		}

		

		// connect adjacent tiles in chunk
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				//Debug.Log("Connecting tiles: i = " + i + ", j = " + j);
				if (chunk[i, j] != null) {
					// connect to northern tile
					if (((j - 1) >= 0) && (chunk[i, j - 1] != null)) {  // if node above if out-of-bounds, or empty, do not link to it
						chunk[i, j].defaultConn[0] = chunk[i, j - 1].index;
						//chunk[i, j].connections.north = chunk[i, j - 1].index;
						//Debug.Log("Connecting tile \"" + chunk[i, j].index + "\" [" + i + ", " + j + "] to \"" + chunk[i, j - 1].index + "\" [" + i + ", " + (j - 1) + "]");
					}

					// connect to southern tile
					if (((j + 1) < height) && (chunk[i, j + 1]) != null) {  // if node below if out-of-bounds, or empty, do not link to it
						chunk[i, j].defaultConn[2] = chunk[i, j + 1].index;
						//chunk[i, j].connections.south = chunk[i, j + 1].index;
						//Debug.Log("Connecting tile \"" + chunk[i, j].index + "\" [" + i + ", " + j + "] to \"" + chunk[i, j + 1].index + "\" [" + i + ", " + (j + 1) + "]");
					}

					// connect to western tile
					if (((i - 1) >= 0) && (chunk[i - 1, j] != null)) {  // if node to left if out-of-bounds, or empty, do not link to it
						chunk[i, j].defaultConn[3] = chunk[i - 1, j].index;
						//chunk[i, j].connections.west = chunk[i - 1, j].index;
						//Debug.Log("Connecting tile \"" + chunk[i, j].index + "\" [" + i + ", " + j + "] to \"" + chunk[i - 1, j].index + "\" [" + (i - 1) + ", " + j + "]");
					}

					// connect to eastern tile
					if (((i + 1) < width) && (chunk[i + 1, j] != null)) {  // if node to right if out-of-bounds, or empty, do not link to it
						chunk[i, j].defaultConn[1] = chunk[i + 1, j].index;
						//chunk[i, j].connections.east = chunk[i + 1, j].index;
						//Debug.Log("Connecting tile \"" + chunk[i, j].index + "\" [" + i + ", " + j + "] to \"" + chunk[i + 1, j].index + "\" [" + (i + 1) + ", " + j + "]");
					}
				}
			}
		}

		// remove links to adjacent tiles, where indicated by list of tiles that should have walls
		// A "wall" is actually just a tile not having a link to another tile in that direction
		if (null != tileWalls) {	// only needs to be done if there is a list of tiles that should have walls
			IEnumerator<TileCoord> wallIterator = tileWalls.GetEnumerator();
			bool notDone2 = true; // indicates that there are still tiles that need walls remaining in the list
			while (notDone2) {
				TileCoord tile = wallIterator.Current;
				if (null != tile) {
					if (tile.North == true) {   // check if north wall
												//chunk[tile.x, tile.y].connections.north = null;
						//chunk[tile.x, tile.y].connections.north = -1;
						chunk[tile.x, tile.y].defaultConn[0] = -1;
					}
					if (tile.South == true) {   // check if south wall
												//chunk[tile.x, tile.y].connections.south = null;
						//chunk[tile.x, tile.y].connections.south = -1;
						chunk[tile.x, tile.y].defaultConn[2] = -1;
					}
					if (tile.East == true) {   // check if East wall
						//chunk[tile.x, tile.y].connections.east = null;
						//chunk[tile.x, tile.y].connections.east = -1;
						chunk[tile.x, tile.y].defaultConn[1] = -1;
					}
					if (tile.West == true) {   // check if west wall
						//chunk[tile.x, tile.y].connections.west = null;
						//chunk[tile.x, tile.y].connections.west = -1;
						chunk[tile.x, tile.y].defaultConn[3] = -1;
					}
				}
				notDone2 = wallIterator.MoveNext();
			}
		}

		return chunk; // return the created chunk, so it can be used when linking tiles together
	}


	/// <summary>
	/// Create a link from the tile at the coordinates indicated in the 1st chunk to 
	/// the tile indicated by the coordinates in the 2nd chunk, but not a connection from the 2nd back to the 1st.
	/// Can be used to link tiles within the same chunk.
	/// Can even be used to link a tile to itself.
	/// </summary>
	/// <param name="chunkFrom"></param>
	/// <param name="chunkTo"></param>
	/// <param name="tileFrom"></param>
	/// <param name="tileTo"></param>
	public static void createOneWayLink(Node[,] chunkFrom, Node[,] chunkTo, TileCoord tileFrom, TileCoord tileTo, GameManager.Direction dir) {
		chunkFrom[tileFrom.x, tileFrom.y].defaultConn[(int)dir] = chunkTo[tileTo.x, tileTo.y].index;
		/*if (dir == GameManager.Direction.North) {
			chunkFrom[tileFrom.x, tileFrom.y].connections.north = chunkTo[tileTo.x, tileTo.y].index;
		} else if (dir == GameManager.Direction.East) {
			chunkFrom[tileFrom.x, tileFrom.y].connections.east = chunkTo[tileTo.x, tileTo.y].index;
		} else if (dir == GameManager.Direction.South) {
			chunkFrom[tileFrom.x, tileFrom.y].connections.south = chunkTo[tileTo.x, tileTo.y].index;
		} else {
			chunkFrom[tileFrom.x, tileFrom.y].connections.west = chunkTo[tileTo.x, tileTo.y].index;
		}*/
	}
	public static void createOneWayLink(LevelMap room, int fromIndex, int toIndex, GameManager.Direction dir) {
		room[fromIndex].defaultConn[(int)dir] = room[toIndex].index;
		/*if (dir == GameManager.Direction.North) {
			room[fromIndex].connections.north = room[toIndex].index;
		} else if (dir == GameManager.Direction.East) {
			room[fromIndex].connections.east = room[toIndex].index;
		} else if (dir == GameManager.Direction.South) {
			room[fromIndex].connections.south = room[toIndex].index;
		} else {
			room[fromIndex].connections.west = room[toIndex].index;
		}*/
	}

	/// <summary>
	/// Create a link between the tile at the coordinates indicated in the 1st chunk and 
	/// the tile indicated by the coordinates in the 2nd chunk.
	/// Can be used to link tiles within the same chunk.
	/// Can even be used to link a tile to itself.
	/// </summary>
	/// <param name="chunk1"></param>
	/// <param name="chunk2"></param>
	/// <param name="tile1"></param>
	/// <param name="tile2"></param>
	public static void createTwoWayLink(Node[,] chunkFrom, Node[,] chunkTo, TileCoord tileFrom, TileCoord tileTo, GameManager.Direction dir) {
		int opposite = ((int)dir + 2 ) % 4;
		chunkFrom[tileFrom.x, tileFrom.y].defaultConn[(int)dir] = chunkTo[tileTo.x, tileTo.y].index;
		chunkTo[tileTo.x, tileTo.y].defaultConn[opposite] = chunkFrom[tileFrom.x, tileFrom.y].index;

		/*if (dir == GameManager.Direction.North) {
			chunkFrom[tileFrom.x, tileFrom.y].connections.north = chunkTo[tileTo.x, tileTo.y].index;
			chunkTo[tileTo.x, tileTo.y].connections.south = chunkFrom[tileFrom.x, tileFrom.y].index;
		} else if (dir == GameManager.Direction.East) {
			chunkFrom[tileFrom.x, tileFrom.y].connections.east = chunkTo[tileTo.x, tileTo.y].index;
			chunkTo[tileTo.x, tileTo.y].connections.west = chunkFrom[tileFrom.x, tileFrom.y].index;
		} else if (dir == GameManager.Direction.South) {
			chunkFrom[tileFrom.x, tileFrom.y].connections.south = chunkTo[tileTo.x, tileTo.y].index;
			chunkTo[tileTo.x, tileTo.y].connections.north = chunkFrom[tileFrom.x, tileFrom.y].index;
		} else {
			chunkFrom[tileFrom.x, tileFrom.y].connections.west = chunkTo[tileTo.x, tileTo.y].index;
			chunkTo[tileTo.x, tileTo.y].connections.east = chunkFrom[tileFrom.x, tileFrom.y].index;
		}*/
	}
	public static void createTwoWayLink(LevelMap room, int fromIndex, int toIndex, GameManager.Direction dir) {
		int opposite = ((int)dir + 2) % 4;
		room[fromIndex].defaultConn[(int)dir] = room[toIndex].index;
		room[toIndex].defaultConn[opposite] = room[fromIndex].index;

		/*if (dir == GameManager.Direction.North) {
			room[fromIndex].connections.north = room[toIndex].index;
			room[toIndex].connections.south = room[fromIndex].index;
		} else if (dir == GameManager.Direction.East) {
			room[fromIndex].connections.east = room[toIndex].index;
			room[toIndex].connections.west = room[fromIndex].index;
		} else if (dir == GameManager.Direction.South) {
			room[fromIndex].connections.south = room[toIndex].index;
			room[toIndex].connections.north = room[fromIndex].index;
		} else {
			room[fromIndex].connections.west = room[toIndex].index;
			room[toIndex].connections.east = room[fromIndex].index;
		}*/
	}

	/// <summary>
	/// Sets the source tile for a map/room. This is where the player starts.
	/// Takes the chunk that the desired tile is in, and the coordinates of the tile within the chunk
	/// </summary>
	/// <param name="chunk"></param>
	/// <param name="tile"></param>
	public static void setSource(LevelMap room, Node[,] chunk, TileCoord tile) {
		setType(room, chunk[tile.x, tile.y].index, Node.TileType.source);
	}

	/// <summary>
	/// Sets the target tile for a map/room. This is where the player needs to draw their line to.
	/// Takes the chunk that the desired tile is in, and the coordinates of the tile within the chunk
	/// </summary>
	/// <param name="chunk"></param>
	/// <param name="tile"></param>
	public static void setTarget(LevelMap room, Node[,] chunk, TileCoord tile) {
		setType(room, chunk[tile.x, tile.y].index, Node.TileType.target);
	}
	
	/// <summary>
	/// changes the type of a tile.
	/// automatically applies visual changes and other supporting stuff to match the change
	/// </summary>
	/// <param name="room"></param>
	/// <param name="tileIndex"></param>
	/// <param name="newType"></param>
	public static void setType(LevelMap room, int tileIndex, Node.TileType newType) {
		//resets visual stuff
		//regular, source, target, checkpointon, checkpointoff
		if (room[tileIndex].type == Node.TileType.checkpoint) {	// if this tile was previously a checkpoint, remove it from the list
			int i;
			bool removed = false;
			for (i = 0; i < room.checkpoints.Length; i++) {
				if (room.checkpoints[i] == tileIndex) {
					removed = true;
					i++;
					break;
				}
			}
			if (removed) {
				for (; i < room.checkpoints.Length; i++) {
					room.checkpoints[i - 1] = room.checkpoints[i];
				}
				int[] temp = new int[room.checkpoints.Length - 1];
				for (i = 0; i < room.checkpoints.Length - 1; i++) {
					temp[i] = room.checkpoints[i];
				}
				room.checkpoints = temp;
			}
		}
		for (int i = 0; i < 9; i++) {
			if (
				room[tileIndex].debris[i] != null && (	// remove sprites used for particular tile types
					room[tileIndex].debris[i].Equals("Source") ||
					room[tileIndex].debris[i].Equals("Target") ||
					room[tileIndex].debris[i].Equals("Checkpoint") ||
					room[tileIndex].debris[i].Equals("Pit_Placeholder")
				)) {
				room[tileIndex].debris[i] = "";
			}
		}

		//set stuff based on tile type
		switch (newType) {
			case Node.TileType.regular:
				room[tileIndex].type = Node.TileType.regular;
				break;
			case Node.TileType.source:
				for (int i = 0; i < room.size; i++) {
					if (room[i].type == Node.TileType.source) {
						room[i].type = Node.TileType.regular;
						room[i].debris[4] = "";
					}
				}
				room[tileIndex].type = Node.TileType.source;
				room[tileIndex].debris[4] = "Source";
				room.sourceNodeIndex = tileIndex;
				break;
			case Node.TileType.target:
				for (int i = 0; i < room.size; i++) {
					if (room[i].type == Node.TileType.target) {
						room[i].type = Node.TileType.regular;
						room[i].debris[4] = "";
					}
				}
				room[tileIndex].type = Node.TileType.target;
				room[tileIndex].debris[4] = "Target";
				room.targetNodeIndex = tileIndex;
				break;
			case Node.TileType.checkpoint:
				room[tileIndex].type = Node.TileType.checkpoint;
				room[tileIndex].debris[4] = "Checkpoint";
				int[] temp = new int[room.checkpoints.Length + 1];	// add this tile to the list of checkpoints
				for (int i = 0; i < room.checkpoints.Length; i++) {
					temp[i] = room.checkpoints[i];
				}
				temp[room.checkpoints.Length] = tileIndex;
				room.checkpoints = temp;
				break;
			case Node.TileType.unwalkable:
				room[tileIndex].type = Node.TileType.unwalkable;	// un-walkable tile type, doen't need anything else
				room[tileIndex].debris[4] = "Pit_Placeholder";
				break;
			default:
				break;
		}
	}

	/// <summary>
	/// Deletes connections that reffer to the other tile, between the given tile and the tile in the given direction
	/// </summary>
	/// <param name="room"></param>
	/// <param name="tileIndex"></param>
	/// <param name="dir"></param>
	public static void createWall(LevelMap room, int tileIndex, GameManager.Direction dir) {
		int otherIndex = (room[tileIndex])[(int)dir];
		int opposite = ((int)dir + 2) % 4;

		room[tileIndex].defaultConn[(int)dir] = -1;
		room[tileIndex].tempConn[(int)dir] = -1;
		room[otherIndex].defaultConn[opposite] = -1;
		room[otherIndex].tempConn[opposite] = -1;
		/*int otherIndex = room[tileIndex].connections[dir];
		Node.ConnectionSet[] thisConns = room[tileIndex].connectionList.ToArray();
		foreach (Node.ConnectionSet set in thisConns) {
			if (set[dir] == otherIndex) {
				set[dir] = -1;
			}
		}
		Node.ConnectionSet[] otherConns = room[otherIndex].connectionList.ToArray();
		foreach (Node.ConnectionSet set in otherConns) {
			if (set[Extensions.inverse(dir)] == tileIndex) {
				set[Extensions.inverse(dir)] = -1;
			}
		}*/
	}

	/// <summary>
	/// Deletes the tile at the given index in the map.
	/// Sets all connections to that tile to null
	/// </summary>
	/// <param name="room"></param>
	/// <param name="index"></param>
	public static void deleteTile(LevelMap room, int index) {
		// if another node in the map has a connection to this node, set that connection to null
		for (int k = 0; k < room.size; k++) {
			// j is index of moved node
			// room[k] is node to check
			// List<ConnectionSet> connectionList is list of connections on node to check
			/*Node.ConnectionSet[] conns = room[k].connectionList.ToArray();
			foreach (Node.ConnectionSet set in conns) {
				for (int dir = 0; dir < 4; dir++) {
					if (set[(GameManager.Direction)dir] == index) {
						//Debug.Log("Deleting connection to node with index " + index);
						set[(GameManager.Direction)dir] = -1;
					}
				}
			}*/
			for (int i = 0; i < 4; i++) {
				if (room[k].defaultConn[i] == index) {
					room[k].defaultConn[i] = -1;
				}
				if (room[k].defaultConn[i] == index) {
					room[k].defaultConn[i] = -1;
				}
			}
		}
		// if deleting the source node for a map, find first valid node in the map and set that to be a source node.
		if (index == room.sourceNodeIndex) {
			for (int k = 0; k < room.size; k++) {
				if ((room[k] != null) && (room[k].index >= 0) && (room[k].index != index)) {
					setType(room, k, Node.TileType.source);
				}
			}
		}
		room[index] = null;
	}

	/// <summary>
	/// Removes null and invalid nodes from the maps array of nodes
	/// </summary>
	/// <param name="room"></param>
	public static void cleanUpMap(LevelMap room) {
		int mapSize = room.size;
		
		/*
		 * For each slot of the array that is suposed to have a node in it based on the map size,
		 * check if that slot actually has a valid node.
		 * If it doesn't, move all of the nodes after it in the array down a slot.
		 */
		for (int i = 0; i < mapSize; i++) {	// for each slot in array
			if ((room[i] == null) || (room[i].index < 0)) {	// check if it has a valid node
				//Debug.Log("Node with index " + i + " does not exist");
				for (int j = (i + 1); j < mapSize; j++) {	// if it isn't valid, move all nodes that come asfter it down one slot
					//Debug.Log("Moving node with index " + j + " down one");
					if ((room[j] != null) || (room[j].index >= 0)) {    // only bother moving nodes that are also valid
						if (GameManager.gameplay.currentIndex == j) {  // if the current tile is the one having its index changed, also update the current index 
							GameManager.gameplay.currentIndex = j - 1; // this prevents teleporting or other error.
						}
						/*if(GameManager.instance.gameplay.currentIndex == j) {	// if the current tile is the one having its index changed, also update the current index 
							GameManager.instance.gameplay.currentIndex = j - 1;	// this prevents teleporting or other error.
						}*/

						room[j - 1] = room[j];
						room[j - 1].index = (j - 1);
						for (int k = 0; k < mapSize; k++) { // for each node that is moved down one, go through the array and adjust all 
															// onnections to that node so that they match the new index of the node.
							// j is index of moved node
							// room[k] is node to check
							// List<ConnectionSet> connectionList is list of connections on node to check
							/*Node.ConnectionSet[] conns = room[k].connectionList.ToArray();
							foreach (Node.ConnectionSet set in conns) {
								for (int dir = 0; dir < 4; dir++) {
									if (set[(GameManager.Direction)dir] == j) {
										//Debug.Log("Found reference to node with index " + j);
										set[(GameManager.Direction)dir] = (j - 1);
									}
								}
							}*/
							for (int dir = 0; dir < 4; dir++) {
								if (room[k].defaultConn[dir] == j) {
									room[k].defaultConn[dir] = (j - 1);
								}
								if (room[k].defaultConn[dir] == j) {
									room[k].defaultConn[dir] = (j - 1);
								}
							}

						}
					}
				}
				// if a node is removed, reduce map size by one.
				mapSize--;
			} else {    // if a node is valid, clean it up
				room[i].PopConnectionStack();
			}
		}
		// make a new array that exactly fits the number of nodes actually in map, move nodes into it, and set the map's array to the new array
		Node[] tempNodes = new Node[mapSize];
		for (int n = 0; n < mapSize; n++) {
			tempNodes[n] = room[n];
		}
		room.setNodes(tempNodes);	// , mapSize, mapSize);
	}


	public static void setCornerDrawing(LevelMap room) {
		Node[] clockwiseNode;
		Node[] counterwiseNode;
		Node tempNodeCW;
		Node tempNodeCCW;
		GameManager.Direction tempDirCW;
		GameManager.Direction tempDirCCW;
		for (int i = 0; i < room.size; i++) {
			for (int j = 0; j < 4; j++) {
				room[i].drawCorner[j] = false;
			}
			for (int j = 0; j < 4; j++) {
				if ((room[i])[j] < 0) {
					room[i].drawCorner[j] = true;
					room[i].drawCorner[(j+3)%4] = true;
				}
				// now in each of the given directions, it checks the nodes that can be reached clockwise and counter cockwise in each direction. 
				// If nodes reached clockwise/counter-clockwise do not match or are null, the corner in that direction is set visible, otherwise that corner is set non-visible.
				clockwiseNode = new Node[4];
				counterwiseNode = new Node[4];
				tempNodeCW = room[i];
				tempNodeCCW = room[i];
				tempDirCW = (GameManager.Direction)j;
				tempDirCCW = Extensions.clockwise((GameManager.Direction)j);
				for (int k = 0; k < 4; k++) {
					clockwiseNode[k] = null;
					counterwiseNode[(6 - k) % 4] = null;
					if (tempNodeCW != null) {
						tempNodeCW = room[(int)tempNodeCW.defaultConn[(int)tempDirCW]];
						tempDirCW = Extensions.clockwise(tempDirCW);
						clockwiseNode[k] = tempNodeCW;
					}
					if (tempNodeCCW != null) {
						tempNodeCCW = room[(int)tempNodeCCW.defaultConn[(int)tempDirCCW]];
						tempDirCCW = Extensions.counterclockwise(tempDirCCW);
						counterwiseNode[(6 - k) % 4] = tempNodeCCW;
					}
				}
				for (int k = 0; k < 4; k++) {
					if ((clockwiseNode[k] == null) || (counterwiseNode[k] == null) || (clockwiseNode[k] != counterwiseNode[k])) {
						room[i].drawCorner[j] = true;
						break;
					}
				}
			}
		}
		for (int i = 0; i < room.size; i++) {
			for (int j = 0; j < 4; j++) {
				int otherIndex = room[i].defaultConn[j];
				int otherDir = (j + 2) % 4;
				if ((otherIndex >= 0) && (room[otherIndex].defaultConn[otherDir] != i)) {
					room[otherIndex].drawCorner[otherDir] = true;
					room[otherIndex].drawCorner[(otherDir + 3) % 4] = true;
				}
			}
		}
		for (int i = 0; i < room.size; i++) {
			for (int j = 0; j < 4; j++) {
				if (room[i].drawCorner[j]) { 
					tempNodeCW = room[i];
					tempNodeCCW = room[i];
					tempDirCW = (GameManager.Direction)j;
					tempDirCCW = Extensions.clockwise((GameManager.Direction)j);
					while (tempNodeCW != null && tempNodeCCW != null) {
						if (tempNodeCW != null) {
							tempNodeCW.drawCorner[(int)tempDirCW] = true;
							tempNodeCW = room[(int)tempNodeCW.defaultConn[(int)tempDirCW]];
							tempDirCW = Extensions.clockwise(tempDirCW);
							if (tempNodeCW != null && tempNodeCW.index == i) {
								tempNodeCW = null;
							}
						}
						if (tempNodeCCW != null) {
							tempNodeCCW.drawCorner[((int)tempDirCCW + 3) % 4] = true;
							tempNodeCCW = room[(int)tempNodeCCW.defaultConn[(int)tempDirCCW]];
							tempDirCCW = Extensions.counterclockwise(tempDirCCW);
							if (tempNodeCCW != null && tempNodeCCW.index == i) {
								tempNodeCCW = null;
							}
						}
					}
				}
			}
		}
	}
}
