using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate_Room_007
{

    public static Map generateRoom()
    {
        Map demoMap = new Map();

        // This map is a long room, with a walled off walway in the middle.
        // The hallway looks short from the outside, and long from the inside

        Node[,] chunk1 = LevelEditor_2.createChunk(demoMap, new Color32(255, 255, 0, 255), 20, 1);



        LevelEditor_2.setSource(chunk1, new LevelEditor_2.TileCoord(0, 0));
        LevelEditor_2.setTarget(chunk1, new LevelEditor_2.TileCoord(6, 0));

        return demoMap;
    }
}
