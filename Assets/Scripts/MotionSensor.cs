using UnityEngine;
using System;
using System.Collections;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class MotionSensor : MonoBehaviour {
		StateSpaceManager stateSpaceManager;
		AICharacterControl characterController;

		// Use this for initialization
		void Start () {
			stateSpaceManager = GameObject.Find("Camera").GetComponent<StateSpaceManager>();
			characterController = GameObject.Find("Ethan").GetComponent<AICharacterControl>();
		}

		void OnTriggerEnter(Collider col) {
			if (col.gameObject.name == "EthanHips" || col.gameObject.name == "EthanHead1") {
				stateSpaceManager.AddDataRecord(stateSpaceManager.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))),
					transform.parent.gameObject.name, "on");
			}
		}

		void OnTriggerExit(Collider col) {
			if (col.gameObject.name == "EthanHips" || col.gameObject.name == "EthanHead1") {
				stateSpaceManager.AddDataRecord(stateSpaceManager.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))),
					transform.parent.gameObject.name, "off");
			}
		}
	}
}
