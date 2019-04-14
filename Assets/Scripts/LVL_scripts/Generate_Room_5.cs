using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Generate_Room_5 {
    public static Map generateRoom()
    {
        Map _map = new Map();

        //This Room will show the player that they can move from one pink tile to the next

        List<LevelEditor_2.TileCoord> emptyTiles = new List<LevelEditor_2.TileCoord>
        {
            new LevelEditor_2.TileCoord(3, 1),
            new LevelEditor_2.TileCoord(3, 2),
            new LevelEditor_2.TileCoord(3, 3),
            new LevelEditor_2.TileCoord(3, 4),
            new LevelEditor_2.TileCoord(3, 5),
            new LevelEditor_2.TileCoord(3, 6),

        };
        Node[,] chunk1 = LevelEditor_2.createChunk(_map, new Color32(244, 173, 66, 255), 4, 9, emptyTiles);
        Node[,] chunk2 = LevelEditor_2.createChunk(_map, Color.cyan, 2, 9);
        Node[,] chunk3 = LevelEditor_2.createChunk(_map, Color.red, 2, 9);

        LevelEditor_2.createTwoWayLink(
                chunk1,
                chunk2,
                new LevelEditor_2.TileCoord(3, 7, false, true, false, false),
                new LevelEditor_2.TileCoord(0, 7, false, false, false, true)
                );
        LevelEditor_2.createTwoWayLink(
                chunk1,
                chunk2,
                new LevelEditor_2.TileCoord(3, 8, false, true, false, false),
                new LevelEditor_2.TileCoord(0, 8, false, false, false, true)
                );
        LevelEditor_2.createTwoWayLink(
                chunk1,
                chunk3,
                new LevelEditor_2.TileCoord(3, 0, false, true, false, false),
                new LevelEditor_2.TileCoord(0, 0, false, false, false, true)
                );
        LevelEditor_2.setSource(chunk1, new LevelEditor_2.TileCoord(2, 6));
        LevelEditor_2.setTarget(chunk3, new LevelEditor_2.TileCoord(1, 4));

        return _map;
    }
	
}
