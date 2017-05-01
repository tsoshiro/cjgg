﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpCtrl : MonoBehaviour {
	public InputManager _inputManager;

	public Text _title;
	public Text _content;

	public Vector3 defaultScale;

	// Use this for initialization
	void Start () {
		defaultScale = this.transform.localScale;

		this.gameObject.SetActive (false);
	}

	public void Open(string pTitle, string pContent = "") {
		_title.text = pTitle;

		if (pContent != "") 
			_content.text = pContent;

		OpenAnimation ();
	}

	// Receivers
	public void actionBtn(GameObject pGameObject) {
		if (_inputManager.disabled) // input無効になっているかどうかチェック
			return;

		if (_inputManager.isLastUpTap ()) {
			this.gameObject.SendMessage ("action" + pGameObject.name);
		}
	}

	void actionButtonClose() {
		Close ();
	}

	public void Close() {
		CloseAnimation ();
	}

	float ANIMATION_TIME = 0.1f;

	void OpenAnimation() {
		this.gameObject.SetActive (true);

		iTween.ScaleTo ( this.gameObject, iTween.Hash("time", ANIMATION_TIME, "scale", defaultScale));
	}

	void CloseAnimation() {
		iTween.ScaleTo( this.gameObject, iTween.Hash("time", ANIMATION_TIME, "scale", Vector3.zero));
		Invoke ("CloseAnimationOnComplete", 0.2f);
	}

	void CloseAnimationOnComplete() {
		this.gameObject.SetActive (false);
	}

	void fixDefaultSize() {
		this.gameObject.SetActive (false);
		this.gameObject.transform.localScale = defaultScale;
	}
}
