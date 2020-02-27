using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SettingsObj {
	/*
	 * This class records settings, so that they may persist between instances of the game.
	 * Has methods for saving and loading the objects that hold these setting variables to a standard location.
	 */
	public int saveNum; // the save used last time. [1-3] are valid, values less than 0 or larger than 3 will cause no save to be loaded.
	public float volume;
	public int resolutionX;
	public int resolutionY;
	public bool vsync;
	public int fullscreen;

	public SettingsObj() {	// constructor. By default, no save was last used.
		saveNum = 0;
		volume = 1.0f;
		//resolutionX = 640;
		//resolutionY = 480;

		vsync = false;
		fullscreen = 0;
	}

	public static bool settingsExists() {	// returns true if a settings file exists in the standard location
		if (File.Exists(Application.persistentDataPath + "\\Settings.txt")) {
			return true;
		} else {
			return false;
		}
	}

	/// <summary>
	/// Saves the given settings object to the standard location
	/// </summary>
	/// <param name="settings"></param>
	public static void saveSettings(SettingsObj settings) {
		if (settings == null) {	// can't save the object if there is no object
			throw new System.ArgumentException("Parameter \"settings\" cannot be null");
		}
		Stream stream = null;
		try {	// crashing due to file access issues is a bitch, so don't crash
			System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			//Debug.Log("Saving to File: \"" + Application.persistentDataPath + "\\Settings.txt\"");
			stream = new FileStream(Application.persistentDataPath + "\\Settings.txt", FileMode.Create, FileAccess.Write);
			formatter.Serialize(stream, settings);
			stream.Close();
		} catch (System.Exception e) {
			Debug.Log(e.Message);	// notify me about any exceptions.
			if (stream != null) {	// make sure the stream is closed if an exception was generated.
				stream.Close();
			}
		}
		return;
	}

	/// <summary>
	/// Load a settings object from the standard location
	/// </summary>
	/// <returns></returns>
	public static SettingsObj loadSettings() {
		if (File.Exists(Application.persistentDataPath + "\\Settings.txt")) {
			Stream stream = null;
			try {	// don't want it to crash if there are issues
				System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				stream = new FileStream(Application.persistentDataPath + "\\Settings.txt", FileMode.Open, FileAccess.Read);
				SettingsObj settings = (SettingsObj)formatter.Deserialize(stream);
				stream.Close();
				return settings;
			} catch (System.Exception e) {
				Debug.Log(e.Message);   // notify me about any exceptions.
				if (stream != null) {   // make sure the stream is closed if an exception was generated.
					stream.Close();
				}
				return new SettingsObj();	// return default settings if there was an issue
			}
		} else {
			Debug.Log("Error: Settings file not found.");	// return default settings if none exist
			return new SettingsObj();
		}
	}

	/// <summary>
	/// Make a copy, used when changing stettings, so that changes can be applied or discarded
	/// </summary>
	/// <returns></returns>
	public SettingsObj copy() {	
		SettingsObj settings = new SettingsObj();
		settings.saveNum = this.saveNum;    // will have to update this for each variable added.
		settings.volume = this.volume;
		settings.resolutionX = this.resolutionX;
		settings.resolutionY = this.resolutionY;
		settings.vsync = this.vsync;
		settings.fullscreen = this.fullscreen;
		return settings;
	}

	public bool Equals(SettingsObj other) {
		if (this.saveNum != other.saveNum) return false;
		else if (!this.volume.Equals(other.volume)) return false;
		else if (this.resolutionX != other.resolutionX) return false;
		else if (this.resolutionY != other.resolutionY) return false;
		else if (this.vsync != other.vsync) return false;
		else if (this.fullscreen != other.fullscreen) return false;
		else return true;
		//else if (this. != other.) return false;
	}
}
