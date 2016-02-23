using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour {
	float smooth = 2.0f;
	float doorOpenAngle = 90.0f;

	private bool open;
	private bool enter;
	
	private Vector3 defaultRotation;
	private Vector3 openRotation;

	CharacterController characterController;

	void Start() {
		defaultRotation = transform.eulerAngles;
		print ("defaultRotation = " + defaultRotation);
		openRotation = new Vector3(defaultRotation.x, defaultRotation.y - doorOpenAngle, defaultRotation.z);

		characterController = GameObject.Find("Anna").GetComponent<CharacterController>();
	}

	void Update() {
		if (open) {
			transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, openRotation, Time.deltaTime * smooth);
		}
		else {
			transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, defaultRotation, Time.deltaTime * smooth);

		}
		
		if (enter) {
			open = !open;
		}
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.name == "Anna" && this.name == characterController.nextAction.obj) {
			enter = true;
			print("OnTriggerEnter");
			print("this action = " + characterController.nextAction.name);
		}
	}

	void OnTriggerExit(Collider col) {
		if (col.gameObject.name == "Anna") {
			enter = false;
			print("OnTriggerExit");

		}
	}
}