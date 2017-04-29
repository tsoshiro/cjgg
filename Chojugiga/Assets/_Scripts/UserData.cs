using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class UserData {

	#region PRIVATE PlayerPrefs
	int DEFAULT_VALUE_INT = 0;
	float DEFAULT_VALUE_FLOAT = 0.0f;
	string DEFAULT_VALUE_STRING = "0";

	private int getUserDataInt (string pKey) {
		return PlayerPrefs.GetInt (pKey, DEFAULT_VALUE_INT);
	}

	private float getUserDataFloat (string pKey)
	{
		return PlayerPrefs.GetFloat (pKey, DEFAULT_VALUE_FLOAT);
	}

	private string getUserDataString (string pKey) {
		return PlayerPrefs.GetString (pKey, DEFAULT_VALUE_STRING);
	}

	private void setUserData (string pKey, int pValue) {
		PlayerPrefs.SetInt (pKey, pValue);
	}

	private void setUserData (string pKey, float pValue)
	{
		PlayerPrefs.SetFloat (pKey, pValue);
	}

	private void setUserData (string pKey, string pValue)
	{
		PlayerPrefs.SetString (pKey, pValue);
	}

	private void saveUserData () {
		PlayerPrefs.Save ();
	}

	private void resetUserData () {
		PlayerPrefs.DeleteAll ();
	}
	#endregion

	#region public interface
	public void save() {
		saveAllData ();
		saveUserData ();
	}

	void saveAllData() {
		setUserData (Const.PREF_FIRST_LAUNCH_TIME, first_launch_time.ToString());
		setUserData (Const.PREF_LAST_LAUNCH_TIME, last_launch_time.ToString ());
		setUserData (Const.PREF_WATCH_MOVIE_COUNT, watch_movie_count);
		setUserData (Const.PREF_OFFER_MOVIE_COUNT, offer_movie_count);
		setUserData (Const.PREF_SHOW_INTERSTITIAL_COUNT, show_interstitial_count);

		setUserData (Const.PREF_BEST_SCORE, best_score);
		setUserData (Const.PREF_COIN, coin);

		setUserData (Const.PREF_PLAY_COUNT, play_count);
		setUserData (Const.PREF_UNLOCKED_CARD, unlocked_card);
		setUserData (Const.PREF_SCORE, score);

		setUserData (Const.PREF_DENIED_FLG, denied_flg);
		setUserData (Const.PREF_REVIEW_FLG, review_flg);
		setUserData (Const.PREF_MESSAGE_FLG, message_flg);
	}

	public void reset() {
		resetUserData ();
	}
	#endregion

	#region RECORD
	public bool checkIfIsNewRecord (string pKey, int pValue) {
		if ( pValue > getUserDataInt(pKey)) {
			setUserData (pKey, pValue);
			return true;
		}
		return false;
	}

	public void addTotalRecords (string pKey, int pValue) {
		int totalRecord = getUserDataInt (pKey);
		totalRecord += pValue;
		setUserData (pKey, totalRecord);
	}
	#endregion

	#region IN GAME USE

	// BASIC_DATA
	public System.DateTime 	first_launch_time;
	public System.DateTime 	last_launch_time;
	public int		launch_count;
	public int 		watch_movie_count;
	public int 		offer_movie_count;
	public int 		show_interstitial_count;

	// GAME_DATA
	public int 		best_score;
	public int 		coin;
	public int 		play_count;
	public string 		unlocked_card;
	public int 		score;

	// FLAGS
	public int 		denied_flg; // 断り 0:未回答, 1: 断り済み
	public int 		review_flg;  // レビュー 0:未レビュー, 1:レビュー済み
	public int 		message_flg; // メッセージ 0:未送信,1 :送信済み

	// 初期データ作成
	public void initUserData() {
		checkNewUser();

		first_launch_time = System.DateTime.Parse( getUserDataString (Const.PREF_FIRST_LAUNCH_TIME) );
		last_launch_time = System.DateTime.Now;
		launch_count = getUserDataInt (Const.PREF_LAUNCH_COUNT) + 1; // 起動回数が増える
		watch_movie_count = getUserDataInt (Const.PREF_WATCH_MOVIE_COUNT);
		offer_movie_count = getUserDataInt (Const.PREF_OFFER_MOVIE_COUNT);
		show_interstitial_count = getUserDataInt (Const.PREF_SHOW_INTERSTITIAL_COUNT);

		best_score = getUserDataInt (Const.PREF_BEST_SCORE);
		coin = getUserDataInt (Const.PREF_COIN);
		play_count = getUserDataInt (Const.PREF_PLAY_COUNT);
		unlocked_card = getUserDataString (Const.PREF_UNLOCKED_CARD);
		score = getUserDataInt (Const.PREF_SCORE);

		denied_flg = getUserDataInt (Const.PREF_DENIED_FLG);
		review_flg = getUserDataInt(Const.PREF_REVIEW_FLG);
		message_flg = getUserDataInt (Const.PREF_MESSAGE_FLG);
	}

	public void debugDataSetUp () {
	}

	void checkNewUser () {
		if (getUserDataInt (Const.PREF_LAUNCH_COUNT) <= 0) {
			DebugLogger.Log ("NEW USER!");
			createBaseUserData ();
		} else {
			DebugLogger.Log ("OLD USER!");
		}
	}

	/// <summary>
	/// 新規ユーザーデータ作成
	/// </summary>
	void createBaseUserData () {
		// Basic Data
		setUserData (Const.PREF_FIRST_LAUNCH_TIME, DateTime.Now.ToString());
		setUserData (Const.PREF_LAST_LAUNCH_TIME, DateTime.Now.ToString());
		setUserData (Const.PREF_LAUNCH_COUNT, 1);

		// Game Data
		setUserData (Const.PREF_COIN, 0);
	 	setUserData (Const.PREF_PLAY_COUNT, 0);
	 	setUserData (Const.PREF_BEST_SCORE, 0);
	 	setUserData (Const.PREF_UNLOCKED_CARD, "");
		setUserData (Const.PREF_SCORE, 0);

		// Flags
	 	setUserData (Const.PREF_DENIED_FLG, 0);
		setUserData (Const.PREF_REVIEW_FLG, 0);
		setUserData (Const.PREF_MESSAGE_FLG, 0);
	}

	public UserData () {
	}
	#endregion

	#region for debug
	/// <summary>
	/// 作成中
	/// </summary>
	/// <returns>The value if exists.</returns>
	/// <param name="value">Value.</param>
	/// <param name="targetField">Target field.</param>
	/// <typeparam name="Type">The 1st type parameter.</typeparam>
	Type setValueIfExists<Type> (Type value, Type targetField)
		where Type : IComparable
	{
		Type defVal = default (Type);
		if (value.CompareTo(defVal) == 0) { // 未設定
			return targetField;
		}
		return value;
	}

	void setCustomData (UserData pUserData) {
		first_launch_time = pUserData.first_launch_time;
		last_launch_time = pUserData.last_launch_time;
		watch_movie_count = pUserData.watch_movie_count;
		offer_movie_count = pUserData.offer_movie_count;
		show_interstitial_count = pUserData.show_interstitial_count;

		best_score = pUserData.best_score;
		coin = pUserData.coin;
		play_count = pUserData.play_count;
		unlocked_card = pUserData.unlocked_card;
		score = pUserData.score;

		denied_flg = pUserData.denied_flg;
		review_flg = pUserData.review_flg;
		message_flg = pUserData.message_flg;
	}
	#endregion
}