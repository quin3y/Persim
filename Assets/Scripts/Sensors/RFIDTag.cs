using UnityEngine;
using System;
using System.Collections.Generic;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class RFIDTag : MonoBehaviour {
		StateSpaceManager stateSpaceManager;
		Transform hand;
		public List<String> distances = new List<String>();

		// Use this for initialization
		void Start () {
			stateSpaceManager = GameObject.Find("Camera").GetComponent<StateSpaceManager>();
			hand = GameObject.FindGameObjectWithTag("RightHand").transform;
		}

		void Update() {
			distances.Add(Vector3.Distance(transform.position, hand.position).ToString());
		}

        // Called when character's hand collides with RFID tag's collider.
		void OnTriggerEnter(Collider col) {
			if (col.gameObject.tag == "LeftHand" || col.gameObject.tag == "RightHand") {
				stateSpaceManager.AddDataRecord(stateSpaceManager.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))),
                    transform.parent.name + " RFID tag", "on");
			}
		}

		void OnTriggerExit(Collider col) {
			if (col.gameObject.tag == "LeftHand" || col.gameObject.tag == "RightHand") {
				stateSpaceManager.AddDataRecord(stateSpaceManager.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))),
					transform.parent.name + " RFID tag", "off");
			}
		}
	}
}
