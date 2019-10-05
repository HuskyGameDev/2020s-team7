using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    //bool animLockout = false;
    public static GameManager instance;
    //public int stringLeft = 21;

    public enum Direction { North, East, South, West }

	//public Node currentPosition = new Node();
	//public Map map;
	//public RenderingHandler nonEuclidRenderer;
	//public bool winTrigger = false;
	//public UnityEngine.UI.Text stringrem;
	//public GameObject wintext;
	//public bool cinimaticMode = false;

	//public string levelName = "006";

	//public float moveAnimSpeed = 0.25f;
	//public float youWinScreenTimeout = 7.0f;

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

		instance = this;
        currentstate = levelselector;

        //levelselector.deactivateAllButtons();
        levelselector.activateButton(levelselector.levelButtons[0]);


        //levelselector.SetActive(true);
        

		InputManager.instance.LoadKeybinds();

		BatchGenerate.GenerateRooms();
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
