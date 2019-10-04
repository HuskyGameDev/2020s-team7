using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : IState {

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
        GameManager.instance.gameplay.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
