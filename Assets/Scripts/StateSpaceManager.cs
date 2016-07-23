using UnityEngine;
using System;
using System.Collections.Generic;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class StateSpaceManager : MonoBehaviour {
		public TimeSpan startTime;
		public List<DataRecord> dataset;
		List<StateSpace> stateSpaceHistory;				// list of state spaces generated during simulation
		StateSpace stateSpaceEvaluator;					// for evaluating a context

		// Use this for initialization
		void Start() {
			startTime = new TimeSpan(0, 8, 20, 0, 0);	// Simulation start time
			dataset = new List<DataRecord>();
//			Debug.Log ("StateSpaceManager starts");
		}

		// manage StateSpaceHistory
		public List<StateSpace> StateSpaceHistory {
			get { return stateSpaceHistory;}
		}

		// manage StateSpaceEvaluator
		public StateSpace StateSpaceEvaluator {
			get { return stateSpaceEvaluator; }
		}

		public void AddDataRecord(TimeSpan time, string objName, string status) {
			dataset.Add(new DataRecord(time, objName, status));
			print(time + ", " + objName + ", " + status);
		}

		public void PrintDataset() {
			foreach (DataRecord record in dataset) {
				record.Print(); 
			}
		}

		public void SaveData() {
			using (System.IO.StreamWriter file = 
				new System.IO.StreamWriter(@"Assets/Files/dataset.txt")) {
				foreach (DataRecord record in dataset) {
					file.WriteLine(record.ToString());
				}
			}
		}

		// initialize state space
		public void InitializeStateSpace() {
			stateSpaceHistory = new List<StateSpace> ();
			stateSpaceEvaluator = new StateSpace ();
			StateSpace firstStateSpace = new StateSpace ();
			
			for (int i = 0; i < SimulationEntity.Objects.Count; i++) {				
				firstStateSpace.ObjectsStatus[i] = SimulationEntity.Objects[i].status;
				stateSpaceEvaluator.ObjectsStatus[i] = SimulationEntity.Objects[i].status;
			}
			firstStateSpace.PrintStateSpace();

			stateSpaceHistory.Add(firstStateSpace);
		}

		// update state space
		public void UpdateStateSpace(TimeSpan time, int objId, string newStatus) {	
			StateSpace newStateSpace = GetLatestStateSpace ();

			newStateSpace.UpdateObjectStatus (time, objId, newStatus);
			stateSpaceHistory.Add (newStateSpace);

			if (newStatus == "on") {
				stateSpaceEvaluator.UpdateObjectStatus (time, objId, newStatus);		// stateSpaceEaluator contains all "on" cases
			}

		}

		// print all the state spaces in history
		public void PrintStateSpaceHistory () {
			foreach (StateSpace ss in stateSpaceHistory) {
				ss.PrintStateSpace ();
			}
		}

		// print the latest state space
		public void PrintLatestStateSpace() {
			GetLatestStateSpace ().PrintStateSpace ();
		}

		// return the latest state space
		public StateSpace GetLatestStateSpace() {
			return stateSpaceHistory [stateSpaceHistory.Count-1];
		}

		// print the state space evaluator
		public void PrintStateSpaceEvaluator() {
			stateSpaceEvaluator.PrintStateSpace();
		}

		// reset state space evaluator
		public void ResetStateSpaceEvaluator() {
			stateSpaceEvaluator = GetLatestStateSpace();
		}

		void OnApplicationQuit() {
			PrintDataset();
			SaveData();
			print("Sensor data saved to file");
		}
	}
}