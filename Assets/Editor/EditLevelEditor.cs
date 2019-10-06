using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EditLevel), true)]
public class EditLevelEditor : Editor {
	private bool gotMap = false;

	public override void OnInspectorGUI() {
		//((EditLevel)target).getCurrentNode();
		if (Application.isPlaying && !gotMap) {
			((EditLevel)target).getCurrentMap();
			gotMap = true;
		}

		if (GUILayout.Button("Make New Level")) {
			((EditLevel)target).getNewMap();
		}
		if (GUILayout.Button("Save Level")) {
			((EditLevel)target).saveLevel();
		}
		if (GUILayout.Button("Load Level")) {
			((EditLevel)target).loadLevelByName();
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
		if (GUILayout.Button("Set this Tile as source")) {
			((EditLevel)target).setSource();
		}
		if (GUILayout.Button("Set this tile as Target")) {
			((EditLevel)target).setTarget();
		}
		if (GUILayout.Button("Apply changes to node")) {
			((EditLevel)target).applyToNode();
		}
		/*
		if (GUILayout.Button("Get Current Node")) {
			((EditLevel)target).getCurrentNode();
		}*/


		base.OnInspectorGUI();
	}
}
