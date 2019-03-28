using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelEditor_2 {

	public class TileCoord {


		public TileCoord (int x, int y) {
			this.x = x;
			this.y = y;
		}

		public TileCoord(int x, int y, bool north, bool east, bool south, bool west) {
			this.x = x;
			this.y = y;
			this.North = north;
			this.East = east;
			this.South = south;
			this.West = west;
		}

		public int x;
		public int y;

		public bool North;
		public bool East;
		public bool South;
		public bool West;

        public GameObject myObject;
	}

	/*
	public static Map createRoom() {
		return new Map();
	}*/

	public static Node[,] createChunk(Map room, Color32 color, int width, int height, List<TileCoord> emptyTiles = null, List<TileCoord> tileWalls = null) {
		Node[,] chunk = new Node[width,height];

		if (null != emptyTiles) {	// do stuff to create chunk with empty tiles
			IEnumerator<TileCoord> emptyIterator = emptyTiles.GetEnumerator();
			// create tiles only where tiles are not indicated as supossed to be empty
			//Debug.Log("Empty tile list exists");
			for (int i = 0; i < width; i++) {
				for (int j = 0; j < height; j++) {
					bool valid = true;  // if a tile should be created for this square
					bool notDone1 = true;   // indicates that not all listed empty tiles have been checked to see if the match the coordinates of the current tile

					while (valid && notDone1) {
						notDone1 = emptyIterator.MoveNext();
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
						chunk[i, j] = new Node(index, color);
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
					chunk[i, j] = new Node(index, color);
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
					if (((j + 1) < height) && (chunk[i, j + 1]) != null) {  // if node above if out-of-bounds, or empty, do not link to it
						chunk[i, j].connections.south = chunk[i, j + 1].index;
						//Debug.Log("Connecting tile \"" + chunk[i, j].index + "\" [" + i + ", " + j + "] to \"" + chunk[i, j + 1].index + "\" [" + i + ", " + (j + 1) + "]");
					}

					// connect to western tile
					if (((i - 1) >= 0) && (chunk[i - 1, j] != null)) {  // if node above if out-of-bounds, or empty, do not link to it
						chunk[i, j].connections.west = chunk[i - 1, j].index;
						//Debug.Log("Connecting tile \"" + chunk[i, j].index + "\" [" + i + ", " + j + "] to \"" + chunk[i - 1, j].index + "\" [" + (i - 1) + ", " + j + "]");
					}

					// connect to eastern tile
					if (((i + 1) < width) && (chunk[i + 1, j] != null)) {  // if node above if out-of-bounds, or empty, do not link to it
						chunk[i, j].connections.east = chunk[i + 1, j].index;
						//Debug.Log("Connecting tile \"" + chunk[i, j].index + "\" [" + i + ", " + j + "] to \"" + chunk[i + 1, j].index + "\" [" + (i + 1) + ", " + j + "]");
					}
				}
			}
		}

		// do walls 
		if (null != tileWalls) {
			IEnumerator<TileCoord> wallIterator = tileWalls.GetEnumerator();
			bool notDone2 = true;
			while (notDone2) {
				TileCoord tile = wallIterator.Current;
				if (null != tile) {
					if (tile.North == true) {	// check if north wall
						chunk[tile.x, tile.y].connections.north = null;
					}

					if (tile.South == true) {   // check if south wall
						chunk[tile.x, tile.y].connections.south = null;
					}

					if (tile.East == true) {   // check if East wall
						chunk[tile.x, tile.y].connections.east = null;
					}

					if (tile.West == true) {   // check if west wall
						chunk[tile.x, tile.y].connections.west = null;
					}
				}
				notDone2 = wallIterator.MoveNext();
			}
		}

		return chunk;
	}

	public static void createOneWayLink(Node[,] chunkFrom, Node[,] chunkTo, TileCoord tileFrom, TileCoord tileTo) {
		if (tileFrom.North == true) {
			chunkFrom[tileFrom.x, tileFrom.y].connections.north = chunkTo[tileTo.x, tileTo.y].index;
		} else if (tileFrom.East == true) {
			chunkFrom[tileFrom.x, tileFrom.y].connections.east = chunkTo[tileTo.x, tileTo.y].index;
		} else if (tileFrom.South == true) {
			chunkFrom[tileFrom.x, tileFrom.y].connections.south = chunkTo[tileTo.x, tileTo.y].index;
		} else if (tileFrom.West == true) {
			chunkFrom[tileFrom.x, tileFrom.y].connections.west = chunkTo[tileTo.x, tileTo.y].index;
		}
		
	}

	public static void createTwoWayLink(Node[,] chunk1, Node[,] chunk2, TileCoord tile1, TileCoord tile2) {
		if (tile1.North == true) {
			chunk1[tile1.x, tile1.y].connections.north = chunk2[tile2.x, tile2.y].index;
		} else if (tile1.East == true) {
			chunk1[tile1.x, tile1.y].connections.east = chunk2[tile2.x, tile2.y].index;
		} else if (tile1.South == true) {
			chunk1[tile1.x, tile1.y].connections.south = chunk2[tile2.x, tile2.y].index;
		} else if (tile1.West == true) {
			chunk1[tile1.x, tile1.y].connections.west = chunk2[tile2.x, tile2.y].index;
		}

		if (tile2.North == true) {
			chunk2[tile2.x, tile2.y].connections.north = chunk1[tile1.x, tile1.y].index;
		} else if (tile2.East == true) {
			chunk2[tile2.x, tile2.y].connections.east = chunk1[tile1.x, tile1.y].index;
		} else if (tile2.South == true) {
			chunk2[tile2.x, tile2.y].connections.south = chunk1[tile1.x, tile1.y].index;
		} else if (tile2.West == true) {
			chunk2[tile2.x, tile2.y].connections.west = chunk1[tile1.x, tile1.y].index;
		}
	}

	//currentPosition = map[0];
	public static void setSource(Node[,] chunk, TileCoord tile) {
		chunk[tile.x, tile.y].data.type = Node.LineData.TileType.source;
		GameManager.instance.currentPosition = chunk[tile.x, tile.y];
		chunk[tile.x, tile.y].color = new Color32(255, 0, 255, 255);
	}


	public static void setTarget(Node[,] chunk, TileCoord tile) {
		chunk[tile.x, tile.y].data.type = Node.LineData.TileType.target;
		chunk[tile.x, tile.y].color = new Color32(255, 0, 255, 255);
	}
    /*
    public static void SetObject(GameObject obj, TileCoord tile)
    {
        tile.myObject = obj;
    }*/
}
