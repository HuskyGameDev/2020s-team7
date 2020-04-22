using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : IState {
	/*
     * Pause menu doesn't have anything for the IState methods.
     * Pause menu is only concerned with the buttons and what they do.
     * */
	public override GameManager.IStateType _stateType { // override the state-enum return value
		get { return GameManager.IStateType.pauseMenu; }
	}

	protected override void _initialize() { }

	protected override void _StartState(IState oldstate) {
		//Debug.Log("PauseMenu starting state...");
	}

	protected override void _EndState(IState newstate) {
		if (!(newstate is Gameplay)) {	// make sure gameplay is set inactive when leaving gameplay state, normaly it is still visible behind pause state
			GameManager.gameplay.gameObject.SetActive(false);
		}
		//Debug.Log("PauseMenu ending state...");
	}

	public override void _Update() {    // accept "esc"/back key input to return to gameplay, same as "esc" key will pause when in gameplay state
		if (Input.GetButtonDown("Back")) {
			GameManager.changeState(GameManager.gameplay, this);
		}
		/*if (InputManager.instance.OnInputDown(InputManager.Action.back)) {
			GameManager.changeState(GameManager.gameplay, this);
		}*/
	}

	public override void _RespondToConfirm(int retVal, string retString) { }

}
