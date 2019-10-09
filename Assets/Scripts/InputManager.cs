using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InputManager {
	/*TODO: 
	 *		[NO] mouse position/click stuff
	 *		[X] Make into a singleton rather than static
	 * 
	 *		[X] joystick - "Axis_1h" & "Axis_1v"
	 *		[X] Revamp axis input
	 *		[X]		button-up/down
	 *		[X]		diagonal-directions
	 *				Should input check every frame happen in the input manager? Or should input be checked every frame by things looking for input?
	 *		[X]		check happens in input-manager, sends notifications when receive input
	 *		
	 *		[?]Make sure buttonDown/Up does not trigger twice from both buttons being pressed if the first button is down when the second if pressed
	*/

	//public enum Action { up, down, left, right, confirm, cancel, action_1, action_2, action_3 };    // what actions/inputs we will has keybindings for
	public enum Action { up, right, down, left, confirm, back, action};
	private Keybindings keybindings = null;  // keybindings for manager to use when checking button presses
	private Keybindings temp_keybindings = null; // potential changes to keybindings, can then be applied or discarded
	private System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();    // used for saving/loading keybindings from file
	private static int numberOfKeys = 7;
	// Used for deciding if arrow key input was "pressed" or "released" this frame
	//private bool[] state = new bool[8];
	//private bool[] lastState = new bool[8]; // 
	//private const float REQUIREDVAL = 0.6f;

	/*	Key notifications take following form: "inputType_inputKey"
	 * where "inputType" is one of these three options:
	 *		"Key"		: key is currently pressed down
	 *		"KeyDown"	: key was pressed down this frame
	 *		"KeyUp"		: key was released this frame
	 * 
	 * and "inputKey" is one of the following options:
	 *		"UpRight"	: direction up & right
	 *		"Up"		: direction up 
	 *		"UpLeft"	: direction up & left
	 *		"Right"		: direction right
	 *		"Left"		: direction left
	 *		"DownRight"	: direction down & right
	 *		"Down"		: direction down
	 *		"DownLeft"	: direction down & left
	 *		"Confirm"	: confirm
	 *		"Cancel"	: cancel
	 *		"Action_1"	: action #1
	 *		"Action_2"	: action #2
	 *		"Action_3"	: action #3
	 *			
	 * Example #1 "KeyDown_Confirm" : confirm key was pressed down this frame
	 * Example #2 "KeyUp_Down" : Down direction key was released this frame
	 */

	#region Singleton Pattern
	public readonly static InputManager instance = new InputManager();	// should only be one instance of the camera manager
	private InputManager() {}	// Constuctor w/ nothing inside
	#endregion

	#region Keybinding Methods
	/// <summary>
	/// Holds keybindings
	/// </summary>
	[System.Serializable]   // this class can be saved/loaded from file
	private class Keybindings {
		public KeyCode[,] keys; // 9 tall, for 9 actions, and 2 wide, for 2 keybindings per action

		/// <summary>
		/// Constructor, with default keybindings.
		/// </summary>
		public Keybindings() {
			keys = new KeyCode[InputManager.numberOfKeys, 2];
			keys[0, 0] = KeyCode.UpArrow;  // up
			keys[0, 1] = KeyCode.W;

			keys[1, 0] = KeyCode.RightArrow;    // right
			keys[1, 1] = KeyCode.D;

			keys[2, 0] = KeyCode.DownArrow;    // down
			keys[2, 1] = KeyCode.S;

			keys[3, 0] = KeyCode.LeftArrow;   // left
			keys[3, 1] = KeyCode.A;

			keys[4, 0] = KeyCode.Return;   // confirm
			keys[4, 1] = KeyCode.Space;

			keys[5, 0] = KeyCode.Escape;  // back
			keys[5, 1] = KeyCode.P;

			keys[6, 0] = KeyCode.RightShift;  // action
			keys[6, 1] = KeyCode.E;

			//keys[5, 0] = KeyCode.Escape;  // cancel
			//keys[5, 1] = KeyCode.Space;
			//keys[6, 0] = KeyCode.LeftControl;   // action_1
			//keys[6, 1] = KeyCode.Mouse0;
			//keys[7, 0] = KeyCode.LeftAlt;    // action_2
			//keys[7, 1] = KeyCode.Mouse0;
			//keys[8, 0] = KeyCode.LeftShift;  // action_3
			//keys[8, 1] = KeyCode.Mouse2;
		}

		/// <summary>
		/// Copies the keybindings from one instance to another
		/// </summary>
		/// <returns></returns>
		public Keybindings Copy() {
			Keybindings copy = new Keybindings();
			for (int i = 0; i < InputManager.numberOfKeys; i++) {
				copy.keys[i, 0] = this.keys[i, 0];
				copy.keys[i, 1] = this.keys[i, 1];
			}

			return copy;
		}
	}

	/// <summary>
	/// Loads keybindings from file if file exist, else loads default keybindsings. 
	/// </summary>
	public void LoadKeybinds() {
		if (File.Exists(Application.persistentDataPath + "\\Keybindings.txt")) {
			//Debug.Log("Loading keybinds from location: \"" + Application.persistentDataPath + "\\Keybindings.txt\"");
			Stream stream = new FileStream(Application.persistentDataPath + "\\Keybindings.txt", FileMode.Open, FileAccess.Read);
			keybindings = (Keybindings)formatter.Deserialize(stream);
			temp_keybindings = keybindings.Copy();
		} else {
			//Debug.Log("Loading default keybinds");
			keybindings = new Keybindings();
			temp_keybindings = keybindings.Copy();
		}
	}

	/// <summary>
	/// Sets temp_keybindings to match default keybindings.
	/// </summary>
	public void ResetKeybinds() {
		temp_keybindings = new Keybindings();
	}

	/// <summary>
	/// Sets keybindings to match temp_keybindings, and saves to file as well.
	/// </summary>
	public void ApplyKeybinds() {
		if (temp_keybindings == null) {
			temp_keybindings = new Keybindings();
		}
		keybindings = temp_keybindings.Copy();

		//Debug.Log("Saving to location: \"" + Application.persistentDataPath + "\\Keybindings.txt\"");
		Stream stream = new FileStream(Application.persistentDataPath + "\\Keybindings.txt", FileMode.Create, FileAccess.Write);
		formatter.Serialize(stream, keybindings);
		stream.Close();
	}

	/// <summary>
	/// Discards changes to temp_keybindings. Sets temp_keybindings to match keybindings.
	/// </summary>
	public void DiscardKeybinds() {
		if (temp_keybindings == null) {
			temp_keybindings = new Keybindings();
		} else {
			temp_keybindings = keybindings.Copy();
		}
	}

	/// <summary>
	/// Modifys a bindings in temp_keybindings. If given key matches one already in use elsewhere, swaps the keybinding that is being replaced and the one that was already in use.
	/// </summary>
	/// <param name="action">Action to change keybinding of</param>
	/// <param name="primary">Should the primary or secondary keybinding be changed</param>
	/// <param name="newKey">New key to set the keybinding to</param>
	public void ModifyKeybinds(Action action, bool primary, KeyCode newKey) {
		//Debug.Log("ModifyKeybind: " + action + ", primary:" + primary + ", to: " + newKey);
		int action_num = (int)action;
		int action_primary = 1;
		if (primary) action_primary = 0;

		int replace_num = -1;
		int replace_primary = -1;
		for (int i = 0; i < 9; i++) {   // find if the newKey matches any already in use.
			if (action_num != i) {
				if (temp_keybindings.keys[i, 0] == newKey) {
					replace_num = i;
					replace_primary = 0;
				} else if (temp_keybindings.keys[i, 1] == newKey) {
					replace_num = i;
					replace_primary = 1;
				}
			}
		}

		if (replace_num >= 0) { // swap newKey and replaced key is newKey was already in use.
			temp_keybindings.keys[replace_num, replace_primary] = temp_keybindings.keys[action_num, action_primary];
		}
		temp_keybindings.keys[action_num, action_primary] = newKey;
	}

	/// <summary>
	/// When called, it waits for any currently pressed buttons to be released, then gets the keyCode for the next button pressed and uses it to call ModifyKeybindingd() with the given "Action action", and "bool primary".
	/// Ends and does nothing if 5s pass with no acceptable input.
	/// </summary>
	/// <param name="action"></param>
	/// <param name="primary"></param>
	/// <returns></returns>
	public IEnumerator<object> WaitForKeybindInput(Action action, bool primary) {
		while (Input.anyKey) {  // wait for all currently pressed keys to be released.
								//Debug.Log("Waiting for key release");
			yield return null;
		}

		yield return null;  // wait a single frame after all keys are released, so the the last key released is not accidentally read as input by the next part.

		bool loop = true;
		float time = 0.0f;
		float waitTime = 5.0f;

		while ((loop) && (time < waitTime)) {   // loop until acceptable input is found/time is up.
												//Debug.Log("Waiting for key press");
			Event e = new Event();
			while ((Event.GetEventCount() > 0) && loop) {   // if there are GUI events, pop and check them
				Event.PopEvent(e);
				//Debug.Log("Events to test");
				if (e.isKey) {  // if event was a key press, get its key code, enter it into ModifyKeybinds(), and exit loop.
								//Debug.Log("Detected key code: " + e.keyCode);
					ModifyKeybinds(action, primary, e.keyCode);
					loop = false;
				} else if (e.isMouse) { // if event was a mouse button press, use case statement to get it's keycode, enter it into ModifyKeybinds(), and exit loop.
										//Debug.Log("Detected mouse button: " + e.button);
					switch (e.button) {
						case 0:
							ModifyKeybinds(action, primary, KeyCode.Mouse0);
							break;
						case 1:
							ModifyKeybinds(action, primary, KeyCode.Mouse1);
							break;
						case 2:
							ModifyKeybinds(action, primary, KeyCode.Mouse2);
							break;
						case 3:
							ModifyKeybinds(action, primary, KeyCode.Mouse3);
							break;
						case 4:
							ModifyKeybinds(action, primary, KeyCode.Mouse4);
							break;
						case 5:
							ModifyKeybinds(action, primary, KeyCode.Mouse5);
							break;
						case 6:
							ModifyKeybinds(action, primary, KeyCode.Mouse6);
							break;
					}
					loop = false;
				}
			}
			waitTime += Time.deltaTime;
			yield return null;
		}

		yield break;
	}

	#endregion

	/*
	/// <summary>
	/// returns the mouse position in pixels on the screen, not in world-coordinates
	/// (0,0) is bottom left corner, top right is (pixelWidth,pixelHeight)
	/// </summary>
	/// <returns></returns>
	public static Vector3 GetMousePos() {
		//Vector3 mousePos = Camera.ScreenToWorldPoint(Input.mousePosition);
		return Input.mousePosition;
		//return new Vector2(2.0f, 2.0f);
	}*/

	#region notification Key check
	/// <summary>
	/// Every frame, check for input and send any notifications
	/// </summary>
	/*
	void Update() {
		//Debug.Log("InputManager checking for inputs");
		if (keybindings == null) {
			LoadKeybinds();
		}

		#region Arrow Key Stuff
		// any joystick state changes (KeyDown()/KeyUp()) should only last a single frame
		//joystickStateChange[0] = false; 
		float verti = Input.GetAxis("Axis_1v");
		float horiz = Input.GetAxis("Axis_1h");

		// Get input from joystick or either bound key
		state[0] = ((verti < -REQUIREDVAL) ? true : false) || Input.GetKey(keybindings.keys[0, 0]) || Input.GetKey(keybindings.keys[0, 1]);  // up
		state[1] = ((verti > REQUIREDVAL) ? true : false) || Input.GetKey(keybindings.keys[1, 0]) || Input.GetKey(keybindings.keys[1, 1]);  // down
		state[2] = ((horiz < -REQUIREDVAL) ? true : false) || Input.GetKey(keybindings.keys[2, 0]) || Input.GetKey(keybindings.keys[2, 1]);  // left
		state[3] = ((horiz > REQUIREDVAL) ? true : false) || Input.GetKey(keybindings.keys[3, 0]) || Input.GetKey(keybindings.keys[3, 1]);  // right
		// diagonal directions are based on cardinal directions
		state[4] = state[0] && state[1]; // up right
		state[5] = state[1] && state[2]; // down right
		state[6] = state[2] && state[3]; // down left
		state[7] = state[3] && state[0]; // up left

		for (int i = 0; i < 8; i++) {	// for each directional input
			string key;
			switch (i) {	// for each posted notification, the last part of the string is the only thing that changes
				case 0:
					key = "Up";
					break;
				case 1:
					key = "Down";
					break;
				case 2:
					key = "Left";
					break;
				case 3:
					key = "Right";
					break;
				case 4:
					key = "UpRight";
					break;
				case 5:
					key = "DownRight";
					break;
				case 6:
					key = "DownLeft";
					break;
				case 7:
					key = "UpLeft";
					break;
				default:
					key = "";
					break;
			}

			if (state[i]) {    // if the key is pressed down
				this.PostNotification("Key_" + key);	// post Key notification
				if (!lastState[i]) {	// if key was not pressed down last frame
					lastState[i] = true;
					this.PostNotification("KeyDown_" + key);	// post KeyDown notification
				}
			} else {	// if key is not pressed
				if (lastState[i]) {	// if key was pressed down last frame
					lastState[i] = false;
					this.PostNotification("KeyUp_" + key);	// post KeyUp notification
				}
			}
		}
		

		#endregion


		#region OnKey Input
		// Send notification if a key is currently pressed down

		// confirm - 4
		if (Input.GetKey(keybindings.keys[4, 0]) | Input.GetKey(keybindings.keys[4, 1])) {
			this.PostNotification("Key_Confirm");
		}

		// cancel - 5
		if (Input.GetKey(keybindings.keys[5, 0]) | Input.GetKey(keybindings.keys[5, 1])) {
			this.PostNotification("Key_Cancel");
		}

		// action_1 - 6
		if (Input.GetKey(keybindings.keys[6, 0]) | Input.GetKey(keybindings.keys[6, 1])) {
			this.PostNotification("Key_Action_1");
		}

		// action_2 - 7
		if (Input.GetKey(keybindings.keys[7, 0]) | Input.GetKey(keybindings.keys[7, 1])) {
			this.PostNotification("Key_Action_2");
		}

		// action_3 - 8
		if (Input.GetKey(keybindings.keys[8, 0]) | Input.GetKey(keybindings.keys[8, 1])) {
			this.PostNotification("Key_Action_3");
		}
		#endregion


		#region OnKeyDown Input
		// Send notification if a key was pressed down this frame

		// confirm - 4
		if (Input.GetKeyDown(keybindings.keys[4, 0]) | Input.GetKeyDown(keybindings.keys[4, 1])) {
			this.PostNotification("KeyDown_Confirm");
		}
		// cancel - 5
		if (Input.GetKeyDown(keybindings.keys[5, 0]) | Input.GetKeyDown(keybindings.keys[5, 1])) {
			this.PostNotification("KeyDown_Cancel");
		}
		// action_1 - 6
		if (Input.GetKeyDown(keybindings.keys[6, 0]) | Input.GetKeyDown(keybindings.keys[6, 1])) {
			this.PostNotification("KeyDown_Action_1");
		}
		// action_2 - 7
		if (Input.GetKeyDown(keybindings.keys[7, 0]) | Input.GetKeyDown(keybindings.keys[7, 1])) {
			this.PostNotification("KeyDown_Action_2");
		}
		// action_3 - 8
		if (Input.GetKeyDown(keybindings.keys[8, 0]) | Input.GetKeyDown(keybindings.keys[8, 1])) {
			this.PostNotification("KeyDown_Action_3");
		}
		#endregion


		#region OnKeyUp Input
		// Send notification if a key was released this frame
		
		// confirm - 4
		if (Input.GetKeyUp(keybindings.keys[4, 0]) | Input.GetKeyUp(keybindings.keys[4, 1])) {
			this.PostNotification("KeyDown_Confirm");
		}
		// cancel - 5
		if (Input.GetKeyUp(keybindings.keys[5, 0]) | Input.GetKeyUp(keybindings.keys[5, 1])) {
			this.PostNotification("KeyDown_Cancel");
		}
		// action_1 - 6
		if (Input.GetKeyUp(keybindings.keys[6, 0]) | Input.GetKeyUp(keybindings.keys[6, 1])) {
			this.PostNotification("KeyDown_Action_1");
		}
		// action_2 - 7
		if (Input.GetKeyUp(keybindings.keys[7, 0]) | Input.GetKeyUp(keybindings.keys[7, 1])) {
			this.PostNotification("KeyDown_Action_2");
		}
		// action_3 - 8
		if (Input.GetKeyUp(keybindings.keys[8, 0]) | Input.GetKeyUp(keybindings.keys[8, 1])) {
			this.PostNotification("KeyDown_Action_3");
		}
		#endregion
	}*/
	#endregion

	#region Direct Key Check
	
	/// <summary>
	/// Check if either of the keys bound to the action are pressed down.
	/// </summary>
	/// <param name="action"></param>
	/// <returns></returns>
	public bool OnInput(Action action) {
		int action_num = (int)action;
		return (Input.GetKey(keybindings.keys[action_num, 0]) | Input.GetKey(keybindings.keys[action_num, 1]));
	}

	/// <summary>
	/// Check if either of the keys bound to the action have been pressed down this frame.
	/// </summary>
	/// <param name="action"></param>
	/// <returns></returns>
	public bool OnInputDown(Action action) {
		int action_num = (int)action;

#if UNITY_EDITOR
		if (keybindings == null) {
			Debug.Log("Error: keybindings object is null, WTF?");
		}
#endif

		return (Input.GetKeyDown(keybindings.keys[action_num, 0]) | Input.GetKeyDown(keybindings.keys[action_num, 1]));
	}

	/// <summary>
	/// Check if either of the keys bound to the action have been released this frame.
	/// </summary>
	/// <param name="action"></param>
	/// <returns></returns>
	public bool OnInputUp(Action action) {
		int action_num = (int)action;
		return (Input.GetKeyUp(keybindings.keys[action_num, 0]) | Input.GetKeyUp(keybindings.keys[action_num, 1]));
	}
	#endregion

	/*public static void WaitForKeybindInput(Action action, bool primary) {
		MonoBehaviour.StartCoroutine(WaitForKeybindInput_Coroutine(action, primary));
	}*/

}

