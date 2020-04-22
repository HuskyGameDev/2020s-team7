using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveObj {
	/*
	 * This class records save data, so that progress in the game may be kept between instances of the game.
	 * Has methods for saving and loading the objects that hold this data to three standard location.
	 */

	public string slotName;	// name to use ingame for this save slot
	public int levelNumber;	// last level visited for this save
	//private int size;   // size of canAccessArray
	private bool[] canAccessArray;


	public SaveObj() {	// constructor
		//size = 10;
		levelNumber = 0;
		canAccessArray = new bool[10];
		canAccessArray[0] = true;
	}

	public SaveObj(string name) {	// constructor that sets name
		slotName = name;
		//size = 10;
		levelNumber = 0;
		canAccessArray = new bool[10];
		canAccessArray[0] = true;
	}

	// returns true if the given index is within the array and is set too true, otherwise returns false.
	// means that if more levels are added to the game, then old saves will not cause issues with out-of-bounds exceptions
	public bool canAccess(int index) {
		//if (index >= size) {
		if (index >= canAccessArray.Length) {
			return false;
		} else {
			return canAccessArray[index];
		}
	}

	// sets the bool value at that index, extending the array to include that index if needed.
	public void setAccess(int index, bool value) {
		if (index >= canAccessArray.Length) {	// extent the array if needed
			int tempsize = canAccessArray.Length;
			while (tempsize <= index) tempsize += 10;	// make sure array is big enought to use that index
			bool[] arrayTemp = new bool[tempsize];
			for (int i = 0; i < canAccessArray.Length; i++) {
				arrayTemp[i] = canAccessArray[i];
			}
			canAccessArray = arrayTemp;
		}
		canAccessArray[index] = value;	// set the value of that index
	}

	/// <summary>
	/// Save the given save object at the standard location using the given integer in the name
	/// </summary>
	/// <param name="saveNum"></param>
	/// <param name="save"></param>
	public static void SaveGame(int saveNum, SaveObj save) {
		if (saveNum > 3 || saveNum < 1) {	// right now, only use three save slots total
			throw new System.ArgumentException("Parameter \"saveNum\" should be in range [1,3]");
		}
		if (save == null) {	// can't save the object if there is no object
			throw new System.ArgumentException("Parameter \"save\" cannot be null");
		}
		Stream stream = null;
		try {	// don't want a file I/O exception to cause issues
			System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			//Debug.Log("Saving to File: \"" + Application.persistentDataPath + "\\Save_" + saveNum + ".txt\"");
			stream = new FileStream(Application.persistentDataPath + "\\Save_" + saveNum + ".txt", FileMode.Create, FileAccess.Write);
			formatter.Serialize(stream, save);
			stream.Close();
		} catch (System.Exception e) {
			Debug.Log(e.Message);   // notify me about any exceptions.
			if (stream != null) {   // make sure the stream is closed if an exception was generated.
				stream.Close();
			}
		}
		return;
	}

	// return true is a save file exists at the standard location using the given integer
	public static bool SaveExists(int saveNum) {
		if (saveNum > 3 || saveNum < 1) {	// only use the save numbers 1-3
			throw new System.ArgumentException("Parameter saveNum should be in range [1,3]");
		}

		if (File.Exists(Application.persistentDataPath + "\\Save_" + saveNum + ".txt")) {
			return true;
		} else {
			return false;
		}
	}

	/// <summary>
	/// Load the save object from the file at the standard loaction and using the given number, if it exists
	/// </summary>
	/// <param name="saveNum"></param>
	/// <returns></returns>
	public static SaveObj LoadGame(int saveNum) {
		if (saveNum > 3 || saveNum < 1) {   // only use the save numbers 1-3
			throw new System.ArgumentException("Parameter saveNum should be in range [1,3]");
		}
		
		if (File.Exists(Application.persistentDataPath + "\\Save_" + saveNum + ".txt")) {	// can only load it if it exists
			Stream stream = null;
			try {   // don't want a file I/O exception to cause issues
				System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				stream = new FileStream(Application.persistentDataPath + "\\Save_" + saveNum + ".txt", FileMode.Open, FileAccess.Read);
				SaveObj save = (SaveObj)formatter.Deserialize(stream);
				stream.Close();
				return save;
			} catch (System.Exception e) {
				Debug.Log(e.Message);   // notify me about any exceptions.
				if (stream != null) {   // make sure the stream is closed if an exception was generated.
					stream.Close();
				}
				return null;
			}
		} else {	// file does not exist, return null
			Debug.Log("Error: Save file " + saveNum + " not found.");
			return null;
		}
	}

	/// <summary>
	/// delete the savefile at the standard location and using the given number
	/// </summary>
	/// <param name="saveNum"></param>
	public static void DeleteGame(int saveNum) {
		if (saveNum > 3 || saveNum < 1) {   // only use the save numbers 1-3
			throw new System.ArgumentException("Parameter saveNum should be in range [1,3]");
		}

		if (File.Exists(Application.persistentDataPath + "\\Save_" + saveNum + ".txt")) {	// can only delete it if it exists
			try {
				File.Delete(Application.persistentDataPath + "\\Save_" + saveNum + ".txt");
			} catch (System.Exception e) {	// catch exceptions
				Debug.Log(e.Message);
			}
		} else {
			throw new System.IO.FileNotFoundException(Application.persistentDataPath + "\\Save_" + saveNum + ".txt does not exist, cannot delete");
			// this should probably return false. Do that in the future
		}
	}
}
