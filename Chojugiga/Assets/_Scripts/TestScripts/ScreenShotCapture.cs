//  ScreenshotCaptor.cs
//  http://kan-kikuchi.hatenablog.com/entry/ScreenshotCaptor
//
//  Created by kan.kikuchi on 2016.08.30.

using UnityEngine;
using System;
using System.IO;
using System.Collections;

/// <summary>
/// スクリーンショットを撮るクラス
/// </summary>
public static class ScreenshotCapture {

	/// <summary>
	/// スクリーンショットを撮る
	/// </summary>
	public static IEnumerator Capture(string imageName = "", Action callback = null){		
		//スクショを保存するパスを作成
		string imagePath = (imageName == "") ? Const.SC_NAME : imageName;

		//iOS、Android実機の時はパスにApplication.persistentDataPathを追加
		#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
		imagePath = Path.Combine(Application.persistentDataPath, imageName);
		#endif

		//前に撮ったスクショを削除
		File.Delete (imagePath);

		//スクリーンショットを撮る
		Application.CaptureScreenshot(imageName);

		//スクリーンショットが保存されるまで待機(最大2秒)
		float latency = 0, latencyLimit = 2;
		while (latency < latencyLimit) {
			//ファイルが存在していればループ終了
			if(File.Exists(imagePath)){
				break;
			}
			latency += Time.deltaTime;
			yield return null;
		}

		//待機時間が上限に達していたら警告表示(おそらくスクショが保存出来ていない時)
		if(latency >= latencyLimit){
			Debug.LogWarning("待機時間が上限に達しました！正常にスクリーンショットが保存できていません！");
		}

		//コールバックが登録されていれば実行
		if(callback != null){
			callback();
		}

	}


}