using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmMenu : MonoBehaviour {
	private IState caller;
	private int returnVal = -1;
	private bool returnString;
	public UnityEngine.UI.Text messageText;
	public UnityEngine.UI.Text returnText;
	public UnityEngine.UI.InputField inputObj;
	public UnityEngine.GameObject cancelObj;
	public UnityEngine.UI.Text acceptText;
	//public UnityEngine.UI.InputField acceptObj;
	public int maxLength = 10;

	public void Awake() {

		if (GameManager.confirmMenu == null || GameManager.confirmMenu == this) {
			GameManager.confirmMenu = this;
		} else {
			Debug.Log("Duplicate ConfirmMenu destroyed");
			DestroyImmediate(this.gameObject);
		}
	}

	/// <summary>
	/// 
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
		caller = GameManager.getCurrentState();
		if (caller != null) {
			caller.setBackground(true);
			this.gameObject.SetActive(true);
		} else {
			throw new System.NullReferenceException("GameManager.instance.currentState must not be null");
		}
	}

	private void resetDialog() {
		messageText.text = "Placeholder text";
		returnText.text = "";
		inputObj.text = "";
		returnVal = -1;
		returnString = false;
		caller = null;
	}

	public void acceptDialog() {
		caller.setBackground(false);
		this.gameObject.SetActive(false);
		caller._RespondToConfirm(returnVal, (returnString ? returnText.text : null));
	}

	public void returnToPrev() {
		caller.setBackground(false);
		this.gameObject.SetActive(false);
		caller._RespondToConfirm(-1, null);
	}
}
