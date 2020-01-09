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

	public void DrawFullNode(Node node, GameManager.Direction? dir, Vector2Int? position) {
		if ((node == null) || (dir == null) || (position == null)) {    // set everything invisible if any of these are null
			this.gameObject.SetActive(false);
		} else {
			// otherwise, draw the stuff as appropriate
			int i;
			for (i = 0; i < 4; i++)
                SetLineFromDir((Direction)i, false);

            if (node.data.hasEnter)	// set line/string visible as appropriate
                SetLineFromDir((Direction)node.data.enter, true);
            if (node.data.hasLeave)
                SetLineFromDir((Direction)node.data.leave, true);

				// set line/string center visible as appropriate
			if (node.data.hasEnter && node.data.hasLeave && node.data.enter.inverse() == node.data.leave)
				lineCenter.gameObject.SetActive(true);
			else
				lineCenter.gameObject.SetActive(false);

			// should evaluate to an array of size 2 for tiles on the cardinal directions, 4 for the middle tile, or 1 otherwise
			Direction[] checkDirection = new Direction[((position.Value.x == 2) ? 2:1) * ((position.Value.y == 2) ? 2 : 1)];

			// add the directions that need to be checked to the array
			// Directions to be check are based on tile location in relation to the center tile
			// North correspondes to upper-right, east to lower-right, south to lower-left, and west to upper-left.
			//    Center tile is checked in upper-right, lower-right, lower-left, upper-right directions,
			//    Tile on the cardinal directions are checked in the two directions pointeed away from the center tile, 
			//    and all other tile are checked in the main direction that points away from the center tile
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
			// now in each of the given directions, it checks the nodes that can be reached clockwise and counter cockwise in each direction. 
			// If nodes reached clockwise/counter-clockwise do not match or are null, the corner in that direction is set visible, otherwise that corner is set non-visible.
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
			//load errorsprite if floor sprite is invalid.
			if (node.floorSprite != null) {
				Sprite fSprite = Resources.Load<Sprite>("Floor_" + node.floorSprite);
				if (fSprite != null) {
					floor.sprite = fSprite;
				} else {
					Debug.Log("Error: sprite \"Floor_<" + node.wallSprite + ">\" not found");
					floor.sprite = Resources.Load<Sprite>("ErrorSprite");
				}
			}
			//Update the wall sprites if this node has one.
			//load errorsprite if wall sprite is invalid.

			if (node.wallSprite != null) {
				/*
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
				}*/
				Sprite wSprite = Resources.Load<Sprite>("Wall_" + node.wallSprite + "_N");
				if (wSprite != null) {
					northWall.sprite = wSprite;
				} else {
					Debug.Log("Error: sprite \"Wall_<" + node.wallSprite + ">_N\" not found");
					northWall.sprite = Resources.Load<Sprite>("ErrorSprite");
				}
				wSprite = Resources.Load<Sprite>("Wall_" + node.wallSprite + "_E");
				if (wSprite != null) {
					eastWall.sprite = wSprite;
				} else {
					Debug.Log("Error: sprite \"Wall_<" + node.wallSprite + ">_E\" not found");
					eastWall.sprite = Resources.Load<Sprite>("ErrorSprite");
				}
				wSprite = Resources.Load<Sprite>("Wall_" + node.wallSprite + "_S");
				if (wSprite != null) {
					southWall.sprite = wSprite;
				} else {
					Debug.Log("Error: sprite \"Wall_<" + node.wallSprite + ">_S\" not found");
					southWall.sprite = Resources.Load<Sprite>("ErrorSprite");
				}
				wSprite = Resources.Load<Sprite>("Wall_" + node.wallSprite + "_W");
				if (wSprite != null) {
					westWall.sprite = wSprite;
				} else {
					Debug.Log("Error: sprite \"Wall_<" + node.wallSprite + ">_W\" not found");
					westWall.sprite = Resources.Load<Sprite>("ErrorSprite");
				}
				wSprite = Resources.Load<Sprite>("Corner_" + node.wallSprite);
				if (wSprite != null) {
					corners.sprite = wSprite;
				} else {
					Debug.Log("Error: sprite \"Corner_<" + node.wallSprite + ">\" not found");
					corners.sprite = Resources.Load<Sprite>("ErrorSprite");
				}
			}
			//Update the corner sprite if this node has one.
			//load errorsprite if corner sprite is invalid.
			/*if (node.cornerSprite != null) {
				Sprite cSprite = Resources.Load<Sprite>(node.cornerSprite);
				if (cSprite != null) {
					corners.sprite = cSprite;
				} else {
					corners.sprite = Resources.Load<Sprite>("ErrorSprite");
				}
			}*/
			//Update the color of each sprite
			floor.color = node.colorF;
			northWall.color = node.colorW;
			eastWall.color = node.colorW;
			southWall.color = node.colorW;
			westWall.color = node.colorW;
			corners.color = node.colorW;

			//set debris visible is it exists
			SetDebrisFromNode(node);

			//set this tile visible
			this.gameObject.SetActive(true);  
			
			//set walls visible as appropriate 
            SetWallsFromNode(node, dir, position);
        }
    }

	// for each of the debris slots, set that sprite if there is a sprite listed for that slot.
	// if a slot has something set, but that sprite is invalid, set it to the error sprite.
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

	//set walls visible if there is not a connection in that direction.
    public void SetWallsFromNode(Node node, GameManager.Direction? dir, Vector2Int? position) {
		northWall.gameObject.SetActive(node.connections.north == -1);
		southWall.gameObject.SetActive(node.connections.south == -1);
		eastWall.gameObject.SetActive(node.connections.east == -1);
		westWall.gameObject.SetActive(node.connections.west == -1);

		//this makes it so that one-way links between tiles don't look funny
		//   Needs to not run on center tile, because center tile doesn't actually 
		//   have a direction that it was accessed from.
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
