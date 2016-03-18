using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class ActivityPlayback
	{
		public List<Action> actions;
		public List<Activity> activities;
		public Dictionary<string, ObjectInfo> objects;

		public Queue<ActionInstance> actionQueue;

		public void Init() {
			actions = Utils.ReadActionXml();
			objects = Utils.ReadObjectXml();
			activities = Utils.ReadActivityXml();

			actionQueue = new Queue<ActionInstance>();
		}
		
		public void CreateActionSequence(int id) {
			for (int i = 0; i < activities[id].objectNames.Count; i++) {
				actionQueue.Enqueue(new ActionInstance(actions[activities[id].actionIds[i]],
				                                       objects[activities[id].objectNames[i]]));
			}
//			actionQueue.Enqueue(new ActionInstance(actions[0], objects["Table"]));
		}

		public void CreateActionQueue() {

		}
	}
}
