using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class DoorController : MonoBehaviour {
		float smooth = 2.0f;
		float doorOpenAngle = 90.0f;

		private bool open;
		private bool enter;
		
		private Vector3 defaultRotation;
		private Vector3 openRotation;

			AICharacterControl characterController;

		void Start() {
			defaultRotation = transform.eulerAngles;
			openRotation = new Vector3(defaultRotation.x, defaultRotation.y - doorOpenAngle, defaultRotation.z);

				AICharacterControl characterController = GameObject.Find("Ethan").GetComponent<AICharacterControl>();
		}

		void Update() {
			
		}

		void OnTriggerEnter(Collider col) {
				if (col.gameObject.name == "Ethan" && this.name == characterController.nextAction.obj) {
				enter = true;
				print("OnTriggerEnter");
				print("this action = " + characterController.nextAction.name);
			}
		}

		void OnTriggerExit(Collider col) {
				if (col.gameObject.name == "Ethan") {
				enter = false;
				print("OnTriggerExit");

			}
		}
	}
}