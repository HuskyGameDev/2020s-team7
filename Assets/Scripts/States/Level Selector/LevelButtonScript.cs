using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButtonScript : MonoBehaviour {


    //This method is called upon creation of the button in the LevelSelector
    //String 's' is stored in this script (the button) until it is called when pressed. It is then passed on to LevelSelector to change the level.

    string s;
	public void changeString(string s)
    {
        this.s = s;
    }

    //This method is called on button pressed.
    public void doButtonThing()
    {
        LevelSelector.changeLevel(s);
    }
}
