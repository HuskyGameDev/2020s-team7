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


		map = Generate_Room_Demo4.generateRoom();
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

        stringrem.text = stringLeft + "/21";
        //Dont do anything past here if we are doing an animation
        if (animLockout)
            return;
        else if (winTrigger) {
            wintext.SetActive(true);
            youWinScreenTimeout -= Time.deltaTime;
            if (youWinScreenTimeout < 0.0f) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            return;
        }



        //KeyCode[] buttonMapping = new KeyCode[] {KeyCode.W, KeyCode.D, KeyCode.S, KeyCode.A};
        //KeyCode[] buttonMapping2 = new KeyCode[] {KeyCode.UpArrow, KeyCode.RightArrow, KeyCode.DownArrow, KeyCode.LeftArrow};

		for (int i = 0; i < 4; i++) {
			//Direction lines up with input manager so we can directly convert to an action from a direction.
            Direction dir = (Direction)i;
            if (InputManager.instance.OnInput((InputManager.Action)i)) {
                if (currentPosition.GetConnectionFromDir(dir) != null) {
                    Node otherNode = map.nodes[(int)currentPosition.GetConnectionFromDir(dir)];
                    //See if the other node has a leave
                    if ((otherNode.data.hasLeave == false && stringLeft > 0) || (otherNode.data.hasLeave && otherNode.data.leave.inverse() == dir)) {
                        animLockout = true;
                        StartCoroutine(CharacterAnimator.instance.AnimateMovement(
                            (bool flag) => {
                                //Handle fake connection stacking
                                {
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
                                }
                                else {
                                    //Do a backup
                                    currentPosition.data.hasEnter =false;
                                    otherNode.data.hasLeave = false;
                                    stringLeft++;
                                }

                                currentPosition = otherNode;
                                nonEuclidRenderer.HandleRender(dir, currentPosition);
                                animLockout = false;
                                if (stringLeft == 0 && currentPosition.data.type == Node.LineData.TileType.target) {
                                    winTrigger = true;
                                }
                            },
                            dir,
                            moveAnimSpeed
                        ));
                    }
                }
                break;
            }
        }
    }




}
