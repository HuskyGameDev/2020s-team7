using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelSelector : IState {

    public static string level = "001";
    private static bool change = false;
    

    public static Map startLevel(string levelName)
    {
        if (File.Exists(Application.dataPath + "/Levels/room_" + levelName))
        {
            Map map = Map.Load(Application.dataPath + "/Levels/room_" + levelName);
            return map;
        }
        else
        {
            Debug.Log("Error: Map file does not exist at path \"" + Application.dataPath + "/Levels/room_" + levelName + "\"");
            return null;
        }
        
    }
    
    public void changeLevel(string s)
    {
        level = s;
        change = true;
        //Debug.Log("change is true, s was changed");
    }

    public override void _Update()
    {
        if (change)
        {
            //Change these two to whatever controls the visuals of the level once imported
            GameManager.instance.map = startLevel(level);
            GameManager.instance.resetLevelAssets();

            //Keep this
            GameManager.instance.gameplay.SetActive(true);
           this.gameObject.SetActive(false);
            change = false;
        }
    }
}
