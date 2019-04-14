using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Direction = GameManager.Direction;

public class RenderTile : MonoBehaviour {
    public SpriteRenderer northWall;
    public SpriteRenderer eastWall;
    public SpriteRenderer southWall;
    public SpriteRenderer westWall;

    public SpriteRenderer northFloor;
    public SpriteRenderer southFloor;
    public SpriteRenderer eastFloor;
    public SpriteRenderer westFloor;

    public SpriteRenderer northLine;
    public SpriteRenderer southLine;
    public SpriteRenderer eastLine;
    public SpriteRenderer westLine;

	public SpriteRenderer[] GetAllSprites { set { } get { return new SpriteRenderer[] { northWall, eastWall, southWall, westWall, northFloor, southFloor, eastFloor, westFloor, northLine, southLine, eastLine, westLine }; } }



    public void CopyState(RenderTile other)
    {
        this.gameObject.SetActive(other.gameObject.activeSelf);
        northWall.gameObject.SetActive(other.northWall.gameObject.activeSelf);
        eastWall.gameObject.SetActive(other.eastWall.gameObject.activeSelf);
        southWall.gameObject.SetActive(other.southWall.gameObject.activeSelf);
        westWall.gameObject.SetActive(other.westWall.gameObject.activeSelf);
    }

    public void SetLayer(int layer)
    {
        this.gameObject.layer = layer;
        northWall.gameObject.layer = layer;
        southWall.gameObject.layer = layer;
        eastWall.gameObject.layer = layer;
        westWall.gameObject.layer = layer;
        northFloor.gameObject.layer = layer;
        southFloor.gameObject.layer = layer;
        eastFloor.gameObject.layer = layer;
        westFloor.gameObject.layer = layer;
    }

    public void DrawFullNode(Node node)
    {
        if (node == null)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            for (int i = 0; i < 4; i++)
                SetLineFromDir((Direction)i, false);

            if (node.data.hasEnter)
                SetLineFromDir((Direction)node.data.enter, true);
            if (node.data.hasLeave)
                SetLineFromDir((Direction)node.data.leave, true);

            //this.gameObject.GetComponent<SpriteRenderer>().color = node.color;
            SetFloorFromDir(Direction.North, node);
            SetFloorFromDir(Direction.South, node);
            SetFloorFromDir(Direction.East, node);
            SetFloorFromDir(Direction.West, node);
            this.gameObject.SetActive(true);   
            SetWallsFromNode(node);
        }
    }

    public void SetLineFromDir(Direction dir, bool b) {
        switch (dir) {
            case Direction.North:
            northLine.gameObject.SetActive(b);
            break;
            case Direction.South:
            southLine.gameObject.SetActive(b);
            break;
            case Direction.East:
            eastLine.gameObject.SetActive(b);
            break;
            case Direction.West:
            westLine.gameObject.SetActive(b);
            break;
        }
    }

    public void SetFloorFromDir(Direction dir, Node node) {
        if (node != null)
            this.gameObject.SetActive(true);
        switch (dir)
		{
			case GameManager.Direction.North:
                if (node == null)
                    northFloor.gameObject.SetActive(false);
	    		else {
                    northFloor.gameObject.SetActive(true);
                    northFloor.color = node.color;
                }
				break;
			case GameManager.Direction.South:
                if (node == null)
                    southFloor.gameObject.SetActive(false);
	    		else {
                    southFloor.gameObject.SetActive(true);
                    southFloor.color = node.color;
                }
				break;
			case GameManager.Direction.East:
                if (node == null)
                    eastFloor.gameObject.SetActive(false);
	    		else {
                    eastFloor.gameObject.SetActive(true);
                    eastFloor.color = node.color;
                }
				break;
			case GameManager.Direction.West:
                if (node == null)
                    westFloor.gameObject.SetActive(false);
	    		else {
                    westFloor.gameObject.SetActive(true);                   
                    westFloor.color = node.color;
                }
				break;
		}
    }
    public void SetWallFromDir(Direction dir, bool b) {
        switch (dir) {
            case Direction.North:
            northWall.gameObject.SetActive(b);
            break;
            case Direction.South:
            southWall.gameObject.SetActive(b);
            break;
            case Direction.West:
            westWall.gameObject.SetActive(b);
            break;
            case Direction.East:
            eastWall.gameObject.SetActive(b);
            break;
        }
    }

    public void SetWallsFromNode(Node node) {
            northWall.gameObject.SetActive(node.connections.north == null);
            southWall.gameObject.SetActive(node.connections.south == null);
            eastWall.gameObject.SetActive(node.connections.east == null);
            westWall.gameObject.SetActive(node.connections.west == null);        
    }
}
