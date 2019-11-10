using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Gameplay : IState {

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
	public float youWinScreenTimeout = 7.0f;
	public Map map;
	public Universe universe;
	public Node currentPosition = null;
	public RenderingHandler nonEuclidRenderer;


    #endregion
    public override void _StartState() {
        //Set up the renderer
		nonEuclidRenderer.initialize();

        //Deactivate the other states just in case
        GameManager.instance.pausemenu.gameObject.SetActive(false);
        GameManager.instance.levelselector.gameObject.SetActive(false);

        //Make sure the level is set to be the beginning of the level
		resetLevelAssets();
	}

	public override void _EndState() {
	}


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
		if (InputManager.instance.OnInputDown(InputManager.Action.back))
        {
            GameManager.instance.lscamera.SetActive(true);
            GameManager.instance.changeState(GameManager.instance.pausemenu, GameManager.instance.levelselector);
        }


        //Shows level selecor. Delete this before final release
        if (Input.GetKeyDown(KeyCode.Q))
        {
			GameManager.instance.lscamera.SetActive(true);
			GameManager.instance.pausemenu.gameObject.SetActive(false);
			GameManager.instance.changeState(GameManager.instance.levelselector, null);
            
        }
		

        //Shows stringleft on screen
        stringrem.text = stringLeft +"/"+ map.stringleft.ToString();

        //Dont do anything past here if we are doing an animation
        if (animLockout)
            return;
        else if (winTrigger) {
            wintext.SetActive(true);
            youWinScreenTimeout -= Time.deltaTime;
            if (youWinScreenTimeout < 0.0f) {
                //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

						nonEuclidRenderer.HandleRender(dir, currentPosition);
						animLockout = false;

                        //Crossing the target tile
                       
                       
						if (map.winConditions()) {

							winTrigger = true;
                            GameManager.instance.changeState(GameManager.instance.levelselector, this);
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
        //Give the player their string back
        stringLeft = map.stringleft;

        //Send the player back to the starting point
		currentPosition = map[map.sourceNodeIndex];
		if ((currentPosition == null) || (currentPosition.index < 0)) {
			int k;
			for (k = 0; k < map.size; k++) {
				if ((map[k] != null) && (map[k].index >= 0)) {
					currentPosition = map[k];
				}
			}
			if (k == map.size) {
				Debug.Log("Error: map source index does not exist/is invalid, no valid nodes available");
			} else {
				Debug.Log("Error: map source index does not exist/is invalid, using first available node");
			}
			
		}
		nonEuclidRenderer.HandleRender(GameManager.Direction.East, currentPosition, false);


	}
}
