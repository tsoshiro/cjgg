using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using NUnit.Framework;

public class Test {
	GachaCtrl gachaCtrl;
	DataCtrl _dataCtrl;
	GameManager _gameManager;

	[SetUp]
	public void init() {
		// DataCtrl初期化
		_dataCtrl = new DataCtrl ();
		_dataCtrl.InitData ();
		_dataCtrl.checkContent ();
	}
		
	public void ReadCardsTest() {
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
		
	/// <summary>
	/// ガチャチェック
	/// </summary>
	public void checkGachaAffordable() {
		bool flg = gachaCtrl.checkGachaAffordable (999);
		Assert.IsFalse (flg);

		flg =  gachaCtrl.checkGachaAffordable (1000);
		Assert.IsTrue (flg);
	
		flg =  gachaCtrl.checkGachaAffordable (1001);
		Assert.IsTrue (flg);	
	}

	[Test]
	public void getCommentTest() {
		List<int> list = _dataCtrl.getCommentIdList (0);
		DebugLogger.Log ("0");
		logList (list);
		Assert.Contains (1, list);

		list = _dataCtrl.getCommentIdList (30);
		DebugLogger.Log ("30");
		logList (list);

		Assert.Contains (9, list);

		list = _dataCtrl.getCommentIdList (89);
		Assert.Contains (14, list);

		list = _dataCtrl.getCommentIdList (91);
		Assert.Contains (19, list);

		list = _dataCtrl.getCommentIdList (100);
		Assert.Contains (20, list);
	}

	[Test]
	public void binarySearchTest() {
		_dataCtrl = new DataCtrl ();
		_dataCtrl.InitData ();
		_dataCtrl.checkContent ();

		List<int> cmList = new List<int> ();
		int n = 10000;
		for (int i = 0; i < n; i++) {
			cmList.Add (i);
		}

		DebugLogger.Log ("linear START");
		testCommentList (cmList, false);

		DebugLogger.Log ("half START");
		testCommentList (cmList, true);
	}

	void testCommentList(List<int> cmList, bool flg) {
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

	[Test]
	public void reviewRequestTest() {
		GameObject obj = new GameObject ();
		obj.AddComponent<ReviewRequestCtrl> ();
		ReviewRequestCtrl rrc = obj.GetComponent<ReviewRequestCtrl> ();

		bool flg = rrc.checkIsToBeAsked (20, 0, 0);
		Assert.IsTrue (flg);

		flg = rrc.checkIsToBeAsked (40, 0, 0);
		Assert.IsTrue (flg);

		flg = rrc.checkIsToBeAsked (42, 0, 0);
		Assert.IsFalse (flg);

		flg = rrc.checkIsToBeAsked (40, 1, 0);
		Assert.IsFalse (flg);

		flg = rrc.checkIsToBeAsked (100, 0, 1);
		Assert.IsTrue (flg);

		flg = rrc.checkIsToBeAsked (200, 0, 1);
		Assert.IsTrue (flg);

		flg = rrc.checkIsToBeAsked (40, 1, 1);
		Assert.IsFalse (flg);

	}
}
