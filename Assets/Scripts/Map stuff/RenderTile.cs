using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Direction = GameManager.Direction;

public class RenderTile : MonoBehaviour {
	public SpriteRenderer corners;
	/*public SpriteRenderer northWall;
    public SpriteRenderer eastWall;
    public SpriteRenderer southWall;
    public SpriteRenderer westWall;*/
	public SpriteRenderer[] walls = new SpriteRenderer[4];

	public SpriteRenderer floor;

	/*public SpriteRenderer northLine;
    public SpriteRenderer southLine;
    public SpriteRenderer eastLine;
    public SpriteRenderer westLine;*/
	public SpriteRenderer[] lines = new SpriteRenderer[4];
	public SpriteRenderer lineCenter;

	private static Vector2Int center = new Vector2Int(2,2);
	//public GenericMask[] cornerMasks;
	public SpriteRenderer[] debris = new SpriteRenderer[9];

	public SpriteRenderer[] GetAllSprites {
		set { }
		get {
			/*return new SpriteRenderer[] {
				corners, northWall, eastWall, southWall, westWall,
				floor,
				northLine, southLine, eastLine, westLine, lineCenter,
				debris[0], debris[1], debris[2], debris[3], debris[4],
				debris[5], debris[6], debris[7], debris[8]
			};*/
			return new SpriteRenderer[] {
				corners, walls[0], walls[1], walls[2], walls[3],
				floor,
				lines[0], lines[1], lines[2], lines[3], lineCenter,
				debris[0], debris[1], debris[2], debris[3], debris[4],
				debris[5], debris[6], debris[7], debris[8]
			};
		}
	}


	/*
    public void CopyState(RenderTile other)
    {
        this.gameObject.SetActive(other.gameObject.activeSelf);
        //northWall.gameObject.SetActive(other.northWall.gameObject.activeSelf);
        //eastWall.gameObject.SetActive(other.eastWall.gameObject.activeSelf);
        //southWall.gameObject.SetActive(other.southWall.gameObject.activeSelf);
        //westWall.gameObject.SetActive(other.westWall.gameObject.activeSelf);
		for (int i = 0; i < 4; i++) {
			walls[i].gameObject.SetActive(other.walls[i].gameObject.activeSelf);
		} 
	}*/

