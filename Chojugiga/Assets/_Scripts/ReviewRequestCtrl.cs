using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReviewRequestCtrl : MonoBehaviour {
	GameManager _gameManager;
	void Start () {
		_gameManager = GameManager.GetInstance ();
	}

	public class Button {
		public string _text;
		public int _buttonImage;
		public string _method;

		public Button(string pText, int pInt, string pMethod) {
			_text = pText;
			_buttonImage = pInt;
			_method = pMethod;
		}
	}

	public bool ReviewRequest () {
		// プレイ回数がx回以上のユーザーに対して
		int playCount = _gameManager._userData.play_count;
		if (CheckIsPlayCountUnderOrAlreadyReviewed (playCount,
													_gameManager._userData.review_flg))
		{
			return false; // 何もしない
		}

		if (CheckIsOkToAskReview(playCount, _gameManager._userData.denied_flg))
		{ // 規定回の倍数ならレビュー依頼してみる
			// ラベル設定
			// 選択肢とそこで呼ばれるアクションの設定
			// メソッドをコールするGameObjectを設定
			string title = "";
			string content = "プレイしていただき、ありがとうございます。\n楽しんでいただけていますか？";

			List<Button> buttons = new List<Button> ();
			buttons.Add (new Button ("はい", (int)Const.ButtonType.POSITIVE, "AskForReview"));
			buttons.Add (new Button ("そんなに...", (int)Const.ButtonType.DEFAULT, "NoThanks"));

			// YES NO ダイアログ
			//「楽しんでいただけていますか？」とのダイアログを出す。
		}
		return false;
	}

	void NoThanks() {
		if (_gameManager._userData.message_flg == 0) {
			AskForMessage ();
		}
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
		string title = "ありがとうございます！";
		string content = "よかったら\n☆5レビュー、\nお願いします。";
		string image = "";

		List<Button> buttons = new List<Button> ();
		buttons.Add (new Button ("レビューする", (int)Const.ButtonType.POSITIVE, "OpenURL"));
		buttons.Add (new Button ("もう表示しない", (int)Const.ButtonType.DEFAULT, "NoMoreRequest"));
		buttons.Add (new Button ("また今度", (int)Const.ButtonType.DEFAULT, "Close"));

		// Dialog出力
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
	}

	void AskForMessage () {
		string title = "";
		string content = "よろしければ\nご意見・ご要望、\nお待ちしています。";

		List<Button> buttons = new List<Button> ();
		buttons.Add (new Button ("ひとこと言う", (int)Const.ButtonType.POSITIVE, "OpenDevURL"));
		buttons.Add (new Button ("もう表示しない", (int)Const.ButtonType.DEFAULT, "NoMoreRequest"));
		buttons.Add (new Button ("また今度", (int)Const.ButtonType.DEFAULT, "Close"));

		// YES NO ダイアログ
		// お問合せ
	}
}
