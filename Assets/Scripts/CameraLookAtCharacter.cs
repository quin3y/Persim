using UnityEngine;
using System.Collections;

public class CameraLookAtCharacter : MonoBehaviour {
	private Transform target;

	void Start () {
		target = GameObject.Find("Ethan").transform;
	}

	// Change camera's position and rotation based on the character's position
	void LateUpdate () {
		if (target.position.x < 5.2f && target.position.z >= 2.2f && target.position.z < 5.6f) {
			transform.position = new Vector3(1.26f, 1.8f, 0.6f);
			transform.rotation = Quaternion.Euler(new Vector3(17.73f, 22.64f, 1.25f));
		}
		else if (target.position.x < 5.2f && target.position.z < 2.2f) {
			transform.position = new Vector3(2.985f, 1.8f, 5.264f);
			transform.rotation = Quaternion.Euler(new Vector3(15.94f, 180f, 0.62f));
		}
		else if ((target.position.x >= 4.8f && target.position.x < 8.2f && target.position.z >= 3.6f && target.position.z < 7f) || 
		         target.position.x >= 5f && target.position.x < 8.8f && target.position.z < 3.6f) {
			transform.position = new Vector3(6.7f, 2f, 1.94f);
			transform.LookAt(target);

		}
		else if ((target.position.x >= 8f && target.position.z >= 3.72f && target.position.z < 9.7f) ||
		         (target.position.x >= 9f && target.position.z < 3.7f)) {
			transform.position = new Vector3(13.143f, 2f, 4.984f);
			transform.LookAt(target);
		}
		else if (target.position.x < 5f && target.position.z >= 6.4f) {
			transform.position = new Vector3(1.35f, 2f, 10.76f);
			transform.LookAt(target);
		}
		else {
			transform.position = new Vector3(5.4f, 2f, 13f);
			transform.LookAt(target);
		}
	}
}
