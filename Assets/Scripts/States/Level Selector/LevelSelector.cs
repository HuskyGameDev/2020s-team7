using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : IState {
	public override void _StartState() {
		//Debug.Log("LevelSelector does not do anything in its _StartState() method");
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

            //Keep this
            GameManager.instance.gameplay.gameObject.SetActive(true);
            //this.gameObject.SetActive(false);
            change = false;
			GameManager.instance.changeState(GameManager.instance.gameplay, this);
        }
    }

    
    #region Buttons

    public Button[] levelButtons;

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
