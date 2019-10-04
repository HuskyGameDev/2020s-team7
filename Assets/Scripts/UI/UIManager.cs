using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {
    public GameObject Objective;
    public GameObject PauseMenu;
    public GameObject ResumeButton;
    public GameObject GameUI;
    // Use this for initialization
    void Start()
    {
        //Objective.SetActive(true);
        //PauseMenu.SetActive(false);
    }
    void Update()
    {
        /*
        if (InputManager.instance.OnInputDown(InputManager.Action.back))
        {
            //if (PauseMenu.active)
			if (PauseMenu.activeInHierarchy) {
                PauseMenu.SetActive(false);
                GameUI.SetActive(true);
            }
            else
            {
                PauseMenu.SetActive(true);
                GameUI.SetActive(false);
            }
            
            
        }
        
    }
    public void ResumeButtonPressed()
    {
        PauseMenu.SetActive(false);
        GameUI.SetActive(true);

    }
    void ExitButtonPressed()
    {
        Application.Quit();
		Debug.Break();
    }*/
    }

    
}
