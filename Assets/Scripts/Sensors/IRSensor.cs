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
		
		// Update is called once per frame
		void Update () {
		/*	if (Vector3.Distance(transform.position, GameObject.Find("EthanRightHand").transform.position) < 0.35f && !isOn) {
				isOn = true;
				stateSpaceManager.AddDataRecord(stateSpaceManager.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))),
					transform.parent.name, "on");
			}
			if (Vector3.Distance(transform.position, GameObject.Find("EthanRightHand").transform.position) >= 0.35f && isOn) {
				isOn = false;
				stateSpaceManager.AddDataRecord(stateSpaceManager.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))),
					transform.parent.name, "off");
			}*/
		}
	}
}