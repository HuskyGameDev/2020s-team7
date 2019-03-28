using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour {

	public delegate void AnimationCallback(bool flag);

	public SpriteRenderer spriteRenderer;

	public static CharacterAnimator instance; //useless comment

	void Start() { instance = this; }

	readonly Vector2[] moveDir = new Vector2[] { new Vector2(0,1), new Vector2(1,0), new Vector2(0,-1), new Vector2(-1,0) };
	public IEnumerator<object> AnimateMovement(AnimationCallback callback, GameManager.Direction dir, float time) {
		float progress = 0.0f;
		Vector3 startPos = this.gameObject.transform.position;
		Vector3 goalPos = startPos + (new Vector3(moveDir[(int)dir].x, moveDir[(int)dir].y, 0));
		//Slide forward
		while (progress <= time) {
			progress += Time.deltaTime;
			this.gameObject.transform.position = Vector3.Lerp(startPos,goalPos, progress / time);
			yield return null;
		}
		//Snap back
		this.gameObject.transform.position = startPos;

		//yield return null;

		callback(true);
	}
}
