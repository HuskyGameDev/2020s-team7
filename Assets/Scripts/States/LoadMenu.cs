using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMenu : IState {
	public UnityEngine.UI.Text[] numUnlockedText = new UnityEngine.UI.Text[3];	// info about each of the three save slots.
	public UnityEngine.UI.Text[] slotNameText = new UnityEngine.UI.Text[3];		// could be changed form more save slots easily.
	public UnityEngine.UI.Button[] resumeButton = new UnityEngine.UI.Button[3];

	private LevelSelector levelSelector;	// local reference to levelSelector, setup in _initialize()

	public override GameManager.IStateType _stateType {	// override the state-enum return value
		get { return GameManager.IStateType.loadMenu; }
	}

	protected override void _initialize() {	// initialize specific to this istate
		levelSelector = (LevelSelector)GameManager.istates[(int)GameManager.IStateType.levelSelector];
	}

	protected override void _StartState(IState oldstate) {
		for (int i = 0; i < 3; i++) {	// for each potential save:
			if (SaveObj.SaveExists(i+1)) {	// if it exists, set the button active, and set the name and levels-unlocked text to the values in the save
				SaveObj save = SaveObj.LoadGame(i+1);
				slotNameText[i].text = save.slotName;
				int count = 0;
				for (int j = 0; j < GameManager.numLevels; j++) {
					if (save.canAccess(j)) count++;
				}
				numUnlockedText[i].text = count + "/" + GameManager.numLevels;
				resumeButton[i].interactable = true;
			} else {    // if it doesn't exists, set the button non-interactive, and set the name and levels-unlocked text to default values
				slotNameText[i].text = "-";
				numUnlockedText[i].text = "~/" + GameManager.numLevels;
				resumeButton[i].interactable = false;
			}
		}
	}

	protected override void _EndState(IState newstate) {
	}

	public override void _Update() { }

	public override void _RespondToConfirm(int retVal, string retString) { }

	/// <summary>
	/// 
	/// </summary>
	/// <param name="num"></param>
	public void loadSlot(int num) {
		GameManager.saveGame = SaveObj.LoadGame(num);
		if (GameManager.saveGame == null) {	// complain if save file doesn't exist/error
			// give file-doesn't exist notification here
			Debug.Log("Error: Can't find save file \"" + num + "\"");
			return;
		}
		GameManager.settings.saveNum = num;	// set the last-save-used to this num, and save that change
		SettingsObj.saveSettings(GameManager.settings);
		if (GameManager.saveGame.levelNumber >= 0 && GameManager.saveGame.levelNumber < levelSelector.levelButtons.Length) {	// load straight to the last visited level if it exists
			LevelSelector.changeLevelHelper(GameManager.saveGame.levelNumber);
			GameManager.changeState(GameManager.gameplay, this);
		} else {
			onClick(levelSelector);	// else, go to level selector
		}
	}
}
