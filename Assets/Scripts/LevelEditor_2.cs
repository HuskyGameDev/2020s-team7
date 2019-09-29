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
	public enum Direction {North, East, South, West};

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
	public static Node[,] createChunk(Map room, Color32 color, int width, int height, List<TileCoord> emptyTiles = null, List<TileCoord> tileWalls = null) {
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
						chunk[i, j] = new Node(index, color, GameManager.instance.spriteBook[0]);
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
					chunk[i, j] = new Node(index, color, GameManager.instance.spriteBook[0]);
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
						chunk[i, j].connections.north = chunk[i, j - 1].index;
						//Debug.Log("Connecting tile \"" + chunk[i, j].index + "\" [" + i + ", " + j + "] to \"" + chunk[i, j - 1].index + "\" [" + i + ", " + (j - 1) + "]");
					}

					// connect to southern tile
					if (((j + 1) < height) && (chunk[i, j + 1]) != null) {  // if node below if out-of-bounds, or empty, do not link to it
						chunk[i, j].connections.south = chunk[i, j + 1].index;
						//Debug.Log("Connecting tile \"" + chunk[i, j].index + "\" [" + i + ", " + j + "] to \"" + chunk[i, j + 1].index + "\" [" + i + ", " + (j + 1) + "]");
					}

					// connect to western tile
					if (((i - 1) >= 0) && (chunk[i - 1, j] != null)) {  // if node to left if out-of-bounds, or empty, do not link to it
						chunk[i, j].connections.west = chunk[i - 1, j].index;
						//Debug.Log("Connecting tile \"" + chunk[i, j].index + "\" [" + i + ", " + j + "] to \"" + chunk[i - 1, j].index + "\" [" + (i - 1) + ", " + j + "]");
					}

					// connect to eastern tile
					if (((i + 1) < width) && (chunk[i + 1, j] != null)) {  // if node to right if out-of-bounds, or empty, do not link to it
						chunk[i, j].connections.east = chunk[i + 1, j].index;
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
					if (tile.North == true) {	// check if north wall
						//chunk[tile.x, tile.y].connections.north = null;
						chunk[tile.x, tile.y].connections.north = -1;
					}

					if (tile.South == true) {   // check if south wall
						//chunk[tile.x, tile.y].connections.south = null;
						chunk[tile.x, tile.y].connections.south = -1;
					}

					if (tile.East == true) {   // check if East wall
						//chunk[tile.x, tile.y].connections.east = null;
						chunk[tile.x, tile.y].connections.east = -1;
					}

					if (tile.West == true) {   // check if west wall
						//chunk[tile.x, tile.y].connections.west = null;
						chunk[tile.x, tile.y].connections.west = -1;
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
	public static void createOneWayLink(Node[,] chunkFrom, Node[,] chunkTo, TileCoord tileFrom, TileCoord tileTo, Direction dir) {
		if (dir == Direction.North) {
			chunkFrom[tileFrom.x, tileFrom.y].connections.north = chunkTo[tileTo.x, tileTo.y].index;
		} else if (dir == Direction.East) {
			chunkFrom[tileFrom.x, tileFrom.y].connections.east = chunkTo[tileTo.x, tileTo.y].index;
		} else if (dir == Direction.South) {
			chunkFrom[tileFrom.x, tileFrom.y].connections.south = chunkTo[tileTo.x, tileTo.y].index;
		} else {
			chunkFrom[tileFrom.x, tileFrom.y].connections.west = chunkTo[tileTo.x, tileTo.y].index;
		}
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
	public static void createTwoWayLink(Node[,] chunkFrom, Node[,] chunkTo, TileCoord tileFrom, TileCoord tileTo, Direction dir) {
		if (dir == Direction.North) {
			chunkFrom[tileFrom.x, tileFrom.y].connections.north = chunkTo[tileTo.x, tileTo.y].index;
			chunkTo[tileTo.x, tileTo.y].connections.south = chunkFrom[tileFrom.x, tileFrom.y].index;
		} else if (dir == Direction.East) {
			chunkFrom[tileFrom.x, tileFrom.y].connections.east = chunkTo[tileTo.x, tileTo.y].index;
			chunkTo[tileTo.x, tileTo.y].connections.west = chunkFrom[tileFrom.x, tileFrom.y].index;
		} else if (dir == Direction.South) {
			chunkFrom[tileFrom.x, tileFrom.y].connections.south = chunkTo[tileTo.x, tileTo.y].index;
			chunkTo[tileTo.x, tileTo.y].connections.north = chunkFrom[tileFrom.x, tileFrom.y].index;
		} else {
			chunkFrom[tileFrom.x, tileFrom.y].connections.west = chunkTo[tileTo.x, tileTo.y].index;
			chunkTo[tileTo.x, tileTo.y].connections.east = chunkFrom[tileFrom.x, tileFrom.y].index;
		}
	}

	/// <summary>
	/// Sets the source tile for a map/room. This is where the player starts.
	/// Takes the chunk that the desired tile is in, and the coordinates of the tile within the chunk
	/// </summary>
	/// <param name="chunk"></param>
	/// <param name="tile"></param>
	public static void setSource(Map room, Node[,] chunk, TileCoord tile) {
		chunk[tile.x, tile.y].data.type = Node.LineData.TileType.source;
		GameManager.instance.currentPosition = chunk[tile.x, tile.y];
		chunk[tile.x, tile.y].floorSprite = GameManager.instance.spriteBook[1];
		//room.sourceNode = chunk[tile.x, tile.y];
		//Debug.Log("Setting");
		room.sourceNodeIndex = chunk[tile.x, tile.y].index;
	}

	/// <summary>
	/// Sets the target tile for a map/room. This is where the player needs to draw their line to.
	/// Takes the chunk that the desired tile is in, and the coordinates of the tile within the chunk
	/// </summary>
	/// <param name="chunk"></param>
	/// <param name="tile"></param>
	public static void setTarget(Map room, Node[,] chunk, TileCoord tile) {
		chunk[tile.x, tile.y].data.type = Node.LineData.TileType.target;
		chunk[tile.x, tile.y].floorSprite = GameManager.instance.spriteBook[2];
		//room.targetNode = chunk[tile.x, tile.y];

		room.targetNodeIndex = chunk[tile.x, tile.y].index;
	}
    /*
    public static void SetObject(GameObject obj, TileCoord tile)
    {
        tile.myObject = obj;
    }*/
}
