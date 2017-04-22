using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
	GameManager _gameManager;

	// Use this for initialization
	void Start () {
		_gameManager = this.gameObject.GetComponent<GameManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		flick ();
	}

	enum TouchState {
		NONE,
		BEGAN,
		TOUCHING,
		END
	}

	TouchState touchState;
	float touchingCounter = 0f;
	float flickDistance = 0f;
	public float FLICK_TIME = 0.2f;
	public float FLICK_DISTANCE = 2f;

	Vector3 touchPosition;
	Vector3 lastTouchPosition;
	Vector3 velocity;

	void flick() {
		switch (touchState) {
		case TouchState.NONE:
			if (Input.GetMouseButtonDown (0)) {
				touchState = TouchState.TOUCHING;
				lastTouchPosition = Input.mousePosition;
			}
			break;
		case TouchState.TOUCHING:
			touchingCounter += Time.deltaTime;
			if (touchingCounter >= FLICK_TIME) { // FLICK_TIME秒過ぎたら距離計測をリセットする
				touchingCounter = 0;
				lastTouchPosition = Input.mousePosition;
			}
				
			if (Input.GetMouseButtonUp (0)) {
				touchState = TouchState.NONE;
				touchPosition = Input.mousePosition;

				if (checkFlick ()) {
					_gameManager.flick ();
//					Debug.Log ("FLICK!");
//					Debug.Log("d:"+getDirection ());
				}
//				Debug.Log("touchPosition:" + touchPosition + " lastTouchPosition:" + lastTouchPosition + " flickDistance:" + flickDistance);
				touchingCounter = 0;
			}
			break;
		}
	}

	bool checkFlick() {
		flickDistance = Vector2.Distance (touchPosition, lastTouchPosition);

		// flickCheck
		if (touchingCounter <= FLICK_TIME &&
			flickDistance >= FLICK_DISTANCE) {
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
