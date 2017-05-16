using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 画面遷移機能のクラス
/// </summary>
public class ScreenCtrl : MonoBehaviour {
	Vector3 displayPosition;
	Vector3 leftStock;
	Vector3 rightStock;

	float animationTime = 0.1f;
	float SCREEN_WIDTH = 1280;

	public PopUpCtrl _popUp;

	public void Init(Vector3 pStartScreen) {
		displayPosition = pStartScreen;

		leftStock = displayPosition;
		leftStock.x -= SCREEN_WIDTH;

		rightStock = displayPosition;
		rightStock.x += SCREEN_WIDTH;
	}

	public void Transition(GameObject pFrom, GameObject pTo) {
		// 
		iTween.MoveTo (pFrom, iTween.Hash ("time", animationTime, "position", leftStock));

		//
		pTo.transform.position = rightStock;
		iTween.MoveTo (pTo, iTween.Hash("time", animationTime, "position", displayPosition)); 
	}

	public void TransitionBackwards(GameObject pFrom, GameObject pTo) {
		// 
		iTween.MoveTo (pFrom, iTween.Hash ("time", animationTime, "position", rightStock));

		// 
		pTo.transform.position = leftStock;
		iTween.MoveTo (pTo, iTween.Hash("time", animationTime, "position", displayPosition)); 		
	}
		
	public void OpenPopUp(string pTitle, string pContent = "") {
		_popUp.Open (pTitle, pContent);
	}

}
