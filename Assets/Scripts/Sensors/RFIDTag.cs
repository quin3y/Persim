using UnityEngine;
using System;
using System.Collections;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class RFIDTag : MonoBehaviour {
		StateSpaceManager stateSpaceManager;

		// Use this for initialization
		void Start () {
			stateSpaceManager = GameObject.Find("Camera").GetComponent<StateSpaceManager>();
		}

		void OnTriggerEnter(Collider col) {
			if (col.gameObject.name == "EthanLeftHand" || col.gameObject.name == "EthanRightHand") {
				stateSpaceManager.AddDataRecord(stateSpaceManager.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))),
                    transform.parent.name, "on");
			}
		}

		void OnTriggerExit(Collider col) {
			if (col.gameObject.name == "EthanLeftHand" || col.gameObject.name == "EthanRightHand") {
				stateSpaceManager.AddDataRecord(stateSpaceManager.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))),
                    transform.parent.name, "off");
			}
		}
	}
}
