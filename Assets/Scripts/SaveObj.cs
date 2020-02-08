using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveObj {
	//string name;
	public string slotName;
	public int levelNumber;
	private int size;
	private bool[] array;

	public SaveObj() {
		size = 10;
		levelNumber = 0;
		array = new bool[10];
		array[0] = true;
	}

	public SaveObj(string name) {
		slotName = name;
		size = 10;
		levelNumber = 0;
		array = new bool[10];
		array[0] = true;
	}

	public bool canAccess(int index) {
		if (index >= size) {
			return false;
		} else {
			return array[index];
		}
	}

	public void setAccess(int index, bool value) {
		if (index >= size) {
			int tempsize = ((index + 1) / 10) + (((index + 1) / 10 == 0) ? 0: 1); // should give nearest multiple of ten that is equal to or above the index
			bool[] arrayTemp = new bool[tempsize * 10];
			for (int i = 0; i < size; i++) {
				arrayTemp[i] = array[i];
			}
			array = arrayTemp;
			size = tempsize;
		}
		array[index] = value;
	}

	public static void SaveGame(int saveNum, SaveObj save) {
		if (saveNum > 3 || saveNum < 1) {
			throw new System.ArgumentException("Parameter \"saveNum\" should be in range [1,3]");
		}
		if (save == null) {
			throw new System.ArgumentException("Parameter \"save\" cannot be null");
		}
		Stream stream = null;
		try {
			System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			Debug.Log("Saving to File: \"" + Application.persistentDataPath + "\\Save_" + saveNum + ".txt\"");
			stream = new FileStream(Application.persistentDataPath + "\\Save_" + saveNum + ".txt", FileMode.Create, FileAccess.Write);
			formatter.Serialize(stream, save);
			stream.Close();
		} catch (System.IO.IOException e) {
			Debug.Log(e.Message);
			if (stream != null) {
				stream.Close();
			}
		}
		return;
	}

	public static bool SaveExists(int saveNum) {
		if (saveNum > 3 || saveNum < 1) {
			throw new System.ArgumentException("Parameter saveNum should be in range [1,3]");
		}

		if (File.Exists(Application.persistentDataPath + "\\Save_" + saveNum + ".txt")) {
			return true;
		} else {
			return false;
		}
	}

	public static SaveObj LoadGame(int saveNum) {
		if (saveNum > 3 || saveNum < 1) {
			throw new System.ArgumentException("Parameter saveNum should be in range [1,3]");
		}
		
		if (File.Exists(Application.persistentDataPath + "\\Save_" + saveNum + ".txt")) {
			Stream stream = null;
			try {
				System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				stream = new FileStream(Application.persistentDataPath + "\\Save_" + saveNum + ".txt", FileMode.Open, FileAccess.Read);
				SaveObj save = (SaveObj)formatter.Deserialize(stream);
				stream.Close();
				return save;
			} catch (System.IO.IOException e) {
				Debug.Log(e.Message);
				if (stream != null) {
					stream.Close();
				}
				return null;
			}
		} else {
			Debug.Log("Error: Save file " + saveNum + " not found.");
			return null;
		}
	}

	public static void DeleteGame(int saveNum) {
		if (saveNum > 3 || saveNum < 1) {
			throw new System.ArgumentException("Parameter saveNum should be in range [1,3]");
		}

		if (File.Exists(Application.persistentDataPath + "\\Save_" + saveNum + ".txt")) {
			try {
				File.Delete(Application.persistentDataPath + "\\Save_" + saveNum + ".txt");
			} catch (System.Exception e) {
				Debug.Log(e.Message);
			}
		} else {
			throw new System.IO.FileNotFoundException(Application.persistentDataPath + "\\Save_" + saveNum + ".txt does not exist, cannot delete");
		}
	}
}
