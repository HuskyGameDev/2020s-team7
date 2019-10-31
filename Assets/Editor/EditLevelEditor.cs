﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EditLevel), true)]
public class EditLevelEditor : Editor {
	private bool gotMap = false;

	public override void OnInspectorGUI() {
		// in the editor, get the reference to the map if you don't already have it
		if (Application.isPlaying && !gotMap) {
			((EditLevel)target).getCurrentMap();
			gotMap = true;
		}

		// A plethora of buttons used to call the methods in the EditLevel class/
		// Do exactly what they says on the tin.
		if (GUILayout.Button("Make New Level")) {
			((EditLevel)target).getNewMap();
		}
		if (GUILayout.Button("Save Level")) {
			((EditLevel)target).saveLevel();
			Debug.Log("WTF!");
		}
		if (GUILayout.Button("Load Level")) {
			((EditLevel)target).loadLevelByName();
			Debug.Log("WTF! v2");
		}

		if (GUILayout.Button("Re-generate levels from scripts")) {
			BatchGenerate.GenerateRooms();
		}
		
		if (GUILayout.Button("Create New Tile-Chunk")) {
			((EditLevel)target).createTileChunk();
		}
		if (GUILayout.Button("Create link from this tile to indicated tile")) {
			((EditLevel)target).createLink();
		}

		if (GUILayout.Button("Delete tile at \"Method Direction\"")) {
			((EditLevel)target).deleteTile();
		}
		if (GUILayout.Button("Set type for this tile")) {
			((EditLevel)target).setType();
		}
		if (GUILayout.Button("Redraw Enviroment")) {
			((EditLevel)target).redraw();
		}
	   
	   if (GUILayout.Button("Apply changes to node")) {
		   ((EditLevel)target).applyToNode();
	   }

	   base.OnInspectorGUI();
	}
}
