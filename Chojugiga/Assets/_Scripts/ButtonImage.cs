using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonImage : MonoBehaviour {
	public int _defaultIndex = 0; // 初期値
	public List<Sprite> imageList; 
	Image _thisImage;
	int imageNowIndex = 0;

	void Start() {
		_thisImage = this.gameObject.GetComponent<Image> ();
		setImage (_defaultIndex);
	}

	public void setImage(int pIndex) {
		if (pIndex >= imageList.Count) {// imageIndexの要素数より多いものを指定
			DebugLogger.Log ("NO IMAGE SET");
			return;
		}

		_thisImage.sprite = imageList [pIndex];
		imageNowIndex = pIndex;
	}

	public int getImageIndex () {
		return imageNowIndex;
	}
}
