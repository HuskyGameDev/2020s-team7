using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate_Room_Demo4 : Generate_Generic_Room {
	override public string getLevelName() {
		return "Demo4";
	}
	override public LevelMap generateRoom() {
		LevelMap demoMap = new LevelMap();

		//Demo room map, relativly complex

		List<LevelEditor_2.TileCoord> emptyTiles1 = new List<LevelEditor_2.TileCoord>();
		emptyTiles1.Add(new LevelEditor_2.TileCoord(2, 0));

		Node[,] chunk1 = LevelEditor_2.createChunk(demoMap, new Color32(244,  66,  66, 255), 5, 1);               //
		Node[,] chunk2 = LevelEditor_2.createChunk(demoMap, new Color32(244, 155,  66, 255), 5, 1, emptyTiles1);
		Node[,] chunk3 = LevelEditor_2.createChunk(demoMap, new Color32(244, 244,  66, 255), 5, 1, emptyTiles1);  //
		Node[,] chunk4 = LevelEditor_2.createChunk(demoMap, new Color32(155, 244,  66, 255), 5, 1, emptyTiles1);
		Node[,] chunk5 = LevelEditor_2.createChunk(demoMap, new Color32( 66, 244,  66, 255), 5, 1, emptyTiles1);  //
		Node[,] chunk6 = LevelEditor_2.createChunk(demoMap, new Color32( 66, 244, 155, 255), 5, 1, emptyTiles1);
		Node[,] chunk7 = LevelEditor_2.createChunk(demoMap, new Color32( 66, 244, 244, 255), 5, 1, emptyTiles1);  //
		Node[,] chunk8 = LevelEditor_2.createChunk(demoMap, new Color32( 66, 155, 244, 255), 5, 1, emptyTiles1);
		Node[,] chunk9 = LevelEditor_2.createChunk(demoMap, new Color32( 66,  66, 244, 255), 5, 1, emptyTiles1);  //
		Node[,] chunk10 = LevelEditor_2.createChunk(demoMap, new Color32(155,  66, 244, 255), 5, 1, emptyTiles1);
		Node[,] chunk11 = LevelEditor_2.createChunk(demoMap, new Color32(244,  66, 244, 255), 5, 1);              //

		Node[,] chunk12 = LevelEditor_2.createChunk(demoMap, new Color32(244, 244, 244, 255), 1, 4);

		LevelEditor_2.createTwoWayLink(
			chunk1,
			chunk12,
			new LevelEditor_2.TileCoord(2, 0),
			new LevelEditor_2.TileCoord(0, 0),
				GameManager.Direction.South
			);
		LevelEditor_2.createTwoWayLink(
			chunk11,
			chunk12,
			new LevelEditor_2.TileCoord(2, 0),
			new LevelEditor_2.TileCoord(0, 3),
				GameManager.Direction.North
			);

		#region links_1
		#region 1-2
		LevelEditor_2.createTwoWayLink(
			chunk1,
			chunk2,
			new LevelEditor_2.TileCoord(0, 0),
			new LevelEditor_2.TileCoord(0, 0),
				GameManager.Direction.South
			);
		LevelEditor_2.createTwoWayLink(
			chunk1,
			chunk2,
			new LevelEditor_2.TileCoord(1, 0),
			new LevelEditor_2.TileCoord(1, 0),
				GameManager.Direction.South
			);
		LevelEditor_2.createTwoWayLink(
			chunk1,
			chunk2,
			new LevelEditor_2.TileCoord(3, 0),
			new LevelEditor_2.TileCoord(3, 0),
				GameManager.Direction.South
			);
		LevelEditor_2.createTwoWayLink(
			chunk1,
			chunk2,
			new LevelEditor_2.TileCoord(4, 0),
			new LevelEditor_2.TileCoord(4, 0),
				GameManager.Direction.South
			);
		#endregion
		#region 2-3
		LevelEditor_2.createTwoWayLink(
			chunk2,
			chunk3,
			new LevelEditor_2.TileCoord(0, 0),
			new LevelEditor_2.TileCoord(0, 0),
				GameManager.Direction.South
			);
		LevelEditor_2.createTwoWayLink(
			chunk2,
			chunk3,
			new LevelEditor_2.TileCoord(1, 0),
			new LevelEditor_2.TileCoord(1, 0),
				GameManager.Direction.South
			);
		LevelEditor_2.createTwoWayLink(
			chunk2,
			chunk3,
			new LevelEditor_2.TileCoord(3, 0),
			new LevelEditor_2.TileCoord(3, 0),
				GameManager.Direction.South
			);
		LevelEditor_2.createTwoWayLink(
			chunk2,
			chunk3,
			new LevelEditor_2.TileCoord(4, 0),
			new LevelEditor_2.TileCoord(4, 0),
				GameManager.Direction.South
			);
		#endregion
		#region 3-4
		LevelEditor_2.createTwoWayLink(
			chunk3,
			chunk4,
			new LevelEditor_2.TileCoord(0, 0),
			new LevelEditor_2.TileCoord(0, 0),
				GameManager.Direction.South
			);
		LevelEditor_2.createTwoWayLink(
			chunk3,
			chunk4,
			new LevelEditor_2.TileCoord(1, 0),
			new LevelEditor_2.TileCoord(1, 0),
				GameManager.Direction.South
			);
		LevelEditor_2.createTwoWayLink(
			chunk3,
			chunk4,
			new LevelEditor_2.TileCoord(3, 0),
			new LevelEditor_2.TileCoord(3, 0),
				GameManager.Direction.South
			);
		LevelEditor_2.createTwoWayLink(
			chunk3,
			chunk4,
			new LevelEditor_2.TileCoord(4, 0),
			new LevelEditor_2.TileCoord(4, 0),
				GameManager.Direction.South
			);
		#endregion
		#region 4-5
		LevelEditor_2.createTwoWayLink(
			chunk4,
			chunk5,
			new LevelEditor_2.TileCoord(0, 0),
			new LevelEditor_2.TileCoord(0, 0),
				GameManager.Direction.South
			);
		LevelEditor_2.createTwoWayLink(
			chunk4,
			chunk5,
			new LevelEditor_2.TileCoord(1, 0),
			new LevelEditor_2.TileCoord(1, 0),
				GameManager.Direction.South
			);
		LevelEditor_2.createTwoWayLink(
			chunk4,
			chunk5,
			new LevelEditor_2.TileCoord(3, 0),
			new LevelEditor_2.TileCoord(3, 0),
				GameManager.Direction.South
			);
		LevelEditor_2.createTwoWayLink(
			chunk4,
			chunk5,
			new LevelEditor_2.TileCoord(4, 0),
			new LevelEditor_2.TileCoord(4, 0),
				GameManager.Direction.South
			);
		#endregion
		#region 5-6
		LevelEditor_2.createTwoWayLink(
			chunk5,
			chunk6,
			new LevelEditor_2.TileCoord(0, 0),
			new LevelEditor_2.TileCoord(0, 0),
				GameManager.Direction.South
			);
		LevelEditor_2.createTwoWayLink(
			chunk5,
			chunk6,
			new LevelEditor_2.TileCoord(1, 0),
			new LevelEditor_2.TileCoord(1, 0),
				GameManager.Direction.South
			);
		LevelEditor_2.createTwoWayLink(
			chunk5,
			chunk6,
			new LevelEditor_2.TileCoord(3, 0),
			new LevelEditor_2.TileCoord(3, 0),
				GameManager.Direction.South
			);
		LevelEditor_2.createTwoWayLink(
			chunk5,
			chunk6,
			new LevelEditor_2.TileCoord(4, 0),
			new LevelEditor_2.TileCoord(4, 0),
				GameManager.Direction.South
			);
		#endregion
		#region 6-7
		LevelEditor_2.createTwoWayLink(
			chunk6,
			chunk7,
			new LevelEditor_2.TileCoord(0, 0),
			new LevelEditor_2.TileCoord(0, 0),
				GameManager.Direction.South
			);
		LevelEditor_2.createTwoWayLink(
			chunk6,
			chunk7,
			new LevelEditor_2.TileCoord(1, 0),
			new LevelEditor_2.TileCoord(1, 0),
				GameManager.Direction.South
			);
		LevelEditor_2.createTwoWayLink(
			chunk6,
			chunk7,
			new LevelEditor_2.TileCoord(3, 0),
			new LevelEditor_2.TileCoord(3, 0),
				GameManager.Direction.South
			);
		LevelEditor_2.createTwoWayLink(
			chunk6,
			chunk7,
			new LevelEditor_2.TileCoord(4, 0),
			new LevelEditor_2.TileCoord(4, 0),
				GameManager.Direction.South
			);
		#endregion
		#region 7-8
		LevelEditor_2.createTwoWayLink(
			chunk7,
			chunk8,
			new LevelEditor_2.TileCoord(0, 0),
			new LevelEditor_2.TileCoord(0, 0),
				GameManager.Direction.South
			);
		LevelEditor_2.createTwoWayLink(
			chunk7,
			chunk8,
			new LevelEditor_2.TileCoord(1, 0),
			new LevelEditor_2.TileCoord(1, 0),
				GameManager.Direction.South
			);
		LevelEditor_2.createTwoWayLink(
			chunk7,
			chunk8,
			new LevelEditor_2.TileCoord(3, 0),
			new LevelEditor_2.TileCoord(3, 0),
				GameManager.Direction.South
			);
		LevelEditor_2.createTwoWayLink(
			chunk7,
			chunk8,
			new LevelEditor_2.TileCoord(4, 0),
			new LevelEditor_2.TileCoord(4, 0),
				GameManager.Direction.South
			);
		#endregion
		#region 8-9
		LevelEditor_2.createTwoWayLink(
			chunk8,
			chunk9,
			new LevelEditor_2.TileCoord(0, 0),
			new LevelEditor_2.TileCoord(0, 0),
				GameManager.Direction.South
			);
		LevelEditor_2.createTwoWayLink(
			chunk8,
			chunk9,
			new LevelEditor_2.TileCoord(1, 0),
			new LevelEditor_2.TileCoord(1, 0),
				GameManager.Direction.South
			);
		LevelEditor_2.createTwoWayLink(
			chunk8,
			chunk9,
			new LevelEditor_2.TileCoord(3, 0),
			new LevelEditor_2.TileCoord(3, 0),
				GameManager.Direction.South
			);
		LevelEditor_2.createTwoWayLink(
			chunk8,
			chunk9,
			new LevelEditor_2.TileCoord(4, 0),
			new LevelEditor_2.TileCoord(4, 0),
				GameManager.Direction.South
			);
		#endregion
		#region 9-10
		LevelEditor_2.createTwoWayLink(
			chunk9,
			chunk10,
			new LevelEditor_2.TileCoord(0, 0),
			new LevelEditor_2.TileCoord(0, 0),
				GameManager.Direction.South
			);
		LevelEditor_2.createTwoWayLink(
			chunk9,
			chunk10,
			new LevelEditor_2.TileCoord(1, 0),
			new LevelEditor_2.TileCoord(1, 0),
				GameManager.Direction.South
			);
		LevelEditor_2.createTwoWayLink(
			chunk9,
			chunk10,
			new LevelEditor_2.TileCoord(3, 0),
			new LevelEditor_2.TileCoord(3, 0),
				GameManager.Direction.South
			);
		LevelEditor_2.createTwoWayLink(
			chunk9,
			chunk10,
			new LevelEditor_2.TileCoord(4, 0),
			new LevelEditor_2.TileCoord(4, 0),
				GameManager.Direction.South
			);
		#endregion
		#region 10-11
		LevelEditor_2.createTwoWayLink(
			chunk10,
			chunk11,
			new LevelEditor_2.TileCoord(0, 0),
			new LevelEditor_2.TileCoord(0, 0),
				GameManager.Direction.South
			);
		LevelEditor_2.createTwoWayLink(
			chunk10,
			chunk11,
			new LevelEditor_2.TileCoord(1, 0),
			new LevelEditor_2.TileCoord(1, 0),
				GameManager.Direction.South
			);
		LevelEditor_2.createTwoWayLink(
			chunk10,
			chunk11,
			new LevelEditor_2.TileCoord(3, 0),
			new LevelEditor_2.TileCoord(3, 0),
				GameManager.Direction.South
			);
		LevelEditor_2.createTwoWayLink(
			chunk10,
			chunk11,
			new LevelEditor_2.TileCoord(4, 0),
			new LevelEditor_2.TileCoord(4, 0),
				GameManager.Direction.South
			);
		#endregion
		#endregion

		#region links_2
		#region 3
		LevelEditor_2.createTwoWayLink(
			chunk3,
			chunk12,
			new LevelEditor_2.TileCoord(1, 0),
			new LevelEditor_2.TileCoord(0, 0),
				GameManager.Direction.East
			);
		LevelEditor_2.createTwoWayLink(
			chunk3,
			chunk12,
			new LevelEditor_2.TileCoord(3, 0),
			new LevelEditor_2.TileCoord(0, 0),
				GameManager.Direction.West
			);
		#endregion
		#region 5
		LevelEditor_2.createTwoWayLink(
			chunk5,
			chunk12,
			new LevelEditor_2.TileCoord(1, 0),
			new LevelEditor_2.TileCoord(0, 1),
				GameManager.Direction.East
			);
		LevelEditor_2.createTwoWayLink(
			chunk5,
			chunk12,
			new LevelEditor_2.TileCoord(3, 0),
			new LevelEditor_2.TileCoord(0, 1),
				GameManager.Direction.West
			);
		#endregion
		#region 7
		LevelEditor_2.createTwoWayLink(
			chunk7,
			chunk12,
			new LevelEditor_2.TileCoord(1, 0),
			new LevelEditor_2.TileCoord(0, 2),
				GameManager.Direction.East
			);
		LevelEditor_2.createTwoWayLink(
			chunk7,
			chunk12,
			new LevelEditor_2.TileCoord(3, 0),
			new LevelEditor_2.TileCoord(0, 2),
				GameManager.Direction.West
			);
		#endregion
		#region 9
		LevelEditor_2.createTwoWayLink(
			chunk9,
			chunk12,
			new LevelEditor_2.TileCoord(1, 0),
			new LevelEditor_2.TileCoord(0, 3),
				GameManager.Direction.East
			);
		LevelEditor_2.createTwoWayLink(
			chunk9,
			chunk12,
			new LevelEditor_2.TileCoord(3, 0),
			new LevelEditor_2.TileCoord(0, 3),
				GameManager.Direction.West
			);
		#endregion
		#endregion

		LevelEditor_2.setSource(demoMap, chunk1, new LevelEditor_2.TileCoord(2, 0));
		LevelEditor_2.setTarget(demoMap, chunk11, new LevelEditor_2.TileCoord(2, 0));

		return demoMap;
	}
}
