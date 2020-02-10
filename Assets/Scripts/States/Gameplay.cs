using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Gameplay : IState {
	public Text levelNameText;
	/*public GameObject pauseButtonObj;
	public GameObject lvlNameObj;
	public GameObject stringRemObj;*/
	public RectTransform pauseButtonTransform;
	public RectTransform lvlNameTransform;
	public RectTransform stringRemTransform;

	#region Initialize Variables
	bool animLockout = false;
	public bool winTrigger = false;
	public bool cinimaticMode = false;
	public bool hasBall = true;
	public int curdir = 0;
	public int stringLeft = 21;
    public UnityEngine.UI.Text stringrem;
	public GameObject wintext;
	public float moveAnimSpeed = 0.25f;
	public float youWinScreenTimeout = 1.0f;
	public LevelMap map;
	public Universe universe;
	/*public Node currentPosition = null;*/
	public Node currentPosition {   // Current position now internally uses an index, so that accessing current 
									// position when stuff is changed doesn't cause as many issues.
		get { return map[currentIndex]; }
		set { if (value != null) {
				currentIndex = value.index;
			} else {
				currentIndex = -1;
			}
		}
	}
	public int currentIndex = -1;	// needs to be public, so it can be updated when editing maps, 
									// so you don't teleport around when deleting tiles.
	public RenderingHandler nonEuclidRenderer;

	private PauseMenu pauseMenu;
	private LevelSelector levelSelector;
	#endregion
#if UNITY_EDITOR // if this is in the editor, need a reference to editLevel in order to call getCurrentNode() every time movement happens
	public EditLevel editLevel = null;
#endif

	//GameManager.gameplay = this;
	public override GameManager.IStateType _stateType {
		get { return GameManager.IStateType.gameplay; }
	}

	public override void _initialize() {
		pauseMenu = (PauseMenu)GameManager.istates[(int)GameManager.IStateType.pauseMenu];
		levelSelector = (LevelSelector)GameManager.istates[(int)GameManager.IStateType.levelSelector];
	}

	public override void _StartState(IState oldstate) {
		GameManager.uiCamera.SetActive(false);
		this.setBackground(false);
		//Set up the renderer
		if (!(oldstate is PauseMenu)) {
			nonEuclidRenderer.initialize();
			//Make sure the level is set to be the beginning of the level
			resetLevelAssets();
		}
		fitUitoScreen();
	}

	public override void _EndState(IState newstate) {
		GameManager.uiCamera.SetActive(true);
		if (newstate is PauseMenu) {
			this.setBackground(true);
			this.gameObject.SetActive(true);
			//GameManager.changeState(pauseMenu, null);
		}
	}

	public override void _RespondToConfirm(int retVal, string retString) { }


	public override void _Update() {
        //Player interaction objects
		if (InputManager.instance.OnInputDown(InputManager.Action.action)) {
			if (currentPosition.hasSign && curdir == 0) {
				//Player reads a sign
				Debug.Log(currentPosition.signMessage);
			} else {
				if (hasBall) {
					//Player drops the ball
					hasBall = false;
				} else if ((currentPosition.data.hasEnter && !currentPosition.data.hasLeave) || stringLeft == map.stringleft && currentPosition.type == Node.TileType.source) {
					//Player pick up the ball
					hasBall = true;
				}
			}
		}
		
		//Shows pause menu
		if (InputManager.instance.OnInputDown(InputManager.Action.back)) {
			GameManager.changeState(pauseMenu,this);
			//pause();
			//GameManager.instance.lscamera.SetActive(true);
            //GameManager.instance.changeState(GameManager.instance.pausemenu, GameManager.instance.levelselector);
        }

		//if (Input.GetKeyDown(KeyCode.Q)) pause();


		//Shows stringleft on screen
		stringrem.text = stringLeft +"/"+ map.stringleft.ToString();

        //Dont do anything past here if we are doing an animation
        if (animLockout)
            return;
        else if (winTrigger) {
            youWinScreenTimeout -= Time.deltaTime;
            if (youWinScreenTimeout < 0.0f) {
				winTrigger = false;
				//GameManager.saveGame.levelNumber
				if (GameManager.saveGame.levelNumber >= 0 && GameManager.saveGame.levelNumber < levelSelector.levelButtons.Length) {
					LevelSelector.changeLevelHelper(GameManager.saveGame.levelNumber);
				} else {
					GameManager.changeState(levelSelector, this);
				}

			}
            return;
        }
        //This for loop deals with inputs and moves the player around.

		for (int i = 0; i < 4; i++) {
			//Direction lines up with input manager so we can directly convert to an action from a direction.
			GameManager.Direction dir = (GameManager.Direction)i;
			if (InputManager.instance.OnInput((InputManager.Action)i)) {
				bool canMove = false;
				Node otherNode = null;
				curdir = i;
				//if (currentPosition.GetConnectionFromDir(dir) != null) {
				if (currentPosition.GetConnectionFromDir(dir) >= 0) {
					//otherNode = map.nodes[(int)currentPosition.GetConnectionFromDir(dir)];
					otherNode = map[(int)currentPosition.GetConnectionFromDir(dir)];
					//See if the other node has a leave
					canMove = (otherNode.data.hasLeave == false && stringLeft > 0) || (otherNode.data.hasLeave && otherNode.data.leave.inverse() == dir || !hasBall);
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
							if (otherNode.GetConnectionFromDir(dir.inverse()) != currentPosition.index) {
								//We need to do a connection stacking
								Node.ConnectionSet newSet = otherNode.connections.Copy();
								newSet[dir.inverse()] = currentPosition.index;
								otherNode.AddToConnectionStack(newSet);
								//Due to the way we handle connection pushing, we need to add this to the previously visables
							}
						}

						if (hasBall) {
							//Tag the current square with line exit dir
							if (otherNode.data.hasLeave == false) {
								currentPosition.data.leave = dir;
								otherNode.data.enter = dir.inverse();
								currentPosition.data.hasLeave = true;
								otherNode.data.hasEnter = true;
								stringLeft--;
							} else {
								//Do a backup
								currentPosition.data.hasEnter = false;
								otherNode.data.hasLeave = false;
								stringLeft++;
							}
						}

						currentPosition = otherNode;

					#if UNITY_EDITOR	// if this is in the editor, call getCurrentNode() every time movement happens, 
										// and apply copied colors and sprites to the new tile is currently drawing.
						if (editLevel != null) {
							editLevel.getCurrentNode();
							editLevel.drawTiles();	// only changes stuff if currently drawing
							//Debug.Log("calling getCurrentNode()...")
						} else {
							Debug.Log("Cannot call getCurrentNode(), there is no reference to editLevel script/object");
						}
					#endif
						nonEuclidRenderer.HandleRender(dir, currentPosition);
						animLockout = false;

                        //Crossing the target tile
                       
                       
						if (map.winConditions()) {
							winTrigger = true;
							wintext.SetActive(true);
							// play win sound here
							levelSelector.unlockLevel();    // unlocks next level
							GameManager.saveGame.levelNumber++;
							SaveObj.SaveGame(GameManager.settings.saveNum, GameManager.saveGame);
							Debug.Log("You win!");
						}
					},
					dir,
					moveAnimSpeed,
					canMove
				));

				break;
			}
		}
	}


    //reverts the level back to initial conditions
	public void resetLevelAssets() {
		//levelNameText.text = "???";
		// reset variables
		animLockout = false;
		winTrigger = false;
		cinimaticMode = false;
		hasBall = true;
		curdir = 0;
		stringLeft = map.stringleft;
		wintext.SetActive(false);
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
			if (k == map.size) {	// means loop above checked all tiles
				Debug.Log("Error: map source index does not exist/is invalid, no valid nodes available");
			} else {
				Debug.Log("Error: map source index does not exist/is invalid, using first available node");
			}
			
		}
		nonEuclidRenderer.HandleRender(GameManager.Direction.East, currentPosition, false);
	}

	/*
	public void pause() {
		GameManager.uiCamera.SetActive(true);
		this.setBackground(true);
		GameManager.changeState(pauseMenu, null);
	}*/

	public void fitUitoScreen() {
		pauseButtonTransform.anchoredPosition = new Vector2(
			Math.Max((Screen.width - pauseButtonTransform.sizeDelta.x) / 2 - 10, pauseButtonTransform.sizeDelta.x - 10),
			Math.Max((Screen.height - pauseButtonTransform.sizeDelta.y) / 2 - 10, pauseButtonTransform.sizeDelta.y - 10)
			);
		lvlNameTransform.anchoredPosition = new Vector2(
			0, 
			Math.Max((Screen.height - lvlNameTransform.sizeDelta.y) / 2 - 10, lvlNameTransform.sizeDelta.y - 10)
			);
		stringRemTransform.anchoredPosition = new Vector2(
			Math.Min((-Screen.width + stringRemTransform.sizeDelta.x) / 2 + 10, -stringRemTransform.sizeDelta.x + 10),
			Math.Max((Screen.height - stringRemTransform.sizeDelta.y) / 2 - 10, stringRemTransform.sizeDelta.y - 10)
			);
	}
}


