using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EditLevel), true)]
public class EditLevelEditor : Editor {

	public override void OnInspectorGUI() {
		if (GUILayout.Button("Generate")) {
			Debug.Log("Doesn't do anything right now");
		}
		if (GUILayout.Button("Get current node")) {
			((EditLevel)target).getCurrentNode();
			//Debug.Log("Doesn't do anything right now");
		}
		if (GUILayout.Button("Apply changes to node")) {
			((EditLevel)target).applyToNode();
			//Debug.Log("Doesn't do anything right now");
		}
		if (GUILayout.Button("Save Level")) {
			((EditLevel)target).saveLevel();
			//Debug.Log("Doesn't do anything right now");
		}
		base.OnInspectorGUI();
	}
}
