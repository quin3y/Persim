using UnityEngine;
using System;
using System.Collections.Generic;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class StateSpaceManager : MonoBehaviour {
		public TimeSpan startTime;
		public List<DataRecord> dataset;
		public static List<ObjectInfo> Objects = new List<ObjectInfo> (); 				// static varaible: a set of objects
		List<StateSpace> stateSpaceHistory;				// list of state spaces generated during simulation
		StateSpace stateSpaceEvaluator;					// for evaluating a context

		// Use this for initialization
		void Start() {
			startTime = new TimeSpan(0, 7, 43, 0, 0);	// Simulation start time
			dataset = new List<DataRecord>();
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
			print("sensor " + time + ", " + objName + ", " + status);
		}

		public void PrintDataset() {
			foreach (DataRecord record in dataset) {
				record.Print(); 
			}
		}

		public void SaveSensorData(string characterName) {
			using (System.IO.StreamWriter file = 
				new System.IO.StreamWriter(@"Assets/Files/" + characterName + "/dataset.txt")) {
				foreach (DataRecord record in dataset) {
					file.WriteLine(record.ToString());
				}
			}

			// Save RFID distances
			List<String> distances = GameObject.Find("RFID Tag").GetComponent<RFIDTag>().distances;

			using (System.IO.StreamWriter file = 
				new System.IO.StreamWriter(@"Assets/Files/" + characterName + "/RFID.txt")) {
				foreach (String d in distances) {
					file.WriteLine(d);
				}
			}
		}

		public String[] GetLastNDataRecords(int n) {
			String[] records = new String[n];
			if (n > dataset.Count) {
				for (int i = 0; i < dataset.Count; i++) {
					records[i] = dataset[i].ToString();
				}
				for (int i = dataset.Count; i < n; i++) {
					records[i] = "";
				}
			}
			else {
				for (int i = 0; i < n; i++) {
					records[i] = dataset[dataset.Count-n+i].ToString();
				}
			}

			return records;
		}

		// initialize state space
		public void InitializeStateSpace() {
			stateSpaceHistory = new List<StateSpace> ();
			stateSpaceEvaluator = new StateSpace ();
			StateSpace firstStateSpace = new StateSpace ();
			
			for (int i = 0; i < Objects.Count; i++) {				
				firstStateSpace.ObjectsStatus[i] = Objects[i].status;
				stateSpaceEvaluator.ObjectsStatus[i] = Objects[i].status;
			}
			firstStateSpace.PrintStateSpace();

			stateSpaceHistory.Add(firstStateSpace);
		}

		// count objects
		public int CountObjects () {
			return Objects.Count;
		}

		// return an object with index
		public ObjectInfo GetObject (int indObject) {
			if (indObject < Objects.Count) {
				return Objects [indObject];
			} 
			else {
				return null;
			}
		}

		public string GetObjectName (int indObject) {
			if (indObject < Objects.Count) {
				return Objects [indObject].name;
			} 
			else {
				return null;
			}
		}

		// update state space
		public void UpdateStateSpace(TimeSpan time, int objId, string newStatus) {	
			StateSpace newStateSpace = GetLatestStateSpace ();

			newStateSpace.UpdateObjectStatus (time, objId, newStatus);
			stateSpaceHistory.Add (newStateSpace);
			if (newStatus == "on") {
				stateSpaceEvaluator.UpdateObjectStatus (time, objId, newStatus);		// stateSpaceEaluator contains all "on" cases
			} 
//			stateSpaceEvaluator.PrintStateSpace ();		
		}

		// print all the state spaces in history
		public void PrintStateSpaceHistory() {
			print (StateSpaceHistory.Count + " state spaces are stored");
			for (int i = 0; i < stateSpaceHistory.Count; i++) {
				print (i); 
				stateSpaceHistory[i].PrintStateSpace ();
			}
		}

		// print the latest state space
		public void PrintLatestStateSpace() {
			GetLatestStateSpace ().PrintStateSpace ();
		}

		// return a copy of the latest state space
		public StateSpace GetLatestStateSpace() {
			StateSpace lStateSpace = new StateSpace ();
			lStateSpace.TimeStamp = stateSpaceHistory [stateSpaceHistory.Count - 1].TimeStamp;
			for (int i = 0; i < stateSpaceHistory [stateSpaceHistory.Count-1].ObjectsStatus.Length; i++) {
				lStateSpace.ObjectsStatus [i] = stateSpaceHistory [stateSpaceHistory.Count - 1].ObjectsStatus [i];
			}
			return lStateSpace;
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
			GameObject[] characters = GameObject.FindGameObjectsWithTag("Character");
			foreach (GameObject character in characters) {
				// Save sensor data to file
//				PrintDataset();
				SaveSensorData(character.name);
				
				// Save activity configuration to file
				AICharacterControl characterController = GameObject.Find(character.name).GetComponent<AICharacterControl>();
				Utils.SaveActivityConfiguration(character.name, characterController.activityPlayback.activities);
				
				Utils.SaveSensorConfiguration(character.name);
				Utils.SaveSpaceInfo(character.name);
				
				// Save unplayed activities
				PlayerPrefs.SetInt("previousLastIndex", PlayerPrefs.GetInt("lastIndexInPlaylist"));
				PlayerPrefs.SetInt("prevRun", 1);
				PlayerPrefs.Save();
			}
		}
	}
}