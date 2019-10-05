using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelGenerator),true)]
public class LevelGeneratorEditor : Editor {

	public override void OnInspectorGUI() {
		if (GUILayout.Button("Generate Render Grid")) {
			//((LevelGenerator)target).CreateAndSaveLevel();
			((LevelGenerator)target).GenerateLevel(); 
		}
		if (GUILayout.Button("Delete Render Grid")) {
			((LevelGenerator)target).DeleteRenderMap();
		}
		base.OnInspectorGUI();
	}

}
