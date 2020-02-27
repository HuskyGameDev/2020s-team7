using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public string sceneToLoad;

	public void StartGame() {
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single); 
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
}
