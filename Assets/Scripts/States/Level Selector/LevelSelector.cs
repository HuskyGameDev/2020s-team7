using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI.CoroutineTween;




public class LevelSelector : IState {

    public Button template;
    public GameObject templateParent;
	public override void _StartState() {
        //Retrieve the level names from the directory
        string[] levelButtonNames = searchForLevels();

        //Create the list of buttons and their names if it hasn't already.
        if (levelButtons.Length == 0) { 
            levelButtons = new Button[levelButtonNames.Length];
            for (int i = 0; i < levelButtons.Length; i++)
            {
                levelButtons[i] = Instantiate(template, templateParent.transform);
                levelButtons[i].gameObject.SetActive(true);
                levelButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = levelButtonNames[i];
                levelButtons[i].GetComponent<LevelButtonScript>().changeString(levelButtonNames[i]);
            }   
        }


        //If the level is won, unlock the next stage.
        //This is not active/working in the game right now due to winTrigger being buggy.
        if (GameManager.instance.gameplay.winTrigger)
        {
            GameManager.instance.gameplay.winTrigger = false;
            
            int i = 0;
    
            while (i < levelButtons.Length && levelButtons[i].interactable)
            {
                i++;
            }
            if (i<levelButtons.Length) levelButtons[i].interactable = true;
        }
	}


    public override void _EndState() {
		//Nothing for end state
	}

    #region Changing Levels
    public static string level = "001";
    private static bool change = false;
    
    //Find the .json file that holds the level data and create a map out of it.
    public static Map startLevel(string levelName)
    {
        if (File.Exists(Application.dataPath + "/Levels/room_" + levelName+".json"))
        {
            Map map = Map.Load(Application.dataPath + "/Levels/room_" + levelName+".json");
            return map;
        }
        else
        {
            Debug.Log("Error: Map file does not exist at path \"" + Application.dataPath + "/Levels/room_" + levelName + "\"");
            return null;
        }
        
    }

    //This doesn't change the level, just the string called 'level'
    public static void changeLevel(string s)
    {
        level = s;
        change = true;
    }

    #endregion

    public override void _Update()
    {

        //This is what ultimately ends up happening when the buttons are pressed.
        //The reason for the work-around is because I can't pass arguments on these button's "onClick" functions since they're instantiated through script. I have to be able to call their onClick functions without needing any arguments.

        if (change)
        {
            GameManager.instance.gameplay.map = startLevel(level);
            GameManager.instance.gameplay.resetLevelAssets();
            change = false;
            GameManager.instance.changeState(GameManager.instance.gameplay, this);
            
        }
        
    }

    //Retrieves the names of the levels from the directory by finding files ending in .json
    public static string[] searchForLevels()
    {
        string[] fileNames = Directory.GetFiles((Application.dataPath + "/Levels/"),"*.json");
        
        //fileNames retrieves the entire path, ex: "C:/Users/jsmith/Documents/Unity/Assets... .json"
        //We rewrite the fileNames strings by only getting the substring of the level name part

        int x = (Application.dataPath + "/Levels/room_").Length;
        for (int i=0; i<fileNames.Length; i++)
        {
            fileNames[i] = fileNames[i].Substring(x,fileNames[i].Length-x-5); //The '5' removes the '.json' part
            
        }
        return fileNames; //fileNames is now a list of names for the buttons
    }

    #region Buttons

    public Button[] levelButtons = null;

    //The names of these methods should be self explanitory
    public Button activateButton(Button button)
    {
        button.interactable = true;
        return button;
    }
    public Button[] getButtons()
    {
        return levelButtons;
    }

    public void deactivateAllButtons()
    {
        foreach(Button b in levelButtons)
        {
            b.interactable = false;
        }
    }

    #endregion
}
