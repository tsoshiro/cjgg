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

	// Label Settings
	public Text _labelAnswer;
	public Text _labelScore;
	public Text _labelTime;
	public Text _labelCoin;
	public Text _labelBestScore;

	// Data管理
	DataCtrl _dataCtrl;
	public UserData _userData;

	// スコア管理
	int score = 0;
	int ADD_SCORE = 1;

	// 時間管理
	float time = 0f;
	float TIME_DEFAULT = 5f;
	float MAX_TIME = 10f;

	float time_passed = 0f;

	// 画像表示位置
	float X = 2f;

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
	public GameObject _UI_group_game;
	public GameObject _UI_group_gacha;
	public GameObject _UI_group_playing; // Playing中だけ出すもの
	public GameObject _pauseUI;

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

		// UI Ctrl初期化
		_screenCtrl = GameObject.Find ("UI").GetComponent<ScreenCtrl> ();
		_screenCtrl.Init (_UI_group_title.transform.position);

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
		if (Input.GetMouseButtonDown (0)) {
			startGame ();
		}
		if (Input.GetKeyDown(KeyCode.Space)) {
			startGame ();
		}
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
//		_UI_group_title.SetActive (true);
//		_UI_group_game.SetActive (false);
//		_UI_group_gacha.SetActive (false);

		state = GameState.TITLE;
	}

	void setGameReady() {
		// 質問リストの初期化
		_questionCtrl.initQuestions ();

		// GameManager内変数の初期化
		state = GameState.STAND_BY;
		time = TIME_DEFAULT;
		time_passed = 0;
		score = 0;

		// UI
		setLabelAnswer ("TAP TO START", false);
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
		setLabelAnswer ("START!!");
		UpdateImage ();
		isCountingDown = false;

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
			if (!_inputManager.disabled)
				setGameReady ();
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

		gameOver ();
	}

	void gameOver() {
		state = GameState.RESULT;

		setLabelAnswer("RESULT : " + score, false);
		CancelInvoke ("disableLabelAnswer"); // Invokeのキャンセル

		_labelAnswer.gameObject.SetActive (true);

		updateUserData (score);
	
		_inputManager.setDisabled (2f);
		Invoke ("guideToReplay", 2f);
	}
		
	void guideToReplay() {
		setLabelAnswer ("RESULT : " + score + "\nREPLAY?", false);
	}

	void addTime(float pTime) {
		time += pTime;
		setLabelTime (time);
	}
	#endregion

	#region USER DATA UPDATE
	/// <summary>
	/// Updates the user data.
	/// </summary>
	/// <param name="pScore">P score.</param>
	void updateUserData(int pScore) {
		addCoin (pScore);
		setHighScore (pScore);
		playCountUp ();

		_userData.save ();
	}

	void addCoin(int pCoin) {
		_userData.coin += pCoin;
		setLabelCoin (_userData.coin);
	}

	void setHighScore(int pScore) {
		if (_userData.checkIfIsNewRecord (Const.PREF_BEST_SCORE, pScore)) {
			_userData.best_score = pScore;
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

	void answer(int pAnswer) {
		if (state != GameState.PLAYING) {
			return;
		}
		string answer = "";
		if (_questionCtrl.getAnswer () < 0) {
			answer = "ERROR!!";
			DebugLogger.Log (answer);
			return;
		}
		DebugLogger.Log ("getAnswer:" + _questionCtrl.getAnswer () + " myAnswer:" + pAnswer);

		if (_questionCtrl.getAnswer () == pAnswer) {
			answer = "CORRECT!";
			addScore (ADD_SCORE);

			addTime (_dataCtrl.getAddTime(score));
		} else {
			answer = "WRONG!";

			gameOver ();
		}
		DebugLogger.Log (answer);
		setLabelAnswer (answer);

		UpdateImage ();
	}
	#endregion

	#region SCORE
	void addScore(int pScore){
		score += pScore;
		setLabelScore (score);
	}

	void setLabelScore(int pScore) {
		_labelScore.text = "SCORE : "+pScore;
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
		}
	}

	void disableLabelAnswer() {
		_labelAnswer.gameObject.SetActive (false);
	}

	void setLabelTime(float pTime) {
		_labelTime.text = "TIME: " + pTime.ToString ("F1");
		_timeManager.setTimeGaugeWidth (time, MAX_TIME);
	}

	void setLabelCoin(int pCoin) {
		_labelCoin.text = "COIN: " + pCoin;
	}

	void setLabelBestScore(int pScore) {
		_labelBestScore.text = "BEST: " + pScore;
	}
	#endregion

	#region Transition
	void SwitchScreen(GameState pFrom, GameState pTo) {
		
	}

	#endregion

	#region Action
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

	// Private Methods
	void actionButtonPlay() {
		if (state == GameState.TITLE) {

			_screenCtrl.Transition (_UI_now, _UI_group_game);
			_UI_now = _UI_group_game;

//			_UI_group_title.SetActive (false);
//			_UI_group_game.SetActive (true);

			state = GameState.STAND_BY;
			setGameReady ();
		}
	}

	void actionButtonGacha() {
		if (state == GameState.TITLE) {
			_screenCtrl.Transition (_UI_now, _UI_group_gacha);
			_UI_now = _UI_group_gacha;

//			_UI_group_title.SetActive (false);
//			_UI_group_game.SetActive (false);
//			_UI_group_gacha.SetActive (true);

			state = GameState.GACHA;
			_gachaCtrl.createCardList ();
		}
	}

	void actionButtonSettings() {
		if (state == GameState.TITLE) {
			_screenCtrl.OpenPopUp ("SETTINGS", "WE ARE GOING TO SET SOME CONFIGURATION!!");
		}
	}

	void actionButtonPlayGacha() {
		if (state == GameState.GACHA) {
			_gachaCtrl.playGacha ();
		}
	}
		
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
		if (state == GameState.PAUSE)
			Continue ();
	}

	void actionButtonHome() {
		if (state != GameState.TITLE) {
			if (state == GameState.PAUSE) {
				stopGame ();
				_pauseUI.SetActive (false);
			}				
			
			state = GameState.TITLE;

			_screenCtrl.TransitionBackwards (_UI_now, _UI_group_title);
			_UI_now = _UI_group_title;
		}
	}

	void stopGame() {
		setGameReady ();
		_imagePanel.GetComponent<SpriteRenderer> ().sprite = null;
	}


	#endregion

	void Pause() {		
		state = GameState.PAUSE;
		_pauseUI.SetActive (true);
	}

	void Continue() {
		state = GameState.PLAYING;
		_pauseUI.SetActive (false);
	}

	void UpdateImage() {
		_imagePanel.GetComponent<SpriteRenderer> ().sprite = _questionCtrl.getQSprite();
		setPosition ();
//		_imagePanel.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("image_3");
	}

	void setPosition() {
		Vector3 pos = new Vector3 ();

		if ((int)_questionCtrl.getQPos () == (int)Const.Position.LEFT) {
			pos.x = - X;
		} else {
			pos.x = X;
		}
		_imagePanel.transform.position = pos;
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