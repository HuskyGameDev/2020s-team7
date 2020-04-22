using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Direction = GameManager.Direction;

public class RenderTile : MonoBehaviour {
	public SpriteRenderer corners;
	public SpriteRenderer[] walls = new SpriteRenderer[4];
	public SpriteRenderer floor;
	public SpriteRenderer[] lines = new SpriteRenderer[4];
	//public SpriteRenderer lineCenter;
	public SpriteRenderer[] debris = new SpriteRenderer[9];

	private static Vector2Int center = new Vector2Int(2,2);

	public SpriteRenderer[] GetAllSprites {
		set { }
		get {
			return new SpriteRenderer[] {
				corners, walls[0], walls[1], walls[2], walls[3],
				floor,
				lines[0], lines[1], lines[2], lines[3], /*lineCenter,*/
				debris[0], debris[1], debris[2], debris[3], debris[4],
				debris[5], debris[6], debris[7], debris[8]
			};
		}
	}

	public void DrawFullNode(Node node, GameManager.Direction? dir, Vector2Int? position, bool grayout = false) {
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
			/*if (node.hasEnter && node.hasLeave && node.enter.inverse() == node.leave)
				lineCenter.gameObject.SetActive(true);
			else
				lineCenter.gameObject.SetActive(false);*/

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
			// now in each of the given directions, it checks the nodes that can be reached clockwise and counter cockwise in each direction. 
			// If nodes reached clockwise/counter-clockwise do not match or are null, the corner in that direction is set visible, otherwise that corner is set non-visible.
			foreach (Direction d in checkDirection) {
				if (node.drawCorner[(int)d]) {
					GameManager.gameplay.nonEuclidRenderer.cornerMaskMap[
						position.Value.x + (((int)d < 2) ? 1 : 0),
						position.Value.y + ((((int)d % 3) == 0) ? 1 : 0)
						].setCornerFromDir(true, dir.Value);
				}
			}

			//Update the floor sprite if this node has one.
			//load errorsprite if floor sprite is invalid.
			if (node.floorSprite != null) {
				//Sprite fSprite = Resources.Load<Sprite>("Floor_" + node.floorSprite);
				Sprite fSprite = GameManager.getSprite("Floor_" + node.floorSprite);
				if (fSprite != null) {
					floor.sprite = fSprite;
				} else {
					Debug.Log("Error: sprite \"Floor_<" + node.floorSprite + ">\" not found");
					//floor.sprite = Resources.Load<Sprite>("ErrorSprite");
					floor.sprite = GameManager.errorSprite;
				}
			}
			//Update the wall sprites if this node has one.
			//load errorsprite if wall sprite is invalid.

			if (node.wallSprite != null) {
				//Sprite wSprite = Resources.Load<Sprite>("Wall_" + node.wallSprite + "_N");
				Sprite wSprite = GameManager.getSprite("Wall_" + node.wallSprite + "_N");
				if (wSprite != null) {
					//northWall.sprite = wSprite;
					walls[0].sprite = wSprite;
				} else {
					Debug.Log("Error: sprite \"Wall_<" + node.wallSprite + ">_N\" not found");
					//walls[0].sprite = Resources.Load<Sprite>("ErrorSprite");
					walls[0].sprite = GameManager.errorSprite;
				}
				//wSprite = Resources.Load<Sprite>("Wall_" + node.wallSprite + "_E");
				wSprite = GameManager.getSprite("Wall_" + node.wallSprite + "_E");
				if (wSprite != null) {
					walls[1].sprite = wSprite;
				} else {
					Debug.Log("Error: sprite \"Wall_<" + node.wallSprite + ">_E\" not found");
					//walls[1].sprite = Resources.Load<Sprite>("ErrorSprite");
					walls[1].sprite = GameManager.errorSprite;
				}
				//wSprite = Resources.Load<Sprite>("Wall_" + node.wallSprite + "_S");
				wSprite = GameManager.getSprite("Wall_" + node.wallSprite + "_S");
				if (wSprite != null) {
					walls[2].sprite = wSprite;
				} else {
					Debug.Log("Error: sprite \"Wall_<" + node.wallSprite + ">_S\" not found");
					//walls[2].sprite = Resources.Load<Sprite>("ErrorSprite");
					walls[2].sprite = GameManager.errorSprite;
				}
				//wSprite = Resources.Load<Sprite>("Wall_" + node.wallSprite + "_W");
				wSprite = GameManager.getSprite("Wall_" + node.wallSprite + "_W");
				if (wSprite != null) {
					walls[3].sprite = wSprite;
				} else {
					Debug.Log("Error: sprite \"Wall_<" + node.wallSprite + ">_W\" not found");
					//walls[3].sprite = Resources.Load<Sprite>("ErrorSprite");
					walls[3].sprite = GameManager.errorSprite;
				}
				//wSprite = Resources.Load<Sprite>("Corner_" + node.wallSprite);
				wSprite = GameManager.getSprite("Corner_" + node.wallSprite);
				if (wSprite != null) {
					corners.sprite = wSprite;
				} else {
					Debug.Log("Error: sprite \"Corner_<" + node.wallSprite + ">\" not found");
					//corners.sprite = Resources.Load<Sprite>("ErrorSprite");
					corners.sprite = GameManager.errorSprite;
				}
			}

			if (grayout) {	// if grayed-out, set all colors as grayed versions
				floor.color = Color.Lerp(node.colorF, Color.gray, 0.9f);
				for (int j = 0; j < walls.Length; j++) {
					walls[j].color = Color.Lerp(node.colorW, Color.gray, 0.9f);
				}
				corners.color = Color.Lerp(node.colorW, Color.gray, 0.9f);
				Color grayish = new Color(0.55f, 0.55f, 0.55f);
				for (int j = 0; j < debris.Length; j++) {
					debris[j].color = grayish;
				}
				for (int j = 0; j < lines.Length; j++) {
					lines[j].color = grayish;
				}
				//lineCenter.color = grayish;
			} else {	// if not grayed-out, set all colors as normal
				floor.color = node.colorF;
				for (int j = 0; j < walls.Length; j++) {
					walls[j].color = node.colorW;
				}
				corners.color = node.colorW;
				for (int j = 0; j < debris.Length; j++) {
					debris[j].color = Color.white;
				}
				for (int j = 0; j < lines.Length; j++) {
					lines[j].color = Color.white;
				}
				//lineCenter.color = Color.white;
			}

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
				//Sprite dSprite = Resources.Load<Sprite>(node.debris[j]);
				Sprite dSprite = GameManager.getSprite(node.debris[j]);
				if (dSprite != null) {
					this.debris[j].sprite = dSprite;
				} else {
					//this.debris[j].sprite = Resources.Load<Sprite>("ErrorSprite");
					this.debris[j].sprite = GameManager.errorSprite;
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
	}


    public void SetWallFromDir(Direction dir, bool b) {
		walls[(int)dir].gameObject.SetActive(b);
	}

	//set walls visible if there is not a connection in that direction.
    public void SetWallsFromNode(Node node, GameManager.Direction? dir, Vector2Int? position) {
		for (int i = 0; i < walls.Length; i++) {
			walls[i].gameObject.SetActive(node[i] == -1);
		}

		//this makes it so that one-way links between tiles don't look funny
		//   Needs to not run on center tile, because center tile doesn't actually 
		//   have a direction that it was accessed from.
		if (!center.Equals(position)) {
			walls[((int)dir+2)%4].gameObject.SetActive(false);
		}
	}
}
