using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelGenerator),true)]
public class LevelGeneratorEditor : Editor {

	public override void OnInspectorGUI() {
		if (GUILayout.Button("Generate")) {
			((LevelGenerator)target).CreateAndSaveLevel();
		}
		base.OnInspectorGUI();
	}
}
