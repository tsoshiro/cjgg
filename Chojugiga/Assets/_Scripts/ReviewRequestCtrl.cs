using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReviewRequestCtrl : MonoBehaviour {
	GameManager _gameManager;
	public PopUpCtrl _popUpCtrl; 

	void Start () {
		_gameManager = GameManager.GetInstance ();
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.A)) {
			createAskingPopUp ();
		}
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
		// ラベル設定
		// 選択肢とそこで呼ばれるアクションの設定
		// メソッドをコールするGameObjectを設定
		string title = "";
		string content = "プレイしていただき、ありがとうございます。\n楽しんでいただけていますか？";

		List<CustomButton> buttons = new List<CustomButton> ();
		buttons.Add (new CustomButton ("そんなに...", (int)Const.ButtonType.DEFAULT, "AskForMessage"));
		buttons.Add (new CustomButton ("はい", (int)Const.ButtonType.POSITIVE, "AskForReview"));

		// YES NO ダイアログ
		//「楽しんでいただけていますか？」とのダイアログを出す。
		_popUpCtrl.Open(title, content, buttons, this.gameObject);
	}

	/// <summary>
	/// 楽しんでいないとの回答に対し、意見をもらうポップアップを表示する
	/// </summary>
	void AskForMessage() {
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
		createAskForReviewPopUp ();
	}

	/// <summary>
	/// レビュー依頼ポップアップを表示
	/// </summary>
	void createAskForReviewPopUp() {
		string title = "ありがとうございます！";
		string content = "よかったら\n☆5レビュー、\nお願いします。";

		List<CustomButton> buttons = new List<CustomButton> ();
		buttons.Add (new CustomButton ("レビューする", (int)Const.ButtonType.POSITIVE, "OpenURL"));
		buttons.Add (new CustomButton ("もう表示しない", (int)Const.ButtonType.DEFAULT, "NoMoreRequest"));
		buttons.Add (new CustomButton ("また今度", (int)Const.ButtonType.DEFAULT, "Close"));

		// Dialog出力
		_popUpCtrl.Open(title, content, buttons, this.gameObject);
	}

	// 開く
	void OpenURL() {
		_gameManager._userData.review_flg = 1;
		_gameManager._userData.save ();

		Application.OpenURL (Const.APP_URL);
	}

	void OpenDevURL() {
		_gameManager._userData.message_flg = 1;
		_gameManager._userData.save ();

		Application.OpenURL (Const.DEV_URL);
	}

	// 断られた際
	void NoMoreRequest() {
		_gameManager._userData.denied_flg = 1;
		_gameManager._userData.save ();
	}


	void Close() {
		// 閉じる
		_popUpCtrl.Close();
	}

	void createAskForMessagePopUp () {
		string title = "";
		string content = "よろしければ\nご意見・ご要望、\nお待ちしています。";

		List<CustomButton> buttons = new List<CustomButton> ();
		buttons.Add (new CustomButton ("ひとこと言う", (int)Const.ButtonType.POSITIVE, "OpenDevURL"));
		buttons.Add (new CustomButton ("もう表示しない", (int)Const.ButtonType.DEFAULT, "NoMoreRequest"));
		buttons.Add (new CustomButton ("また今度", (int)Const.ButtonType.DEFAULT, "Close"));

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