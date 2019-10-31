using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Direction = GameManager.Direction;

public class RenderTile : MonoBehaviour {
	public SpriteRenderer corners;
	public SpriteRenderer northWall;
    public SpriteRenderer eastWall;
    public SpriteRenderer southWall;
    public SpriteRenderer westWall;

    public SpriteRenderer floor;

    public SpriteRenderer northLine;
    public SpriteRenderer southLine;
    public SpriteRenderer eastLine;
    public SpriteRenderer westLine;
	public SpriteRenderer lineCenter;

	private static Vector2Int center = new Vector2Int(2,2);
	//public GenericMask[] cornerMasks;
	public SpriteRenderer[] debris = new SpriteRenderer[9];

	public SpriteRenderer[] GetAllSprites {
		set { }
		get {
			return new SpriteRenderer[] {
				corners, northWall, eastWall, southWall, westWall,
				floor,
				northLine, southLine, eastLine, westLine, lineCenter,
				debris[0], debris[1], debris[2], debris[3], debris[4],
				debris[5], debris[6], debris[7], debris[8]
			};
		}
	}



    public void CopyState(RenderTile other)
    {
        this.gameObject.SetActive(other.gameObject.activeSelf);
        northWall.gameObject.SetActive(other.northWall.gameObject.activeSelf);
        eastWall.gameObject.SetActive(other.eastWall.gameObject.activeSelf);
        southWall.gameObject.SetActive(other.southWall.gameObject.activeSelf);
        westWall.gameObject.SetActive(other.westWall.gameObject.activeSelf);
    }

