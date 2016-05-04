﻿using UnityEngine;
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
			Debug.Log("Have you fallen?");
			askedIfFallen = true;
		}
		previousHeadPositionY = currentHeadPositionY;
	}

	void OnTriggerEnter(Collider c) {
		if (c.name == "EthanHead1") { //if the head is close to the ground
			headCloseToGround = true;
		}
	}
}
