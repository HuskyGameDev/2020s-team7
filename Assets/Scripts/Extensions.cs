using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions  {
	public static GameManager.Direction inverse(this GameManager.Direction dir) {
		switch (dir)
		{
			case GameManager.Direction.North:
				return GameManager.Direction.South;
				break;
			case GameManager.Direction.South:
				return GameManager.Direction.North;
				break;
			case GameManager.Direction.East:
				return GameManager.Direction.West;
				break;
			case GameManager.Direction.West:
				return GameManager.Direction.East;
				break;
		}
		return GameManager.Direction.East;
	}

	public static Vector2Int offset(this GameManager.Direction dir) {
		switch (dir)
		{
			case GameManager.Direction.North:
				return new Vector2Int(0,1);
				break;
			case GameManager.Direction.South:
				return new Vector2Int(0,-1);
				break;
			case GameManager.Direction.East:
				return new Vector2Int(1,0);
				break;
			case GameManager.Direction.West:
				return new Vector2Int(-1,0);
				break;
		}
		return new Vector2Int(0,0);
	}
}
