using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSCamera : MonoBehaviour {

    // Use this for initialization
    

    public static void toState(GameObject g, GameObject p)
    {
        g.SetActive(true);
        p.SetActive(false);
    }

    public void _Update()
    {
        
    }
}
