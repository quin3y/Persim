using UnityEngine;
using System.Collections;

//script to be attached to the floor to detect when the player has fallen.  floor must have box collider
public class FallDetection : MonoBehaviour {

	public float fallVelocityThreshold;

	GameObject head;
	float currentHeadPositionY;
	float previousHeadPositionY;
	float headVelocity;
	bool askedIfFallen;
	bool headCloseToGround;

	//initializing everything
	void Start() {
		//Debug.Log("saved from player prefs:" + PlayerPrefs.GetFloat("vel"));
		head = GameObject.Find("EthanHead1");
		currentHeadPositionY = head.transform.position.y; //the head's position in the current frame
		previousHeadPositionY = head.transform.position.y; //the head's position in the previous frame
		askedIfFallen = false; //gets set to true when "Have you fallen?" gets printed to the console
		headCloseToGround = false; //gets set to true when the head is close to the ground
	}

	void Update () {
		currentHeadPositionY = head.transform.position.y;
		headVelocity = (currentHeadPositionY - previousHeadPositionY) / Time.deltaTime; //calculating the head's velocity
		//Debug.Log (headVelocity);
		//PlayerPrefs.SetFloat("vel", headVelocity);
		if (headCloseToGround && headVelocity < fallVelocityThreshold && !askedIfFallen) { //conditions for falling
			ReportFall();
			askedIfFallen = true;
		}
		previousHeadPositionY = currentHeadPositionY;
	}

	void OnTriggerEnter(Collider c) {
		if (c.name == "EthanHead1") { //if the head is close to the ground
			headCloseToGround = true;
		}
	}

	void ReportFall() {
		Vector3 pos = GameObject.Find("Ethan").transform.position;
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
	}
}
