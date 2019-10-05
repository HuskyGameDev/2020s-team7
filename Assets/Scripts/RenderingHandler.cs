using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Direction = GameManager.Direction;

public class RenderingHandler : MonoBehaviour {

    //public int memoryLayer;
    //public int activeLayer;
	//public SpriteMask mask;
    [SerializeField]
    public RenderMap renderMap = new RenderMap(0);
    public RenderMap altRenderMap = new RenderMap(0);
	public MaskMap cornerMaskMap = new MaskMap(0);
	//public MaskMap altCornerMaskMap = new MaskMap(0);
	public MaskMap tileMaskMap = new MaskMap(0);
	//public MaskMap altTileMaskMap = new MaskMap(0);

	public RenderMap[] renderLayers { get { return new RenderMap[] { renderMap, altRenderMap }; } }
	//public MaskMap[] cornerMaskLayers { get { return new MaskMap[] { cornerMaskMap, altCornerMaskMap }; } }
	//public MaskMap[] tileMaskLayers { get { return new MaskMap[] { tileMaskMap, altTileMaskMap }; } }
	public List<Node> prevVisibleNodes = new List<Node>();
	private List<Node> visibleNodes;
	//[SerializeField] public List<List<GameObject>> renderTiles = new List<List<GameObject>>();
	//[SerializeField] public GameObject renderTiles = new GameObject[renderMap.dim,renderMap.dim];
	// Use this for initialization

	[SerializeField]
	private DrawPathInstruction[] drawPathInstructions = new DrawPathInstruction[0];
	//public DrawPathInstruction[] drawPathInstructions;

	Vector2Int center;
	void Start () {
		initialize();
	}

	public void initialize() {
		center = new Vector2Int(renderMap.dim / 2, renderMap.dim / 2);

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
	/// 
	/// </summary>
	/// <param name="dpi"></param>
	/// <param name="oldPosition"></param>
	/// <param name="oldNode"></param>
	private void HandleDPI(DrawPathInstruction dpi, Vector2Int oldPosition, Node oldNode) {
		//If our map does not have a node in the instruction dir, we cannot render in that dir
		//if (oldNode.GetConnectionFromDir(dpi.dir) == null) {
		if (oldNode.GetConnectionFromDir(dpi.dir) < 0) {
			return;
		}

		Vector2Int position = oldPosition + dpi.dir.offset();
		RenderTile tile = renderLayers[dpi.dir.renderLayer()][position.x, position.y];
		Node node = GameManager.instance.gameplay.map[(int)oldNode.GetConnectionFromDir(dpi.dir)];
		//Draw the node, regarless of if it is null (null node is handled by the drawer)
		tile.DrawFullNode(node);
		//If the node is not null, then we continue on with the instructions
		if (node != null) {
			//Add it to the list of drawn nodes
			if (visibleNodes.Contains(node) == false)
				visibleNodes.Add(node);
			//Follow instructions on the next node
			foreach (DrawPathInstruction nDpi in dpi.nextInstructions)
				HandleDPI(nDpi, position, node);
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="direction"></param>
	/// <param name="node"></param>
	/// <param name="doShift"></param>
	public void HandleRender(Direction direction, Node node, bool doShift = true)
    {
		//if (doShift) ShiftGrid(direction);
		//Blank out all render tiles
		foreach (RenderTile t in renderMap)
			t.DrawFullNode(null);
		foreach (RenderTile t in altRenderMap)
			t.DrawFullNode(null);

		//Track a list of all map nodes we render this cycle
		visibleNodes = new List<Node>();

        //Handle the render tile we are standing on (always layer 1)
        renderMap[center.x, center.y].DrawFullNode(node);
        if (visibleNodes.Contains(node) == false)
            visibleNodes.Add(node);
 
		//Follow all drawing paths
		foreach (DrawPathInstruction dpi in drawPathInstructions) {
			HandleDPI(dpi, center, node);
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
