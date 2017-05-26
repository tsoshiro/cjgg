using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;
using System.Collections.Generic;

using GoogleMobileAds.Api;

public class AdvertisementManager : MonoBehaviour {
	GameObject callBackObj;
	public string TEST_DEVICE_ID = "6befb76ee0c14f275cd09f2702c88528";

	public void ShowRewardedAd(GameObject pObj = null)
	{
		if (Advertisement.isInitialized &&
			Advertisement.IsReady ()) {
			if (pObj != null) {
				callBackObj = pObj;
			}
			Advertisement.Show (null, new ShowOptions {
				////trueだとUnityが止まり、音もミュートになる
				//pause = true, 
				//広告が表示された後のコールバック設定
				resultCallback = HandleShowResult
			});
		} else {
			Debug.Log ("NOT INITIALIZED!");
		}
	}

	private void HandleShowResult(ShowResult result)
	{
		switch (result)
		{
		case ShowResult.Finished:
			Debug.Log ("The ad was successfully shown.");
			// YOUR CODE TO REWARD THE GAMER
			// Give coins etc.
			if (callBackObj != null) {
				callBackObj.SendMessage ("movieCallBack", "0");
			}
			break;
		case ShowResult.Skipped:
			Debug.Log("The ad was skipped before reaching the end.");
			if (callBackObj != null) {
				callBackObj.SendMessage ("movieCallBack", "1");
			}
			break;
		case ShowResult.Failed:
			Debug.LogError("The ad failed to be shown.");
			if (callBackObj != null) {
				callBackObj.SendMessage ("movieCallBack", "2");
			}
			break;
		}
	}


	// UnityAds
	[Header("-----MovieReward-----")]
	public string Android_gameId;
	public string ios_gameId;
	public bool isUnityAdsTestMode = false; // 本番=false テスト=true
	[SerializeField]
	string gameId;

	// AdMob
	[Header("-----AdMob-----")]
	[Header("Interstitial")]
	public List<string> Android_interstitial;
	public List<string> ios_interstitial;

	[SerializeField]
	List<string> intersAdUnitId;

	[Header("Banner")]
	public string Android_banner;
	public string ios_banner;

	[SerializeField]
	string bannerAdUnitId;

	public bool isAdMobTestMode = false; // 本番=false テスト=true


	private List<InterstitialAd> _interstitial;
	private List<AdRequest> request;

	// インタースティシャルがクローズしているか
	List<bool> is_close_interstitial;

	void Awake() {
		// IDを初期化
		setAdUnitId();

		// 起動時にロード
		initInterstitial();

		// 動画リワードあるなら
//		setMovieReward ();
	}

	void setAdUnitId() {
		intersAdUnitId = new List<string> ();

		#if UNITY_ANDROID
		intersAdUnitId = Android_interstitial;
		bannerAdUnitId = Android_banner;
		#elif UNITY_IOS
		intersAdUnitId = ios_interstitial;
		bannerAdUnitId = ios_banner;
		#else
		intersAdUnitId = {"unexpected_platform"};
		bannerAdUnitId = "unexpected_platform";
		#endif
	}

	public void setMovieReward () {
	#if UNITY_ADROID
		gameId = Android_gameId;
	#elif UNITY_IPHONE
		gameId = ios_gameId;
	#else
		gameId = ios_gameId;
	#endif
		if (Advertisement.isSupported) { // If the platform is supported,
			Advertisement.Initialize (gameId, isUnityAdsTestMode); // initialize Unity Ads.
		}
	}

	/// <summary>
	/// インタースティシャル広告をリクエストする(更新)
	/// </summary>
	/// <param name="pIndex">P index.</param>
	void RequestInterstitial(int pIndex) {
		Debug.Log ("RequestInterstitial adUnitId:"+intersAdUnitId[pIndex]);

		if (is_close_interstitial[pIndex]) {
			Debug.Log ("Destroy ad");
			_interstitial[pIndex].Destroy ();
		}
	
		updateInterstitialAd(pIndex);
	}

	void initInterstitial() {
		// Interstitialリスト初期化
		_interstitial = new List<InterstitialAd> ();
		request = new List<AdRequest> ();
		is_close_interstitial = new List<bool> ();
		for (int i = 0; i < intersAdUnitId.Count; i++) {
			addInterstitialAd (i);
		}
	}

	/// <summary>
	/// InterstitialAdのインスタンスを作成し、リストに追加する
	/// </summary>
	/// <param name="pIndex">P index.</param>
	void addInterstitialAd(int pIndex) {
		InterstitialAd ia = new InterstitialAd (intersAdUnitId [pIndex]);
		AdRequest ar = createRequest (isAdMobTestMode);

		// load inters
		ia.LoadAd( ar );
		ia.OnAdClosed += HandleInterstitialClosed;

		// リストに追加
		_interstitial.Add( ia );
		request.Add ( ar );
		is_close_interstitial.Add ( false );
	}

	// 初期化以降・更新する
	void updateInterstitialAd(int pIndex) {
		InterstitialAd ia = new InterstitialAd (intersAdUnitId [pIndex]);
		AdRequest ar = createRequest (isAdMobTestMode);

		// load inters
		ia.LoadAd( ar );
		ia.OnAdClosed += HandleInterstitialClosed;

		// リストを更新
		_interstitial[pIndex] = ia;
		request [pIndex] = ar;
		is_close_interstitial [pIndex] = false;
	
	}
		
	AdRequest createRequest (bool isTest) {
		if (isTest) {
			return new AdRequest.Builder ().
							   AddTestDevice(TEST_DEVICE_ID).
							   Build ();
		}
		return new AdRequest.Builder ().
						  Build ();
	}


	int indexNow = 0; // コールバック処理用にインデックス変数を保持する
	public void showInterstitial(int pIndex = 0) {
		indexNow = pIndex;
		_interstitial [pIndex].Show ();
	}
		
	public void ShowBanner() {
		// Create a 320x50 banner at the top of the screen.
		BannerView bannerView = new BannerView(bannerAdUnitId, AdSize.Banner, AdPosition.Bottom);

		// Create an empty ad request.
		AdRequest request = createRequest(isAdMobTestMode);

		// Load the banner with the request.
		bannerView.LoadAd(request);
	}
		
	public void HandleInterstitialClosed(object sender, System.EventArgs args) {
		Debug.Log ("HandleInterstitialClosed");
		is_close_interstitial[indexNow] = true;
		RequestInterstitial (indexNow);
	}

}