using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCtrlBase : MonoBehaviour {
	public GameManager _gameManager;

	// Use this for initialization
	void Start () {
		_gameManager = GameManager.GetInstance ();
	}

	public void actionBtn(GameObject pGameObject) {
		if (_gameManager._inputManager.disabled) // input無効になっているかどうかチェック
			return;

		if (_gameManager._inputManager.isLastUpTap ()) {
			this.gameObject.SendMessage ("action" + pGameObject.name);
		}
	}
}