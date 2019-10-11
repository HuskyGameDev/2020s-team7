using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IState : MonoBehaviour {
    /*
     * This is the state class. All states implement this class
     * They have an update method which is called by GameManager instead of the unity engine.
     * They also have a start state method and end state method
    */

    public abstract void _Update();
	public abstract void _StartState();
	public abstract void _EndState();
}
