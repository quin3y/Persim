using UnityEngine;
using System;
using System.Collections.Generic;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class ContextDrivenSimulation {		
		AICharacterControl characterController;
		StateSpaceManager stateSpaceManager;

		Activity curActivity;
		double[] contextDistances;

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
			characterController.PlayActivity(characterController.playlist.Pop());
			Debug.Log ("activity " + curActivity.id + "(" + curActivity.name + ") is performed");
			// test case
//			stateSpaceManager.UpdateStateSpace (stateSpaceManager.startTime.Add (TimeSpan.FromSeconds (Mathf.Round (Time.time))), 11, "on");
//			stateSpaceManager.UpdateStateSpace (stateSpaceManager.startTime.Add (TimeSpan.FromSeconds (Mathf.Round (Time.time))), 12, "on");
		}

		public void EvaluateStateSpace() {
			StateSpace curStateSpace = stateSpaceManager.GetLatestStateSpace ();
			Debug.Log (stateSpaceManager.StateSpaceHistory.Count + " state spaces are stored");
			curStateSpace.PrintStateSpace ();
			contextDistances = new double[SimulationEntity.CurContext.CountNextContexts()];

			for (int i = 0; i < SimulationEntity.CurContext.CountNextContexts(); i++) {
				int nextContextID = SimulationEntity.CurContext.GetNextContext (i).ID;
				Context nextContext = SimulationEntity.ContextGraph.GetContext (nextContextID);
				float nextContextProb = SimulationEntity.CurContext.GetNextContext (i).Probability;

				string objName;
				int distance = 0;
				bool satisfiableForAlways = true, satisfiableForNever = true, satisfiableForMaybe = false;
				Debug.Log (nextContext.Name + " is evaluated");
				for (int j = 0; j < nextContext.CountContextConditions (); j++) {
					objName = nextContext.GetContextCondition (j).ObjectName;

					// evaluation process
					// if condition status is always, all associated objects' status should be 1
					// if condition status is never, all associated objects' status should be 0
					// if condition status is maybe, any of associated objects' status should be 1
					// others, ignore (filtered)
					// TODO generalize the process to cover other possible status terms (on/off, used/not unsed, 0/1, ...)
					if (nextContext.ContextConditions[j].ObjectStatus == "always" && curStateSpace.GetObjectStatus(objName) == "off" ) {
//						Debug.Log (j + "/" + (nextContext.CountContextConditions () - 1) + " " + objName + ", always out");
						satisfiableForAlways = false;	
						break;
					}
					else if (nextContext.ContextConditions[j].ObjectStatus == "never" && curStateSpace.GetObjectStatus(objName) == "on" ) {
//						Debug.Log (j + "/" + (nextContext.CountContextConditions () - 1) + " " + objName + ", never out");
						satisfiableForAlways = false;
						break;
					}
					else if (nextContext.ContextConditions[j].ObjectStatus == "maybe") {
						if (curStateSpace.GetObjectStatus (objName) == "on")
							satisfiableForMaybe = true;
						else {
							distance++;						// TODO Euclidean distance
//							Debug.Log (j + "/" + (nextContext.CountContextConditions () - 1) + " " + objName + ", " + distance);

						}
					}
				}
				if (!satisfiableForMaybe && distance == 0)		// when there is no Maybe condition
					satisfiableForMaybe = true;

				// store distances for each next context
				if (satisfiableForAlways && satisfiableForNever && satisfiableForMaybe)
					contextDistances [i] = Math.Sqrt (distance);
				else
					contextDistances [i] = Double.PositiveInfinity;			// The next context which is not reachable has positivie infinity
			}
		}

		public void TransitToNextContext() {
			double least = Double.PositiveInfinity;
			int newContextID = SimulationEntity.CurContext.ID;
			for (int k = 0; k < contextDistances.Length ;k++) {
				Context context = SimulationEntity.GetContext(SimulationEntity.CurContext.NextContexts[k].ID);

				if (contextDistances [k] != -1.0 && contextDistances [k] < least) {
					least = contextDistances [k];
					newContextID = k;
				}
			}

			if (least < Double.PositiveInfinity) {
				SimulationEntity.CurContext = SimulationEntity.GetContext(newContextID);
				Debug.Log ("least is " + least + " and change current context into current context " + newContextID);
			}
			else
				Debug.Log ("least is " + least + " and keep in the current context " + newContextID);
		}
	}
}

