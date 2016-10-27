using UnityEngine;
using System;
using System.Collections;

//script to be attached to the floor to detect when the player has fallen.  floor must have box collider
namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class FallDetection : MonoBehaviour {

		public float fallVelocityThreshold;
		public bool characterCouldFall;

		float currentHeadPositionY;
		float previousHeadPositionY;
		float headVelocity;
		bool headCloseToGround;
		GameObject head;
		StateSpaceManager stateSpaceManager;

		//initializing everything
		void Start() {
			//Debug.Log("saved from player prefs:" + PlayerPrefs.GetFloat("vel"));
			head = GameObject.FindGameObjectWithTag("Head");
			stateSpaceManager = GameObject.Find("Camera").GetComponent<StateSpaceManager>();
			currentHeadPositionY = head.transform.position.y; //the head's position in the current frame
			previousHeadPositionY = head.transform.position.y; //the head's position in the previous frame
			characterCouldFall = true; //gets set to true when "Have you fallen?" gets printed to the console
			headCloseToGround = false; //gets set to true when the head is close to the ground
		}

		void Update () {
			currentHeadPositionY = head.transform.position.y;
			headVelocity = (currentHeadPositionY - previousHeadPositionY) / Time.deltaTime; //calculating the head's velocity

			if (headCloseToGround && headVelocity < fallVelocityThreshold && characterCouldFall) { //conditions for falling
				ReportFall();
				characterCouldFall = false;
				headCloseToGround = false;
			}
			previousHeadPositionY = currentHeadPositionY;
		}

		void OnTriggerEnter(Collider c) {
			if (c.tag == "Head") { //if the head is close to the ground
				headCloseToGround = true;
			}
		}

		void ReportFall() {
			Vector3 pos = GameObject.FindGameObjectWithTag("Character").transform.position;
			string loc;
			if (pos.x < 4.4f && pos.z < 5.6f) {
				loc = "bathroom";
			}
			else if (pos.x < 4.4f && pos.z >= 5.6f && pos.z < 9.6f) {
				loc = "kitchen";
			}
			else if (pos.x < 4.4f && pos.z >= 9.6f && pos.z < 16.5f) {
				loc = "dining room";
			}
			else if (pos.x >= 4.4f && pos.z >= 9.6f && pos.z < 16.5f) {
				loc = "living room";
			}
			else if (pos.x >= 4.4f && pos.x < 7.8f && pos.z < 9.6f) {
				loc = "hallway";
			}
			else {
				loc = "bedroom";
			}
			print("Character fell in the " + loc);
			stateSpaceManager.AddDataRecord(stateSpaceManager.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))),
				"Floor motion sensor", "on");
		}
	}
}
