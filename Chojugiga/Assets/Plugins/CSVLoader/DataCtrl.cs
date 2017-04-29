using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCtrl {
	List<TimeMaster> _timeMasterList;

	public void InitData() {
		InitTimeMasteData ();
	}

	public void InitTimeMasteData() {
		_timeMasterList = new List<TimeMaster>();

		var entityMasterTable = new TimeMasterTable ();
		entityMasterTable.Load ();
		foreach (var entitymaster in entityMasterTable.All) {
			_timeMasterList.Add (entitymaster);
		}
	}
		
	public float getAddTime(int pScore) {
		int n = 0;
		for (int i = _timeMasterList.Count - 1; i >= 0; i--) {
			if (_timeMasterList [i].SCORE_IS_LOWER_THAN <= pScore) {
				n = i;
				break;
			}
		}
		return _timeMasterList [n].ADD_TIME;
	}

	public int getNextTargetScore(int pScore) {
		int n = 0;
		for (int i = _timeMasterList.Count - 1; i >= 0; i--) {
			if (_timeMasterList [i].SCORE_IS_LOWER_THAN <= pScore) {
				n = (i >= _timeMasterList.Count - 1) ? i : i + 1;
				break;
			}
		}
		return _timeMasterList[n].SCORE_IS_LOWER_THAN;
	}

	public void checkContent() {
		for (int i = 0; i < _timeMasterList.Count; i++) {
			TimeMaster tm = _timeMasterList [i];
			DebugLogger.Log ("[" + i + "] SCORE : "+ tm.SCORE_IS_LOWER_THAN + " ADD_TIME : " + tm.ADD_TIME);
		}
	}
}
