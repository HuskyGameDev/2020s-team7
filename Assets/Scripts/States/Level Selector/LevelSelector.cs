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
	public UnityEngine.UI.Text numUnlockedText;	// text for the name and number of level unlocked of the current save slot
	public UnityEngine.UI.Text slotNameText;

	public static string[] levelNames;	// array of level names. Makes it easier to load levels from other scripts
	public Button template;
    public GameObject templateParent;
	public Button[] levelButtons = null;
	public static LevelSelector instance;	// singleton for use by the buttons. Probably bad and should be changed

	//public static string level = "001";
	//public static int level = 0;
	//private static bool change = false;

	public override GameManager.IStateType _stateType { // override the state-enum return value
		get { return GameManager.IStateType.levelSelector; }
	}

	public void Start() {
	}

	protected override void _StartState(IState oldstate) {
		if (GameManager.saveGame == null) {	// shouldn't be able to see this if no save is selected anyway
			slotNameText.text = "-";
			numUnlockedText.text = "~/" + GameManager.numLevels;
			Debug.Log("Error: can select levels");
		} else {	// if save exists, set the savename and number-of-levels-unlocked text, as well as activating only buttoons for unlocked levels
			int count = 0;
			for (int i = 0; i < levelButtons.Length; i++) {
				if (GameManager.saveGame.canAccess(i)) {
					count++;
					levelButtons[i].gameObject.SetActive(true);
					//levelButtons[i].interactable = true;
				} else {
					levelButtons[i].gameObject.SetActive(false);
					//levelButtons[i].interactable = false;
				}
			}
			levelButtons[0].gameObject.SetActive(true);

			slotNameText.text = GameManager.saveGame.slotName;
			numUnlockedText.text = count + "/" + GameManager.numLevels;
		}
	}


	protected override void _EndState(IState newstate) {
		//Nothing for end state
	}

	public override void _RespondToConfirm(int retVal, string retString) { }

	protected override void _initialize() {
		if ((instance == null) || (instance == this)) {	// setup singleton
			instance = this;
		} else {
			Debug.Log("Duplicate GameManager destroyed");
			DestroyImmediate(this.gameObject);
		}
		//Retrieve the level names from the directory
		levelNames = searchForLevels();	// get level names

		//Create the list of buttons and their names if it hasn't already.
		levelButtons = new Button[levelNames.Length];
		for (int i = 0; i < levelButtons.Length; i++) {
			levelButtons[i] = Instantiate(template, templateParent.transform);
			levelButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = levelNames[i];
			levelButtons[i].GetComponent<LevelButtonScript>().index = i;	// set index for this button
		}
		GameManager.numLevels = levelButtons.Length;	// set number of levels that exist in the game manager
		levelButtons[0].gameObject.SetActive(true);	// make sure level 0 is always accessible. 
	}

	/// <summary>
	/// set the first locked level to unlocked
	/// should be changed
	/// </summary>
	public void unlockLevel() {
		int i = 0;
		while (GameManager.saveGame.canAccess(i)) {
			i++;
		}
		if (i < levelButtons.Length) {
			//levelButtons[i].gameObject.SetActive(true);
			GameManager.saveGame.setAccess(i, true);
		}
	}

	#region Changing Levels
    //Find the .json file that holds the level data and create a map out of it.
    //public static LevelMap startLevel(string levelName) {
	public static LevelMap startLevel(string levelName) {
		GameManager.gameplay.levelNameText.text = levelName;
		//string levelName = levelNames[index];
		if (File.Exists(Application.dataPath + "/Levels/room_" + levelName+".json")) {
            LevelMap map = LevelMap.Load(Application.dataPath + "/Levels/room_" + levelName+".json");
            return map;
        } else {
            Debug.Log("Error: Map file does not exist at path \"" + Application.dataPath + "/Levels/room_" + levelName + "\"");
            return null;
        }
        
    }

    //This doesn't change the level, just the string called 'level'
	/*public static void changeLevel(int num) {
		level = num;
		change = true;
	}*/

	#endregion

	public override void _Update()
    {

        //This is what ultimately ends up happening when the buttons are pressed.
        //The reason for the work-around is because I can't pass arguments on these button's "onClick" functions since they're instantiated through script. I have to be able to call their onClick functions without needing any arguments.
        /*if (change) {
			change = false;
			//changeLevelHelper(level ,this);
			changeLevelHelper(level);
			GameManager.changeState(GameManager.gameplay, this);
		}*/
        
    }

	/// <summary>
	/// Should change the name of this
	/// sets up gameplay with the level with the given index in the levelNames[] array
	/// </summary>
	/// <param name="index"></param>
	public static void changeLevelHelper(int index) { //, IState state) {
													  //change = false;
		string name = levelNames[index];
		GameManager.gameplay.map = startLevel(name);	// load level map
		GameManager.gameplay.resetLevelAssets();	// reset everything
		//GameManager.changeState(GameManager.gameplay, state);
	}

    //Retrieves the names of the levels from the directory by finding files ending in .json
    public static string[] searchForLevels() {
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

    //The names of these methods should be self explanitory
	/*
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
	*/
    #endregion
}
