using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SplashCtrl : MonoBehaviour {
	/// 使い方
	/// 1. SplashCtrlをGameManagerオブジェクトにアタッチ
	/// 2. ロゴ、フレームをそれぞれアタッチ

	public FadeCtrl _logo;
	public FadeCtrl _frame;
	bool isSplashStarted = false;

	// UIの場合
	bool isUI;

	// Use this for initialization
	void Start () {
		Init ();
	}

	void Init() {
		Color c = _logo.gameObject.GetComponent<Image> ().color;
		c.a = 0;
		_logo.gameObject.GetComponent<Image> ().color = c;
	}

	void Update () {
		if (!Application.isShowingSplashScreen) {
			if (!isSplashStarted) {
				StartCoroutine (splashFlow ());
				isSplashStarted = true;
			}
		}

	}

	IEnumerator splashFlow () {
		yield return 1;

		// フェードイン
		FadeIn(_logo, 0.8f);
//		iTween.FadeTo (_logo, iTween.Hash ("a", 1.0f, "time", 0.5f));

		yield return new WaitForSeconds (0.8f);

		// 待機
		yield return new WaitForSeconds(1f);

		// フェードアウト
		FadeOut(_logo, 0.5f);
//		iTween.FadeTo (_logo, iTween.Hash ("a", 0f, "time", 0.5f));
		yield return new WaitForSeconds (0.5f);

		FadeOut (_frame, 0.5f);
//		iTween.FadeTo (_frame, iTween.Hash ("a", 0f, "time", 0.5f));
		yield return new WaitForSeconds (0.5f);

		finishSplashFlow ();
	}
		
	void FadeIn(FadeCtrl pFade, float pTime) {
		pFade.Fade (0, 1.0f, pTime);
	}

	void FadeOut(FadeCtrl pFade, float pTime) {
		pFade.Fade (1.0f, 0f, pTime);
	}
				
	void finishSplashFlow () {
		// 終了
		// BGM再生
		GameManager.GetInstance ()._audioManager.playBGM ();

		// Splashシーンを非アクティブ化
		this.gameObject.SetActive (false);

		GameManager.GetInstance ()._adCtrl.ShowBanner ();
	}
}