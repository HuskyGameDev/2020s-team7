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

	public GenericMask[] cornerMasks;

	public SpriteRenderer[] GetAllSprites { set { } get { return new SpriteRenderer[] { northWall, eastWall, southWall, westWall, floor, northLine, southLine, eastLine, westLine, lineCenter}; } }



    public void CopyState(RenderTile other)
    {
        this.gameObject.SetActive(other.gameObject.activeSelf);
        northWall.gameObject.SetActive(other.northWall.gameObject.activeSelf);
        eastWall.gameObject.SetActive(other.eastWall.gameObject.activeSelf);
        southWall.gameObject.SetActive(other.southWall.gameObject.activeSelf);
        westWall.gameObject.SetActive(other.westWall.gameObject.activeSelf);
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


			if (node.data.hasEnter && node.data.hasLeave && node.data.enter.inverse() == node.data.leave)
				lineCenter.gameObject.SetActive(true);
			else
				lineCenter.gameObject.SetActive(false);


			//Update the floor sprite if this node has one.
			//if (node.floorSprite != null) floor.sprite = node.floorSprite;
			if (node.floorSprite != null) {
				Sprite fSprite = Resources.Load<Sprite>(node.floorSprite);
				if (fSprite != null) {
					floor.sprite = fSprite;
				} else {
					floor.sprite = Resources.Load<Sprite>("ErrorSprite");
				}
				//floor.sprite = Resources.Load<Sprite>("rock_05_disp");
			}
			//Update the floor color
			floor.color = Color.Lerp(node.color/*new Color32(node.r, node.g, node.b, node.a)*/, Color.gray, .3f);

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
		northWall.gameObject.SetActive(node.connections.north == -1);
		southWall.gameObject.SetActive(node.connections.south == -1);
		eastWall.gameObject.SetActive(node.connections.east == -1);
		westWall.gameObject.SetActive(node.connections.west == -1);
		/*
		northWall.gameObject.SetActive(node.connections.north == null);
		southWall.gameObject.SetActive(node.connections.south == null);
		eastWall.gameObject.SetActive(node.connections.east == null);
		westWall.gameObject.SetActive(node.connections.west == null);
		*/
	}
}
