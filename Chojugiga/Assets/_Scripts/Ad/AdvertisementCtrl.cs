using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvertisementCtrl : MonoBehaviour {
	GameManager _gameManager;
	AdvertisementManager _adManager;

	void Start () {
		_gameManager = GameManager.GetInstance ();
		_adManager = this.transform.GetComponentInChildren<AdvertisementManager> ();
	}

	/// <summary>
	/// フッターバナーを表示
	/// </summary>
	public void ShowBanner() {
		_adManager.ShowBanner ();	
	}

	public void ShowInters(int pIndex = 0) {
		_adManager.showInterstitial (pIndex);
	}

	// インタースティシャルを表示するかどうか確認し、表示
	public void checkInterstitial ()
	{
		bool flg = checkInterFlgFromValues (_gameManager._userData.play_count);
		if (flg) {
			ShowInters (Const.AD_INTER_NUM_END);
		} else { 
			DebugLogger.Log("Don't show Interstitial this time");
		}
	}

	// 判定処理を切り出し
	public bool checkInterFlgFromValues (int pPlayCount, int pRestartCount = 0) {
		int inter_value = pPlayCount + pRestartCount;
		DebugLogger.Log ("playCount:" + pPlayCount + "\n"
				   + "restartCount:" + pRestartCount + "\n"
				   + "inter_value:" + inter_value);

		if (inter_value % Const.AD_INTERVAL_INTER == 0) {
			return true;
		}
		return false;
	}
}
