using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {


	#region Variables
	[SerializeField]
	public static GameManager instance;
	public enum Direction { North, East, South, West }  // north = 0, east = 1, South = 2, West = 3
	public enum IStateType {gameMenu, gameplay, newGameMenu, loadMenu, deleteMenu, levelSelector, pauseMenu, settingMenu};	// enum each of the current Istates
	public static Gameplay gameplay;	// this is needed pretty often, so keep a reference to it on the game manager

	//public static Dictionary<IStateType, IState> istates;    // doesn't show up in the editor
	public static IState[] istates = new IState[System.Enum.GetValues(typeof(IStateType)).Length];	// list of all istates in the scene
	public static ConfirmMenu confirmMenu;	// not an Istate, so needs its own reference

	public static SettingsObj settings;	// the current settings
	public static SaveObj saveGame; // the current save info

	public static EventSystem eventSystem;	// used for checking if the currentSelectedObject of the UI navigation is null

	//public FMOD.studio.bus;	// once I know what name to look-up, I can use this to effect the volume

	public static int numLevels = 0;	// 0 is a placeholder, is updated by levelSelector when it instantiates its buttons

	[SerializeField]
	private static IState currentstate; // current Istate. this istate's _update() is called every update, every other istate's _update() is not

	//public Sprite[] spriteBook;
	[SerializeField]
	private Sprite[] _spriteBook;   // used because unity doesn't make static variables visible in the editor
	public static Sprite[] spriteBook;
	[SerializeField]
	private Sprite _errorSprite;	// used because unity doesn't make static variables visible in the editor
	public static Sprite errorSprite;
	public static Dictionary<string, int> spriteIndex = new Dictionary<string, int>(); 
	#endregion

	public void Awake() {
		#region finding objects/setting refferences
		if ((instance == null) || (instance == this)) {	// singleton pattern
			instance = this;
		} else {
			Debug.Log("Duplicate GameManager destroyed");
			DestroyImmediate(this.gameObject);
		}

		IState[] objList = (IState[])Resources.FindObjectsOfTypeAll(typeof(IState));	// find all IState objects
		// should be easily tweakable to work when switching between scenes
		foreach (IState state in objList) {
			if (istates[(int)state._stateType] == null) {	// add one of each type to list of istates, delete any duplicates
				istates[(int)state._stateType] = state;	// the type enum is used to put each istate in a unique nd predictable slot
				state.gameObject.SetActive(false);
			} else {
				Debug.Log("Destroyed duplicate of IState " + state._stateType);
				DestroyImmediate(state.gameObject);
			}
		}
		for (int i = 0; i < istates.Length; i++) {	// check that one of each type exists
			if (istates[i] == null) {
				Debug.Log("Error: IState object of type \"" + (IStateType)i + "\" not found");
			} else {
				istates[i].initialize();	// initialize each that exists
			}
		}

		ConfirmMenu[] conList = (ConfirmMenu[])Resources.FindObjectsOfTypeAll(typeof(ConfirmMenu));
		if (conList.Length > 0) {
			confirmMenu = conList[0];   // only use one copy of the ConfirmMenu object
			confirmMenu.gameObject.SetActive(false);
		} else {
			Debug.Log("Error: No ConfirmMenu object is exists");
		}
		for (int i = 1; i < conList.Length; i++) DestroyImmediate(conList[i].gameObject);   // destroy extras


		#endregion

		//settup spritebook & index
		spriteBook = _spriteBook;	// move info from non-static variables to static variables
		errorSprite = _errorSprite;
		for (int s = 0; s < spriteBook.Length; s++) {	// get the index and name of every sprite, so you can look them up by name
			if (spriteBook[s] != null) {
				string lowercase = spriteBook[s].name.ToLowerInvariant();
				//Debug.Log(string.Format("Trying to add sprite {0} to sprite index", lowercase));
				if (!spriteIndex.ContainsKey(lowercase)) {	// don't add any duplicate names
					spriteIndex.Add(lowercase, s);
				} else {
					throw new System.Exception("Error: sprite with name" + spriteBook[s].name + " present in spritebook more than once");
				}
			}
		}

		// load settings stuff, and apply them.
		if (SettingsObj.settingsExists()) {	//load the settings if the file exists
			settings = SettingsObj.loadSettings();
		} else {	// else create new file
			Debug.Log("Creating new settings file");
			settings = new SettingsObj();	
			SettingsObj.saveSettings(settings);
		}
		applySettings(settings);    // apply the saved settings
		// consider changing so that it keeps resolution settings from start-up menu

		// load from save if possible, or load default save.
		if (settings.saveNum >= 1 && settings.saveNum <= 3) {   // if saveNum = 1, 2, or 3, savefile should exist, else one won't
			saveGame = SaveObj.LoadGame(settings.saveNum);
			if (saveGame == null) {
				Debug.Log("Indicated save does not exist, load an existing save or create a new one.");
				settings.saveNum = 0;
				SettingsObj.saveSettings(settings);
			}
		}
		

		gameplay = (Gameplay)istates[(int)GameManager.IStateType.gameplay]; // get gameplay IState from istate[Enum.gameplay] slot of array

		if (istates[(int)GameManager.IStateType.gameMenu] != null) {	// set current state to gamemenu, or complain if gameMenu istate doesn't exist
			currentstate = istates[(int)GameManager.IStateType.gameMenu];
			currentstate.StartState(null);
			istates[(int)GameManager.IStateType.gameMenu].gameObject.SetActive(true);
		}

		//InputManager.instance.LoadKeybinds();   // load keybindings

		eventSystem = (EventSystem)FindObjectOfType(typeof(EventSystem));
	}

	/// <summary>
	/// Call this whenever settings are changed.
	/// </summary>
	/// <param name="setObj"></param>
	public static void applySettings(SettingsObj setObj) {
		//Debug.Log("Warning: Volume settings do nothing right now");
		
		//masterChannel.setVolume(volume);
		switch (setObj.fullscreen) {
			case 0:
				Screen.SetResolution(setObj.resolutionX, setObj.resolutionY, FullScreenMode.Windowed);
				break;
			case 1:
				Screen.SetResolution(setObj.resolutionX, setObj.resolutionY, FullScreenMode.MaximizedWindow);
				break;
			case 2:
				Screen.SetResolution(setObj.resolutionX, setObj.resolutionY, FullScreenMode.FullScreenWindow);
				break;
			case 3:
				Screen.SetResolution(setObj.resolutionX, setObj.resolutionY, FullScreenMode.ExclusiveFullScreen);
				break;
			default:
				Screen.SetResolution(setObj.resolutionX, setObj.resolutionY, FullScreenMode.Windowed);
				break;
		}
		QualitySettings.vSyncCount = (setObj.vsync ? 1 : 0);
	}

	// Update is called once per frame
	void Update() {
		//Calls the 'update' of the current state
		if (currentstate != null) {
			currentstate._Update();
			if (GameManager.eventSystem.currentSelectedGameObject == null) {
				currentstate.resetSelected();
			}
		} else {
			Debug.Log("currentState is null");
		}
	}

	public static void changeState(IState g, IState p)
    {
		
		//Changes the state to 'g' and deactivates 'p' if not null
		if (g != null) {
			//Debug.Log("GameManagerManager: Switching to " + g._stateType.ToString());
			g.gameObject.SetActive(true);   // set active first, so Istate can overrride it in it's _StartState method if needed.
			g.StartState(p);	// do general things, and also things that are specific depending on the old state
		}
		if (p != null) {
			p.gameObject.SetActive(false);   // set inactive first, so Istate can overrride it in it's _EndState method if needed.
			p.EndState(g); // do general things, and also things that are specific depending on the new state
		}
		currentstate = g;
    }

	public static IState getCurrentState() { return currentstate; }	// accesor for private variable


	/// <summary>
	/// get a sprite from the spritebook by name.
	/// Ignores case
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	public static Sprite getSprite(string name) {
		name = name.ToLowerInvariant();
		if (spriteIndex.ContainsKey(name)) {
			return spriteBook[spriteIndex[name]];
		} else {
			return errorSprite;	// return this if can't find name in spritebook
		}
		
	}
}
