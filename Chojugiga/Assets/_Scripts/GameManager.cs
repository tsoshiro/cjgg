using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingletonMonoBehaviour<GameManager> {
	public GameObject _imagePanel;
	public QuestionCtrl _questionCtrl;
	public InputManager _inputManager;
	TimeManager _timeManager;
	GachaCtrl _gachaCtrl;
	ScreenCtrl _screenCtrl;
	CommentCtrl _commentCtrl;
	ScreenShotCtrl _screenShotCtrl;

	// Label Settings
	public Text _labelAnswer;
	public Text _labelScore;
	public Text _labelTime;
	public Text _labelCoin;
	public Text _labelBestScore;

	public GameObject _objWrongReason;
	public Text _labelWrongReason;

	// Data管理
	DataCtrl _dataCtrl;
	public UserData _userData;

	// スコア管理
	int score = 0;
	int ADD_SCORE = 1;
	bool isBest = false; // ベストスコアが出た時だけTRUE

	// 時間管理
	float time = 0f;
	float TIME_DEFAULT = 5f;
	float MAX_TIME = 10f;

	float time_passed = 0f;

	// ゲーム終了時のリザルト画面に遷移するまでの待ち時間
	float WAIT_TIME_WRONG_ANSWER = 1.5f;
	float WAIT_TIME_TIME_UP = 0.6f;

	// 画像表示位置
	float X = 153f;

	// ゲーム開始時のカウントダウン
	int START_COUNT_DOWN = 3;

	enum GameState {
		TITLE,
		STAND_BY,
		PLAYING,
		PAUSE,
		RESULT,
		HELP,
		GACHA
	}

	GameState state;	// 現在のステート

	GameObject _UI_now; // 現在のUI

	public GameObject _UI_group_title;
	public GameObject _UI_group_standby;
	public GameObject _UI_group_settings;
	public GameObject _UI_group_game;
	public GameObject _UI_group_gacha;
	public GameObject _UI_group_playing; // Playing中だけ出すもの
	public GameObject _UI_group_result;

	public GameObject _pauseUI;
	public GameObject _resultUI;

	#region SceneCtrl
	ResultSceneCtrl _resultSceneCtrl;
	#endregion

	void Awake() {
		InitData ();
		InitUserData ();
	}

	void InitData() {
		_dataCtrl = new DataCtrl ();
		_dataCtrl.InitData ();
		_dataCtrl.checkContent ();
	}

	void InitUserData() {
		_userData = new UserData ();
		_userData.initUserData ();
	}

	// Use this for initialization
	void Start () {
		_inputManager = this.gameObject.GetComponent<InputManager> ();
		_timeManager = this.gameObject.GetComponent<TimeManager> ();

		_gachaCtrl = _UI_group_gacha.GetComponent<GachaCtrl> ();
		_screenShotCtrl = this.gameObject.GetComponent<ScreenShotCtrl> ();

		// UI Ctrl初期化
		_screenCtrl = GameObject.Find ("UI").GetComponent<ScreenCtrl> ();
		_screenCtrl.Init (_UI_group_title.transform.position);

		// SceneCtrl初期化
		_resultSceneCtrl = _UI_group_result.GetComponent<ResultSceneCtrl>();

		_objWrongReason = _labelWrongReason.transform.parent.gameObject;

		state = GameState.TITLE;
		_UI_now = _UI_group_title;
	}
		
	// Update is called once per frame
	void Update () {
		if (state == GameState.STAND_BY) {
			UpdateStandBy ();
		} else if (state == GameState.PLAYING) {
			UpdatePlaying ();
		} else if (state == GameState.RESULT) {
			UpdateResult ();
		} else if (state == GameState.GACHA) {
			UpdateGacha ();
		}
	}

	void UpdateStandBy() {
	}

	void UpdateGacha() {
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			_gachaCtrl.playGacha ();
		}
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			backToTitle ();
		}
	}

	void backToTitle() {
		_screenCtrl.TransitionBackwards (_UI_group_game, _UI_group_title);

		state = GameState.TITLE;
	}

	/// <summary>
	/// ゲームの準備
	/// インゲームデータを初期化
	/// </summary>
	void setGameReady() {
		// 質問リストの初期化
		_questionCtrl.initQuestions ();

		// GameManager内変数の初期化
		state = GameState.STAND_BY;
		time = TIME_DEFAULT;
		time_passed = 0;
		score = 0;
		isBest = false;

		// UI
		setLabelAnswer ("", false);
		ColorEditor.setColorFromColorCode (_labelAnswer.gameObject, Const.COL_POSITIVE);
		_objWrongReason.SetActive (false);
		setLabelScore (score);
		setLabelTime (time);
		setLabelCoin (_userData.coin);
		setLabelBestScore (_userData.best_score);
	}

	bool isCountingDown = false; // ゲーム開始時カウントダウンがスタートしているかどうか

	/// <summary>
	/// ゲームスタート。ゲーム開始時カウントダウンコルーチンを開始。
	/// </summary>
	void startGame() {
		if (state != GameState.STAND_BY||
			isCountingDown)
			return;
		StartCoroutine (startCountDown ());
	}

	/// <summary>
	/// カウントダウンを開始し、完了したらゲームをスタートする。
	/// </summary>
	/// <returns>The count down.</returns>
	IEnumerator startCountDown() {
		int cd = START_COUNT_DOWN;
		isCountingDown = true;
		for (int i = cd; i > 0; i--) {
			setLabelAnswer(i.ToString("D1"), false);
			yield return new WaitForSeconds (0.5f);
		}

		state = GameState.PLAYING;
		setLabelAnswer (Const.LBL_START);
		UpdateImage ();
		isCountingDown = false;

		_imagePanel.SetActive (true);
		_UI_group_playing.SetActive (true);
	}

	void UpdatePlaying() {
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			answerLeft ();
		} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
			answerRight ();
		} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
			answerDown ();
		}

		countTime ();
	}

	void UpdatePause() {
		// Pause期間
	}

	void UpdateResult() {
		if (Input.GetMouseButtonDown (0) ||
			Input.GetKeyDown(KeyCode.Space) ) {
			if (!_inputManager.disabled) {
//				_resultUI.SetActive (false);
//				setGameReady ();
			}
		}
	}

	#region TIME
	void countTime() {
		time -= Time.deltaTime;
		time_passed += Time.deltaTime;

		if (time <= 0) {
			timeUp ();
		}
		setLabelTime (time);
	}

	void timeUp() {
		time = 0;
		_labelWrongReason.text = "TIME UP!";

		gameOver (true);
	}

	void addTime(float pTime) {
		time += pTime;
		setLabelTime (time);
	}
	#endregion

	#region GameOver
	void gameOver(bool pIsTimeUp) {
		StartCoroutine (gameOverCoroutine (pIsTimeUp));		
	}

	IEnumerator gameOverCoroutine(bool pIsTimeUp) {
		state = GameState.RESULT;

		// 理由の表示
		_objWrongReason.SetActive(true);

		// 結果のセーブ
		updateUserData (score);

		// 結果の表示(リザルト画面用)
		showResult (score);

		// 入力の拒否
		// 入力無効時間の設定
		float waitTime = (pIsTimeUp) ? WAIT_TIME_TIME_UP : WAIT_TIME_WRONG_ANSWER;
		_inputManager.setDisabled (waitTime + 0);

		yield return new WaitForSeconds(waitTime);

		// リザルト画面に遷移
		Transition (_UI_now, _UI_group_result);
	}
		
	/// <summary>
	/// リザルトのデータを画面に出力しておく(遷移ではない)
	/// </summary>
	/// <param name="pScore">P score.</param>
	void showResult(int pScore) {
		// コメント取得
		int id = _dataCtrl.getCommentId(pScore);
		string comment = _dataCtrl.getComment(id);

		// 結果データ生成
		Result result = ScriptableObject.CreateInstance<Result> ();
		result.score = pScore;
		result.bestScore = _userData.best_score;
		result.coin = _userData.coin;
		result.isBest = isBest;
		result.comment = comment;

		// リザルト画面に出力
		_resultSceneCtrl.setResult(result);

		// 画像差し替え(余裕があれば)
	}
		
	void guideToReplay() {
		_commentCtrl.setResult (_commentCtrl._result.text + "\nREPLAY?");
	}
	#endregion

	#region USER DATA UPDATE
	/// <summary>
	/// Updates the user data.
	/// </summary>
	/// <param name="pScore">P score.</param>
	void updateUserData(int pScore) {
		addCoin (pScore); // 累計
		setBestScore (pScore); // ベストスコア更新
		playCountUp (); // プレイ回数更新

		_userData.save ();
	}

	void addCoin(int pCoin) {
		_userData.coin += pCoin;
		setLabelCoin (_userData.coin);
	}

	void setBestScore(int pScore) {
		// ベストスコアか確認し、trueなら数値を更新
		if (_userData.checkIfIsNewRecord (Const.PREF_BEST_SCORE, pScore)) {
			_userData.best_score = pScore;
			isBest = true;
			setLabelBestScore (pScore);
		};
	}

	void playCountUp() {
		_userData.play_count++;
	}
	#endregion

	#region ANSWER ACTION
	void answerLeft() {
		answer (0);
	}

	void answerRight() {
		answer (1);
	}

	void answerDown() {
		answer (2);
	}

	void answer(int pInputAnswer) {
		if (state != GameState.PLAYING ||
			_inputManager.disabled) {
			return;
		}
		string answer = "";
		if (_questionCtrl.getAnswer () < 0) {
			answer = "ERROR!!";
			DebugLogger.Log (answer);
			return;
		}
		DebugLogger.Log ("getAnswer:" + _questionCtrl.getAnswer () + " myAnswer:" + pInputAnswer);

		if (_questionCtrl.getAnswer () == pInputAnswer) {
			answer = Const.LBL_CORRECT;
			addScore (ADD_SCORE);

			addTime (_dataCtrl.getAddTime(score));
			setLabelAnswer (answer);
			UpdateImage ();
		} else {
			answer = Const.LBL_WRONG;
			wrongAnswer (_questionCtrl.getAnswer());

			// 色を変更
			ColorEditor.setColorFromColorCode (_labelAnswer.gameObject, Const.COL_NEGATIVE);
			setLabelAnswer (answer, false);
		}
		DebugLogger.Log (answer);
	}

	/// <summary>
	/// 誤答した時に、正しい答えとその理由を文字列で出力する
	/// </summary>
	/// <param name="pRightAnswer">P right answer.</param>
	void wrongAnswer(int pRightAnswer) {
		// WrongReasonを取得
		WrongReason wr = ScriptableObject.CreateInstance<WrongReason>();
		wr.setReason (pRightAnswer, _questionCtrl.getQuestion());
		string str = wr.reasonText;

		// WrongReasonラベルに出力
		_labelWrongReason.text = str;

		gameOver (false);
	}
	#endregion

	#region SCORE
	void addScore(int pScore){
		score += pScore;
		setLabelScore (score);
	}

	void setLabelScore(int pScore) {
		_labelScore.text = Const.LBL_SCORE+":"+pScore;
	}

	#endregion

	#region UI
	/// <summary>
	/// LabelAnswerの文字列変更。第二引数がtrueなら指定秒後に消える
	/// </summary>
	/// <param name="pStr">P string.</param>
	/// <param name="isFading">If set to <c>true</c> is fading.</param>
	void setLabelAnswer(string pStr, bool isFading = true) {
		_labelAnswer.text = pStr;
		_labelAnswer.gameObject.SetActive (true);
		if (isFading) {
			Invoke ("disableLabelAnswer", 0.4f);
		} else {
			CancelInvoke (); // 消えないようにする
		}
	}

	void disableLabelAnswer() {
		_labelAnswer.gameObject.SetActive (false);
	}

	void setLabelTime(float pTime) {
		_labelTime.text = Const.LBL_TIME+":" + pTime.ToString ("F1");
		_timeManager.setTimeGaugeWidth (time, MAX_TIME);
	}

	void setLabelCoin(int pCoin) {
		_labelCoin.text = Const.LBL_COIN+":" + pCoin;
	}

	void setLabelBestScore(int pScore) {
		_labelBestScore.text = Const.LBL_BEST+":" + pScore;
	}
	#endregion

	#region ActionButton
	// Receivers
	public void actionBtn(GameObject pGameObject) {
		if (_inputManager.disabled) // input無効になっているかどうかチェック
			return;

		if (_inputManager.isLastUpTap ()) {
			this.gameObject.SendMessage ("action" + pGameObject.name);
		}
	}
		
	public void flick() {
		if (state == GameState.PLAYING) 
			answerDown ();
	}

	// Public Methods

	/// <summary>
	/// リプレイ(RESULT→STAND_BY)
	/// </summary>
	public void Replay() {
		if (state == GameState.RESULT) {
			TransitionBackwards (_UI_now, _UI_group_standby);
			state = GameState.STAND_BY;
			setGameReady ();
		}
	}
		
	/// <summary>
	/// タイトルに戻る(RESULT→TITLE, SETTING→TITLE, PAUSE→TITLE)
	/// </summary>
	public void Title() {
		if (state == GameState.PAUSE) { // PAUSE→TITLEの場合
			stopGame ();
			_pauseUI.SetActive (false);
		}				
		state = GameState.TITLE;
		TransitionBackwards (_UI_now, _UI_group_title);
	}

	/// <summary>
	/// 結果を共有
	/// </summary>
	public void ShareResult() {
		_screenShotCtrl.actionShare (true);
	}

	// Private Methods
	// TITLE SCENE
	void actionButtonPlay() {
		if (state == GameState.TITLE) {
			Transition (_UI_now, _UI_group_standby);
			state = GameState.STAND_BY;
			setGameReady ();
		}
	}

	void actionButtonSettings() {
		if (state == GameState.TITLE) {
			Transition (_UI_now, _UI_group_settings);
		}
	}

	/// <summary>
	/// タイトル画像を共有
	/// </summary>
	void actionButtonShare() {
		if (state == GameState.TITLE) {
			_screenShotCtrl.actionShare (true);
		}
	}

	// SETTING SCENE
	void actionButtonBack() {
		Title ();
	}

	void actionButtonReview() {
		
	}

	bool isSoundOn = true;
	void actionButtonSound() {
		isSoundOn = !isSoundOn;

		// TODO サウンドのON/OFF設定を変える
		// AudioManager


		// ボタンの見た目を変える
		GameObject btn = GameObject.Find ("ButtonSound");

		// ONならPOSITIVE
		string btnStr = Const.BTN_SOUND_ON;
		int btnImageIndex = 0; 
		if (!isSoundOn) {
			// OFFならNEGATIVE
			btnStr = Const.BTN_SOUND_OFF;
			btnImageIndex = 1; 
		}

		// 画像と文字を更新
		btn.GetComponentInChildren<Text>().text = btnStr;
		btn.GetComponent<ButtonImage> ().setImage (btnImageIndex);
	}

	void actionButtonOtherGames() {
		
	}

	// STAND_BY SCENE
	void actionButtonStart() {
		if (state == GameState.STAND_BY) {
			Transition (_UI_now, _UI_group_game);
			startGame ();
		}
	}

	// DONT USE
	void actionButtonGacha() {
		if (state == GameState.TITLE) {
			Transition (_UI_now, _UI_group_gacha);

			state = GameState.GACHA;
			_gachaCtrl.createCardList ();
		}
	}

	void actionButtonPlayGacha() {
		if (state == GameState.GACHA) {
			_gachaCtrl.playGacha ();
		}
	}
		
	// IN GAME
	void actionButtonLeft() {
		answerLeft ();
	}

	void actionButtonRight() {
		answerRight ();
	}

	void actionButtonPause() {
		if (state == GameState.PLAYING) 
			Pause ();
	}

	void actionButtonContinue() {
		if (state == GameState.PAUSE) 
			Continue ();
	}

	void actionButtonPlayFromStart() {
		// PLAY FROM RESTART
		if (state == GameState.PAUSE) {
			// ゲームを停止
			stopGame ();
			_pauseUI.SetActive (false);

			// STAND_BY画面にして準備する
			TransitionBackwards(_UI_now, _UI_group_standby);
			setGameReady();
		}
	}

	void actionButtonHome() {
		Title ();
	}
	#endregion

	void stopGame() {
		setGameReady ();
		_imagePanel.GetComponent<Image> ().sprite = null;
		_imagePanel.SetActive (false);
	}
		
	void Transition(GameObject pFrom, GameObject pTo) {
		_screenCtrl.Transition (pFrom, pTo);
		_UI_now = pTo;
	}

	void TransitionBackwards(GameObject pFrom, GameObject pTo) {
		_screenCtrl.TransitionBackwards (pFrom, pTo);
		_UI_now = pTo;
	}

	void Pause() {		
		state = GameState.PAUSE;
		_pauseUI.SetActive (true);
	}

	void Continue() {
		state = GameState.PLAYING;
		_pauseUI.SetActive (false);
	}

	void UpdateImage() {
		_imagePanel.GetComponent<Image>().sprite = _questionCtrl.getQSprite();
		setPosition ();
	}

	void setPosition() {
		Vector3 pos = new Vector3 ();

		if ((int)_questionCtrl.getQPos () == (int)Const.Position.LEFT) {
			pos.x = - X;
		} else {
			pos.x = X;
		}
		_imagePanel.transform.localPosition = pos;
	}

	void OnApplicationPause (bool pauseStatus)
	{
		if (pauseStatus) {
			DebugLogger.Log("applicationWillResignActive or onPause");	
		} else {
			if (state == GameState.PLAYING)
				Pause ();
		}
	}

	public void resetPlayerPrefs() {
		_userData.reset ();
	}
}