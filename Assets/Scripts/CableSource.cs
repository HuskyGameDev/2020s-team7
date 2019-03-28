using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableSource : MonoBehaviour, Interactable {
    public int length;
    public int GetLength()
    {
        return length;
    }
    public void Interact()
    {
        this.gameObject.SetActive(false); /* maybe keep this  probably not*/
        Debug.Log("You grabbed my tape!");
    }
}
