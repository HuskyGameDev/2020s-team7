using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IState : MonoBehaviour {
	/*
     * This is the state class. All states implement this class
     * They have an update method which is called by GameManager instead of the unity engine.
     * They also have a start state method and end state method
    */

	protected int dialogReturnVal = -1;
	protected string returnString;
	protected Component[] uiButtons;
	public abstract GameManager.IStateType _stateType {
		get;
	}

	public void initialize() {
		uiButtons = gameObject.GetComponentsInChildren(typeof(UnityEngine.UI.Button), true);
		_initialize();
	}

	public abstract void _initialize();

	public abstract void _Update();
	public abstract void _StartState(IState oldState);
	public abstract void _EndState(IState newState);
	
	public abstract void _RespondToConfirm(int retVal, string retString);

	public void setBackground(bool background) {
		foreach (UnityEngine.UI.Button b in uiButtons) {
			b.interactable = !background;
		}
	}

	public void onClick(IState g) {
		GameManager.changeState(g, this);
		/*if (!g.Equals(GameManager.istates[(int)GameManager.IStateType.gameplay])) {
			GameManager.changeState(g, this);
			this.gameObject.SetActive(false);
			GameManager.istates[(int)GameManager.IStateType.gameplay].gameObject.SetActive(false);
		}*/
	}
}
