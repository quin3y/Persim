using UnityEngine;
using System.Collections;

public class ContactSensor : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.tag == "Door frame") {
			print("door closed");
		}
	}

	void OnTriggerExit(Collider col) {
		if (col.gameObject.tag == "Door frame") {
			print("door opened");
		}
	}
}
