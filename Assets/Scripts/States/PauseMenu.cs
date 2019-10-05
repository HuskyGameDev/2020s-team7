using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : IState {
	public override void _StartState() {
		//Debug.Log("PauseMenu does not do anything in its _StartState() method");
	}

	public override void _EndState() {
		//Debug.Log("PauseMenu does not do anything in its _EndState() method");
	}

	public override void _Update()
    {
        
    }

    public void onClick(IState g)
    {
        if (!g.Equals(GameManager.instance.gameplay))
        {
            GameManager.instance.changeState(g, this);
            this.gameObject.SetActive(false);
            GameManager.instance.gameplay.gameObject.SetActive(false);
        }
    }
   public void resume()
    {
        GameManager.instance.changeState(GameManager.instance.gameplay, this);
    }
}
