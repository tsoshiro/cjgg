using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public GameObject _imagePanel;
	public QuestionCtrl _questionCtrl;

	float X = 1.5f;

	// Use this for initialization
	void Start () {
		UpdateImage ();
	}
		
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			answerLeft ();
		} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
			answerRight ();
		} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
			answerDown ();
		}

		if (Input.GetMouseButtonDown (0)) {
			UpdateImage ();
		}
	}

	void answerLeft() {
		answer (0);
	}

	void answerRight() {
		answer (1);
	}

	void answerDown() {
		answer (2);
	}

	void answer(int pAnswer) {
		if (_questionCtrl.getAnswer () < 0) {
			Debug.Log ("ERROR!!!!!");
			return;
		}
		if (_questionCtrl.getAnswer () == pAnswer) {
			Debug.Log ("CORRECT!");
		} else {
			Debug.Log ("WRONG!");
		}

		UpdateImage ();
	}


	void UpdateImage() {
		_imagePanel.GetComponent<SpriteRenderer> ().sprite = _questionCtrl.getQSprite();
		setPosition ();
//		_imagePanel.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("image_3");
	}

	void setPosition() {
		Vector3 pos = new Vector3 ();

		if ((int)_questionCtrl.getQPos () == 0) {
			pos.x = X;
		} else {
			pos.x = -X;
		}
		_imagePanel.transform.position = pos;
	}
}
