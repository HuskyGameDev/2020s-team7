using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Gameplay : IState {
	public Text levelNameText;  // text element used to indicate current level
	public Text checkpointsText;
	public UnityEngine.UI.Text stringrem;
	public GameObject wintext;
	public GameObject winSound; // object that FMOD event emmitter is attached to, set to trigger when made active
	public Image signImage; // used to change background image of sign
	public Text signText;   // used to change text of sign

	#region Initialize Variables
	bool animLockout = false;
	public bool winTrigger = false;
	public bool cinimaticMode = false;
	public bool hasBall = true;
	public int curdir = 0;
	public int stringLeft = 21;
	public bool pitWalk = false;    // for use when editing levels. Allows walking over unwalkable-type tiles.

	public float moveAnimSpeed = 0.25f;
	public float youWinScreenTimeout = 1.0f;
	public LevelMap map;
	public Universe universe;
	public Node currentPosition {   // Current position now internally uses an index, so that accessing current 
									// position when stuff is changed doesn't cause as many issues.
		get { return map[currentIndex]; }
		set {
			if (value != null) {
				currentIndex = value.index;
			} else {
				currentIndex = -1;
			}
		}
	}
	public int currentIndex = -1;   // needs to be public, so it can be updated when editing maps, 
									// so you don't teleport around when deleting tiles.
	public RenderingHandler nonEuclidRenderer;

	private PauseMenu pauseMenu;    // local pause menu and level selector references. Set up in initilizer
	private LevelSelector levelSelector;
	#endregion
#if UNITY_EDITOR // if this is in the editor, need a reference to editLevel in order to call getCurrentNode() every time movement happens
	public EditLevel editLevel = null;
#endif

	//GameManager.gameplay = this;
	public override GameManager.IStateType _stateType { // override the state-enum return value
		get { return GameManager.IStateType.gameplay; }
	}

	protected override void _initialize() { // setup local references
		pauseMenu = (PauseMenu)GameManager.istates[(int)GameManager.IStateType.pauseMenu];
		levelSelector = (LevelSelector)GameManager.istates[(int)GameManager.IStateType.levelSelector];
	}

	protected override void _StartState(IState oldstate) {
		//GameManager.uiObject.SetActive(false);	//not strictly needed, but makes it easy to make sure no menu stuff is visible
		if (!(oldstate is PauseMenu)) {
			nonEuclidRenderer.initialize();
			//Make sure the level is set to be the beginning of the level, set UI text
			resetLevelAssets();
		}
	}

	protected override void _EndState(IState newstate) {
		//GameManager.uiObject.SetActive(true);	// set menu stuff active
		if (newstate is PauseMenu) {    // if new state is pause menu, set this as background, and keep active
			this.gameObject.SetActive(true);
		}
	}

	public override void _RespondToConfirm(int retVal, string retString) { }


	public override void _Update() {
		//Player interaction objects
		//if (InputManager.instance.OnInputDown(InputManager.Action.action)) {
		if (Input.GetButtonDown("Action")) {
			if (currentPosition.hasSign/* && curdir == 0*/) {
				//Player reads a sign
				///////// make sign visible/not visible
				Debug.Log(currentPosition.signMessage);
			} else {
				if (hasBall) {
					//Player drops the ball
					hasBall = false;
					//} else if ((currentPosition.data.hasEnter && !currentPosition.data.hasLeave) || stringLeft == map.stringleft && currentPosition.type == Node.TileType.source) {
				} else if ((currentPosition.hasEnter && !currentPosition.hasLeave) || stringLeft == map.stringleft && currentPosition.type == Node.TileType.source) {
					//Player pick up the ball
					hasBall = true;
				}
			}
		}

		//Shows pause menu
		//if (InputManager.instance.OnInputDown(InputManager.Action.back)) {
		if (Input.GetButtonDown("Back")) {
			GameManager.changeState(pauseMenu, this);
		}

		//Dont do anything past here if we are doing an animation
		if (animLockout)
			return;
		else if (winTrigger) {
			youWinScreenTimeout -= Time.deltaTime;
			if (youWinScreenTimeout < 0.0f) {
				winTrigger = false;
				// load the next level if it exists, else return to level selector
				if (GameManager.saveGame.levelNumber >= 0 && GameManager.saveGame.levelNumber < levelSelector.levelButtons.Length) {
					LevelSelector.changeLevelHelper(GameManager.saveGame.levelNumber);
				} else {
					GameManager.changeState(levelSelector, this);
				}

			}
			return;
		}
		//This for loop deals with inputs and moves the player around.

		//for (int i = 0; i < 4; i++) {
		//Direction lines up with input manager so we can directly convert to an action from a direction.
		//GameManager.Direction dir = (GameManager.Direction)i;
		bool directionInput = false;
		GameManager.Direction dir = GameManager.Direction.North;
		float ver = Input.GetAxisRaw("Vertical");
		float hor = Input.GetAxisRaw("Horizontal");
		if (!Mathf.Approximately(ver, 0.0f) && ver > 0) {	// if multiple axis are active, choose only one, with priority N > E > S > W
			directionInput = true;
		} else if (!Mathf.Approximately(hor, 0.0f) && hor > 0) {
			dir = GameManager.Direction.East;
			directionInput = true;
		} else if (!Mathf.Approximately(ver, 0.0f) && ver < 0) {
			dir = GameManager.Direction.South;
			directionInput = true;
		} else if (!Mathf.Approximately(hor, 0.0f) && hor < 0) {
			dir = GameManager.Direction.West;
			directionInput = true;
		}

		//if (InputManager.instance.OnInput((InputManager.Action)i)) {
		if (directionInput) {	// only go into if a direction input occured
			bool canMove = false;
			Node otherNode = null;
			//curdir = i;
			//curdir = (int)dir;
			if (hasBall && (currentPosition.hasLeave || (currentPosition.type != Node.TileType.source && !currentPosition.hasLeave && !currentPosition.hasEnter))) {
				hasBall = false;
			}
			if (currentPosition[(int)dir] >= 0) {
				otherNode = map[currentPosition[(int)dir]];
				//See if the other node has a leave
				canMove = (
					(
						(!otherNode.hasLeave && !otherNode.hasEnter && stringLeft > 0) || ( // can't carry string onto tiles if it already has string on it
							otherNode.hasLeave && otherNode.leave.inverse() == dir &&   // unless you are rolling up the string
							currentPosition.hasEnter && currentPosition.enter == dir) ||
						!hasBall) &&    // this stuff about string doesn't matter if you are not carrying string
					!map.disjoint(currentPosition, dir) &&  // can't walk through one-ways if it would sever the string
					(otherNode.type != Node.TileType.unwalkable || pitWalk) // can't walk over pits, unless leveleditor "walk over pits" setting is set
					);
			}
			if (cinimaticMode && Input.GetKey(KeyCode.Space)) canMove = false;
			animLockout = true;
			StartCoroutine(CharacterAnimator.instance.AnimateMovement(
				(bool flag) => {
					if (!canMove) {
						animLockout = false;
						return;
					}
						//Handle fake connection stacking
						{
							//If the connection from this node to the other is one-way
							if (otherNode[(int)dir.inverse()] != currentPosition.index) {
								// temp override connection to that node
								otherNode[(int)dir.inverse()] = currentPosition.index;
						}
					}

					if (hasBall) {
							//Tag the current square with line exit dir
							if (!otherNode.hasLeave) {
							currentPosition.leave = dir;
							currentPosition.hasLeave = true;
							otherNode.enter = dir.inverse();
							otherNode.hasEnter = true;
							stringLeft--;
						} else {
								//Do a backup
								currentPosition.hasEnter = false;
							otherNode.hasLeave = false;
							stringLeft++;
						}
					}

					currentPosition = otherNode;

#if UNITY_EDITOR    // if this is in the editor, call getCurrentNode() every time movement happens, 
						// and apply copied colors and sprites to the new tile is currently drawing.
						if (editLevel != null) {
						editLevel.getCurrentNode();
						editLevel.drawTiles();  // only changes stuff if currently drawing
												//Debug.Log("calling getCurrentNode()...")
						} else {
						Debug.Log("Cannot call getCurrentNode(), there is no reference to editLevel script/object");
					}
#endif
						nonEuclidRenderer.HandleRender(dir, currentPosition);
					animLockout = false;
					setUItext();    // apply changes to UI text: string left, checkpoints.

						if (map.winConditions()) {
						winTrigger = true;
						wintext.SetActive(true);    // make win text visible
							winSound.SetActive(true);   // play sound
														// play win sound here
							levelSelector.unlockLevel();    // unlocks next level
							GameManager.saveGame.levelNumber++; // advance last level visited, so will auto-load next level
							SaveObj.SaveGame(GameManager.settings.saveNum, GameManager.saveGame);   // save changes to levels accessible and last-level-visited
							Debug.Log("You win!");
					}
				},
				dir,
				moveAnimSpeed,
				canMove
			));

			//break;
		}
		//}
	}

	/// <summary>
	/// set UI text: string left and checkpoints if applicable
	/// </summary>
	/// <param name="setup"></param>
	public void setUItext(bool setup = false) {
		stringrem.text = String.Format("{0}/{1}", stringLeft, map.stringleft);
		if (map.checkpoints.Length > 0) {
			if (setup) checkpointsText.transform.parent.gameObject.SetActive(true);
			checkpointsText.text = String.Format("{0}/{1}", map.countCheckpoints(), map.checkpoints.Length);
		} else {
			if (setup) checkpointsText.transform.parent.gameObject.SetActive(false);
		}

	}

	//reverts the level back to initial conditions
	public void resetLevelAssets() {
		// reset variables
		animLockout = false;
		winTrigger = false;
		cinimaticMode = false;
		hasBall = true;
		//curdir = 0;
		stringLeft = map.stringleft;
		wintext.SetActive(false);   // make sure win text is not visible
		winSound.SetActive(false);  // set inactive, so can be triggered again.
		youWinScreenTimeout = 1.0f;

		//Send the player back to the starting point
		currentPosition = map[map.sourceNodeIndex];
		if ((currentPosition == null) || (currentPosition.index < 0)) {
			int k;
			for (k = 0; k < map.size; k++) {
				if ((map[k] != null) && (map[k].index >= 0)) {
					currentPosition = map[k];
					break;
				}
			}
			if (k == map.size) {    // means loop above checked all tiles
				Debug.Log("Error: map source index does not exist/is invalid, no valid nodes available");
			} else {
				Debug.Log("Error: map source index does not exist/is invalid, using first available node");
			}

		}
		nonEuclidRenderer.HandleRender(GameManager.Direction.East, currentPosition, false);
		setUItext(true);    // if resetting level stuff, also update UI text
	}

}


