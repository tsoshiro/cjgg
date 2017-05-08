using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using NUnit.Framework;

public class Test {
	GachaCtrl gachaCtrl;
	DataCtrl _dataCtrl;

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
		int id = _dataCtrl.getCommentId (0);
		Assert.AreEqual (1, id);

		id = _dataCtrl.getCommentId (6);
		Assert.AreEqual (3, id);

		List<int> list = _dataCtrl.getCommentIdList (89);
		Assert.Contains (17, list);

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
}
