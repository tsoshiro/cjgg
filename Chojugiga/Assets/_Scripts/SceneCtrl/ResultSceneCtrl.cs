using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultSceneCtrl : SceneCtrlBase {
	public Text _labelScore;
	public Text _labelBest;
	public Text _labelCoin;
	public Text _comment;
	public Image _image;

	public void setResult(Result pResult) {
		_labelScore.text = ""+pResult.score;
		_labelBest.text = ""+pResult.bestScore;
		_labelCoin.text = ""+pResult.coin;
		_comment.text = pResult.comment;
	}

	// StandBy画面に遷移
	void actionButtonReplay() {
		_gameManager.Replay ();
	}

	void actionButtonToHome() {
		_gameManager.Title ();
	}

	void actionButtonShare() {
//		_gameManager.
	}
}
