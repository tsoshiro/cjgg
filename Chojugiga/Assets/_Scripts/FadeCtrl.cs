using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeCtrl : MonoBehaviour {
	Image _image;

	void Start() {
		_image = this.GetComponent<Image> ();
	}

	public void Fade(float pFrom, float pTo, float pTime) {
		iTween.ValueTo(this.gameObject, iTween.Hash("from", pFrom, "to", pTo, "time", pTime, "onupdate", "UpdateAlpha"));
	}

	void UpdateAlpha(float pValue) {
		Color c = _image.color;
		c.a = pValue;
		_image.color = c;
	}
}