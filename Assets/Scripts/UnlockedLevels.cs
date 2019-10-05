using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockedLevels : IState
{
    Button button;
    public override void _EndState()
    {
        
    }

    public override void _StartState()
    {
        
    }

    

    public override void _Update()
    {
        button.interactable = true;
    }
}
