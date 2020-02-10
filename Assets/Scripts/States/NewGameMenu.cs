using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameMenu : IState {
	public UnityEngine.UI.Text[] numUnlockedText = new UnityEngine.UI.Text[3];// info about each of the three save slots.
	public UnityEngine.UI.Text[] slotNameText = new UnityEngine.UI.Text[3];     // could be changed form more save slots easily.
	public UnityEngine.UI.Button[] resumeButton = new UnityEngine.UI.Button[3];
	private int number;
	private LevelSelector levelSelector;    // local reference to levelSelector, setup in _initialize()

	public override GameManager.IStateType _stateType { // override the state-enum return value
		get { return GameManager.IStateType.newGameMenu; }
	}

	public override void _initialize() {
		levelSelector = (LevelSelector)GameManager.istates[(int)GameManager.IStateType.levelSelector];
	}

	public override void _StartState(IState oldstate) {
		this.setBackground(false);  // make sure buttons are active
		for (int i = 0; i < 3; i++) {   // for each potential save:
			if (SaveObj.SaveExists(i+1)) {  // if exists, set name and number-unlocked to match info in save, and make button non-interactible
				SaveObj save = SaveObj.LoadGame(i+1);
				slotNameText[i].text = save.slotName;
				int count = 0;
				for (int j = 0; j < GameManager.numLevels; j++) {
					if (save.canAccess(j)) count++;
				}
				numUnlockedText[i].text = count + "/" + GameManager.numLevels;
				resumeButton[i].interactable = false;
			} else {    // else set text to defualt and set interactible
				slotNameText[i].text = "-";
				numUnlockedText[i].text = "~/" + GameManager.numLevels;
				resumeButton[i].interactable = true;
			}
		}
	}

	public override void _EndState(IState newstate) {
	}

	public override void _Update() {
	}

	/// <summary>
	/// Called by confirmMenu when returning.
	/// If gets success value, will create save the slot at number, else will do nothing
	/// </summary>
	/// <param name="retVal"></param>
	/// <param name="retString"></param>
	public override void _RespondToConfirm(int retVal, string retString) {
		switch (retVal) {
			case 0:
				if (GameManager.settings.saveNum <= 3 && GameManager.settings.saveNum >= 1 && GameManager.saveGame != null) {   // save existing game if it exists
					SaveObj.SaveGame(GameManager.settings.saveNum, GameManager.saveGame);
				}

				GameManager.saveGame = new SaveObj(!retString.Equals("") ? retString : "Slot " + number);
				GameManager.settings.saveNum = number;
				SaveObj.SaveGame(number, GameManager.saveGame);
				SettingsObj.saveSettings(GameManager.settings);
				if (GameManager.saveGame.levelNumber >= 0 && GameManager.saveGame.levelNumber < levelSelector.levelButtons.Length) {
					LevelSelector.changeLevelHelper(GameManager.saveGame.levelNumber);
					GameManager.changeState(GameManager.gameplay, this);
				} else {
					onClick(levelSelector);
				}
				break;
			default:	// canceled, do nothing
				break;
		}
	}

	/// <summary>
	/// set up the dialog and return value on the ConfirmMenu
	/// </summary>
	/// <param name="num"></param>
	public void newSlot(int num) {
		//Debug.Log("newSlot() does nothing right now");
		// change this to allow somebody to input a name
		number = num;
		GameManager.confirmMenu.setupDialog(0, "Create save slot with this name:", true, true);
	}
}
