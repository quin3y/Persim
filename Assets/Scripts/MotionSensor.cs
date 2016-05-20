using UnityEngine;
using System;
using System.Collections;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class MotionSensor : MonoBehaviour {
		StateSpace stateSpace;
		AICharacterControl characterController;


		// Use this for initialization
		void Start () {
			stateSpace = GameObject.Find("Camera").GetComponent<StateSpace>();
			characterController = GameObject.Find("Ethan").GetComponent<AICharacterControl>();
		}

		void OnTriggerEnter(Collider col) {
			if (col.gameObject.name == "EthanHips") {
				stateSpace.AddDataRecord(stateSpace.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))),
					  this.gameObject.transform.parent.gameObject.name, "on");
			}
		}

		void OnTriggerExit(Collider col) {
			if (col.gameObject.name == "EthanHips") {
				stateSpace.AddDataRecord(stateSpace.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))),
					this.gameObject.transform.parent.gameObject.name, "off");
			}
		}
	}
}
