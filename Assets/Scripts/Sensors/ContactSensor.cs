using UnityEngine;
using System;
using System.Collections;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class ContactSensor : MonoBehaviour {
		AICharacterControl characterController;
		StateSpaceManager stateSpaceManager;

		// Use this for initialization
		void Start () {
			characterController = GetComponent<AICharacterControl>();
			stateSpaceManager = GameObject.Find("Camera").GetComponent<StateSpaceManager>();
		}

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
