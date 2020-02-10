using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMenu : IState {
	public UnityEngine.UI.Text[] numUnlockedText = new UnityEngine.UI.Text[3];
	public UnityEngine.UI.Text[] slotNameText = new UnityEngine.UI.Text[3];
	public UnityEngine.UI.Button[] resumeButton = new UnityEngine.UI.Button[3];

	private LevelSelector levelSelector;
	private Gameplay gameplay;

	public override GameManager.IStateType _stateType {
		get { return GameManager.IStateType.loadMenu; }
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
				resumeButton[i].interactable = true;
			} else {
				slotNameText[i].text = "-";
				numUnlockedText[i].text = "~/" + GameManager.numLevels;
				resumeButton[i].interactable = false;
			}
		}
	}

	public override void _EndState(IState newstate) {
	}

	public override void _Update() { }

	public override void _RespondToConfirm(int retVal, string retString) { }

	public void loadSlot(int num) {
		//GameManager.instance.saveGame;
		//GameManager.instance.settings;
		//Debug.Log("loadSlot() does nothing right now");
		GameManager.settings.saveNum = num;
		GameManager.saveGame = SaveObj.LoadGame(num);
		SettingsObj.saveSettings(GameManager.settings);
		if (GameManager.saveGame.levelNumber >= 0 && GameManager.saveGame.levelNumber < levelSelector.levelButtons.Length) {
			LevelSelector.changeLevelHelper(GameManager.saveGame.levelNumber);
			GameManager.changeState(GameManager.gameplay, this);
		} else {
			onClick(levelSelector);
		}
	}
}
