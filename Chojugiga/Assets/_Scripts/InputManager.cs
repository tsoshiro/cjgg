using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
	GameManager _gameManager;

	public bool disabled = false;
	float disabledTime = 0f;

	// Use this for initialization
	void Start () {
		_gameManager = this.gameObject.GetComponent<GameManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (disabled) {
			disabledTime -= Time.deltaTime;
			if (disabledTime <= 0) {
				disabled = false;
			}
			return;
		}
		flick ();
	}

	public void setDisabled(float pTime) {
		disabledTime = pTime;
		disabled = true;
	}

	enum TouchState {
		NONE,
		BEGAN,
		TOUCHING,
		END
	}

	TouchState touchState;
	float touchingTime = 0f;
	float touchingCounter = 0f;
	float flickDistance = 0f;

	public float TAP_TIME = 0.1f; // タップして離すまでの時間で、そのUpがタップかそうでないかを決める
	public float FLICK_TIME = 0.2f;
	public float FLICK_DISTANCE = 2f;

	Vector3 firstTouchPosition;
	Vector3 touchPosition;
	Vector3 lastTouchPosition;
	Vector3 velocity;

	void flick() {
		if (Input.GetMouseButtonDown (0)) {
			firstTouchPosition = Input.mousePosition;
			touchPosition = Input.mousePosition;
			lastTouchPosition = Input.mousePosition;

			touchingTime = 0;
			touchingCounter = 0;
		} else if (Input.GetMouseButton (0)) {
			touchingCounter += Time.deltaTime;
			touchingTime += Time.deltaTime;

			touchPosition = Input.mousePosition;

			if (touchingCounter >= FLICK_TIME) { // FLICK_TIME秒過ぎたら距離計測をリセットする
				touchingCounter = 0;
				lastTouchPosition = Input.mousePosition;
			}
		} else if (Input.GetMouseButtonUp (0)) {
			touchPosition = Input.mousePosition;
			flickDistance = Vector2.Distance (touchPosition, lastTouchPosition);
			if (checkFlick ()) {
				_gameManager.flick ();
			}
		}

		return;


		switch (touchState) {
		case TouchState.NONE:
			if (Input.GetMouseButtonDown (0)) {
				touchState = TouchState.TOUCHING;
				firstTouchPosition = Input.mousePosition;
				lastTouchPosition = Input.mousePosition;
				touchingTime = 0;
				touchingCounter = 0;
			}
			break;
		case TouchState.TOUCHING:
			touchingCounter += Time.deltaTime;
			touchingTime += Time.deltaTime;

			touchPosition = Input.mousePosition;

			if (touchingCounter >= FLICK_TIME) { // FLICK_TIME秒過ぎたら距離計測をリセットする
				touchingCounter = 0;
				lastTouchPosition = Input.mousePosition;
			}
				
			if (Input.GetMouseButtonUp (0)) {
				touchState = TouchState.NONE;
				flickDistance = Vector2.Distance (touchPosition, lastTouchPosition);

				if (checkFlick ()) {
					_gameManager.flick ();
//					Debug.Log ("FLICK!");
//					Debug.Log("d:"+getDirection ());
				}
//				Debug.Log("touchPosition:" + touchPosition + " lastTouchPosition:" + lastTouchPosition + " flickDistance:" + flickDistance);
			}
			break;
		}
	}

	public bool isLastUpTap() {
		float dist = Vector2.Distance(firstTouchPosition, touchPosition);
//		Debug.Log ("dist:" + dist);
		if (dist < FLICK_DISTANCE) {
			return true;
		}
//		if (touchingTime <= TAP_TIME) {
//			
//			return true;
//		}
		return false;
	}

	bool checkFlick() {
		// flickCheck
		if (flickDistance >= FLICK_DISTANCE) {
			return true;
		}
		return false;
	}

	enum Direction {
		UP,
		RIGHT,
		DOWN,
		LEFT
	}

	/// <summary>
	/// Gets the direction.
	/// </summary>
	/// <returns>The direction.</returns>
	Direction getDirection() {
		float x = touchPosition.x - lastTouchPosition.x;
		float y = touchPosition.y - lastTouchPosition.y;
		float rad = Mathf.Atan2 (y, x);
		float v = rad * Mathf.Rad2Deg;
		Direction d = Direction.UP;
		if (v > -45 && v <= 45) {
			d = Direction.RIGHT;
		} else if (v >= 45 && v < 135) {
			d = Direction.UP;
		} else if (v >= 135 ||
		           v <= -135) {
			d = Direction.LEFT;
		} else {
			d = Direction.DOWN;
		}

		return d;
	}
}
