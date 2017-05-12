using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenShotTest : MonoBehaviour {
	public TextMeshProUGUI _label;
	public GameObject _wall;

	int counter = 0;
	string fileName = "sc.png";

	public void capture() {
		counter++;
		string str = "Capture Count\n" + counter;
		_label.text = str;

		// Capture
		StartCoroutine (ScreenshotCaptor.Capture (
			fileName,
			callbackCapture)
		);

		_wall.SetActive (true);
	}
		
	void callbackCapture() {
		// capture success!
		_wall.SetActive(false);
		actionButtonShare ();
	}

	void actionButtonCapture() {
		capture ();
	}

	void actionButtonShare() {
		SocialConnector.SocialConnector.Share ("シェアテスト",
			"http://www.google.co.jp", 
			Application.persistentDataPath + "/" + fileName);
	}

}
