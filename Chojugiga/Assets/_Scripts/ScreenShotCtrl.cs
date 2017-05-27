using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShotCtrl : MonoBehaviour {
	bool isScreenShotSaved = false;
	bool isScreenShotSaveFailed = false;

	public void takeScreenShot() {
		isScreenShotSaved = false;
		isScreenShotSaveFailed = false;

		// Capture
		StartCoroutine (ScreenshotCapture.Capture (
			Const.SC_NAME,
			callbackCapture)
		);
	}

	void callbackCapture() {
		// capture success!
		isScreenShotSaved = true;
	}

	public void actionShare(string pMessage = "", bool pTakeScBefore = false) {
		// pTakeScBeforeならその場でスクリーンショットを撮ってからシェア
		if (pTakeScBefore) {
			takeScreenShot ();
		}
		// シェアコルーチン開始
		StartCoroutine (shareCoroutine (pMessage));
	}
		
	IEnumerator shareCoroutine(string pMessage) {
		// 操作できなくする
		GameManager.GetInstance()._inputManager.disabled = true;

		// セーブ処理が終わるまで待機
		yield return StartCoroutine (waitForSaveCoroutine ());

		// この時点で、isScreenShotSaveFailed = true
		// もしくはisScreenShotSaved = trueになっている

		// シェア機能呼び出し
		if (isScreenShotSaveFailed) {
			// 失敗していた場合、画像は添付しない
			share (pMessage, false);
		} else if (isScreenShotSaved) {
			// 成功していれば、画像は添付する
			share (pMessage, true);
		}

		// flgをリセット
		isScreenShotSaved = false;
		isScreenShotSaveFailed = false;
	}

	void share(string pMessage, bool pWithImage) {
		GameManager.GetInstance()._inputManager.disabled = false;
		if (pWithImage) {
			SocialConnector.SocialConnector.Share (Const.SHARE_MESSAGE,
				Const.APP_STORE_URL_BITLY, 
				Application.persistentDataPath + "/" + Const.SC_NAME);	
		} else {
			SocialConnector.SocialConnector.Share (Const.SHARE_MESSAGE,
				Const.APP_STORE_URL_BITLY);
		}
	}


	float TIME_OUT = 2f;
	public IEnumerator waitForSaveCoroutine() {
		// タイムアウト処理変数
		float time = 0;

		// スクリーンショットが保存完了する、もしくは失敗するまで待機
		while (!isScreenShotSaved) { // 
			// タイムアウト処理
			time += Time.deltaTime;
			if (time >= TIME_OUT) { // n秒経過
				isScreenShotSaveFailed = true;
				break;
			}
			yield return null;
		}
	}

	#region DEBUG
	[ContextMenu("Method")]
	public void Test() {
		isScreenShotSaved = false;
		isScreenShotSaveFailed = false;

		StartCoroutine (waitForSaveCoroutine ());
	}
	#endregion
}
