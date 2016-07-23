using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class ActivityPlayback {
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
		
        // Build the action sequence using activity playback algorithm
		public void CreateActionSequence(int id) {
            int[] actionOccur = new int[actions.Count];

            for (int i = 0; i < activities[id].objectNames.Count; i++) {
                // Draw a random number from [min-occur, max-occur]
                System.Random r = new System.Random();
                int times = r.Next(activities[id].importances[i], activities[id].maxOccurs[i] + 1);
                if (times > 0) {
                    // If this action doesn't have prereq OR has prereq and it happened
                    if ((activities[id].prereqs[i] == -1) ||
                        (activities[id].prereqs[i] > -1 && actionOccur[activities[id].prereqs[i]] == 1)) {
                        int actionId = activities[id].actionIds[i];
                        for (int j = 0; j < times; j++) {
                            actionQueue.Enqueue(new ActionInstance(actions[actionId],
                                    objects[activities[id].objectNames[i]]));
                            Debug.Log(actions[actionId].name);
                        }
                        actionOccur[actionId] = 1;
                    }
                    else if (activities[id].prereqs[i] > -1 && actionOccur[activities[id].prereqs[i]] == 0) {
                        Debug.Log(actions[activities[id].actionIds[i]].name + " won't happen");
                    }
                }
                else {
                    Debug.Log(actions[activities[id].actionIds[i]].name + " times = 0");
                }
            }
		}

		public string GetActivityName(int id) {
			return activities[id].name;
		}

		public List<string> GetObjectList() {
			return new List<string>(objects.Keys);
		}
	}
}
