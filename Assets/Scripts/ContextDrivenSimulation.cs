using UnityEngine;
using System;
using System.Collections.Generic;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class ContextDrivenSimulation {
		Activity curActivity;
		AICharacterControl characterController;


		public ContextDrivenSimulation (AICharacterControl characterController) {
			this.characterController = characterController;
		}

		public void RunSimulationLoop() {
			SelectContextActivities ();
			ScheduleContextActivities ();
			PerformContextActivity ();
			EvaluateStateSpace ();
			TransitToNextContext ();


		}

		public void SelectContextActivities() {
			for (int i = 0; i < SimulationEntity.CurContext.CountContextActivities(); i++) {
				SimulationEntity.ActivityPlayList.AddActivity (SimulationEntity.CurContext.GetContextActivity (i).ID);
			}
		}

		public void ScheduleContextActivities() {
			for (int i = 0; i < SimulationEntity.ActivityPlayList.Count(); i++) {
				curActivity = SimulationEntity.GetActivity (SimulationEntity.ActivityPlayList.GetList () [i]);
				Debug.Log ("activity_" + curActivity.id + "(" + curActivity.name + ") is scheduled");
			}
			// TODO: scheduling algorithm
			curActivity = SimulationEntity.GetActivity (SimulationEntity.ActivityPlayList.GetList () [0]);
		}

		public void PerformContextActivity() {
			Debug.Log ("activity_" + curActivity.id + "(" + curActivity.name + ") will be performed");
			Time.timeScale = 1;
			characterController.PlayActivity(18);
//			characterController.PlayActivity(SimulationEntity.ActivityPlayList.Pop());

			// TODO: performing the activity
		}

		public void EvaluateStateSpace() {			
			for (int i = 0; i < SimulationEntity.CurContext.CountNextContexts(); i++) {
				int nextContextID = SimulationEntity.CurContext.GetNextContext (i).ID;
				Context nextContext = SimulationEntity.ContextGraph.GetContext (nextContextID);
				float nextContextProb = SimulationEntity.CurContext.GetNextContext (i).Probability;
				Debug.Log ("context_" + nextContextID + "(" + nextContext.Name + ") will be evaluated with probability of " + nextContextProb.ToString());
				for (int j = 0; j < nextContext.CountContextConditions (); j++) {
					Debug.Log (nextContext.GetContextCondition(j).ObjectName);
					// TODO: evaluation 
				}
			}
		}

		public void TransitToNextContext() {
			Debug.Log (">transit context activities");
		}

	}
}

