using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommentMasterTable : MasterTableBase<CommentMaster> {
	override public void Load() {
		Load(convertClassToFilePath (this.GetType ().Name));
	}
}

public class CommentMaster : MasterBase {
	public int ID { get; private set; }
	public int SCORE_IS_OVER { get; private set; }
	public string COMMENT { get; private set; }
}