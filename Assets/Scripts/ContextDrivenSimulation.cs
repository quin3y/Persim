using UnityEngine;
using System;
using System.Collections.Generic;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class ContextDrivenSimulation {		
		AICharacterControl characterController;
		StateSpaceManager stateSpaceManager;

		Activity curActivity;										// currently performing activity
		List<Context.ContextActivity> selectedContextActivities;	// list of selected context activities
		List<int> scheduledContextActivities;						// list of scheduled context activities
		double[] contextDistances;

		public ContextDrivenSimulation (AICharacterControl characterController, StateSpaceManager stateSpaceManager) {
			this.characterController = characterController;
			this.stateSpaceManager = stateSpaceManager;
			selectedContextActivities = new List<Context.ContextActivity> ();
			scheduledContextActivities = new List<int> ();
		}

		// select all possible activities from the current context and the next contexts
		public void SelectContextActivities() {
//			if (selectedContextActivities.Count == 0) {
				AddContextActivities (SimulationEntity.CurContext);

				foreach (Context.NextContext nc in SimulationEntity.CurContext.NextContexts) {
					int contextID = nc.ID;
					Context cont = SimulationEntity.ContextGraph.GetContext (contextID);
					AddContextActivities (cont);
				}
//			}
			Debug.Log (selectedContextActivities.Count + " activities are selected");
		}

		// add possible activities as context activities
		public void AddContextActivities(Context cont) {
			Context.ContextActivity contActivity;

			for (int i = 0; i < cont.CountContextActivities(); i++) {
				contActivity = cont.GetContextActivity (i);

				if (IsAvailable (cont.GetContextActivity (i).ID) && !contActivity.Performed) {					
					selectedContextActivities.Add (contActivity);
				}
			}
		}

		// check whether the context activity is available
		public bool IsAvailable(int activityID) {
			string objName;
			for (int j = 0; j < SimulationEntity.Activities [activityID].objectNames.Count; j++) {
				objName = SimulationEntity.Activities [activityID].objectNames [j];
//				Debug.Log (activityID + " with " + objName + " " + stateSpaceManager.GetLatestStateSpace ().ObjectsStatus [j]);
				if (stateSpaceManager.GetLatestStateSpace ().ObjectsStatus [j] == "on") {	// not available if the object is used
					return false;
				}
			}

			return true;
		}

		// schecule all the possible activities. the first activity in the schedule will be performed next.
		public void ScheduleContextActivities() {
			int curID = 0;
			Activity activity;

			for (int i = 0; i < selectedContextActivities.Count; i++) {
				activity = SimulationEntity.GetActivity (selectedContextActivities[i].ID);
//				Debug.Log ("activity " + activity.id + "(" + activity.name + ") is scheduled");

//				TODO: scheduling algorithm
				if (i == 0) {
					curID = selectedContextActivities[0].ID;
					curActivity = activity;
				}
			}

			characterController.playlist.AddActivity (curID);
		}

		// perform a scheduled activity by playing a character, which will update state space
		public void PerformContextActivity() {			
			Time.timeScale = 1;

			characterController.PlayActivity(characterController.playlist.Pop ());
			selectedContextActivities [0].Performed = true;

			Debug.Log ("activity " + curActivity.id + "(" + curActivity.name + ") is performed");

			scheduledContextActivities.Add (curActivity.id);
			SimulationEntity.ScheduledActivities.Add (curActivity.id);

			// TODO test cases
//			stateSpaceManager.UpdateStateSpace (stateSpaceManager.startTime.Add (TimeSpan.FromSeconds (Mathf.Round (Time.time))), 11, "on");
//			stateSpaceManager.UpdateStateSpace (stateSpaceManager.startTime.Add (TimeSpan.FromSeconds (Mathf.Round (Time.time))), 11, "off");
//			stateSpaceManager.UpdateStateSpace (stateSpaceManager.startTime.Add (TimeSpan.FromSeconds (Mathf.Round (Time.time))), 12, "on");
//			stateSpaceManager.UpdateStateSpace (stateSpaceManager.startTime.Add (TimeSpan.FromSeconds (Mathf.Round (Time.time))), 12, "off");
		}

		// evalucate state space and find all reachable contexts
		public void EvaluateStateSpace() {
			Debug.Log (SimulationEntity.CurContext.Name + " is evaluated with " + SimulationEntity.CurContext.CountNextContexts() + " next contexts");
			StateSpace curStateSpace = stateSpaceManager.StateSpaceEvaluator;
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

		// find the only ONE context to reach as next context
		public void TransitToNextContext() {
			double least = Double.PositiveInfinity;
			int newContextID = SimulationEntity.CurContext.ID;
			for (int k = 0; k < contextDistances.Length ;k++) {

				if (contextDistances [k] != -1.0 && contextDistances [k] < least) {
					least = contextDistances [k];
					newContextID = k;
				}
			}
			stateSpaceManager.ResetStateSpaceEvaluator ();

			if (least < Double.PositiveInfinity) {
				SimulationEntity.CurContext = SimulationEntity.GetContext (newContextID);
				Debug.Log ("the least distance is " + least + " and change current context into current context " + newContextID);
				ResetScheduledContextActivityList ();
			} 
			else {
				Debug.Log ("the least distance is " + least + " and keep in the current context " + newContextID);
			}
			ResetSelectedContextActivityList ();
		}

		public void ResetSelectedContextActivityList() {
			selectedContextActivities.Clear ();
		}

		public void ResetScheduledContextActivityList() {
			scheduledContextActivities.Clear ();
		}
	}
}

