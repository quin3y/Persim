using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class ContactSensor : MonoBehaviour {
		AICharacterControl characterController;


		// Use this for initialization
		void Start () {
			characterController = GetComponent<AICharacterControl>();

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
}