    public void DrawFullNode(Node node, GameManager.Direction? dir, Vector2Int? position)
    {
        if ((node == null) || (dir == null) || (position == null))
        {
            this.gameObject.SetActive(false);
        }
        else
        {
			int i;
			for (i = 0; i < 4; i++)
                SetLineFromDir((Direction)i, false);

            if (node.data.hasEnter)
                SetLineFromDir((Direction)node.data.enter, true);
            if (node.data.hasLeave)
                SetLineFromDir((Direction)node.data.leave, true);


			if (node.data.hasEnter && node.data.hasLeave && node.data.enter.inverse() == node.data.leave)
				lineCenter.gameObject.SetActive(true);
			else
				lineCenter.gameObject.SetActive(false);

			// should evaluate to an array of size 2 for tiles on the cardinal directions, 4 for the middle tile, or 1 otherwise
			// CODING IN PROGRESS HERE - UNFINISHED
			
			Direction[] checkDirection = new Direction[((position.Value.x == 2) ? 2:1) * ((position.Value.y == 2) ? 2 : 1)];
			i = 0;
			if ((position.Value.x <= 2) && (position.Value.y <= 2)) {
				checkDirection[i] = Direction.South;
				i++;
			}
			if ((position.Value.x >= 2) && (position.Value.y >= 2)) {
				checkDirection[i] = Direction.North;
				i++;
			}
			if ((position.Value.x <= 2) && (position.Value.y >= 2)) {
				checkDirection[i] = Direction.West;
				i++;
			}
			if ((position.Value.x >= 2) && (position.Value.y <= 2)) {
				checkDirection[i] = Direction.East;
			}
			Node[] clockwiseNode;
			Node[] counterwiseNode;
			Node tempNodeCW;
			Node tempNodeCCW;
			Direction tempDirCW;
			Direction tempDirCCW;
			bool draw;
			foreach (Direction d in checkDirection) {
				clockwiseNode = new Node[4];
				counterwiseNode = new Node[4];
				tempNodeCW = node;
				tempNodeCCW = node;
				draw = false;
				tempDirCW = d;
				tempDirCCW = Extensions.clockwise(d);
				for (i = 0; i < 4; i++) {
					if (tempNodeCW != null) {
						tempNodeCW = GameManager.instance.gameplay.map[(int)tempNodeCW.GetConnectionFromDir(tempDirCW)];
						tempDirCW = Extensions.clockwise(tempDirCW);
						clockwiseNode[i] = tempNodeCW;
					}
					if (tempNodeCCW != null) {
						tempNodeCCW = GameManager.instance.gameplay.map[(int)tempNodeCCW.GetConnectionFromDir(tempDirCCW)];
						tempDirCCW = Extensions.counterclockwise(tempDirCCW);
						counterwiseNode[(6 - i) % 4] = tempNodeCCW;
					}
				}
				for (i = 0; i < 4; i++) {
					if ((clockwiseNode[i] == null) || (counterwiseNode[i] == null) || (clockwiseNode[i] != counterwiseNode[i])) {
						draw = true;
						break;
					}
				}
				/* draw corner */
				GameManager.instance.gameplay.nonEuclidRenderer.cornerMaskMap[
					position.Value.x + ((d == Direction.North || d == Direction.East) ? 1 : 0), 
					position.Value.y + ((d == Direction.North || d == Direction.West) ? 1 : 0)
					].setCornerFromDir(draw, dir.Value);
			}

			//Update the floor sprite if this node has one.
			//if (node.floorSprite != null) floor.sprite = node.floorSprite;
			if (node.floorSprite != null) {
				Sprite fSprite = Resources.Load<Sprite>(node.floorSprite);
				if (fSprite != null) {
					floor.sprite = fSprite;
				} else {
					floor.sprite = Resources.Load<Sprite>("ErrorSprite");
				}
			}
			if (node.wallSprite != null) {
				Sprite wSprite = Resources.Load<Sprite>(node.wallSprite);
				if (wSprite != null) {
					northWall.sprite = wSprite;
					eastWall.sprite = wSprite;
					southWall.sprite = wSprite;
					westWall.sprite = wSprite;
				} else {
					wSprite = Resources.Load<Sprite>("ErrorSprite");
					northWall.sprite = wSprite;
					eastWall.sprite = wSprite;
					southWall.sprite = wSprite;
					westWall.sprite = wSprite;
				}
			}
			if (node.cornerSprite != null) {
				Sprite cSprite = Resources.Load<Sprite>(node.cornerSprite);
				if (cSprite != null) {
					corners.sprite = cSprite;
				} else {
					corners.sprite = Resources.Load<Sprite>("ErrorSprite");
				}
			}
			//Update the floor color
			floor.color = node.colorF;
			northWall.color = node.colorW;
			eastWall.color = node.colorW;
			southWall.color = node.colorW;
			westWall.color = node.colorW;
			corners.color = node.colorW;

			SetDebrisFromNode(node);

			this.gameObject.SetActive(true);   
            SetWallsFromNode(node, dir, position);
        }
    }

	public void SetDebrisFromNode(Node node) {
		for (int j = 0; j < 9; j++) {
			if (node.debris[j] != null && node.debris[j].Length > 0) {
				Sprite dSprite = Resources.Load<Sprite>(node.debris[j]);
				if (dSprite != null) {
					this.debris[j].sprite = dSprite;
				} else {
					this.debris[j].sprite = Resources.Load<Sprite>("ErrorSprite");
				}
				this.debris[j].gameObject.SetActive(true);
			} else {
				this.debris[j].gameObject.SetActive(false);
			}
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

    public void SetWallsFromNode(Node node, GameManager.Direction? dir, Vector2Int? position) {
		northWall.gameObject.SetActive(node.connections.north == -1);
		southWall.gameObject.SetActive(node.connections.south == -1);
		eastWall.gameObject.SetActive(node.connections.east == -1);
		westWall.gameObject.SetActive(node.connections.west == -1);
		if (!center.Equals(position)) {
			switch (dir) {
				case Direction.North:
					southWall.gameObject.SetActive(false);
					break;
				case Direction.East:
					westWall.gameObject.SetActive(false);
					break;
				case Direction.South:
					northWall.gameObject.SetActive(false);
					break;
				case Direction.West:
					eastWall.gameObject.SetActive(false);
					break;
			}
		}
	}
}
