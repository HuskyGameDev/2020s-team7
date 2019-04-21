using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate_Room_006  {

    public static Map generateRoom()
    {
        Map demoMap = new Map();

        // This map is a long room, with a walled off walway in the middle.
        // The hallway looks short from the outside, and long from the inside

        List<LevelEditor_2.TileCoord> emptyTiles = new List<LevelEditor_2.TileCoord>
        {
            new LevelEditor_2.TileCoord(1, 1),
            new LevelEditor_2.TileCoord(5, 1),
            new LevelEditor_2.TileCoord(5, 4),
            new LevelEditor_2.TileCoord(3, 4),
            new LevelEditor_2.TileCoord(1, 4),
            new LevelEditor_2.TileCoord(3, 1),

        };

        Node[,] chunk1 = LevelEditor_2.createChunk(demoMap, new Color32(255, 255, 0, 255), 7, 6, emptyTiles);
        Node[,] chunkAE = LevelEditor_2.createChunk(demoMap, Color.red, 1, 3);
        Node[,] chunkBF = LevelEditor_2.createChunk(demoMap, Color.green, 1, 3);
        Node[,] chunkCD = LevelEditor_2.createChunk(demoMap, Color.blue, 1, 3);

        LevelEditor_2.createTwoWayLink(
            chunk1,
            chunkAE,
            new LevelEditor_2.TileCoord(1, 2),
            new LevelEditor_2.TileCoord(0, 2),
            LevelEditor_2.Direction.North
            );
        LevelEditor_2.createTwoWayLink(
            chunk1,
            chunkAE,
            new LevelEditor_2.TileCoord(3, 3),
            new LevelEditor_2.TileCoord(0, 0),
            LevelEditor_2.Direction.South
            );




        LevelEditor_2.createTwoWayLink(
            chunk1,
            chunkBF,
            new LevelEditor_2.TileCoord(3, 2),
            new LevelEditor_2.TileCoord(0, 2),
            LevelEditor_2.Direction.North
            );
        LevelEditor_2.createTwoWayLink(
            chunk1,
            chunkBF,
            new LevelEditor_2.TileCoord(5, 3),
            new LevelEditor_2.TileCoord(0, 0),
            LevelEditor_2.Direction.South
            );



        LevelEditor_2.createTwoWayLink(
            chunk1,
            chunkCD,
            new LevelEditor_2.TileCoord(5, 2),
            new LevelEditor_2.TileCoord(0, 2),
            LevelEditor_2.Direction.North
            );
        LevelEditor_2.createTwoWayLink(
            chunk1,
            chunkCD,
            new LevelEditor_2.TileCoord(1, 3),
            new LevelEditor_2.TileCoord(0, 0),
            LevelEditor_2.Direction.South
            );




        LevelEditor_2.setSource(chunk1, new LevelEditor_2.TileCoord(0, 2));
        LevelEditor_2.setTarget(chunk1, new LevelEditor_2.TileCoord(3, 3));

        return demoMap;
    }
}
