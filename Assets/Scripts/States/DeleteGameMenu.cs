//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeleteGameMenu : IState {
	private IState previous;	// previuos IState, allows it to return to loadMenu or newGameMenu as appropriate
	private int number; // save to delete, will delete it if _RespondToConfirm() receives the accept value
	public UnityEngine.UI.Text[] numUnlockedText = new UnityEngine.UI.Text[3];  // info about each of the three save slots.
	public UnityEngine.UI.Text[] slotNameText = new UnityEngine.UI.Text[3];		// could be changed form more save slots easily.
	public UnityEngine.UI.Button[] slotButton = new UnityEngine.UI.Button[3];
	public UnityEngine.UI.Button returnButton;

	public override GameManager.IStateType _stateType { // override the state-enum return value
		get { return GameManager.IStateType.deleteMenu; }
	}

	protected override void _initialize() { }

	protected override void _StartState(IState oldstate) {
		previous = oldstate;    // get previous state, so can return to it
		firstSelected = returnButton;
		refresh();  // refresh which slots are selectible / info on slots
		//resetSelected();
	}

	protected override void _EndState(IState newstate) {
	}

	public override void _Update() {
	}

	/// <summary>
	/// Called by confirmMenu when returning.
	/// If gets success value, will delete the slot at number, else will do nothing
	/// </summary>
	/// <param name="retVal"></param>
	/// <param name="retString"></param>
	public override void _RespondToConfirm(int retVal, string retString) {	// if get success va
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

	/// <summary>
	/// update which slots are selectible and the info displayed on them
	/// </summary>
	public void refresh() {
		for (int i = 2; i >= 0; i--) {   // for each save:
			if (SaveObj.SaveExists(i+1)) {	// if exists, set name and number-unlocked to match info in save, and make button interactible
				SaveObj save = SaveObj.LoadGame(i+1);
				slotNameText[i].text = save.slotName;
				int count = 0;
				for (int j = 0; j < GameManager.numLevels; j++) {
					if (save.canAccess(j)) count++;
				}
				numUnlockedText[i].text = count + "/" + GameManager.numLevels;
				slotButton[i].interactable = true;
				firstSelected = slotButton[i];
			} else {	// else set text to defualt and set non-interactible
				slotNameText[i].text = "-";
				numUnlockedText[i].text = "~/" + GameManager.numLevels;
				slotButton[i].interactable = false;
			}
		}
	}

	/// <summary>
	/// set up the dialog and return value on the ConfirmMenu
	/// </summary>
	/// <param name="num"></param>
	public void deleteSlot(int num) {
		// ask to confirm delete?
		number = num;
		SaveObj save = SaveObj.LoadGame(num);
		GameManager.confirmMenu.setupDialog(0, "Are you sure you want to delete the save file \"" + save.slotName + "\"", false, true);
	}

	/// <summary>
	/// return to previous istate
	/// </summary>
	public void returnToPrev() {
		//Debug.Log("returnToPrev() does nothing right now");
		onClick(previous);
	}
}
