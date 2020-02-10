using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : IState {
	//public int numLevels = 22;
	public UnityEngine.UI.Text numUnlockedText;
	public UnityEngine.UI.Text slotNameText;
	public UnityEngine.UI.Button resumeButton;
	public UnityEngine.UI.Button saveButton;
	//public bool saveFound;
	//public bool settingsFound;
	private LevelSelector levelSelector;
	private Gameplay gameplay;

	public override GameManager.IStateType _stateType {
		get { return GameManager.IStateType.gameMenu; }
	}

	public override void _initialize() {
		levelSelector = (LevelSelector)GameManager.istates[(int)GameManager.IStateType.levelSelector];
		//gameplay = (Gameplay) GameManager.istates[(int)GameManager.IStateType.levelSelector];
	}

	public override void _StartState(IState oldstate) {
		this.setBackground(false);
		//Debug.Log("SaveMenu does not do anything in its _StartState() method");
		if (GameManager.settings.saveNum < 1 || 
				GameManager.settings.saveNum > 3 || 
				!SaveObj.SaveExists(GameManager.settings.saveNum)) {
			slotNameText.text = "-";
			numUnlockedText.text = "~/" + GameManager.numLevels;
			resumeButton.interactable = false;
			saveButton.interactable = false;
		} else {
			slotNameText.text = GameManager.saveGame.slotName;
			int count = 0;
			for (int i = 0; i < GameManager.numLevels; i++) {
				if (GameManager.saveGame.canAccess(i)) count++;
			}
			numUnlockedText.text = count + "/" + GameManager.numLevels;
			resumeButton.interactable = true;
			saveButton.interactable = true;
		}
	}

	public override void _EndState(IState newstate) {
		//Debug.Log("SaveMenu does not do anything in its _EndState() method");
	}

	public override void _Update() { }

	public override void _RespondToConfirm(int retVal, string retString) { }

	public void resumeGame() {
		//Debug.Log("Resume Game does nothing right now");
		// Go to level GameManager.instance.saveGame.levelNumber if it is not negative?
		if (GameManager.saveGame.levelNumber >= 0 && GameManager.saveGame.levelNumber < levelSelector.levelButtons.Length) {
			LevelSelector.changeLevelHelper(GameManager.saveGame.levelNumber);
			GameManager.changeState(GameManager.gameplay, this);
		} else {
			onClick(levelSelector);
		}
	}

	/*public void newGame() {
		Debug.Log("New Game does nothing right now");
	}*/
	/*
	public void saveGame() {
		SaveObj.SaveGame(GameManager.settings.saveNum, GameManager.saveGame);
		Debug.Log("Saved the game");
	}*/

	public void quitGame() {
		//Debug.Log("Quit does nothing right now");
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
}
