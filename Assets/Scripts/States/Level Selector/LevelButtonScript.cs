using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButtonScript : MonoBehaviour {


    //This method is called upon creation of the button in the LevelSelector
    //String 's' is stored in this script (the button) until it is called when pressed. It is then passed on to LevelSelector to change the level.

    //public string s;
	public int index;	// changed this to use ints, so loading the levels from other scripts is easier
						// This is set when instatiating from prefab
	/*public void changeString(string s)
    {
        this.s = s;
    }*/

    //This method is called on button pressed.
    public void doButtonThing()
    {
		//LevelSelector.changeLevel(s);
		//LevelSelector.changeLevel(index);
		if (index != GameManager.saveGame.levelNumber) {	// update and save the last level visited if it is different from the current last level visited.
			GameManager.saveGame.levelNumber = index;
			SaveObj.SaveGame(GameManager.settings.saveNum, GameManager.saveGame);
		}
		LevelSelector.changeLevelHelper(index);	// load level with given index number
		GameManager.changeState(GameManager.gameplay, LevelSelector.instance);	// switch to game-mode
	}
}
