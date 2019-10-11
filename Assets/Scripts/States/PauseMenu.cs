using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : IState {
    /*
     * Pause menu doesn't have anything for the IState methods.
     * Pause menu is only concerned with the buttons and what they do.
     * */
	public override void _StartState() {
		//Debug.Log("PauseMenu does not do anything in its _StartState() method");
	}

	public override void _EndState() {
		//Debug.Log("PauseMenu does not do anything in its _EndState() method");
	}

	public override void _Update()
    {
        
    }

    /*
     * onClick changes to the state specified. Right now it only applies to the level selector.
     * We could add more states since it is dynamic
     * */

    public void onClick(IState g)
    {
        if (!g.Equals(GameManager.instance.gameplay))
        {
            GameManager.instance.changeState(g, this);
            this.gameObject.SetActive(false);
            GameManager.instance.gameplay.gameObject.SetActive(false);
        }
    }

    /*
     * Resumes the game. Had to be different from the onClick method.
     * */
   public void resume()
    {
        GameManager.instance.resumeState(GameManager.instance.gameplay);
        gameObject.SetActive(false);
    }
}
