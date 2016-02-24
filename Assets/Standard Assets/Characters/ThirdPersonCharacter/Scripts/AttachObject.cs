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
			if (col.gameObject.name == "EthanRightHand") {
				characterController.rightObject = this.gameObject;
				transform.parent = col.gameObject.transform;
				transform.localPosition = Vector3.zero;
				print(this.gameObject.name);
			}
			
		}
	}
}
