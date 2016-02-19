using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour {
	float smooth = 2.0f;
	float doorOpenAngle = 90.0f;
	private bool open;
	private bool enter;
	
	private Vector3 defaultRot;
	private Vector3 openRot;

	void Start() {
		defaultRot = transform.eulerAngles;
		openRot = new Vector3 (defaultRot.x, defaultRot.y - doorOpenAngle, defaultRot.z);
	}

	void Update (){
		if(open){
			transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, openRot, Time.deltaTime * smooth);
		}
		else {
			transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, defaultRot, Time.deltaTime * smooth);
		}
		
		if(enter){
			open = !open;
		}
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.name == "Anna") {
			print("colliding with Anna");
			enter = true;
		}
	}

	void OnTriggerExit(Collider col) {
		if (col.gameObject.name == "Anna") {
			enter = false;
		}
	}
}