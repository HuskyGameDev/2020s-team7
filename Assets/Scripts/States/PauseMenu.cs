using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : IState {
	public override void _StartState() {
		Debug.Log("PauseMenu does not do anything in its _StartState() method");
	}

	public override void _EndState() {
		Debug.Log("PauseMenu does not do anything in its _EndState() method");
	}

	public override void _Update()
    {
        
    }

    public void onClick(GameObject g)
    {
        g.SetActive(true);
        this.gameObject.SetActive(false);
    }
   public void resume()
    {
        GameManager.instance.gameplay.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
