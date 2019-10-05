using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : IState {
	bool animLockout = false;
	public bool winTrigger = false;
	public bool cinimaticMode = false;

	public int stringLeft = 21;
	public UnityEngine.UI.Text stringrem;
	public GameObject wintext;

	public float moveAnimSpeed = 0.25f;
	public float youWinScreenTimeout = 7.0f;

	public Map map;
	public Universe universe;
	//public Node currentPosition = new Node();
	public Node currentPosition = null;

	public RenderingHandler nonEuclidRenderer;

	public override void _StartState() {
		nonEuclidRenderer.initialize();
		resetLevelAssets();
	}

	public override void _EndState() {
		Debug.Log("Gameplay does not do anything in its _EndState() method");
	}


	public override void _Update() {
		//Debug.Log("Gameplay _Update() being called");
		
		if (InputManager.instance.OnInputDown(InputManager.Action.back))
        {
            GameManager.instance.lscamera.SetActive(true);
			GameManager.instance.pausemenu.gameObject.SetActive(true);
			GameManager.instance.gameplay.gameObject.SetActive(false);
			GameManager.instance.levelselector.gameObject.SetActive(false);
            
            //if (pausemenu.activeInHierarchy) Debug.Log("I'm active");
            //changeState(lscamera, gameplay);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
			GameManager.instance.lscamera.SetActive(true);
			GameManager.instance.pausemenu.gameObject.SetActive(false);
			GameManager.instance.changeState(GameManager.instance.levelselector, null);
            
        }
		
        stringrem.text = stringLeft + "/21";
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






		//KeyCode[] buttonMapping = new KeyCode[] {KeyCode.W, KeyCode.D, KeyCode.S, KeyCode.A};
		//KeyCode[] buttonMapping2 = new KeyCode[] {KeyCode.UpArrow, KeyCode.RightArrow, KeyCode.DownArrow, KeyCode.LeftArrow};

		for (int i = 0; i < 4; i++) {
			//Direction lines up with input manager so we can directly convert to an action from a direction.
			GameManager.Direction dir = (GameManager.Direction)i;
			if (InputManager.instance.OnInput((InputManager.Action)i)) {
				bool canMove = false;
				Node otherNode = null;
				//if (currentPosition.GetConnectionFromDir(dir) != null) {
				if (currentPosition.GetConnectionFromDir(dir) >= 0) {
					//otherNode = map.nodes[(int)currentPosition.GetConnectionFromDir(dir)];
					otherNode = map[(int)currentPosition.GetConnectionFromDir(dir)];
					//See if the other node has a leave
					canMove = (otherNode.data.hasLeave == false && stringLeft > 0) || (otherNode.data.hasLeave && otherNode.data.leave.inverse() == dir);
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

						currentPosition = otherNode;

						nonEuclidRenderer.HandleRender(dir, currentPosition);
						animLockout = false;
						if (stringLeft == 0 && currentPosition.type == Node.TileType.target) {
							winTrigger = true;
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

	public void resetLevelAssets() {
		stringLeft = 21;
		nonEuclidRenderer.HandleRender(GameManager.Direction.East, currentPosition, false);
	}
}
