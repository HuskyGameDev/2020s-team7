using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate_Tutorial_002 : Generate_Generic_Room {

    override public string getLevelName()
    {
        return "Tutorial2";
    }

    public override Map generateRoom()
    {

        Map _map = new Map();

        //This Room will introduce the player to the non-euclidean spaces

        List<LevelEditor_2.TileCoord> emptyTiles = new List<LevelEditor_2.TileCoord>();
            emptyTiles.Add(new LevelEditor_2.TileCoord(2, 1));

        Node[,] chunk1 = LevelEditor_2.createChunk(_map, new Color32(244, 173, 66, 255), 5, 3, emptyTiles);
        Node[,] chunk2 = LevelEditor_2.createChunk(_map, Color.red, 1, 10);

            LevelEditor_2.createTwoWayLink(
                chunk1,
                chunk2,
                new LevelEditor_2.TileCoord(2, 0),
                new LevelEditor_2.TileCoord(0, 0),
                GameManager.Direction.South
                );
            LevelEditor_2.createTwoWayLink(
                chunk1,
                chunk2,
                new LevelEditor_2.TileCoord(2, 2),
                new LevelEditor_2.TileCoord(0, 9),
				GameManager.Direction.North
                );

        LevelEditor_2.setSource(_map, chunk1, new LevelEditor_2.TileCoord(0, 0));
        LevelEditor_2.setTarget(_map, chunk1, new LevelEditor_2.TileCoord(4, 2));


        return _map;
    }
}
