using UnityEngine;
using System;
using System.Collections;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class IRSensor : MonoBehaviour {
		StateSpaceManager stateSpaceManager;
		bool isOn;

		// Use this for initialization
		void Start () {
			stateSpaceManager = GameObject.Find("Camera").GetComponent<StateSpaceManager>();
			isOn = false;
		}
		
		void Update () {
            // React when character's hand is in IR sensor's range
			if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("RightHand").transform.position) < 0.35f && !isOn) {
				isOn = true;
				stateSpaceManager.AddDataRecord(stateSpaceManager.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))),
					transform.parent.name + " IR sensor", "on");
			}
			if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("RightHand").transform.position) >= 0.35f && isOn) {
				isOn = false;
				stateSpaceManager.AddDataRecord(stateSpaceManager.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))),
					transform.parent.name + " IR sensor", "off");
			}
		}
	}
}