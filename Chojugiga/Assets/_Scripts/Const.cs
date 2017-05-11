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
}
