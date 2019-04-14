using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Direction = GameManager.Direction;

public class RenderingHandler : MonoBehaviour {

    public int memoryLayer;
    public int activeLayer;
	public SpriteMask mask;
    [SerializeField]
    public RenderMap renderMap = new RenderMap(0);
    public RenderMap altRenderMap = new RenderMap(0);

	public List<Node> prevVisibleNodes = new List<Node>();
    //[SerializeField] public List<List<GameObject>> renderTiles = new List<List<GameObject>>();
    //[SerializeField] public GameObject renderTiles = new GameObject[renderMap.dim,renderMap.dim];
	// Use this for initialization

    Vector2Int center;
	void Start () {
		center = new Vector2Int(renderMap.dim /2, renderMap.dim / 2);
	}

    public void HandleRender(Direction direction, Node node, bool doShift = true)
    {
        //if (doShift) ShiftGrid(direction);
        
        List<Node> visibleNodes = new List<Node>();
        //Handle the one we are standing on.
        renderMap[center.x, center.y].DrawFullNode(node);
        renderMap[center.x, center.y].SetLayer(activeLayer);
        if (visibleNodes.Contains(node) == false)
            visibleNodes.Add(node);
        

        //Handle the 4 cardinal directions
        for (int i = 0; i < 4; i++) {
            Direction dir = (Direction)i;
            Vector2Int position = center + dir.offset();
            Node drawNode = (node.GetConnectionFromDir(dir) == null) ? null : GameManager.instance.map[(int)node.GetConnectionFromDir(dir)];
            renderMap[position.x, position.y].DrawFullNode(drawNode);
            if (drawNode != null && visibleNodes.Contains(drawNode) == false)
                visibleNodes.Add(drawNode);

            if (node.GetConnectionFromDir(dir) != null)
                renderMap[position.x, position.y].SetLayer(activeLayer);
        }

        //Blank out the diagnols (these will be rendred again)
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 4; j++) {
                if (i==j) 
                    continue;
                else {
                    Direction primaryDir = (Direction)i;
                    Direction secondaryDir = (Direction)j;
                    //We dont want to handle the inverse Scenario (this was already renderd)
                    if (primaryDir.inverse() == secondaryDir)
                        continue;

                    //Get the diagnol node
                    Vector2Int position = center + primaryDir.offset() + secondaryDir.offset();
                    //Draw the two relevent triangles
                    renderMap[position.x,position.y].SetFloorFromDir(Direction.North, null);
                    renderMap[position.x,position.y].SetFloorFromDir(Direction.South, null);
                    renderMap[position.x,position.y].SetFloorFromDir(Direction.West, null);
                    renderMap[position.x,position.y].SetFloorFromDir(Direction.East, null);
                    renderMap[position.x,position.y].SetWallFromDir(Direction.North, false);
                    renderMap[position.x,position.y].SetWallFromDir(Direction.South, false);
                    renderMap[position.x,position.y].SetWallFromDir(Direction.West, false);
                    renderMap[position.x,position.y].SetWallFromDir(Direction.East, false);

                }
            }
        }

        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 4; j++) {
                if (i==j) {
                    Direction dir = (Direction)i;
                    Vector2Int position = center + dir.offset() + dir.offset();
                    if (node.GetConnectionFromDir(dir) != null) {
                        Node primaryNode = GameManager.instance.map[(int)node.GetConnectionFromDir(dir)];
                        if (primaryNode != null) {
                            Node drawNode = (primaryNode.GetConnectionFromDir(dir) == null) ? null : GameManager.instance.map[(int)primaryNode.GetConnectionFromDir(dir)];
                            renderMap[position.x, position.y].DrawFullNode(drawNode);
                            renderMap[position.x, position.y].SetLayer(activeLayer);
                        }
                    }
                }
                else {
                    Direction primaryDir = (Direction)i;
                    Direction secondaryDir = (Direction)j;
                    //We dont want to handle the inverse Scenario (this was already renderd)
                    if (primaryDir.inverse() == secondaryDir)
                        continue;
                    //Dont render if that side doesnt exist
                    if (node.GetConnectionFromDir(primaryDir) == null)
                        continue;

                    //Get the node offset from center
                    Node primaryNode = GameManager.instance.map[(int)node.GetConnectionFromDir(primaryDir)];

                    //If there is nothing on the second direction, we are not rendering this tile from this perspective
                    if (primaryNode.GetConnectionFromDir(secondaryDir) == null)
                        continue;


                    {
                        List<Node> deepAdd = primaryNode.GetFullStackConnectionsFromDir(secondaryDir);
                        foreach (Node n in deepAdd) {
                            if (visibleNodes.Contains(n) == false)
                                visibleNodes.Add(n);
                        }
                    }

                    //Get the diagnol node
                    Vector2Int position = center + primaryDir.offset() + secondaryDir.offset();
                    Node diagnolNode = GameManager.instance.map[(int)primaryNode.GetConnectionFromDir(secondaryDir)];
                    //Draw the two relevent triangles
                    renderMap[position.x,position.y].SetFloorFromDir(primaryDir, diagnolNode);


                    if (diagnolNode.data.enter == primaryDir && diagnolNode.data.hasEnter)
                        renderMap[position.x,position.y].SetLineFromDir(primaryDir,true );
                    if (diagnolNode.data.leave == primaryDir && diagnolNode.data.hasLeave)
                        renderMap[position.x,position.y].SetLineFromDir(primaryDir,true );


                    renderMap[position.x,position.y].SetFloorFromDir(secondaryDir.inverse(), diagnolNode);
                    if (diagnolNode.data.enter == secondaryDir.inverse() && diagnolNode.data.hasEnter)
                        renderMap[position.x,position.y].SetLineFromDir(secondaryDir.inverse(),true );
                    if (diagnolNode.data.leave == secondaryDir.inverse() && diagnolNode.data.hasLeave)
                        renderMap[position.x,position.y].SetLineFromDir(secondaryDir.inverse(),true );

                    //Put this on the right layer
                    renderMap[position.x,position.y].SetLayer(activeLayer);


                    //Check primaryDir from diagnol to see if we need a wall
                    if (diagnolNode.GetConnectionFromDir(primaryDir) == null) {
                        //Debug.Log("Wall: " +(primaryDir)+"->" +(secondaryDir)+"->"+(primaryDir));
                        renderMap[position.x,position.y].SetWallFromDir(primaryDir, true);
                    }

                }

            }
        }

        //Handle the popping of things leaving view
        //Remove all things currently visible from the previously visible
        foreach (Node item in visibleNodes)
        {
            while (prevVisibleNodes.Contains(item)) {
                prevVisibleNodes.Remove(item);
            }
        }
        //Then go through what remains in previously visible
        foreach (Node item in prevVisibleNodes) {
            //This method will reduce it down the the bottom connection stack
            item.PopConnectionStack();
        }
        prevVisibleNodes = visibleNodes;
    }


    public void ShiftGrid(GameManager.Direction direction) {
        for (int x = 0; x < renderMap.dim; x++)
        {
            for (int y = 0; y < renderMap.dim; y++)
            {
                for (int i = 0; i < 4; i++)
                    renderMap[x, y].SetLineFromDir((Direction)i, false);
                renderMap[x, y].SetLayer(memoryLayer);
            }
        }
        switch (direction)
        {
            case GameManager.Direction.North:
                for (int x = 0; x < renderMap.dim; x++)
                {
                    for (int y = 0; y < renderMap.dim-1; y++) // Dont do final row since it doesnt have anything after it
                    {
                        renderMap[x, y].CopyState(renderMap[x,y+1]);
                    }
                }
                //Handle blanking out the last row out
                for (int x = 0; x < renderMap.dim; x++)
                {
                    renderMap[x, renderMap.dim-1].DrawFullNode(null);
                }
                break;
            case GameManager.Direction.East:
                for (int x = 0; x < renderMap.dim-1; x++)
                {
                    for (int y = 0; y < renderMap.dim; y++) // Dont do final row since it doesnt have anything after it
                    {
                        renderMap[x, y].CopyState(renderMap[x + 1, y]);
                    }
                }
                //Handle blanking out the last row out
                for (int y = 0; y < renderMap.dim; y++)
                {
                    renderMap[renderMap.dim-1, y].DrawFullNode(null);
                }
                //Handling blanking the last row out
                break;
            case GameManager.Direction.South:
                for (int x = 0; x < renderMap.dim; x++)
                {
                    for (int y = renderMap.dim-1; y > 0; y--) // Dont do final row since it doesnt have anything after it
                    {
                        renderMap[x, y].CopyState(renderMap[x, y - 1]);
                    }
                }
                //Handling blanking the last row out
                //Handle blanking out the last row out
                for (int x = 0; x < renderMap.dim; x++)
                {
                    renderMap[x, 0].DrawFullNode(null);
                }
                break;
            case GameManager.Direction.West:
                for (int x = renderMap.dim-1; x > 0; x--)
                {
                    for (int y = 0; y < renderMap.dim; y++) // Dont do final row since it doesnt have anything after it
                    {
                        renderMap[x, y].CopyState(renderMap[x - 1, y]);
                    }
                }
                //Handling blanking the last row out
                for (int y = 0; y < renderMap.dim; y++)
                {
                    renderMap[0, y].DrawFullNode(null);
                }
                break;
            default:
                break;
        }
    }


	[System.Serializable]
    public class RenderMap
    {
        [SerializeField]
        public RenderTile[] tiles;

        public int dim;

        public RenderMap(int tileAmt) {
            this.tiles = new RenderTile[tileAmt];
        }

        public RenderTile this[int x, int y]
        {
            get { return tiles[(int)(y * dim) + x];  }
            set { tiles[(int)(y * dim) + x] = value; }
        }
    }
}
