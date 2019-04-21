using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Generate_Room_005 {
    public static Map generateRoom()
    {
        Map _map = new Map();

        //This Room will show the player that they can move from one pink tile to the next

        List<LevelEditor_2.TileCoord> Chunk1EmptyTiles = new List<LevelEditor_2.TileCoord>
        {
            new LevelEditor_2.TileCoord(3, 1),
            new LevelEditor_2.TileCoord(3, 2),
            new LevelEditor_2.TileCoord(3, 3),
            new LevelEditor_2.TileCoord(3, 4),
            new LevelEditor_2.TileCoord(3, 5),
            new LevelEditor_2.TileCoord(3, 6),

        };
        Node[,] chunk1 = LevelEditor_2.createChunk(_map, new Color32(244, 173, 66, 255), 4, 9, Chunk1EmptyTiles);
        Node[,] chunk2 = LevelEditor_2.createChunk(_map, Color.cyan, 2, 4);
        Node[,] chunk3 = LevelEditor_2.createChunk(_map, Color.red, 2, 4);

        LevelEditor_2.createTwoWayLink(
                chunk1,
                chunk2,
                new LevelEditor_2.TileCoord(3, 7),
                new LevelEditor_2.TileCoord(0, 2),
                LevelEditor_2.Direction.East
                );
        LevelEditor_2.createTwoWayLink(
                chunk1,
                chunk2,
                new LevelEditor_2.TileCoord(3, 8),
                new LevelEditor_2.TileCoord(0, 3),
                LevelEditor_2.Direction.East
                );
        LevelEditor_2.createTwoWayLink(
                chunk1,
                chunk3,
                new LevelEditor_2.TileCoord(3, 0),
                new LevelEditor_2.TileCoord(0, 0),
                LevelEditor_2.Direction.East
                );
        LevelEditor_2.createOneWayLink(
                chunk2,
                chunk1,
                new LevelEditor_2.TileCoord(0, 0),
                new LevelEditor_2.TileCoord(3, 0),
                LevelEditor_2.Direction.West
                );
        LevelEditor_2.createOneWayLink(
                chunk3,
                chunk1,
                new LevelEditor_2.TileCoord(0, 2),
                new LevelEditor_2.TileCoord(3, 7),
                LevelEditor_2.Direction.West
                );
        LevelEditor_2.createOneWayLink(
                chunk3,
                chunk1,
                new LevelEditor_2.TileCoord(0, 3),
                new LevelEditor_2.TileCoord(3, 8),
                LevelEditor_2.Direction.West
                );

        LevelEditor_2.setSource(chunk1, new LevelEditor_2.TileCoord(2, 6));
        LevelEditor_2.setTarget(chunk3, new LevelEditor_2.TileCoord(1, 1));

        return _map;
    }
	
}
