using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IState : MonoBehaviour {

    public abstract void _Update();
	public abstract void _StartState();
	public abstract void _EndState();
}
