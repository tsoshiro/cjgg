using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using NUnit.Framework;

public class Test {

	[Test]
	public void EditorTest() {
		//Arrange
		var gameObject = new GameObject();

		//Act
		//Try to rename the GameObject
		var newGameObjectName = "My game object";
		gameObject.name = newGameObjectName;

		//Assert
		//The object has a new name
		Assert.AreEqual(newGameObjectName, gameObject.name);
	}

	void init() {
		var gameObject = new GameObject ();
		gameObject.AddComponent<GachaCtrl>();
		gachaCtrl = gameObject.GetComponent<GachaCtrl> ();
	}

	[Test]
	public void ReadCardsTest() {
		init ();
		string sampleString = "1,3,4,5,8";
		List<int> sampleList = gachaCtrl.getListFromString (sampleString);

		logList (sampleList);

		Assert.AreEqual(1, sampleList[0]);
		Assert.AreEqual(4, sampleList[2]);
		Assert.AreEqual(8, sampleList[4]);

		string str = "";
		str = gachaCtrl.getGachaResult (sampleList, 10);
		Assert.AreEqual(str, "1,3,4,5,8,10");

		str = gachaCtrl.getGachaResult (sampleList, 6);
		Assert.AreEqual(str, "1,3,4,5,6,8,10");

		logList (sampleList);

		nullTest ();
	}

	GachaCtrl gachaCtrl;

	void nullTest() {
		string sampleString = "";
		List<int> sampleList = gachaCtrl.getListFromString (sampleString);

		Assert.IsEmpty (sampleList);

		string str = gachaCtrl.getGachaResult (sampleList, 6);
		Assert.AreEqual(str, "6");
	}

	void logList <T>(List<T> pList){
		for (int i = 0; i < pList.Count; i++) {
			DebugLogger.Log ("["+i+"]:"+pList[i]);
		}
	}

	[Test]
	public void checkGachaAffordable() {
		init ();

		bool flg = gachaCtrl.checkGachaAffordable (999);
		Assert.IsFalse (flg);

		flg =  gachaCtrl.checkGachaAffordable (1000);
		Assert.IsTrue (flg);
	
		flg =  gachaCtrl.checkGachaAffordable (1001);
		Assert.IsTrue (flg);	
	}

	[Test]
	public void getCommentTest() {
		// DataCtrl初期化
		_dataCtrl = new DataCtrl ();
		_dataCtrl.InitData ();
		_dataCtrl.checkContent ();

		int id = _dataCtrl.getCommentId (0);
		Assert.AreEqual (1, id);

		id = _dataCtrl.getCommentId (6);
		Assert.AreEqual (3, id);

		id = _dataCtrl.getCommentId (89);
//		Assert.Contains (17, id);
		DebugLogger.Log ("id:" + id);
	}


	DataCtrl _dataCtrl;
	[Test]
	public void halfTest() {
		_dataCtrl = new DataCtrl ();
		_dataCtrl.InitData ();
		_dataCtrl.checkContent ();

		List<int> cmList = new List<int> ();
		int n = 10000;
		for (int i = 0; i < n; i++) {
			cmList.Add (i);
		}

		DebugLogger.Log ("linear START");
		timeList (cmList, false);

		DebugLogger.Log ("half START");
		timeList (cmList, true);
	}

	void timeList(List<int> cmList, bool flg) {
		float time = Time.realtimeSinceStartup;
		int result = getTargetMethod (50, cmList, flg);
		Assert.AreEqual (50, result);

		result = getTargetMethod (500, cmList,flg);
		Assert.AreEqual (500, result);

		result = getTargetMethod (875, cmList,flg);
		Assert.AreEqual (875, result);

		result = getTargetMethod (875, cmList,flg);
		Assert.AreEqual (875, result);

		result = getTargetMethod (999, cmList,flg);
		Assert.AreEqual (999, result);


		result = getTargetMethod (9999, cmList,flg);
		Assert.AreEqual (9999, result);


		time = Time.realtimeSinceStartup - time;
		DebugLogger.Log ("time:"+time);
	}

	int getTargetMethod(int pScore, List<int> pList, bool flg) {
		if (flg) {
			return _dataCtrl.getTargetBinary(pScore, pList);
		} else {
			return _dataCtrl.getTargetLinear (pScore, pList);
		}
	}
}
