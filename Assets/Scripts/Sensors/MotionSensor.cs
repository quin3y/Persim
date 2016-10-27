using UnityEngine;
using System;
using System.Collections;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class MotionSensor : MonoBehaviour {
		StateSpaceManager stateSpaceManager;

		// Use this for initialization
		void Start () {
			stateSpaceManager = GameObject.Find("Camera").GetComponent<StateSpaceManager>();
		}

		void OnTriggerEnter(Collider col) {
			if (col.gameObject.tag == "Hip" || col.gameObject.tag == "Head") {
				stateSpaceManager.AddDataRecord(stateSpaceManager.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))),
					transform.parent.gameObject.name + " motion sensor", "on");
			}
		}

		void OnTriggerExit(Collider col) {
			if (col.gameObject.tag == "Hip" || col.gameObject.tag == "Head") {
				stateSpaceManager.AddDataRecord(stateSpaceManager.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))),
					transform.parent.gameObject.name + " motion sensor", "off");
			}
		}
	}
}
