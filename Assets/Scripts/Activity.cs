using UnityEngine;
using System;
using System.Collections.Generic;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class Activity {
		public int id {get; set;}
		public string name {get; set;}
		public string objectName {get; set;}
		public List<Int32> actionIds;
		public List<string> objectNames;

		public Activity() {
			actionIds = new List<Int32>();
			objectNames = new List<string>();
		}

		// Add the 1st action by default, user can change
		public void AddAction() {
			actionIds.Add(1);
			objectNames.Add("Bathroom light switch");
		}

		public void DeleteAction(int index) {
			if (actionIds.Count == 0)    return;

			actionIds.RemoveAt(index);
			objectNames.RemoveAt(index);
		}
	}
}
