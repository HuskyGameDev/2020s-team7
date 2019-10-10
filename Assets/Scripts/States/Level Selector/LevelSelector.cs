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
        //Debug.Log("LevelSelector does not do anything in its _StartState() method");
        string[] levelButtonNames = searchForLevels();
        if (levelButtons.Length == 0) { 
            levelButtons = new Button[levelButtonNames.Length];
            for (int i = 0; i < levelButtons.Length; i++)
            {
                Debug.Log("Trying to instantiate");
                levelButtons[i] = Instantiate(template, templateParent.transform);
                levelButtons[i].gameObject.SetActive(true);
                levelButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = levelButtonNames[i];
                levelButtons[i].GetComponent<LevelButtonScript>().changeString(levelButtonNames[i]);
            }



            
                    
        }
            
           
  
                //Debug.Log(levelButtons[i].GetComponentInChildren<Text>().text);
                //levelButtons[i].GetComponentInChildren<Text>().text = levelButtonNames[i];
        if (GameManager.instance.gameplay.winTrigger)
        {
            GameManager.instance.gameplay.winTrigger = false;
            
            int i = 0;
            while (levelButtons[i].interactable)
            {
                i++;
            }
            levelButtons[i].interactable = true;
        }
	}


    public override void _EndState() {
		//Debug.Log("LevelSelector does not do anything in its _EndState() method");
	}

    #region Changing Levels
    public static string level = "001";
    private static bool change = false;
    

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
    
    public static void changeLevel(string s)
    {
        //Debug.Log("Button was pressed");
        level = s;
        change = true;
        //Debug.Log("change is true, s was changed to "+level);
    }

    #endregion

    public override void _Update()
    {
        if (change)
        {
            //Debug.Log("change is true in _Update");
            //Change these two to whatever controls the visuals of the level once imported
            GameManager.instance.gameplay.map = startLevel(level);
            GameManager.instance.gameplay.resetLevelAssets();

           
            //this.gameObject.SetActive(false);
            change = false;
            GameManager.instance.changeState(GameManager.instance.gameplay, this);
            
        }
        
    }

    public static string[] searchForLevels()
    {
        string[] fileNames = Directory.GetFiles((Application.dataPath + "/Levels/"),"*.json");
        
        int x = (Application.dataPath + "/Levels/room_").Length;
        for (int i=0; i<fileNames.Length; i++)
        {
            fileNames[i] = fileNames[i].Substring(x,fileNames[i].Length-x-5);
            
        }
        return fileNames;
    }
    #region Buttons

    public Button[] levelButtons = null;

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
