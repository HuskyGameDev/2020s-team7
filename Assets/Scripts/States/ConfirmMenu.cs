using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmMenu : MonoBehaviour {
	private IState caller;  // Istate that set up confirmMenu last. Will return to that Istate when acceptDialog() or returnToPrev() called
	private int returnVal = -1; // value to set in caller when returning with acceptDialog(), returns -1 when returning with returnToPrev()
	private bool returnString;	// string value to set when returning with a string
	public UnityEngine.UI.Text messageText;	// message text to display, set by the caller when setting-up
	public UnityEngine.UI.Text returnText;  // whether a string should be returned, set by the caller when setting-up
	public UnityEngine.UI.InputField inputObj;	// inputfield object, turned off when not returning a string
	public UnityEngine.GameObject cancelObj;	// cancel button, turned off when displaying a notification with no options
	public UnityEngine.UI.Text acceptText;	// accept button, may use different phrase/text when used for a notification rather than a confirmation.
	public int maxLength = 10;	// max length of text to accept for return string

	public void Awake() {	// singleton pattern
		if (GameManager.confirmMenu == null || GameManager.confirmMenu == this) {
			GameManager.confirmMenu = this;
		} else {
			Debug.Log("Duplicate ConfirmMenu destroyed");
			DestroyImmediate(this.gameObject);
		}
	}

	/// <summary>
	/// Setup the dialog, success return value, wether a string should be returned, etc
	/// </summary>
	/// <param name="successVal"></param>
	/// <param name="message"></param>
	/// <param name="stringReturn"></param>
	public void setupDialog(int successVal, string message, bool returnString, bool cancelable) {
		messageText.text = message;
		returnVal = successVal;
		this.returnString = returnString;
		inputObj.gameObject.SetActive(returnString);
		inputObj.text = "";
		cancelObj.SetActive(cancelable);
		if (cancelable) {
			acceptText.text = "Accept";
		} else {
			acceptText.text = "OK";
		}
		caller = GameManager.getCurrentState();	// the caller must also be the current istate
		if (caller != null) {
			caller.setInteractible(false);
			this.gameObject.SetActive(true);
		} else {
			throw new System.NullReferenceException("GameManager.instance.currentState must not be null");
		}
	}

	private void resetDialog() {	// reset stuff, so incorrect values aren't returned next time
		messageText.text = "Placeholder text";
		returnText.text = "";
		inputObj.text = "";
		returnVal = -1;
		returnString = false;
		caller = null;
	}

	// accept whatever the dialog says, set the success value and the return string on the caller, and return to caller
	public void acceptDialog() {
		caller.setInteractible(true);
		this.gameObject.SetActive(false);
		caller._RespondToConfirm(returnVal, (returnString ? returnText.text : null));
	}

	// cancel the dialog, set return value to -1 on caller, and return
	public void returnToPrev() {
		caller.setInteractible(true);
		this.gameObject.SetActive(false);
		caller._RespondToConfirm(-1, null);
	}
}
