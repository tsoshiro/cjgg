using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommentCtrl : MonoBehaviour {
	public Text _result;
	public Text _comment;
	public Image _image;

	GameManager _gameManager;

	void Start() {
		_gameManager = GameManager.GetInstance ();
	}

	public void setComment(string pStr) {
		_comment.text = pStr;
	}

	public void setImage(Sprite pSprite) {
		_image.sprite = pSprite;
	}

	public void setResult(string pStr) {
		_result.text = pStr;
	}
}
