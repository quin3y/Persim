using UnityEngine;
using System;
using System.Collections;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class ContactSensor : MonoBehaviour {
		StateSpaceManager stateSpaceManager;

		// Use this for initialization
		void Start () {
			stateSpaceManager = GameObject.Find("Camera").GetComponent<StateSpaceManager>();
		}

        // Called when contact sensor part A and B collide
		void OnTriggerEnter(Collider col) {
			if (col.gameObject.name == "Contact Sensor B") {
				stateSpaceManager.AddDataRecord(stateSpaceManager.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))),
					transform.parent.parent.gameObject.name + " contact sensor", "on");
			}
		}

		void OnTriggerExit(Collider col) {
			if (col.gameObject.name == "Contact Sensor B") {
				stateSpaceManager.AddDataRecord(stateSpaceManager.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))),
					transform.parent.parent.gameObject.name + " contact sensor", "off");
			}
		}
	}
}
