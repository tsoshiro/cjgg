using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingletonMonoBehaviour<GameManager> {
	public GameObject _imagePanel;
	public QuestionCtrl _questionCtrl;
	InputManager _inputManager;
	TimeManager _timeManager;

	// Label Settings
	public Text _labelAnswer;
	public Text _labelScore;
	public Text _labelTime;

	// Data管理
	DataCtrl _dataCtrl;

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
		HELP
	}

	GameState state;	// 現在のステート

	void Awake() {
		InitData ();
	}

	void InitData() {
		_dataCtrl = new DataCtrl ();
		_dataCtrl.InitData ();
		_dataCtrl.checkContent ();
	}

	// Use this for initialization
	void Start () {
		_inputManager = this.gameObject.GetComponent<InputManager> ();
		_timeManager = this.gameObject.GetComponent<TimeManager> ();

		setGameReady ();
	}
		
	// Update is called once per frame
	void Update () {
		if (state == GameState.STAND_BY) {
			UpdateStandBy ();
		} else if (state == GameState.PLAYING) {
			UpdatePlaying ();
		} else if (state == GameState.RESULT) {
			UpdateResult ();
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
	}

	bool isCountingDown = false; // ゲーム開始時カウントダウンがスタートしているかどうか

	/// <summary>
	/// ゲームスタート。ゲーム開始時カウントダウンコルーチンを開始。
	/// </summary>
	void startGame() {
		if (isCountingDown)
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
	void actionButtonLeft() {
		answerLeft ();
	}

	void actionButtonRight() {
		if (state == GameState.PLAYING) 
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

	#endregion

	public GameObject _pauseUI;


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
}