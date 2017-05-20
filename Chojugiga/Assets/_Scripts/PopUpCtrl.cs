﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PopUpCtrl : MonoBehaviour {
	InputManager _inputManager;

	public Text _title;
	public Text _content;

	public Vector3 defaultScale;

	public GameObject _buttonPrefab;
	public List<Sprite> _buttonImages;

	float X_VALUE = 160f;
	float Y_VALUE = 80f;
	float RESIZE_RATE = 0.75f;

	List<GameObject> _buttons = new List<GameObject>();

	// Use this for initialization
	void Start () {
		_inputManager = GameManager.GetInstance ()._inputManager;

		// defaultScaleに初期値を入れ、Scaleをゼロにしてインアクティブにしておく
		defaultScale = this.transform.localScale;
		this.transform.localScale = Vector3.zero;
		this.gameObject.SetActive (false);
	}

	void resetButtons() {
		if (_buttons.Count <= 0)
			return;
		for (int i = 0; i < _buttons.Count; i++) {
			Destroy (_buttons [i]);
		}
	}

	public void Open(string pTitle, string pContent, List<CustomButton> pButtonList, GameObject pTargetObject) {
		resetButtons ();

		_title.text = pTitle;
		_content.text = pContent;

		// コールバック先設定
		target = pTargetObject;
		List<Vector3> posList = getPositions (pButtonList.Count);

		// Buttonの配置
		_buttons = new List<GameObject>();
		for (int i = 0; i < pButtonList.Count; i++) {
			setButtonSetting (pButtonList [i], posList[i]);
		}

		OpenAnimation ();
	}

	List<Vector3> getPositions(int pCount) {
		List<Vector3> result = new List<Vector3> ();

		// 行数から開始Y座標が決まる
		int rowCount = (pCount / 2) + 1;
		float yStartPos = rowCount * Y_VALUE / 2;

		// 座標設定用変数
		float x, y;
		Vector3 pos;

		// 1行目のボタン数は、余りが出るかどうかで決まる
		bool isFirstRowSingle = (pCount % 2 == 0) ? false : true;

		for (int i = 0; i < pCount; i++) {
			// 行数をIndex的に取得
			// 1行目のボタンがひとつの場合、すぐに行が切り替わる
			int row = (isFirstRowSingle) ? (i+1)/2 : i/2;

			if (i == 0 && isFirstRowSingle) { // 1行目、かつ1行目のボタン数がひとつの時の処理
				x = 0;
			} else { // そうでない場合	
				// 2で割り切れるかで、左側か右側かが決まる
				bool isLeft = (i % 2 == 0) ? false : true;
				// 1行目のボタンがひとつの場合、上記設定が逆になる
				isLeft = (isFirstRowSingle) ? isLeft : !isLeft;

				x = (isLeft) ? - X_VALUE : X_VALUE;
			}
			y = yStartPos - Y_VALUE * row;
			pos = new Vector3(x, y, 0);
			result.Add(pos);
		}

		return result;
	}

	void setButtonSetting(CustomButton pButton, Vector3 pPosition) {
		GameObject obj = (GameObject)Instantiate(_buttonPrefab);
		obj.name = pButton._method;

		// ボタンにコールバックを仕込む
		Button btn = obj.GetComponent<Button> ();
		btn.onClick.AddListener (()=> actionCallbackBtn(obj));

		Image img = obj.GetComponent<Image> ();
		img.SetNativeSize ();
		img.preserveAspect = false;

		// 文字列更新・サイズ更新
		Text targetText = obj.GetComponentInChildren<Text> ();
		targetText.text = pButton._text;
		targetText.fontSize = Mathf.RoundToInt((float)targetText.fontSize * RESIZE_RATE);

		setButtonImage (obj, pButton._buttonImage);

		// 位置調整
		obj.transform.SetParent(this.transform);
		obj.transform.localPosition = new Vector3 (0, -100, 0); // BasePos設定
		obj.transform.localPosition += pPosition;

		// サイズ調整
		RectTransform rc = obj.GetComponent<RectTransform> ();
		rc.sizeDelta = rc.sizeDelta * RESIZE_RATE;
		obj.transform.localScale = Vector3.one;

		_buttons.Add (obj);
	}

	public void Open(string pTitle, string pContent = "") {
		_title.text = pTitle;

		if (pContent != "") 
			_content.text = pContent;

		OpenAnimation ();
	}

	public void Open(string pContent = "") {
		_title.text = "";

		if (pContent != "")
			_content.text = pContent;

		OpenAnimation ();
	}

	#region ButtonImage
	void setButtonImage(GameObject pGameObject, int pImageIndex) {
		pGameObject.GetComponent<Image> ().sprite = _buttonImages [pImageIndex];
	}
	#endregion

	#region ActionButton
	GameObject target;
	void actionCallbackBtn(GameObject pGameObject) {
		Debug.Log ("actionCallbackBtn:" + pGameObject.name);
		if (_inputManager.disabled) // input無効になっているかどうかチェック
			return;

		if (_inputManager.isLastUpTap ()) {
			target.SendMessage (pGameObject.name);
		}
	}

	// Receivers
	public void actionBtn(GameObject pGameObject) {
		Debug.Log ("actionBtn:" + pGameObject.name);
		if (_inputManager.disabled) // input無効になっているかどうかチェック
			return;

		if (_inputManager.isLastUpTap ()) {
			this.gameObject.SendMessage ("action" + pGameObject.name);
		}
	}

	void actionButtonClose() {
		Close ();
	}
	#endregion

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
		Invoke ("CloseAnimationOnComplete", ANIMATION_TIME * 3);
	}

	void CloseAnimationOnComplete() {
		this.gameObject.SetActive (false);
	}

	void fixDefaultSize() {
		this.gameObject.SetActive (false);
		this.gameObject.transform.localScale = defaultScale;
	}

	#region DATA


	#endregion
}
