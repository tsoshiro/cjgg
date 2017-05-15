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
	/// <summary>
	/// コメントIDからコメントを取得
	/// </summary>
	/// <returns>The comment.</returns>
	/// <param name="pId">P identifier.</param>
	public string getComment(int pId) {
		string str = _commentMasterList [pId - 1].COMMENT;
		return convertComment(str);
	}

	/// <summary>
	/// 改行処理
	/// </summary>
	/// <returns>The comment.</returns>
	/// <param name="pString">P string.</param>
	string convertComment(string pString) {
		string str = pString;
		str = str.Replace ("<br>", "\n");
		return str;
	}

	/// <summary>
	/// スコアからコメントIDを取得
	/// </summary>
	/// <returns>The comment identifier.</returns>
	/// <param name="pScore">P score.</param>
	public int getCommentId(int pScore) {
		List<int> availableIdList = getCommentIdList (pScore);

		if (availableIdList.Count <= 0) { // リストに何も入っていない=バグ
			return -1;
		}

		// 利用可能IDからランダムに返す
		int randomId = Random.Range (0, availableIdList.Count);

		return availableIdList[randomId];
	}

	/// <summary>
	/// 獲得スコアから利用可能なコメントID一覧を取得
	/// </summary>
	/// <returns>The comment master list.</returns>
	public List<int> getCommentIdList(int pScore) {
		int targetScore = getTargetScore (pScore, _commentMasterList);

		List<int> availableIdList = getAvailableIdList (targetScore, _commentMasterList);
		return availableIdList;
	}
		
	/// <summary>
	/// 獲得スコアで、SCORE_IS_OVERの値を取得
	/// </summary>
	/// <returns>The target score.</returns>
	/// <param name="pScore">P score.</param>
	/// <param name="pList">P list.</param>
	public int getTargetScore(int pScore, List<CommentMaster> pList) {
		for (int i = pList.Count-1; i >= 0; i--) {
			// SCOREがSCORE_IS_OVERより上なら、その時のスコアをターゲットスコアとして設定する
			if (pScore >= pList [i].SCORE_IS_OVER) {
				return pList [i].SCORE_IS_OVER;
			}
		}	
		return 0;
	}

	/// <summary>
	/// 獲得したスコアで、表示される候補リストを取得
	/// </summary>
	/// <returns>The available identifier list.</returns>
	/// <param name="pScore">P score.</param>
	/// <param name="pList">P list.</param>
	public List<int> getAvailableIdList(int pTargetScore, List<CommentMaster> pList) {
		List<int> list = new List<int> ();

		int targetScoreIndex = getTargetScoreIndex (pTargetScore, pList);
		list.Add(pList[targetScoreIndex].ID);

		// 重複分もリストに追加
		// + 方向に
		for (int i = targetScoreIndex + 1; i < pList.Count; i++) {
			if (pList [i].SCORE_IS_OVER == pTargetScore) {
				list.Add (pList [i].ID);
				continue;
			}
			break;
		}

		// - 方向に
		for (int i = targetScoreIndex - 1; i >= 0; i--) {
			if (pList [i].SCORE_IS_OVER == pTargetScore) {
				list.Add (pList [i].ID);
				continue;
			}
			break;
		}

		return list;
	}
		
	/// <summary>
	/// 獲得したスコアが対応するSCORE_IS_OVERが、Listの何要素目にあるかを取得
	/// </summary>
	/// <returns>The target score index.</returns>
	/// <param name="pScore">P score.</param>
	/// <param name="pList">P list.</param>
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

	/// <summary>
	/// 二分探索でターゲットスコアが存在するインデックスを取得
	/// </summary>
	/// <returns>The target binary.</returns>
	/// <param name="pScore">P score.</param>
	/// <param name="pList">P list.</param>
	public int getTargetBinary(int pTargetScore, List<int> pList) {
		int left = 0;
		int right = pList.Count - 1;

		while (left <= right) {
			int mid = (left + right) / 2;
			if (pList[mid] == pTargetScore) {
				return mid;
			} else if (pList[mid] < pTargetScore) {
				left = mid + 1;
			} else {
				right = mid;
			}
		}
		return -1;
	}

	/// <summary>
	/// 線形探索
	/// </summary>
	/// <returns>The target linear.</returns>
	/// <param name="pScore">P score.</param>
	/// <param name="pList">P list.</param>
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
