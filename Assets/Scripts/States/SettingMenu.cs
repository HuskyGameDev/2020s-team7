using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingMenu : IState {
	/*
	 * this whole state does nothing right now. Will add stuff to it later.
	 */
	private bool current = false;	// if the temp settings match the current settings
	private IState previous;	// previous state, so can return to it.
	private SettingsObj tempSettings;	// temp settings, so can spply discard changes

	public UnityEngine.UI.Button applySettingsButton;
	public UnityEngine.UI.Button discardSettingsButton;

	public override GameManager.IStateType _stateType {
		get { return GameManager.IStateType.settingMenu; }
	}

	public override void _initialize() { }

	public override void _StartState(IState oldstate) {
		this.setBackground(false);
		//Debug.Log("SaveMenu does not do anything in its _StartState() method");
		previous = oldstate;
		discardChanges(); // get fresh copy of current settings
		refresh();
	}

	public override void _EndState(IState newstate) {
		//Debug.Log("SaveMenu does not do anything in its _EndState() method");
	}

	public override void _Update() {
	}

	public override void _RespondToConfirm(int retVal, string retString) {
		switch (retVal) {
			case 0:	// yes, want to leave without saving settings
				onClick(previous);
				break;
			default:    // canceled, do nothing
				break;
		}
	}

	public void refresh() {
		applySettingsButton.interactable = !current;
		discardSettingsButton.interactable = !current;
	}

	public void changedSetting() {
		current = false;
		refresh();
	}

	public void applySettings() {
		GameManager.settings = tempSettings;
		SettingsObj.saveSettings(GameManager.settings);
		discardChanges();   // so tempSettings doesn't point to the same thing as GameManager.settings
		current = true;
		refresh();
	}

	public void discardChanges() {
		tempSettings = GameManager.settings.copy();
	}

	public void returnToPrev() {
		if (current) {
			onClick(previous);
		} else {
			GameManager.confirmMenu.setupDialog(0, "Are you sure you want to leave without applying changes?", false, true);
		}
	}
}
