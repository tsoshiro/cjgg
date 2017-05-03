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
		DataCtrl _dataCtrl = new DataCtrl ();
		_dataCtrl.InitData ();
		_dataCtrl.checkContent ();

		int id = _dataCtrl.getCommentId (0);
		Assert.AreEqual (1, id);

		id = _dataCtrl.getCommentId (6);
		Assert.AreEqual (2, id);

		id = _dataCtrl.getCommentId (89);
		DebugLogger.Log ("id:" + id);

	}
}
