using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Const {
	public enum AnimalType {
		FROG,
		RABBIT,
		OTHERS
	}
	public enum Position {
		LEFT,
		RIGHT
	}
	public enum Color {
		NONE,
		RED,
		BLUE,
		GREEN
	}

	public static int GACHA_COST = 1000;

	#region STRING NAME
	public static string FROG = "カエル";
	public static string RABBIT = "ウサギ";
	public static string OTHERS = "それ以外";

	public static string LEFT = "ひだり";
	public static string RIGHT = "みぎ";

	public static string ANS_RIGHT = "◯";
	public static string ANS_WRONG = "×";

	public static string LBL_SCORE = "スコア";
	public static string LBL_BEST = "ベスト";
	public static string LBL_TIME = "タイム";
	public static string LBL_COIN = "コイン";

	public static string LBL_START = "スタート";
	public static string LBL_CORRECT 	= "正解！";
	public static string LBL_WRONG 		= "ハズレ！";
	#endregion

	#region PLAYER_PREFS
	public static string PREF_FIRST_LAUNCH_TIME	= "first_launch_time";
	public static string PREF_LAST_LAUNCH_TIME		= "last_launch_time";
	public static string PREF_LAUNCH_COUNT		 	= "launch_count";
	public static string PREF_WATCH_MOVIE_COUNT	= "watch_movie_count";
	public static string PREF_OFFER_MOVIE_COUNT 	= "offer_movie_count";
	public static string PREF_SHOW_INTERSTITIAL_COUNT = "show_interstitial_count";
	public static string PREF_BEST_SCORE		 	= "best_score";
	public static string PREF_COIN				 	= "coin";
	public static string PREF_PLAY_COUNT 			= "play_count";
	public static string PREF_UNLOCKED_CARD 		= "unlocked_card";
	public static string PREF_SCORE 				= "score";
	public static string PREF_DENIED_FLG			= "denied_flg";
	public static string PREF_REVIEW_FLG			= "review_flg";
	public static string PREF_MESSAGE_FLG			= "message_flg";
	#endregion

	#region SOUND
	public static float BGM_VOLUME = 1.0f;
	#endregion

	#region COLOR
	public static string COL_POSITIVE = "6B9E8B"; // 緑
	public static string COL_NEGATIVE = "A62121"; // 赤
	public static string COL_DEFAULT = "9F9F9FFF"; // 灰色
	public static string COL_BG = "F7F1CD"; // ベージュ
	public static string COL_BROWN = "806835"; // 茶色
	public static string COL_DARK = "130D0D"; // 黒
	#endregion
}
