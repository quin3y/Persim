using UnityEngine;
using System;
using System.Collections;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class IRSensor : MonoBehaviour {
		StateSpace stateSpace;
		bool isOn;

		// Use this for initialization
		void Start () {
			stateSpace = GameObject.Find("Camera").GetComponent<StateSpace>();
			isOn = false;
		}
		
		// Update is called once per frame
		void Update () {
			if (Vector3.Distance(transform.position, GameObject.Find("EthanRightHand").transform.position) < 0.35f && !isOn) {
				isOn = true;
				stateSpace.AddDataRecord(stateSpace.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))),
					"faucet", "on");
			}
			if (Vector3.Distance(transform.position, GameObject.Find("EthanRightHand").transform.position) >= 0.35f && isOn) {
				isOn = false;
				stateSpace.AddDataRecord(stateSpace.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))),
					"faucet", "off");
			}
		}
	}
}