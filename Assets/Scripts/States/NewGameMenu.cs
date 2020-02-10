using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameMenu : IState {
	public UnityEngine.UI.Text[] numUnlockedText = new UnityEngine.UI.Text[3];
	public UnityEngine.UI.Text[] slotNameText = new UnityEngine.UI.Text[3];
	public UnityEngine.UI.Button[] resumeButton = new UnityEngine.UI.Button[3];
	private int number;
	private LevelSelector levelSelector;
	private Gameplay gameplay;

	public override GameManager.IStateType _stateType {
		get { return GameManager.IStateType.newGameMenu; }
	}

	public override void _initialize() {
		levelSelector = (LevelSelector)GameManager.istates[(int)GameManager.IStateType.levelSelector];
		//gameplay = (Gameplay) GameManager.istates[(int)GameManager.IStateType.levelSelector];
	}

	public override void _StartState(IState oldstate) {
		this.setBackground(false);
		for (int i = 0; i < 3; i++) {
			if (SaveObj.SaveExists(i+1)) {
				SaveObj save = SaveObj.LoadGame(i+1);
				slotNameText[i].text = save.slotName;
				int count = 0;
				for (int j = 0; j < GameManager.numLevels; j++) {
					if (save.canAccess(j)) count++;
				}
				numUnlockedText[i].text = count + "/" + GameManager.numLevels;
				resumeButton[i].interactable = false;
			} else {
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

	public void newSlot(int num) {
		//Debug.Log("newSlot() does nothing right now");
		// change this to allow somebody to input a name
		number = num;
		GameManager.confirmMenu.setupDialog(0, "Create save slot with this name:", true, true);
	}
}
