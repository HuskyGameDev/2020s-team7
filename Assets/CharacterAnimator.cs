using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{

    public delegate void AnimationCallback(bool flag);

    public SpriteRenderer spriteRenderer;

    public static CharacterAnimator instance; //useless comment

    public Sprite[] sprites;
    private SpriteRenderer spriteR;
    private bool foot = false;
    private ArrayList characterSelect;
    private int characterInput = 0;

    void Start() { instance = this; }
 
    readonly Vector2[] moveDir = new Vector2[] { new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0) };
    public IEnumerator<object> AnimateMovement(AnimationCallback callback, GameManager.Direction dir, float time)
    {
        float progress = 0.0f;
        Vector3 startPos = this.gameObject.transform.position;
        Vector3 goalPos = startPos + (new Vector3(moveDir[(int)dir].x, moveDir[(int)dir].y, 0));

        //characterSelect = new ArrayList();
        //int[][] character1 = new int[][] { 1, 2 },;
        //characterSelect.Add(character1);
        //characterSelect.Add(character2);
        //characterSelect.Add(character3);

        //changes the direction of the player sprite
        spriteR = gameObject.GetComponent<SpriteRenderer>();
        if ((int)dir == 0)
        {
            spriteR.sprite = sprites[0];
        }
        else if ((int)dir == 1)
        {
            spriteR.sprite = sprites[1];
        }
        else if ((int)dir == 2)
        {
            spriteR.sprite = sprites[2];
        }
        else if ((int)dir == 3)
        {
            spriteR.sprite = sprites[3];
        }
        foot = !foot;
        //Slide forward
        while (progress <= time)
        {
            if ((int)dir == 0)
            {
                if (foot)
                {
                    spriteR.sprite = sprites[4];
                }
                else
                {
                    spriteR.sprite = sprites[8];
                }
                
            }
            else if ((int)dir == 1)
            {
                if (foot)
                {
                    spriteR.sprite = sprites[5];
                }
                else
                {
                    spriteR.sprite = sprites[9];
                }
            }
            else if ((int)dir == 2)
            {
                if (foot)
                {
                    spriteR.sprite = sprites[6];
                }
                else
                {
                    spriteR.sprite = sprites[10];
                }
            }
            else if ((int)dir == 3)
            {
                if (foot)
                {
                    spriteR.sprite = sprites[7];
                }
                else
                {
                    spriteR.sprite = sprites[11];
                }
            }
            progress += Time.deltaTime;
            this.gameObject.transform.position = Vector3.Lerp(startPos, goalPos, progress / time);
            yield return null;
        }
        //Snap back
        this.gameObject.transform.position = startPos;
        if ((int)dir == 0)
        {
            spriteR.sprite = sprites[0];
        }
        else if ((int)dir == 1)
        {
            spriteR.sprite = sprites[1];
        }
        else if ((int)dir == 2)
        {
            spriteR.sprite = sprites[2];
        }
        else if ((int)dir == 3)
        {
            spriteR.sprite = sprites[3];
        }

        //yield return null;

        callback(true);
    }
}
