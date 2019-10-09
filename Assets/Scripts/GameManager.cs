using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    //bool animLockout = false;
	[SerializeField]
    public static GameManager instance;
    //public int stringLeft = 21;

    public enum Direction { North, East, South, West }
	

	#region States

	public GameObject mainmenu;
    public Gameplay gameplay;
    public GameObject lscamera;

    public LevelSelector levelselector;
    public PauseMenu pausemenu;

    private IState currentstate;
    #endregion


    //public Sprite[] spriteBook;
    public string[] spriteBook;
	// Use this for initialization

	void Start() {
		if ((instance == null) || (instance == this)) {
			instance = this;
		} else {
			DestroyImmediate(this.gameObject);
		}
		
        currentstate = levelselector;
        currentstate._StartState();

        //levelselector.deactivateAllButtons();
        //levelselector.activateButton(levelselector.levelButtons[0]);


        //levelselector.SetActive(true);
        

		InputManager.instance.LoadKeybinds();

		//BatchGenerate.GenerateRooms();

		/*
		if (File.Exists(Application.dataPath + "/Levels/room_" + levelName)) {
			map = Map.Load(Application.dataPath + "/Levels/room_" + levelName);
		} else {
			//Debug.Log("Error: Map file does not exist at path \"" + Application.dataPath + "/Levels/room_" + levelName + "\"");
		}*/

		//nonEuclidRenderer.HandleRender(Direction.East, currentPosition, false);
	}


	
	// Update is called once per frame
	void Update () {
		currentstate._Update();
    }

    public void changeState(IState g, IState p)
    {
		//Debug.Log("Changing state");
		
		

		if (g != null) {
			g._StartState();
			g.gameObject.SetActive(true);
		}
		if (p != null) {
			p._EndState();
			p.gameObject.SetActive(false);
		}
		currentstate = g;
    }


}
