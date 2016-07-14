using UnityEngine;
using System;
using System.Collections;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class DataRecord {
		public TimeSpan time;
		public string objName;
		public string status;

		public DataRecord(TimeSpan time, string objName, string status) {
			this.time = time;
			this.objName = objName;
			this.status = status;
		}

		public void Print() {
			Debug.Log(time + ", " + objName + ", " + status);
		}

		public string ToString() {
			return time + ", " + objName + ", " + status;
		}
	}
}