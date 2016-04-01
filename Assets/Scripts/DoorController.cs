using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class DoorController : MonoBehaviour {
		AICharacterControl characterController;
		
		// Use this for initialization
		void Start () {
			characterController = GameObject.Find("Ethan").GetComponent<AICharacterControl>();
			
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		void OnTriggerEnter(Collider col) {
			if (this.gameObject.name == characterController.nextAction.obj.name) {
				if (col.gameObject.name == "EthanRightHand" && characterController.nextAction.name == "Open door") {
				    print("open the door");
				}
			}
		}
	}
}
