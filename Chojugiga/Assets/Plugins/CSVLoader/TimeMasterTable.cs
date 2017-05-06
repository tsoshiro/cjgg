using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeMasterTable : MasterTableBase<TimeMaster> {
	override public void Load() {
		Load(convertClassToFilePath (this.GetType ().Name));
	}
}

public class TimeMaster : MasterBase {
	public int SCORE_IS_LOWER_THAN { get; private set; }
	public float ADD_TIME { get; private set; }
}