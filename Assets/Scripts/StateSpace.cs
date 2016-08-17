using UnityEngine;
using System;
using System.Collections.Generic;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class StateSpace
	{
		TimeSpan timeStamp;					// time stamp for the state space
		string[] objectsStatus;				// state space consisting of status of objects

		public StateSpace ()
		{
			timeStamp = new TimeSpan(0, 8, 20, 0, 0);		// TODO: initial time for the time stamp
			objectsStatus = new string[StateSpaceManager.Objects.Count];
		}

		// manage a timestamp
		public TimeSpan TimeStamp {
			get { return timeStamp; }
			set { timeStamp = value; }
		}

		// return object status
		public string[] ObjectsStatus {
			get { return objectsStatus; }
		}

		// update a status of object
		public void UpdateObjectStatus (TimeSpan time, int index, string newStatus) {
			if (objectsStatus [index] != newStatus) {
				timeStamp = time;
				objectsStatus [index] = newStatus;
			}
		}

		// print state space
		public void PrintStateSpace() {
			string strSpace = "";
			foreach (string s in objectsStatus) {
				strSpace += s;
				strSpace += " ";
			}
//			Debug.Log(strSpace);
		}

		// check the availability of the object with id
		public Boolean IsAvailable(int id) {
			if (objectsStatus [id] == "off") //TODO: define the status of "on" and "off"
				return true;
			else
				return false;
		}

		// check the availability of the object with name
		public Boolean IsAvailable(string name) {
			int id = SimulationEntity.GetObject (name).id;
			if (objectsStatus[id] == "off") //TODO: define the status of "on" and "off"
				return true;
			else
				return false;
		}

		public string GetObjectStatus(string name) {
			int id = SimulationEntity.GetObject (name).id;

			return objectsStatus[id];
		}
	}
}

