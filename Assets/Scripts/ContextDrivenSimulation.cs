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

		public void RunSimulationLoop() {
			SelectContextActivities ();
			ScheduleContextActivities ();
			PerformContextActivity ();
//			EvaluateStateSpace ();
//			TransitToNextContext ();
		}

		public void SelectContextActivities() {
//			Debug.Log ("select activities with " + stateSpaceManager.StateSpaceHistory.Count);
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
			Debug.Log ("activity " + curActivity.id + "(" + curActivity.name + ") will be performed");
		}

		public void PerformContextActivity() {			
			Time.timeScale = 1;
//			characterController.PlayActivity(characterController.playlist.Pop());
			Debug.Log ("activity " + curActivity.id + "(" + curActivity.name + ") is performed");
			// test case
			stateSpaceManager.UpdateStateSpace (stateSpaceManager.startTime.Add (TimeSpan.FromSeconds (Mathf.Round (Time.time))), 11, "on");
		}

		public void EvaluateStateSpace() {
			StateSpace curStateSpace = stateSpaceManager.GetLatestStateSpace ();
			curStateSpace.PrintStateSpace ();

			for (int i = 0; i < SimulationEntity.CurContext.CountNextContexts(); i++) {
				int nextContextID = SimulationEntity.CurContext.GetNextContext (i).ID;
				Context nextContext = SimulationEntity.ContextGraph.GetContext (nextContextID);
				float nextContextProb = SimulationEntity.CurContext.GetNextContext (i).Probability;
				//Debug.Log ("context_" + nextContextID + "(" + nextContext.Name + ") will be evaluated with probability of " + nextContextProb.ToString());

				string objName;
				for (int j = 0; j < nextContext.CountContextConditions (); j++) {
					objName = nextContext.GetContextCondition (j).ObjectName;
//					Debug.Log (objName);
					Debug.Log (objName + " " + curStateSpace.IsAvailable(objName));
					// TODO: evaluation 
				}
			}
		}

		public void TransitToNextContext() {
			Debug.Log (">transit context activities");
		}

	}
}

