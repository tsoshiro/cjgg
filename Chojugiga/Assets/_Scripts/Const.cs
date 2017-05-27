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
	public static string LBL_COIN = "トータル";

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
	public static float BGM_VOLUME = .32f;

	public static int SE_BUTTON 		= 0; // ボタン押下
	public static int SE_PAGE_TRANS 	= 1; // 進む
	public static int SE_CORRECT_CHIME 	= 2; // 正解した時
	public static int SE_WRONG_CHIME 	= 3; // 間違えた時
	public static int SE_RESULT_FX 		= 4; // 結果画面遷移時
	public static int SE_BEST		 	= 5; // ベストスコア取得時ジングル
	public static int SE_PAGE_TRANS_BW	= 6; // 戻る
	public static int SE_START			= 7; // スタート時



	#endregion

	#region COLOR
	public static string COL_POSITIVE 	= "6B9E8B"; // 緑
	public static string COL_NEGATIVE 	= "A62121"; // 赤
	public static string COL_DEFAULT 	= "9F9F9FFF"; // 灰色
	public static string COL_BG 		= "F7F1CD"; // ベージュ
	public static string COL_BROWN 		= "806835"; // 茶色
	public static string COL_DARK 		= "130D0D"; // 黒
	#endregion

	#region SHARE
	public static string SC_NAME		= "sc_shot.png";
	public static string SHARE_MESSAGE 	= "鳥獣戯画で脳トレができるゆるーいアプリ";

	public static string SHARE_MESSAGE_SCORE_01 = "脳トレ鳥獣戯画で";
	public static string SHARE_MESSAGE_SCORE_02 = "点取ったよ";

	public const string APP_ID = "1241697757";
	public const string APP_STORE_URL = 
		"https://itunes.apple.com/us/app/id"
		+APP_ID
		+"?l=ja&ls=1&mt=8";
	public const string APP_STORE_URL_BITLY = "http://apple.co/2qZGB76";

	public const string APP_STORE_REVIEW_URL =
		"itms-apps://itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?id="
		+APP_ID
		+"&onlyLatestVersion=true&pageNumber=0&sortOrdering=1&type=Purple+Software";

	public static string DEV_URL 		= "http://bit.ly/2rsY7yY"; // FIXED
	public static string SUPPORT_URL	= "http://bit.ly/2i4YJtp";
	public static string FEATURE_URL	= "http://bit.ly/2qqwyFp"; // ダ鳥獣戯画サイト
	public static string PIXELBEAT_URL	= "http://bit.ly/2qnjkNB"; // pixelbeat

	public static string MSG_ASKING_POPUP = "プレイしていただき、ありがとうございます。\n楽しんでいただけていますか？";
	public static string MSG_ASKING_POPUP_ANS_YES 	= "はい";
	public static string MSG_ASKING_POPUP_ANS_NO	= "そんなに…";

	public static string MSG_ASKING_REVIEW_TITLE = "ありがとうございます！";
	public static string MSG_ASKING_REVIEW_CONTENT = "よかったら\n☆5レビュー、\nお願いします。";
	public static string MSG_ASKING_REVIEW_ANS_OK = "レビューする";
	public static string MSG_ASKING_REVIEW_ANS_NO = "また今度";
	public static string MSG_ASKING_REVIEW_ANS_NEVER = "もう表示しない";

	public static string MSG_ASKING_MESSAGE = "よろしければ\nご意見・ご要望、\nお待ちしています。";
	public static string MSG_ASKING_MESSAGE_ANS_OK = "ひとこと言う";
	public static string MSG_ASKING_MESSAGE_ANS_NO = "また今度";
	public static string MSG_ASKING_MESSAGE_ANS_NEVER = "もう表示しない";

	#endregion

	#region BUTTON
	public static string BTN_SOUND_ON = "サウンドあり";
	public static string BTN_SOUND_OFF = "サウンドなし";
	public enum ButtonType {
		POSITIVE, NEGATIVE, DEFAULT
	}
	#endregion

	#region AD
	public static int AD_INTER_NUM_END = 0;
	public static int AD_INTER_NUM_BG 	= 1;

	public static int AD_INTERVAL_INTER = 5;
	#endregion

	#region REVIEW
	public static int INTERVAL_REVIEW_REQUEST = 10;
	public static int INTERVAL_REVIEW_REQUEST_CANCELED_ONCE = 100;
	#endregion

	#region ANALYTICS
	public static string UA_SCORE 				= "score";
	public static string UA_ONE_PLAY_TIME 		= "onePlayTime";

	public static string UA_PRESSED_SHARE_BTN_TITLE 	= "pressedShareButtonTitle";
	public static string UA_PRESSED_SHARE_BTN_RESULT 	= "pressedShareButtonResult";

	public static string UA_ASKING_POP_UP 		= "askingPopUp";
	public static string UA_PRESSED_ENJOYING	= "pressedEnjoying";
	public static string UA_PRESSED_NOT_ENJOYING	= "pressedNotEnjoying";
	public static string UA_PRESSED_REVIEW = "pressedReview";
	public static string UA_PRESSED_NOT_REVIEW_NOW = "pressedNotReviewNow";
	public static string UA_PRESSED_NOT_REVIEW_EVER = "pressedNotReviewEver";

	public static string UA_PRESSED_MESSAGE = "pressedMessage";
	public static string UA_PRESSED_NOT_MESSAGE_NOW = "pressedNotMessageNow";
	public static string UA_PRESSED_NOT_MESSAGE_EVER = "pressedNotMessageEver";
	#endregion


}
