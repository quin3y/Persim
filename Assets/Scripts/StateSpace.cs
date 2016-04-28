using UnityEngine;
using System;
using System.Collections.Generic;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class StateSpace : MonoBehaviour {
		public TimeSpan startTime;
		public List<DataRecord> dataset;

		// Use this for initialization
		void Start () {
			startTime = new TimeSpan(0, 8, 20, 0, 0);    // Simulation start time
			dataset = new List<DataRecord>();
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

		void OnApplicationQuit() {
			print("Printing dataset...");
			PrintDataset();
			SaveData();
			print("Data saved to file");
		}
	}
}