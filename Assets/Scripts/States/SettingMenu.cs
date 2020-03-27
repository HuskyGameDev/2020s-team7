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

	public UnityEngine.UI.Button applySettingsButton;	// Used so that values can be retreived when it changes.
	public UnityEngine.UI.Button discardSettingsButton;	// The UI elements will call a method when the value changes,
	public UnityEngine.UI.Dropdown fullScreenDropdown;	// but do not support passing the changed value directly to
	public UnityEngine.UI.Dropdown resolutionDropdown;	// that method.
	public UnityEngine.UI.Toggle vsyncToggle;
	public UnityEngine.UI.Slider volSlider;

	// list of common resolutions, can be changed easily
	public static int[,] resolutions = new int[,] { 
										{640, 360},
										{800, 600},
										{1024, 768},
										{1280, 720},
										{1280, 800},
										{1280, 1024},
										{1360, 768},
										{1366, 768},
										{1440, 900},
										{1536, 864},
										{1600, 900},
										{1680, 1050},
										{1920, 1080},
										{1920, 1200},
										{2048, 1152},
										{2560, 1080},
										{2560, 1440},
										{3440, 1440},
										{3840, 2160},
										{4096, 2160}
										};

	public override GameManager.IStateType _stateType {
		get { return GameManager.IStateType.settingMenu; }
	}

	protected override void _initialize() {
		for (int i = 0; i < (resolutions.Length/2); i++) {	// add all of the resolutions as options to the reolution dropdown
			string text = string.Format("{0} x {1}", resolutions[i,0], resolutions[i, 1]);
			UnityEngine.UI.Dropdown.OptionData data = new UnityEngine.UI.Dropdown.OptionData(text);
			resolutionDropdown.options.Add(data);
		}
	}

	protected override void _StartState(IState oldstate) {
		//Debug.Log("SaveMenu does not do anything in its _StartState() method");
		previous = oldstate;
		discardChanges(); // get fresh copy of current settings
		refresh();
	}

	protected override void _EndState(IState newstate) {
		//Debug.Log("SaveMenu does not do anything in its _EndState() method");
		GameManager.applySettings(GameManager.settings);
	}

	public override void _Update() {
	}

	public override void _RespondToConfirm(int retVal, string retString) {
		switch (retVal) {
			case 0:	// yes, want to leave without saving settings
				onClick(previous);
				FMOD.Studio.Bus bus = FMODUnity.RuntimeManager.GetBus("bus:/");
				bus.setVolume(GameManager.settings.volume);
				break;
			default:    // canceled, do nothing
				break;
		}
	}

	public void setFullscreen() {
		tempSettings.fullscreen = fullScreenDropdown.value;
		refresh();
	}

	public void setResolution() {
		tempSettings.resolutionX = resolutions[resolutionDropdown.value, 0];
		tempSettings.resolutionY = resolutions[resolutionDropdown.value, 1];
		refresh();
	}

	public void setVSync() {
		tempSettings.vsync = vsyncToggle.isOn;
		refresh();
	}

	public void setVolume() {
		tempSettings.volume = volSlider.value;
		FMOD.Studio.Bus bus = FMODUnity.RuntimeManager.GetBus("bus:/");
		bus.setVolume(tempSettings.volume);
		refresh();
	}

	/// <summary>
	/// apply temp settings, so use can see changes.
	/// Change current to false if any temp settings do not match saved settings
	/// </summary>
	public void refresh() {
		current = GameManager.settings.Equals(tempSettings);
		applySettingsButton.interactable = !current;
		discardSettingsButton.interactable = !current;
		fullScreenDropdown.value = tempSettings.fullscreen;
		vsyncToggle.isOn = tempSettings.vsync;
		volSlider.value = tempSettings.volume;
		for (int i = 0; i < (resolutions.Length/2); i++) {
			if (tempSettings.resolutionX == resolutions[i, 0] && tempSettings.resolutionY == resolutions[i, 1]) {
				resolutionDropdown.value = i;
				break;
			}
		}
		GameManager.applySettings(tempSettings);
	}

	public void applySettings() {
		GameManager.settings = tempSettings;
		SettingsObj.saveSettings(GameManager.settings);
		discardChanges();   // so tempSettings doesn't point to the same thing as GameManager.settings
		//current = true;
		refresh();
	}

	public void discardChanges() {
		tempSettings = GameManager.settings.copy();
		refresh();
	}

	public void returnToPrev() {
		if (current) {
			onClick(previous);
		} else {
			GameManager.confirmMenu.setupDialog(0, "Are you sure you want to leave without applying changes?", false, true);
		}
	}
}
