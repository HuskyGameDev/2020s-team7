using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Direction = GameManager.Direction;

public class RenderingHandler : MonoBehaviour {

/*#if UNITY_EDITOR // if this is in the editor, need a reference to editLevel in order to call getCurrentNode() every time movement happens
	public EditLevel editLevel = null;
#endif*/

	[SerializeField]
    public RenderMap renderMap = new RenderMap(0);	// used to render tiles that are accessed from a vertical direction
    public RenderMap altRenderMap = new RenderMap(0);   // used to render tiles that are accessed from a horizontal direction
	public MaskMap cornerMaskMap = new MaskMap(0);	// list of masks use to mask tile corners
	public MaskMap tileMaskMap = new MaskMap(0);    // list of masks use to mask tiles

	public RenderMap[] renderLayers { get { return new RenderMap[] { renderMap, altRenderMap }; } }
	public List<Node> prevVisibleNodes = new List<Node>();  // lists nodes that where visible last time HandleRender() was called
	private List<Node> visibleNodes;	// lists nodes that are visible now

	// holds the pattern in which tiles should be accessed for rendering
	[SerializeField]
	private DrawPathInstruction[] drawPathInstructions = new DrawPathInstruction[0];

	Vector2Int center;	// indicates the (x,y) coordinates of the center. Should be (2,2), for a 5X5 grid
	
	void Start () {
		initialize();
	}

	// Use this for initialization
	public void initialize() {
		center = new Vector2Int(renderMap.dim / 2, renderMap.dim / 2);

		// drawPathInstructions does need to be hardcoded, but this algorithm makes it so only 1/4 of it needs to be written, since it is the same pattern in each of the 4 directions
		if (drawPathInstructions.Length < 4) {
			drawPathInstructions = new DrawPathInstruction[4];
			for (int i = 0; i < 4; i++) {
				Direction dir = (Direction)i;
				Direction left = dir.counterclockwise();
				Direction right = dir.clockwise();
				drawPathInstructions[i] = new DrawPathInstruction(dir, new DrawPathInstruction[] {
				new DrawPathInstruction(dir, new DrawPathInstruction[] { new DrawPathInstruction(left, new DrawPathInstruction[] { }), new DrawPathInstruction(right, new DrawPathInstruction[] { })}),
				new DrawPathInstruction(left, new DrawPathInstruction[] { new DrawPathInstruction(dir, new DrawPathInstruction[] { new DrawPathInstruction(left, new DrawPathInstruction[] { })}) }),
				new DrawPathInstruction(right, new DrawPathInstruction[] { new DrawPathInstruction(dir, new DrawPathInstruction[] { new DrawPathInstruction(right, new DrawPathInstruction[] { })}) }),
			});
			}
		}
	}


	/// <summary>
	/// Recursivly follows the drawpath instructions in order to render the tiles
	/// </summary>
	/// <param name="dpi"></param>
	/// <param name="oldPosition"></param>
	/// <param name="oldNode"></param>
	private void HandleDPI(DrawPathInstruction dpi, Vector2Int oldPosition, Node oldNode, bool grayout) {
		//If our map does not have a node in the instruction dir, we cannot render in that dir
		//if (oldNode.GetConnectionFromDir(dpi.dir) == null) {
		if (oldNode[(int)dpi.dir] < 0) {	// if this does not point to a node
			return;
		}
		

		Vector2Int position = oldPosition + dpi.dir.offset();
		RenderTile tile = renderLayers[dpi.dir.renderLayer()][position.x, position.y];
		grayout = grayout || GameManager.gameplay.map.disjoint(oldNode, dpi.dir);
		/*if (GameManager.gameplay.map.disjoint(oldNode, dpi.dir)) {
			tile.SetWallFromDir(dpi.dir, true);
			return;
			// Maybe make if gray things out???
		}*/
		Node node = GameManager.gameplay.map[oldNode[(int)dpi.dir]];
		//Draw the node, regarless of if it is null (null node is handled by the drawer)
		tile.DrawFullNode(node, dpi.dir, position, grayout);
		//If the node is not null, then we continue on with the instructions
		if (node != null) {
			//Add it to the list of drawn nodes
			if (visibleNodes.Contains(node) == false)
				visibleNodes.Add(node);
			//Follow instructions on the next node
			foreach (DrawPathInstruction nDpi in dpi.nextInstructions)
				HandleDPI(nDpi, position, node, grayout);
		}
	}

