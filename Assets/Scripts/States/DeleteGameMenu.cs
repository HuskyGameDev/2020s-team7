//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteGameMenu : IState {
	private IState previous;
	private int number;
	public UnityEngine.UI.Text[] numUnlockedText = new UnityEngine.UI.Text[3];
	public UnityEngine.UI.Text[] slotNameText = new UnityEngine.UI.Text[3];
	public UnityEngine.UI.Button[] resumeButton = new UnityEngine.UI.Button[3];

	public override GameManager.IStateType _stateType {
		get { return GameManager.IStateType.deleteMenu; }
	}

	public override void _initialize() { }

	public override void _StartState(IState oldstate) {
		this.setBackground(false);
		previous = oldstate;
		refresh();
	}

	public override void _EndState(IState newstate) {
	}

	public override void _Update() {
	}

	public override void _RespondToConfirm(int retVal, string retString) {
		switch(retVal) {
			case 0 :    // accept delete
				if (number == GameManager.settings.saveNum) {
					GameManager.settings.saveNum = 0;
					SettingsObj.saveSettings(GameManager.settings);
					GameManager.saveGame = null;
				}
				SaveObj.DeleteGame(number);
				//refresh();
				onClick(previous);
				break;
			default :	// canceled, do nothing
				//refresh();
				break;
		}
	}

	public void refresh() {
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

	public void deleteSlot(int num) {
		// ask to confirm delete?
		number = num;
		SaveObj save = SaveObj.LoadGame(num);
		GameManager.confirmMenu.setupDialog(0, "Are you sure you want to delete the save file \"" + save.slotName + "\"", false, true);
		
		/*
		if (num == GameManager.instance.settings.saveNum) {
			GameManager.instance.settings.saveNum = 0;
			GameManager.instance.saveGame = null;
		}
		SaveObj.DeleteGame(num);
		//refresh();
		onClick(previous);*/
	}

	public void returnToPrev() {
		//Debug.Log("returnToPrev() does nothing right now");
		onClick(previous);
	}
}
