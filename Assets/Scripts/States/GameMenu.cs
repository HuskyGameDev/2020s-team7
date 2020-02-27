using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : IState {
	//public int numLevels = 22;
	public UnityEngine.UI.Text numUnlockedText;	// text for current save slot
	public UnityEngine.UI.Text slotNameText;
	public UnityEngine.UI.Button resumeButton;	// buttons for resume and level select. Disabled if no save is currently selected
	public UnityEngine.UI.Button levelSelectButton;
	private LevelSelector levelSelector;    // local reference to levelSelector, setup in _initialize()

	public override GameManager.IStateType _stateType {
		get { return GameManager.IStateType.gameMenu; }
	}

	protected override void _initialize() {
		levelSelector = (LevelSelector)GameManager.istates[(int)GameManager.IStateType.levelSelector];
	}

	protected override void _StartState(IState oldstate) {
		if (GameManager.settings.saveNum < 1 ||		// set up save slot info display
				GameManager.settings.saveNum > 3 || 
				!SaveObj.SaveExists(GameManager.settings.saveNum)) {
			slotNameText.text = "-";
			numUnlockedText.text = "~/" + GameManager.numLevels;
			resumeButton.interactable = false;	// set buttons unusable
			levelSelectButton.interactable = false;
		} else {
			slotNameText.text = GameManager.saveGame.slotName;
			int count = 0;
			for (int i = 0; i < GameManager.numLevels; i++) {
				if (GameManager.saveGame.canAccess(i)) count++;
			}
			numUnlockedText.text = count + "/" + GameManager.numLevels;
			resumeButton.interactable = true;	// set buttons accesible
			levelSelectButton.interactable = true;
		}
	}

	protected override void _EndState(IState newstate) {
		//Debug.Log("SaveMenu does not do anything in its _EndState() method");
	}

	public override void _Update() { }

	public override void _RespondToConfirm(int retVal, string retString) { }

	/// <summary>
	/// load straight to last-visited-level if it is set to a valid number, else go to level selector
	/// </summary>
	public void resumeGame() {
		//Debug.Log("Resume Game does nothing right now");
		if (GameManager.saveGame.levelNumber >= 0 && GameManager.saveGame.levelNumber < levelSelector.levelButtons.Length) {
			LevelSelector.changeLevelHelper(GameManager.saveGame.levelNumber);
			GameManager.changeState(GameManager.gameplay, this);
		} else {
			onClick(levelSelector);
		}
	}

	/// <summary>
	/// quit the game/exit play mode on the editor
	/// </summary>
	public void quitGame() {
		//Debug.Log("Quit does nothing right now");
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
}
