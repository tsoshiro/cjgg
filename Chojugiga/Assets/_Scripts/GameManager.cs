using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public GameObject _imagePanel;
	public QuestionCtrl _questionCtrl;
	InputManager _inputManager;

	// Settings
	public Text _labelAnswer;
	public Text _labelScore;
	public Text _labelTime;

	// SCORE
	int score = 0;
	int ADD_SCORE = 20;

	// TIME
	float time = 0f;
	float TIME_DEFAULT = 30f;

	float X = 2f;

	enum GameState {
		TITLE,
		STAND_BY,
		PLAYING,
		PAUSE,
		RESULT,
		HELP
	}

	GameState state;

	// Use this for initialization
	void Start () {
		_inputManager = this.gameObject.GetComponent<InputManager> ();

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
		state = GameState.STAND_BY;
		time = TIME_DEFAULT;
		score = 0;

		setLabelAnswer ("TAP TO START", false);
		setLabelScore (score);
		setLabelTime (time);
	}

	void startGame() {
		state = GameState.PLAYING;
		setLabelAnswer ("START!!");
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

	void UpdateResult() {
		if (Input.GetMouseButtonDown (0)) {
			setGameReady ();
		}
	}

	#region TIME
	void countTime() {
		time -= Time.deltaTime;
		if (time <= 0) {
			timeUp ();
		}
		setLabelTime (time);
	}

	void timeUp() {
		time = 0;
		state = GameState.RESULT;

		setLabelAnswer("RESULT : " + score, false);
		CancelInvoke ("disableLabelAnswer");

		_labelAnswer.gameObject.SetActive (true);

		_inputManager.setDisabled (1f);
	}
	#endregion

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
			Debug.Log (answer);
			return;
		}
		Debug.Log ("getAnswer:" + _questionCtrl.getAnswer () + " myAnswer:" + pAnswer);
		if (_questionCtrl.getAnswer () == pAnswer) {
			answer = "CORRECT!";
			addScore (ADD_SCORE);
		} else {
			answer = "WRONG!";
		}
		Debug.Log (answer);
		setLabelAnswer (answer);

		UpdateImage ();
	}

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
	}
	#endregion

	#region Action
	// Receivers
	public void actionBtn(GameObject pGameObject) {
		if (_inputManager.disabled)
			return;

		if (_inputManager.isLastUpTap ()) {
			this.gameObject.SendMessage ("action" + pGameObject.name);
		}
	}

	public void flick() {
		answerDown ();
	}

	// Private Methods
	void actionButtonLeft() {
		answerLeft ();
	}

	void actionButtonRight() {
		answerRight ();
	}

	#endregion

	void UpdateImage() {
		_imagePanel.GetComponent<SpriteRenderer> ().sprite = _questionCtrl.getQSprite();
		setPosition ();
//		_imagePanel.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("image_3");
	}

	void setPosition() {
		Vector3 pos = new Vector3 ();

		if ((int)_questionCtrl.getQPos () == 0) {
			pos.x = X;
		} else {
			pos.x = -X;
		}
		_imagePanel.transform.position = pos;
	}
}
