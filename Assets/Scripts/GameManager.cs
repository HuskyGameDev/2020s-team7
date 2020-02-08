using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {


	#region Variables
	[SerializeField]
	public static GameManager instance;
	public enum Direction { North, East, South, West }
	public enum IStateType {gameMenu, gameplay, newGameMenu, loadMenu, deleteMenu, levelSelector, pauseMenu, settingMenu};
	public static Gameplay gameplay;

	//public static Dictionary<IStateType, IState> istates;    // doesn't show up in the editor
	public static IState[] istates = new IState[System.Enum.GetValues(typeof(IStateType)).Length];
	public static GameObject uiCamera;
	public static ConfirmMenu confirmMenu;

	public static SettingsObj settings;
	public static SaveObj saveGame;

	public static int numLevels = 22;

	[SerializeField]
	private static IState currentstate;

	//public Sprite[] spriteBook;
	public string[] spriteBook;
	#endregion

	public void Awake() {
		#region finding objects/setting refferences
		if ((instance == null) || (instance == this)) {
			instance = this;
		} else {
			Debug.Log("Duplicate GameManager destroyed");
			DestroyImmediate(this.gameObject);
		}

		IState[] objList = (IState[])Resources.FindObjectsOfTypeAll(typeof(IState));
		foreach (IState state in objList) {
			if (istates[(int)state._stateType] == null) {
				istates[(int)state._stateType] = state;
				state.gameObject.SetActive(false);
			} else {
				Debug.Log("Destroyed duplicate of IState " + state._stateType);
				DestroyImmediate(state.gameObject);
			}
		}
		for (int i = 0; i < istates.Length; i++) {
			if (istates[i] == null) {
				Debug.Log("Error: IState object of type \"" + (IStateType)i + "\" not found");
			} else {
				istates[i].initialize();
			}
		}

		ConfirmMenu[] conList = (ConfirmMenu[])Resources.FindObjectsOfTypeAll(typeof(ConfirmMenu));
		if (conList.Length > 0) {
			confirmMenu = conList[0];   // only use one copy of the ConfirmMenu object
			confirmMenu.gameObject.SetActive(false);
		} else {
			Debug.Log("Error: No ConfirmMenu object is exists");
		}
		for (int i = 1; i < conList.Length; i++) DestroyImmediate(conList[i].gameObject);

		Camera[] camList = (Camera[])Resources.FindObjectsOfTypeAll(typeof(Camera));
		foreach (Camera cam in camList) {
			if (cam.gameObject.name == "UI_Camera") {
				if (uiCamera == null) {
					uiCamera = cam.gameObject;
				} else {
					DestroyImmediate(cam.gameObject);
				}
			}
		}
		#endregion
		// load settings stuff, and apply them.
		if (SettingsObj.settingsExists()) {
			settings = SettingsObj.loadSettings();
		} else {
			Debug.Log("Creating new settings file");
			settings = new SettingsObj();
			SettingsObj.saveSettings(settings);
		}

		// load from save if possible, or load default save.
		if (settings.saveNum >= 1 && settings.saveNum <= 3) {
			saveGame = SaveObj.LoadGame(settings.saveNum);
			if (saveGame == null) {
				Debug.Log("Indicated save does not exist, load an existing save or create a new one.");
				settings.saveNum = 0;
				SettingsObj.saveSettings(settings);
			}
		}

		gameplay = (Gameplay)istates[(int)GameManager.IStateType.gameplay];

		/*if (istates[(int)GameManager.IStateType.levelSelector] != null) {
			((LevelSelector)istates[(int)GameManager.IStateType.levelSelector]).initialize();
		}*/
		if (istates[(int)GameManager.IStateType.gameMenu] != null) {
			currentstate = istates[(int)GameManager.IStateType.gameMenu];
			currentstate._StartState(null);
			istates[(int)GameManager.IStateType.gameMenu].gameObject.SetActive(true);
		}

		InputManager.instance.LoadKeybinds();
	}

	public void Start() {
		
		/*
		levelselector.initialize();

		gameplay.gameObject.SetActive(false);
		//gameMenu.gameObject.SetActive(false);
		levelselector.gameObject.SetActive(false);
		pausemenu.gameObject.SetActive(false);
		loadMenu.gameObject.SetActive(false);
		deleteGameMenu.gameObject.SetActive(false);
		newGameMenu.gameObject.SetActive(false);
		settingMenu.gameObject.SetActive(false);
		confirmMenu.gameObject.SetActive(false);

		currentstate = gameMenu;
		currentstate._StartState();
		gameMenu.gameObject.SetActive(true);
		*/
	}

	// Update is called once per frame
	void Update() {
		//Calls the 'update' of the current state
		if (currentstate != null) {
			currentstate._Update();
		} else {
			Debug.Log("currentState is null");
		}
	}

	public static void changeState(IState g, IState p)
    {
		//Changes the state to 'g' and deactivates 'p' if not null
		if (g != null) {
			g.gameObject.SetActive(true);
			g._StartState(p);
		}
		if (p != null) {
			p.gameObject.SetActive(false);
			p._EndState(g);
		}
		currentstate = g;
    }

	public static IState getCurrentState() { return currentstate; }


}
