using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public GameObject startGame;
    public GameObject quitGame;

    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene_correct", LoadSceneMode.Single); 
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
