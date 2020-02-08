using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate_Tutorial_001 : Generate_Generic_Room {

	override public string getLevelName()
    {
        return "Tutorial1";
    }

    public override LevelMap generateRoom()
    {

        LevelMap _map = new LevelMap();

        //This Room will show the player how to move and that they need to move a certain distance to complete a level

        Node[,] chunk1 = LevelEditor_2.createChunk(_map, new Color32(244, 173, 66, 255), 5, 3);


        LevelEditor_2.setSource(_map, chunk1, new LevelEditor_2.TileCoord(0, 0));
        LevelEditor_2.setTarget(_map, chunk1, new LevelEditor_2.TileCoord(4, 0));


        return _map;
    }

}
