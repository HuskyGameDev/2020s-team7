using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButtonScript : MonoBehaviour {

    string s;
	public void changeString(string s)
    {
        this.s = s;
    }
    public void doButtonThing()
    {
        LevelSelector.changeLevel(s);
    }
}
