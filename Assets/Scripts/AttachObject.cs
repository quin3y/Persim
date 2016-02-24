using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class AttachObject : MonoBehaviour {
		AICharacterControl characterController;


		// Use this for initialization
		void Start () {
			characterController = GameObject.Find("Ethan").GetComponent<AICharacterControl>();

		}
		
		// Update is called once per frame
		void Update () {
		
		}

		void OnTriggerEnter(Collider col) {
			if (this.gameObject.name == characterController.nextAction.obj) {
				if (col.gameObject.name == "EthanLeftHand" && characterController.nextAction.name == "Pick up left") {
					characterController.leftObject = this.gameObject;
					transform.parent = col.gameObject.transform;
					transform.localPosition = characterController.allObjects[this.gameObject.name].inHandPosition;
					transform.localRotation = 
						Quaternion.Euler(characterController.allObjects[this.gameObject.name].inHandRotation);
				}
				if (col.gameObject.name == "EthanRightHand" && characterController.nextAction.name == "Pick up right") {
					characterController.rightObject = this.gameObject;
					transform.parent = col.gameObject.transform;
					transform.localPosition = characterController.allObjects[this.gameObject.name].inHandPosition;
					transform.localRotation =
						Quaternion.Euler(characterController.allObjects[this.gameObject.name].inHandRotation);
				}
			}

			
		}
	}
}
