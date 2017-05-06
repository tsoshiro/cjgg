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

	#region TIME
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
	#endregion

	#region Comment
	public string getComment(int pId) {
		string str = _commentMasterList [pId - 1].COMMENT;
		return str;
	}
		
	public int getCommentId(int pScore) {
		int targetScore = getTargetScore (pScore, _commentMasterList);

		List<int> availableIdList = getAvailableIdList (targetScore, _commentMasterList);

		if (_commentMasterList.Count <= 0) { // リストに何も入っていない=バグ
			return -1;
		}

		int randomId = Random.Range (0, availableIdList.Count);

		return availableIdList[randomId];
	}
		
	public int getTargetScore(int pScore, List<CommentMaster> pList) {
		for (int i = pList.Count-1; i >= 0; i--) {
			// SCOREがSCORE_IS_OVERより上なら、その時のスコアをターゲットスコアとして設定する
			if (pScore >= pList [i].SCORE_IS_OVER) {
				return pList [i].SCORE_IS_OVER;
			}
		}	
		return 0;
	}

	public List<int> getAvailableIdList(int pScore, List<CommentMaster> pList) {
		List<int> list = new List<int> ();

		int targetScoreIndex = getTargetScoreIndex (pScore, pList);
		list.Add(pList[targetScoreIndex].ID);

		// 重複分もリストに追加
		// + 方向に
		for (int i = targetScoreIndex + 1; i < pList.Count; i++) {
			if (pList [i].SCORE_IS_OVER == pScore) {
				list.Add (pList [i].ID);
			}
			break;
		}

		// - 方向に
		for (int i = targetScoreIndex - 1; i >= 0; i--) {
			if (pList [i].SCORE_IS_OVER == pScore) {
				list.Add (pList [i].ID);
			}
			break;
		}

		return list;
	}
		
	public int getTargetScoreIndex(int pScore, List<CommentMaster> pList) {
		int left = 0;
		int right = pList.Count - 1;

		while (left <= right) {
			int mid = (left + right) / 2;
			CommentMaster cm = pList [mid];
			if (cm.SCORE_IS_OVER == pScore) {
				return mid;
			} else if (cm.SCORE_IS_OVER < pScore) {
				left = mid + 1;
			} else {
				right = mid;
			}
		}
		return -1;
	}

	public int getTargetBinary(int pScore, List<int> pList) {
		int left = 0;
		int right = pList.Count - 1;

		while (left <= right) {
			int mid = (left + right) / 2;
			if (pList[mid] == pScore) {
				return mid;
			} else if (pList[mid] < pScore) {
				left = mid + 1;
			} else {
				right = mid;
			}
		}
		return -1;
	}

	public int getTargetLinear(int pScore,List<int> pList) {
		int result = 0;
		for (int i = 0; i < pList.Count; i++) {
			if (pList [i] == pScore) {
				result = i;
				break;
			}
		}
		return result;
	}
	#endregion
}
