using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	[SerializeField]
    public static GameManager instance;
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
	void Start() {
		if ((instance == null) || (instance == this)) {
			instance = this;
		} else {
			DestroyImmediate(this.gameObject);
		}
		
        currentstate = levelselector;
        currentstate._StartState();

        //Uncomment these when ready for level progression
        //levelselector.deactivateAllButtons();
        //levelselector.activateButton(levelselector.levelButtons[0]);
        //levelselector.gameObject.SetActive(true);
        InputManager.instance.LoadKeybinds();
	}


	
	// Update is called once per frame
	void Update () {
        //Calls the 'update' of the current state
		currentstate._Update();
    }

    public void changeState(IState g, IState p)
    {
		//Changes the state to 'g' and deactivates 'p' if not null
		
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

    public void resumeState(IState g)
    {
        currentstate = g;
    }


}
