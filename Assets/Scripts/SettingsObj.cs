using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SettingsObj {
	public int saveNum;

	public SettingsObj() {
		saveNum = 0;
	}

	public static bool settingsExists() {
		if (File.Exists(Application.persistentDataPath + "\\Settings.txt")) {
			return true;
		} else {
			return false;
		}
	}

	public static void saveSettings(SettingsObj settings) {
		if (settings == null) {
			throw new System.ArgumentException("Parameter \"settings\" cannot be null");
		}
		Stream stream = null;
		try {
			System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			Debug.Log("Saving to File: \"" + Application.persistentDataPath + "\\Settings.txt\"");
			stream = new FileStream(Application.persistentDataPath + "\\Settings.txt", FileMode.Create, FileAccess.Write);
			formatter.Serialize(stream, settings);
			stream.Close();
		} catch (System.IO.IOException e) {
			Debug.Log(e.Message);
			if (stream != null) {
				stream.Close();
			}
		}
		return;
	}

	public static SettingsObj loadSettings() {
		if (File.Exists(Application.persistentDataPath + "\\Settings.txt")) {
			Stream stream = null;
			try {
				System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				stream = new FileStream(Application.persistentDataPath + "\\Settings.txt", FileMode.Open, FileAccess.Read);
				SettingsObj settings = (SettingsObj)formatter.Deserialize(stream);
				stream.Close();
				return settings;
			} catch (System.IO.IOException e) {
				Debug.Log(e.Message);
				if (stream != null) {
					stream.Close();
				}
				return new SettingsObj();
			}
		} else {
			Debug.Log("Error: Settings file not found.");
			return new SettingsObj();
		}
	}

	public SettingsObj copy() {
		SettingsObj settings = new SettingsObj();
		settings.saveNum = this.saveNum;
		return settings;
	}
}
