using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IState : MonoBehaviour {
	/*
     * This is the state class. All states implement this class
     * They have an update method which is called by GameManager instead of the unity engine.
     * They also have a start state method and end state method
    */

	protected int dialogReturnVal = -1; // value returned by confirmMenu, -1 or unassigned values indicate a negative/neutral, 
										// with anything else indicating accept, with the exact value indicating what it was that was accepted.
	protected string returnString;	// string returned by the confirm menu, used for slot names right now, possibly more in the future
	protected Component[] uiButtons;	// list of buttons under this object. They are disabled when this object is put in the background, so that they are visible but not interactible.
	public abstract GameManager.IStateType _stateType {	// used to check if every state that should exist does exist. Each state has an associated enum
		get;
	}

	public void initialize() {	// do the generic initialize stuff, and also the specific initialize stuff
		uiButtons = gameObject.GetComponentsInChildren(typeof(UnityEngine.UI.Button), true);
		_initialize();
	}

	public abstract void _initialize();	// specific initilize stuff goes here.

	public abstract void _Update();	// any update stuff for this state goes here. Right now only Gameplay uses it.
	public abstract void _StartState(IState oldState);	// called when switching to a state. Gets old state as arguement, so can do things depending on the type
	public abstract void _EndState(IState newState);    // called when switching away from a state. Gets new state as arguement, so can do things depending on the type

	public abstract void _RespondToConfirm(int retVal, string retString);	// called by the confirmMenu on whatever set-up the confirm menu

	public void setBackground(bool background) {	// sets all buttons active/not active
		foreach (UnityEngine.UI.Button b in uiButtons) {
			b.interactable = !background;
		}
	}

	public void onClick(IState g) {	// makes it easy to switch from this state to amother using untiy UI buttons
		GameManager.changeState(g, this);
		/*if (!g.Equals(GameManager.istates[(int)GameManager.IStateType.gameplay])) {
			GameManager.changeState(g, this);
			this.gameObject.SetActive(false);
			GameManager.istates[(int)GameManager.IStateType.gameplay].gameObject.SetActive(false);
		}*/
	}
}
