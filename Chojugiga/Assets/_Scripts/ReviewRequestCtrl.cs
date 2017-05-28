using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReviewRequestCtrl : MonoBehaviour {
	GameManager _gameManager;
	public PopUpCtrl _popUpCtrl; 

	void Start () {
		_gameManager = GameManager.GetInstance ();
	}

	[ContextMenu("CreatePopup")]
	public void sampleRequest() {
		createAskingPopUp ();
	}
		

	// プレイ回数から楽しんでいるかポップアップを出すか判定する
	public bool checkIsToBeAsked(int pPlayCount, int pReviewFlg, int pDeniedFlg) {
		// プレイ回数がx回以上のユーザーに対して
		int playCount = pPlayCount;
		if (CheckIsPlayCountUnderOrAlreadyReviewed (playCount, pReviewFlg))
		{
			return false; // 何もしない
		}

		if (CheckIsOkToAskReview(playCount, pDeniedFlg))
		{ // 規定回の倍数ならレビュー依頼してみる
			return true;
		}
		return false;	
	}

	/// <summary>
	/// プレイ回数によって楽しんでいるかポップアップを表示
	/// </summary>
	/// <returns><c>true</c>, if is to be asked was checked, <c>false</c> otherwise.</returns>
	public void checkIsToBeAsked () {
		UserData ud = _gameManager._userData;
		if (checkIsToBeAsked (ud.play_count, ud.review_flg, ud.denied_flg))
			createAskingPopUp ();
	}

	/// <summary>
	/// 楽しんでいるかポップアップを表示
	/// </summary>
	void createAskingPopUp() {
		_gameManager._analyticsManager.SendCounterEvent (Const.UA_ASKING_POP_UP);

		// ラベル設定
		// 選択肢とそこで呼ばれるアクションの設定
		// メソッドをコールするGameObjectを設定
		string title = "";
		string content = Const.MSG_ASKING_POPUP;

		List<CustomButton> buttons = new List<CustomButton> ();
		buttons.Add (new CustomButton (
			Const.MSG_ASKING_POPUP_ANS_NO,
			(int)Const.ButtonType.DEFAULT,
			"AskForMessage"));
		buttons.Add (new CustomButton (
			Const.MSG_ASKING_POPUP_ANS_YES,
			(int)Const.ButtonType.POSITIVE, 
			"AskForReview"));

		// YES NO ダイアログ
		//「楽しんでいただけていますか？」とのダイアログを出す。
		_popUpCtrl.Open(title, content, buttons, this.gameObject);
	}

	/// <summary>
	/// 楽しんでいないとの回答に対し、意見をもらうポップアップを表示する。すでにメッセージ回答済みなら何もせず閉じる
	/// </summary>
	void AskForMessage() {
		_gameManager._analyticsManager.SendCounterEvent (Const.UA_PRESSED_NOT_ENJOYING);

		if (_gameManager._userData.message_flg == 0)
			createAskForMessagePopUp ();
		else
			Close ();
	}

	public bool CheckIsPlayCountUnderOrAlreadyReviewed (int playCount, int reviewDoneFlg)
	{
		DebugLogger.Log ("CheckIsPlayCountUnderOrAlreadyReviewed\nplayCount:" + playCount + " reviewDoneFlg:" + reviewDoneFlg);
		if (playCount < Const.INTERVAL_REVIEW_REQUEST || // 規定回以上プレイしていない
			reviewDoneFlg == 1) // レビュー済み
		{
			return true; // 何もしない
		}
		return false;
	}

	public bool CheckIsOkToAskReview (int playCount, int deniedFlg)
	{
		// 以前レビュー以来を断ったかどうかによって間隔を変える
		int reviewRequestFreqCount
			= (deniedFlg == 0)
			? Const.INTERVAL_REVIEW_REQUEST
	   		: Const.INTERVAL_REVIEW_REQUEST_CANCELED_ONCE;

		if (playCount % reviewRequestFreqCount == 0) {
			return true;
		}
		return false;
	}

	void AskForReview () {
		_gameManager._analyticsManager.SendCounterEvent (Const.UA_PRESSED_ENJOYING);
		createAskForReviewPopUp ();
	}

	/// <summary>
	/// レビュー依頼ポップアップを表示
	/// </summary>
	void createAskForReviewPopUp() {
		string title = Const.MSG_ASKING_REVIEW_TITLE;
		string content = Const.MSG_ASKING_REVIEW_CONTENT;

		List<CustomButton> buttons = new List<CustomButton> ();
		buttons.Add (new CustomButton (
			Const.MSG_ASKING_REVIEW_ANS_OK,
			(int)Const.ButtonType.POSITIVE,
			"OpenURL"));
		buttons.Add (new CustomButton (
			Const.MSG_ASKING_REVIEW_ANS_NEVER,
			(int)Const.ButtonType.DEFAULT,
			"NoMoreReviewRequest"));
		buttons.Add (new CustomButton (
			Const.MSG_ASKING_REVIEW_ANS_NO,
			(int)Const.ButtonType.DEFAULT,
			"CloseReviewRequest"));

		// Dialog出力
		_popUpCtrl.Open(title, content, buttons, this.gameObject);
	}

	// 「レビューしてください」に対して「はい」と答えたで、レビューページを開く
	void OpenURL() {
		_gameManager._analyticsManager.SendCounterEvent (Const.UA_PRESSED_REVIEW);

		_gameManager._userData.review_flg = 1;
		_gameManager._userData.save ();

		// ポップアップを閉じてから開く
		Close ();
		OpenReviewURL ();
	}

	/// <summary>
	/// レビューページを開く
	/// </summary>
	public void OpenReviewURL() {
		Application.OpenURL (Const.APP_STORE_REVIEW_URL);
	}

	/// <summary>
	/// サポートページに遷移する
	/// </summary>
	void OpenSupportURL() {
		_gameManager._userData.message_flg = 1;
		_gameManager._userData.save ();

		// ポップアップを閉じてから開く
		Close ();
		Application.OpenURL (Const.SUPPORT_URL);
	}

	// レビュー依頼に対して「もう表示しない」
	void NoMoreReviewRequest() {
		_gameManager._analyticsManager.SendCounterEvent (Const.UA_PRESSED_NOT_REVIEW_EVER);
		NoMoreRequest ();
	}
	// メッセージ依頼に対して「もう表示しない」
	void NoMoreMessageRequest() {
		_gameManager._analyticsManager.SendCounterEvent (Const.UA_PRESSED_NOT_MESSAGE_EVER);
		NoMoreRequest ();
	}

	// 断られた際
	void NoMoreRequest() {
		_gameManager._analyticsManager.SendCounterEvent (Const.UA_PRESSED_NOT_REVIEW_EVER);

		_gameManager._userData.denied_flg = 1;
		_gameManager._userData.save ();

		Close ();
	}

	// レビュー依頼に対して「また今度」
	void CloseReviewRequest() {
		_gameManager._analyticsManager.SendCounterEvent (Const.UA_PRESSED_NOT_REVIEW_NOW);

		Close ();
	}

	// メッセージ依頼に対して「また今度」
	void CloseMessageRequest() {
		_gameManager._analyticsManager.SendCounterEvent (Const.UA_PRESSED_NOT_MESSAGE_NOW);

		Close ();
	}

	void Close() {
		// 閉じる
		_popUpCtrl.Close();
	}

	void createAskForMessagePopUp () {
		string title = "";
		string content = Const.MSG_ASKING_MESSAGE;

		List<CustomButton> buttons = new List<CustomButton> ();
		buttons.Add (new CustomButton (
			Const.MSG_ASKING_MESSAGE_ANS_OK,
			(int)Const.ButtonType.POSITIVE,
			"OpenSupportURL"));
		buttons.Add (new CustomButton (
			Const.MSG_ASKING_MESSAGE_ANS_NEVER, 
			(int)Const.ButtonType.DEFAULT, 
			"NoMoreMessageRequest"));
		buttons.Add (new CustomButton (
			Const.MSG_ASKING_MESSAGE_ANS_NO,
			(int)Const.ButtonType.DEFAULT,
			"CloseMessageRequest"));

		// YES NO ダイアログ
		// お問合せ
		_popUpCtrl.Open(title, content, buttons, this.gameObject);
	}
}
public class CustomButton {
	public string _text;
	public int _buttonImage;
	public string _method;

	public CustomButton(string pText, int pInt, string pMethod) {
		_text = pText;
		_buttonImage = pInt;
		_method = pMethod;
	}
}