using UnityEngine;
using System;
using System.Collections.Generic;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class ContextDrivenSimulation {
		Activity curActivity;
		AICharacterControl characterController;
		StateSpaceManager stateSpaceManager;

		public ContextDrivenSimulation (AICharacterControl characterController, StateSpaceManager stateSpaceManager) {
			this.characterController = characterController;
			this.stateSpaceManager = stateSpaceManager;
		}

		public void SelectContextActivities() {
			int activityID;

			for (int i = 0; i < SimulationEntity.CurContext.CountContextActivities(); i++) {
				activityID = SimulationEntity.CurContext.GetContextActivity (i).ID;

				if (IsAvailable (activityID)) {
					SimulationEntity.ActivityPlayList.AddActivity (SimulationEntity.CurContext.GetContextActivity (i).ID);
				}
			}

			foreach (Context.NextContext nc in SimulationEntity.CurContext.NextContexts) {
				int contextID = nc.ID;
				Context cont = SimulationEntity.ContextGraph.GetContext (contextID);

				for (int i = 0; i < cont.CountContextActivities(); i++) {
					activityID = cont.GetContextActivity (i).ID;

					if (IsAvailable (activityID)) {
						SimulationEntity.ActivityPlayList.AddActivity (cont.GetContextActivity (i).ID);
					}
				}
			}
			Debug.Log (SimulationEntity.ActivityPlayList.Count () + " activities are added");
		}

		// check whether the context activity is available
		public bool IsAvailable(int activityID) {
			string objName;
			for (int j = 0; j < SimulationEntity.Activities [activityID].objectNames.Count; j++) {
				objName = SimulationEntity.Activities [activityID].objectNames [j];
//				Debug.Log (activityID + " with " + objName + " " + stateSpaceManager.GetLatestStateSpace ().ObjectsStatus [j]);
				if (stateSpaceManager.GetLatestStateSpace ().ObjectsStatus [j] == "on") {
					return false;
				}
			}

			return true;
		}
			
		public void ScheduleContextActivities() {
			int curID = 0;
			Activity activity;
			for (int i = 0; i < SimulationEntity.ActivityPlayList.Count(); i++) {
				activity = SimulationEntity.GetActivity (SimulationEntity.ActivityPlayList.GetList () [i]);
//				Debug.Log ("activity " + activity.id + "(" + activity.name + ") is scheduled");

//				TODO: scheduling algorithm
				if (i == 0) {
					curID = SimulationEntity.ActivityPlayList.GetList () [0];
					curActivity = activity;
				}
			}

			characterController.playlist.AddActivity (curID);
//			Debug.Log ("activity " + curActivity.id + "(" + curActivity.name + ") will be performed");
		}

		public void PerformContextActivity() {			
			Time.timeScale = 1;
//			characterController.PlayActivity(characterController.playlist.Pop());
			Debug.Log ("activity " + curActivity.id + "(" + curActivity.name + ") is performed");
			// test case
			stateSpaceManager.UpdateStateSpace (stateSpaceManager.startTime.Add (TimeSpan.FromSeconds (Mathf.Round (Time.time))),  1, "on");
			stateSpaceManager.UpdateStateSpace (stateSpaceManager.startTime.Add (TimeSpan.FromSeconds (Mathf.Round (Time.time))), 11, "on");
		}

		public void EvaluateStateSpace() {
			StateSpace curStateSpace = stateSpaceManager.GetLatestStateSpace ();
			Debug.Log (stateSpaceManager.StateSpaceHistory.Count + " state spaces are stored");
			curStateSpace.PrintStateSpace ();

			for (int i = 0; i < SimulationEntity.CurContext.CountNextContexts(); i++) {
				int nextContextID = SimulationEntity.CurContext.GetNextContext (i).ID;
				Context nextContext = SimulationEntity.ContextGraph.GetContext (nextContextID);
				float nextContextProb = SimulationEntity.CurContext.GetNextContext (i).Probability;

				string objName;
				int [] distances = new int[nextContext.CountContextConditions ()];
				int distance = 0;
				bool satisfiableOfMaybe = false;
				for (int j = 0; j < nextContext.CountContextConditions (); j++) {
					objName = nextContext.GetContextCondition (j).ObjectName;

					// evaluation process
					// if condition status is always, all associated objects' status should be 1
					// if condition status is never, all associated objects' status should be 0
					// if condition status is maybe, any of associated objects' status should be 1
					// others, ignore (filtered)
					// TODO generalize the process to cover other possible status terms (on/off, used/not unsed, 0/1, ...)
//					if (nextContext.ContextConditions[j].ObjectStatus == "always" && curStateSpace.GetObjectStatus(objName) == "off" ) {	
//						Debug.Log (j + "/" + (nextContext.CountContextConditions () - 1) + " " + objName);
//						distance++;
//					}
//					elseif (nextContext.ContextConditions[j].ObjectStatus == "never" && curStateSpace.GetObjectStatus(objName) == "on" ) {	
//						Debug.Log (j + "/" + (nextContext.CountContextConditions () - 1) + " " + objName);
//						distance++;
//					}
//					elseif (nextContext.ContextConditions[j].ObjectStatus == "always" && curStateSpace.GetObjectStatus(objName) == "off" && !satisfiableOfMaybe) {	
//						satisfiableOfMaybe = true;
//						Debug.Log (j + "/" + (nextContext.CountContextConditions () - 1) + " " + objName);
//					}
				}
			}
		}

		public void TransitToNextContext() {
			Debug.Log (">transit context activities");
		}

	}
}