	public void DrawFullNode(Node node, GameManager.Direction? dir, Vector2Int? position) {
		if ((node == null) || (dir == null) || (position == null)) {    // set everything invisible if any of these are null
			this.gameObject.SetActive(false);
		} else {
			// otherwise, draw the stuff as appropriate
			int i;
			for (i = 0; i < 4; i++)
                SetLineFromDir((Direction)i, false);

            if (node.hasEnter)	// set line/string visible as appropriate
                SetLineFromDir(node.enter, true);
            if (node.hasLeave)
                SetLineFromDir(node.leave, true);

				// set line/string center visible as appropriate
			if (node.hasEnter && node.hasLeave && node.enter.inverse() == node.leave)
				lineCenter.gameObject.SetActive(true);
			else
				lineCenter.gameObject.SetActive(false);

			// should evaluate to an array of size 2 for tiles on the cardinal directions, 4 for the middle tile, or 1 otherwise
			// add the directions that need to be checked to the array
			// Directions to be check are based on tile location in relation to the center tile
			// North correspondes to upper-right, east to lower-right, south to lower-left, and west to upper-left.
			//    Center tile is checked in upper-right, lower-right, lower-left, upper-right directions,
			//    Tile on the cardinal directions are checked in the two directions pointeed away from the center tile, 
			//    and all other tile are checked in the main direction that points away from the center tile
			Direction[] checkDirection = new Direction[((position.Value.x == 2) ? 2 : 1) * ((position.Value.y == 2) ? 2 : 1)];
			i = 0;
			if (position.Value.x <= 2) {
				if (position.Value.y <= 2) {
					checkDirection[i] = Direction.South;
					i++;
				}
				if (position.Value.y >= 2) {
					checkDirection[i] = Direction.West;
					i++;
				}
			}
			if (position.Value.x >= 2) {
				if (position.Value.y >= 2) {
					checkDirection[i] = Direction.North;
					i++;
				}
				if (position.Value.y <= 2) {
					checkDirection[i] = Direction.East;
				}
			}
			/*if ((position.Value.x <= 2) && (position.Value.y <= 2)) {
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
			}*/
			// now in each of the given directions, it checks the nodes that can be reached clockwise and counter cockwise in each direction. 
			// If nodes reached clockwise/counter-clockwise do not match or are null, the corner in that direction is set visible, otherwise that corner is set non-visible.
			foreach (Direction d in checkDirection) {
				if (node.drawCorner[(int)d]) {
					GameManager.gameplay.nonEuclidRenderer.cornerMaskMap[
						position.Value.x + (((int)d < 2) ? 1 : 0),
						position.Value.y + ((((int)d % 3) == 0) ? 1 : 0)
						].setCornerFromDir(true, dir.Value);
					/*GameManager.gameplay.nonEuclidRenderer.cornerMaskMap[
						position.Value.x + ((d == Direction.North || d == Direction.East) ? 1 : 0),
						position.Value.y + ((d == Direction.North || d == Direction.West) ? 1 : 0)
						].setCornerFromDir(true, dir.Value);*/
				}
			}
			/*if (node.drawCorner[0]) {
				GameManager.gameplay.nonEuclidRenderer.cornerMaskMap[position.Value.x + 1, position.Value.y + 1].setCornerFromDir(node.drawCorner[0], dir.Value);
			}
			if (node.drawCorner[1]) {
				GameManager.gameplay.nonEuclidRenderer.cornerMaskMap[position.Value.x + 1, position.Value.y].setCornerFromDir(node.drawCorner[1], dir.Value);
			}
			if (node.drawCorner[2]) {
				GameManager.gameplay.nonEuclidRenderer.cornerMaskMap[position.Value.x, position.Value.y].setCornerFromDir(node.drawCorner[2], dir.Value);
			}
			if (node.drawCorner[3]) {
				GameManager.gameplay.nonEuclidRenderer.cornerMaskMap[position.Value.x, position.Value.y + 1].setCornerFromDir(node.drawCorner[3], dir.Value);
			}*/


			//Update the floor sprite if this node has one.
			//load errorsprite if floor sprite is invalid.
			if (node.floorSprite != null) {
				Sprite fSprite = Resources.Load<Sprite>("Floor_" + node.floorSprite);
				if (fSprite != null) {
					floor.sprite = fSprite;
				} else {
					Debug.Log("Error: sprite \"Floor_<" + node.floorSprite + ">\" not found");
					floor.sprite = Resources.Load<Sprite>("ErrorSprite");
				}
			}
			//Update the wall sprites if this node has one.
			//load errorsprite if wall sprite is invalid.

			if (node.wallSprite != null) {
				Sprite wSprite = Resources.Load<Sprite>("Wall_" + node.wallSprite + "_N");
				if (wSprite != null) {
					//northWall.sprite = wSprite;
					walls[0].sprite = wSprite;
				} else {
					Debug.Log("Error: sprite \"Wall_<" + node.wallSprite + ">_N\" not found");
					walls[0].sprite = Resources.Load<Sprite>("ErrorSprite");
				}
				wSprite = Resources.Load<Sprite>("Wall_" + node.wallSprite + "_E");
				if (wSprite != null) {
					walls[1].sprite = wSprite;
				} else {
					Debug.Log("Error: sprite \"Wall_<" + node.wallSprite + ">_E\" not found");
					walls[1].sprite = Resources.Load<Sprite>("ErrorSprite");
				}
				wSprite = Resources.Load<Sprite>("Wall_" + node.wallSprite + "_S");
				if (wSprite != null) {
					walls[2].sprite = wSprite;
				} else {
					Debug.Log("Error: sprite \"Wall_<" + node.wallSprite + ">_S\" not found");
					walls[2].sprite = Resources.Load<Sprite>("ErrorSprite");
				}
				wSprite = Resources.Load<Sprite>("Wall_" + node.wallSprite + "_W");
				if (wSprite != null) {
					walls[3].sprite = wSprite;
				} else {
					Debug.Log("Error: sprite \"Wall_<" + node.wallSprite + ">_W\" not found");
					walls[3].sprite = Resources.Load<Sprite>("ErrorSprite");
				}
				wSprite = Resources.Load<Sprite>("Corner_" + node.wallSprite);
				if (wSprite != null) {
					corners.sprite = wSprite;
				} else {
					Debug.Log("Error: sprite \"Corner_<" + node.wallSprite + ">\" not found");
					corners.sprite = Resources.Load<Sprite>("ErrorSprite");
				}
			}

			floor.color = node.colorF;
			for (int j = 0; j < walls.Length; j++) {
				walls[j].color = node.colorW;
			}
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

	/// <summary>
	/// 
	/// </summary>
	/// <param name="dir"></param>
	/// <param name="b"></param>
    public void SetLineFromDir(Direction dir, bool b) {
		lines[(int)dir].gameObject.SetActive(b);
		/*switch (dir) {
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
        }*/
	}


    public void SetWallFromDir(Direction dir, bool b) {
		walls[(int)dir].gameObject.SetActive(b);
		/*switch (dir) {
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
        }*/
	}

	//set walls visible if there is not a connection in that direction.
    public void SetWallsFromNode(Node node, GameManager.Direction? dir, Vector2Int? position) {
		for (int i = 0; i < walls.Length; i++) {
			walls[i].gameObject.SetActive(node[i] == -1);
		}
		//northWall.gameObject.SetActive(node[0] == -1);
		//southWall.gameObject.SetActive(node[2] == -1);
		//eastWall.gameObject.SetActive(node[1] == -1);
		//westWall.gameObject.SetActive(node[3] == -1);

		//this makes it so that one-way links between tiles don't look funny
		//   Needs to not run on center tile, because center tile doesn't actually 
		//   have a direction that it was accessed from.
		if (!center.Equals(position)) {
			walls[((int)dir+2)%4].gameObject.SetActive(false);
			/*switch (dir) {
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
			}*/
		}
	}
}
