using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCtrl {
	List<TimeMaster> _timeMasterList;
	List<CommentMaster> _commentMasterList;

	public void InitData() {
		_timeMasterList = InitMasterData<TimeMaster, TimeMasterTable> (_timeMasterList);
		_commentMasterList = InitMasterData<CommentMaster, CommentMasterTable> (_commentMasterList);
	}

	#region TIME
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

		for (int i = 0; i < _commentMasterList.Count; i++) {
			CommentMaster m = _commentMasterList [i];
			DebugLogger.Log ("[" + i + "] ID : "+ m.ID + " SCORE_IS_OVER : " + m.SCORE_IS_OVER + " COMMENT:" +m.COMMENT);
		}
	}
	#endregion

	#region COMMON
	public List<T> InitMasterData <T, U> (List<T> pMasterList)
		where T : MasterBase,  new() // MasterBase
		where U : MasterTableBase<T>, new() // MasterTableBase<CommentMaster>
	{
		pMasterList = new List<T> ();

		var entityMasterTable = new U();
		entityMasterTable.Load ();
		foreach (var entitymaster in entityMasterTable.All) {
			pMasterList.Add (entitymaster);
		}

		return pMasterList;
	}
	#endregion
}
