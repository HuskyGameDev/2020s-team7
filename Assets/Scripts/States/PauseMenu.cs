using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : IState {
	/*
     * Pause menu doesn't have anything for the IState methods.
     * Pause menu is only concerned with the buttons and what they do.
     * */
	public override GameManager.IStateType _stateType {
		get { return GameManager.IStateType.pauseMenu; }
	}

	public override void _initialize() { }

	public override void _StartState(IState oldstate) {
		this.setBackground(false);
		//Debug.Log("PauseMenu does not do anything in its _StartState() method");
	}

	public override void _EndState(IState newstate) {
		if (!(newstate is Gameplay)) {
			GameManager.gameplay.gameObject.SetActive(false);
		}
		//Debug.Log("PauseMenu does not do anything in its _EndState() method");
	}

	public override void _Update() {
		if (InputManager.instance.OnInputDown(InputManager.Action.back)) {
			GameManager.changeState(GameManager.gameplay, this);
		}
	}

	public override void _RespondToConfirm(int retVal, string retString) { }

	/*
     * Resumes the game. Had to be different from the onClick method.
     * */
   /*public void resume() {
		//GameManager.instance.resumeState(GameManager.instance.gameplay);
		GameManager.changeState(GameManager.gameplay, null);
		GameManager.gameplay.setBackground(false);
		this.gameObject.SetActive(false);
    }*/
}
