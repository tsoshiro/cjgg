using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {
	public GameObject _timeGauge;
	float timeGaugeBaseWidth = 0f;


	// Use this for initialization
	void Start () {
		timeGaugeBaseWidth = _timeGauge.transform.localScale.x;
	}

	public void setTimeGaugeWidth(float pTime, float pMaxTime) {	
		float gaugeX = timeGaugeBaseWidth * (pTime / pMaxTime);
		Vector3 gaugeScale = new Vector3 (gaugeX,
			_timeGauge.transform.localScale.y,
			_timeGauge.transform.localScale.z);
			_timeGauge.transform.localScale = gaugeScale;
	}
}