	/// <summary>
	/// Does all of the setup in order to draw/render all of the tiles that should be visible from the current location
	/// </summary>
	/// <param name="direction"></param>
	/// <param name="node"></param>
	/// <param name="doShift"></param>
	public void HandleRender(Direction direction, Node node, bool doShift = true) {
		//Blank out all render tiles
		foreach (RenderTile t in renderMap)
			t.DrawFullNode(null, null, null);
		foreach (RenderTile t in altRenderMap)
			t.DrawFullNode(null, null, null);
		for (int x = 0; x < renderMap.dim + 1; x++) {
			for (int y = 0; y < renderMap.dim + 1; y++) {
				GameManager.gameplay.nonEuclidRenderer.cornerMaskMap[x, y].setCornerFromDir(false, Direction.North);
				GameManager.gameplay.nonEuclidRenderer.cornerMaskMap[x, y].setCornerFromDir(false, Direction.East);
			}
		}


		//Track a list of all map nodes we render this cycle
		visibleNodes = new List<Node>();

        //Handle the render tile we are standing on (always layer 1)
        renderMap[center.x, center.y].DrawFullNode(node, Direction.North, center);
        if (visibleNodes.Contains(node) == false)
            visibleNodes.Add(node);
 
		//Follow all drawing paths
		foreach (DrawPathInstruction dpi in drawPathInstructions) {
			HandleDPI(dpi, center, node, false);
		}


        //Remove all currently seen nodes from perviously seen nodes
        foreach (Node item in visibleNodes)
        {
            while (prevVisibleNodes.Contains(item)) {
                prevVisibleNodes.Remove(item);
            }
        }
        //Then go through what remains in previously visible and pop connection stack since we no longer need fake a connection
        foreach (Node item in prevVisibleNodes) {
            //This method will reduce it down the the bottom connection stack
            item.PopConnectionStack();
        }
        prevVisibleNodes = visibleNodes;
    }

	#region Oldcode "public void ShiftGrid(GameManager.Direction direction) {...}"
	/*
	public void ShiftGrid(GameManager.Direction direction) {
		RenderMap[] maps = new RenderMap[] { renderMap, altRenderMap };
		foreach (RenderMap rM in maps) {
			for (int x = 0; x < rM.dim; x++) {
				for (int y = 0; y < rM.dim; y++) {
					for (int i = 0; i < 4; i++)
						rM[x, y].SetLineFromDir((Direction)i, false);
				}
			}
			switch (direction) {
				case GameManager.Direction.North:
					for (int x = 0; x < rM.dim; x++) {
						for (int y = 0; y < rM.dim - 1; y++) // Dont do final row since it doesnt have anything after it
						{
							rM[x, y].CopyState(rM[x, y + 1]);
						}
					}
					//Handle blanking out the last row out
					for (int x = 0; x < rM.dim; x++) {
						rM[x, rM.dim - 1].DrawFullNode(null);
					}
					break;
				case GameManager.Direction.East:
					for (int x = 0; x < rM.dim - 1; x++) {
						for (int y = 0; y < rM.dim; y++) // Dont do final row since it doesnt have anything after it
						{
							rM[x, y].CopyState(rM[x + 1, y]);
						}
					}
					//Handle blanking out the last row out
					for (int y = 0; y < rM.dim; y++) {
						rM[rM.dim - 1, y].DrawFullNode(null);
					}
					//Handling blanking the last row out
					break;
				case GameManager.Direction.South:
					for (int x = 0; x < rM.dim; x++) {
						for (int y = rM.dim - 1; y > 0; y--) // Dont do final row since it doesnt have anything after it
						{
							rM[x, y].CopyState(rM[x, y - 1]);
						}
					}
					//Handling blanking the last row out
					//Handle blanking out the last row out
					for (int x = 0; x < rM.dim; x++) {
						rM[x, 0].DrawFullNode(null);
					}
					break;
				case GameManager.Direction.West:
					for (int x = rM.dim - 1; x > 0; x--) {
						for (int y = 0; y < rM.dim; y++) // Dont do final row since it doesnt have anything after it
						{
							rM[x, y].CopyState(rM[x - 1, y]);
						}
					}
					//Handling blanking the last row out
					for (int y = 0; y < rM.dim; y++) {
						rM[0, y].DrawFullNode(null);
					}
					break;
				default:
					break;
			}
		}
    }
	*/
	#endregion
	
		/// <summary>
		/// Holds information used in the draw paths
		/// </summary>
	//[System.Serializable]
	private struct DrawPathInstruction {
		public Direction dir;
		//public int renderLayer;
		public DrawPathInstruction[] nextInstructions;
		public DrawPathInstruction(Direction dir, DrawPathInstruction[] nextInstructions) {
			this.dir = dir;
			//this.renderLayer = renderLayer;
			this.nextInstructions = nextInstructions;
		}
	}

	/// <summary>
	/// holds an array of render-tiles
	/// </summary>
	[System.Serializable]
    public class RenderMap : IEnumerable
    {
        [SerializeField]
        public RenderTile[] tiles;

        public int dim;

        public RenderMap(int dim) {
            this.tiles = new RenderTile[dim * dim];
			this.dim = dim;
        }

        public RenderTile this[int x, int y]
        {
            get { return tiles[(int)(y * dim) + x];  }
            set { tiles[(int)(y * dim) + x] = value; }
        }

		public IEnumerator GetEnumerator() {
			return tiles.GetEnumerator();
		}
	}

	/// <summary>
	/// holds an array of masks, for either tilesor tile corners
	/// </summary>
	[System.Serializable]
	public class MaskMap : IEnumerable {
		[SerializeField]
		public GenericMask[] masks;

		public int dim;

		public MaskMap(int dim) {
			if (dim > 0) {
				this.masks = new GenericMask[(dim) * (dim)];
				this.dim = dim;
			} else {
				this.masks = new GenericMask[0];
				this.dim = 0;
			}
		}

		public GenericMask this[int x, int y] {
			get { return masks[(int)(y * dim) + x]; }
			set { masks[(int)(y * dim) + x] = value; }
		}

		public IEnumerator GetEnumerator() {
			return masks.GetEnumerator();
		}
	}
}
