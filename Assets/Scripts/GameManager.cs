using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    bool animLockout = false;
    public static GameManager instance;
    public int stringLeft = 21;

    public enum Direction { North, East, South, West }

    public Node currentPosition = new Node();
    public Map map;
    public RenderingHandler nonEuclidRenderer;
    public bool winTrigger = false;
    public UnityEngine.UI.Text stringrem;
    public GameObject wintext;

	public Sprite[] spriteBook;
	// Use this for initialization
	void Start () {
        instance = this;

		InputManager.instance.LoadKeybinds();

		map = Generate_Room_5.generateRoom();
        if (currentPosition == null)
            currentPosition = map[0];
		nonEuclidRenderer.HandleRender(Direction.East, currentPosition, false);
	}

    public float moveAnimSpeed = 0.25f;
    public float youWinScreenTimeout = 7.0f;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}
		//Update text on the string remaining
        stringrem.text = stringLeft + "/21";

        //Dont do anything past here if we are doing an animation
        if (animLockout)
            return;
		//Check if we need to show the win stuff.
        else if (winTrigger) {
            wintext.SetActive(true);
            youWinScreenTimeout -= Time.deltaTime;
            if (youWinScreenTimeout < 0.0f) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            return;
        }
		//Loop through all possible movement directions and check if the player is trying to move in that direction
		for (int i = 0; i < 4; i++) {
			//Direction lines up with input manager so we can directly convert to an action from a direction.
            Direction dir = (Direction)i;
            if (InputManager.instance.OnInput((InputManager.Action)i)) {
                if (currentPosition.GetConnectionFromDir(dir) != null) {
                    Node otherNode = map.nodes[(int)currentPosition.GetConnectionFromDir(dir)];
                    //Check if we have string remaining to move, and we are not trying to move onto a square that has the line on it already (unless the player is backing up)
                    if ((otherNode.data.hasLeave == false && stringLeft > 0) || (otherNode.data.hasLeave && otherNode.data.leave.inverse() == dir)) {
                        animLockout = true;
                        StartCoroutine(CharacterAnimator.instance.AnimateMovement(
                            (bool flag) => {
                                //Handle fake connection stacking
                                {
                                    if (otherNode.GetConnectionFromDir(dir.inverse()) != currentPosition.index) {
										//We need to traverse on the horizontal axis of this direction in order to see if there are any squares that need to be supressed.
										Direction[] traverseSet = { dir.counterclockwise(), dir.clockwise() };
										//Start with the current cross pairing
										Node perspectiveNode = currentPosition;
										Node accrossNode = otherNode;
										bool didCenter = false;
										Debug.Log( traverseSet[0] + " |  " + traverseSet[1]);
										foreach (Direction travelDir in traverseSet) {
											while (true) {
												//If there is no where to travel up, break
												if (perspectiveNode == null || accrossNode == null)
													break;
												//If there is no conneection from where we are coming from, break
												if (perspectiveNode.GetConnectionFromDir(dir) == null)
													break;
												//Finnaly, if this is already normal connection, do not override it
												if (accrossNode.GetConnectionFromDir(dir.inverse()) == perspectiveNode.index)
													break;
												//If we pass those three checks, we need to override this node.
												//Make sure we do not create another connection at the current position
												if ((didCenter == true && perspectiveNode == currentPosition) == false) {
													Node.ConnectionSet newSet = accrossNode.connections.Copy();
													//If this is not the center connection, then we need to ensure that this connection does not get popped without the center one getting popped, so we flag it as being controlled by other.
													if (perspectiveNode != currentPosition)
														newSet.supressedBy = otherNode;
													newSet[dir.inverse()] = perspectiveNode.index;
													accrossNode.AddToConnectionStack(newSet);
												}
												//Advance forward and check those.
												perspectiveNode = (perspectiveNode.GetConnectionFromDir(travelDir) == null) ? null : map[(int)perspectiveNode.GetConnectionFromDir(travelDir)];
												accrossNode = (accrossNode.GetConnectionFromDir(travelDir) == null) ? null : map[(int)accrossNode.GetConnectionFromDir(travelDir)];
											}
											//Reset so we can traverse the other direction.
											perspectiveNode = currentPosition;
											accrossNode = otherNode;
											//Mark that we already did the center
											didCenter = true;
										}
									}
                                }

                                //IF the node we are trying to move to does not have a line leaving it, we know we are advancing into a new square
                                if (otherNode.data.hasLeave == false) {
                                    currentPosition.data.leave = dir;
                                    otherNode.data.enter = dir.inverse();
                                    currentPosition.data.hasLeave = true;
                                    otherNode.data.hasEnter = true;
                                    stringLeft--;
                                }
                                else {
                                    //If we have made it this far in the checks and the other node has a line leaving it, we know that this is a backup move, so we just delete our current nodes enter and the new ones leave.
                                    currentPosition.data.hasEnter =false;
                                    otherNode.data.hasLeave = false;
                                    stringLeft++;
                                }
								//Update our current position
                                currentPosition = otherNode;
								//Have the renderer draw the new position
                                nonEuclidRenderer.HandleRender(dir, currentPosition);
								//Disable the controlls lockout for the animation
                                animLockout = false;
								//Check if we have no string remaining and we are on a target node, Brute forcetrigger a victory if so
								if (stringLeft == 0 && currentPosition.data.type == Node.LineData.TileType.target) {
                                    winTrigger = true;
                                }
                            },
                            dir,
                            moveAnimSpeed
                        ));
                    }
                }
				//Since the player moved in this direction, break the loop so we do not move in any other directions.
                break;
            }
        }
    }

}
