using UnityEngine;
using System;
using System.Collections.Generic;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class Activity
	{
		public int id {get; set;}
		public string name {get; set;}
		public List<Int32> actionIds;
		public List<String> objectNames;

		public Activity() {
			actionIds = new List<Int32>();
			objectNames = new List<String>();
		}
	}
}
