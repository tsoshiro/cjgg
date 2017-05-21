using System.Collections;
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

	public iTween.EaseType _easeType = iTween.EaseType.easeInOutBack;
	public float ANIMATION_TIME = 0.22f;

	float X_VALUE = 150f;
	float Y_VALUE = 120f;
	float RESIZE_RATE = 0.70f;
	float RESIZE_FONT_RATE = 0.6f;
	float BASE_Y_POS = -200f;


	List<GameObject> _buttons = new List<GameObject>();

	// Use this for initialization
	void Start () {
		_inputManager = GameManager.GetInstance ()._inputManager;

		// defaultScaleに初期値を入れ、Scaleをゼロにしてインアクティブにしておく
		defaultScale = this.transform.localScale;
		this.transform.localScale = Vector3.zero;
		this.gameObject.SetActive (false);
	}

	public void Open(string pTitle, string pContent, List<CustomButton> pButtonList, GameObject pTargetObject) {
		_title.text = pTitle;
		_content.text = pContent;

		// コールバック先設定
		target = pTargetObject;
		List<Vector3> posList = getPositions (pButtonList.Count);

		// Buttonの配置
		for (int i = 0; i < pButtonList.Count; i++) {
			bool isButtonCreated = (_buttons != null) ? (i <= _buttons.Count - 1) : true;
			// ボタンが生成済みなら流用、そうでないなら新規生成
			GameObject obj = (isButtonCreated) ? _buttons [i] : InstantiateButton ();
			setButtonSetting (obj, pButtonList [i], posList [i]);
		}

		// 使うボタンを活性、使わないボタンを非活性にする
		// {0 1 2 3} 4 5 ← 4と5は使わないとする
		// _buttons.Count = 6 : pButtonList.Count = 4
		for (int i = 0; i < _buttons.Count; i++) {
			bool isUsedThisTime = (i < pButtonList.Count);
			_buttons [i].SetActive (isUsedThisTime); // 使うなら活性、使わないなら非活性
		}

		OpenAnimation ();
	}

	List<Vector3> getPositions(int pCount) {
		List<Vector3> result = new List<Vector3> ();

		// 行数から開始Y座標が決まる
		int rowCount = (pCount+1) / 2;
		float yStartPos = rowCount * Y_VALUE / 2;
		Debug.Log ("yStartPos:" + yStartPos);

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

			// startPosから、Y_VALUEの行数分下に
			y = yStartPos - Y_VALUE * row;

			pos = new Vector3(x, y, 0);
			result.Add(pos);
		}

		return result;
	}

	GameObject InstantiateButton() {
		GameObject obj = (GameObject)Instantiate (_buttonPrefab);

		// Image設定
		Image img = obj.GetComponent<Image> ();
		img.SetNativeSize ();
		img.preserveAspect = false;


		// 文字サイズ初期設定
		Text targetText = obj.GetComponentInChildren<Text> ();
		targetText.fontSize = Mathf.RoundToInt((float)targetText.fontSize * RESIZE_FONT_RATE);

		// オブジェクトサイズ調整・初期設定
		obj.transform.SetParent(this.transform);
		RectTransform rc = obj.GetComponent<RectTransform> ();
		rc.sizeDelta = new Vector2(rc.sizeDelta.x * RESIZE_RATE, rc.sizeDelta.y);
		obj.transform.localScale = Vector3.one;

		return obj;
	}

	void setButtonSetting(GameObject pButtonObject, CustomButton pButton, Vector3 pPosition) {
		GameObject obj = pButtonObject;
		obj.name = pButton._method;

		// ボタンにコールバックを仕込む
		Button btn = obj.GetComponent<Button> ();
		btn.onClick.RemoveAllListeners ();
		btn.onClick.AddListener (()=> actionCallbackBtn(obj));


		// 文字列更新
		Text targetText = obj.GetComponentInChildren<Text> ();
		targetText.text = pButton._text;

		setButtonImage (obj, pButton._buttonImage);

		// 位置調整
		obj.transform.localPosition = new Vector3 (0, BASE_Y_POS, 0); // BasePos設定
		obj.transform.localPosition += pPosition;

		if (!_buttons.Contains(obj)) // 含まない場合は、追加する
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
		DebugLogger.Log ("actionCallbackBtn:" + pGameObject.name);
		if (_inputManager.disabled) // input無効になっているかどうかチェック
			return;

		if (_inputManager.isLastUpTap ()) {
			GameManager.GetInstance()._audioManager.play (Const.SE_PAGE_TRANS);
			target.SendMessage (pGameObject.name);
		}
	}

	// Receivers
	public void actionBtn(GameObject pGameObject) {
		DebugLogger.Log ("actionBtn:" + pGameObject.name);
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

	void OpenAnimation() {
		this.gameObject.SetActive (true);
		iTween.ScaleTo ( this.gameObject, iTween.Hash("time", ANIMATION_TIME, "scale", defaultScale, "easetype", _easeType));
	}

	void CloseAnimation() {
		iTween.ScaleTo( this.gameObject, iTween.Hash("time", ANIMATION_TIME, "scale", Vector3.zero, "easetype", _easeType));
		iTween.MoveTo ( this.gameObject, iTween.Hash("time", ANIMATION_TIME, "oncomplete", "CloseAnimationOnComplete", "oncompletetarget", this.gameObject));
	}

	void CloseAnimationOnComplete() {
		this.gameObject.SetActive (false);
	}

	void fixDefaultSize() {
		this.gameObject.SetActive (false);
		this.gameObject.transform.localScale = defaultScale;
	}

}
