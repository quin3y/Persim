﻿using UnityEngine;
using System.Collections;

// Change camera's position and rotation based on character's position.

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class CameraLookAtCharacter : MonoBehaviour {
		public Transform characterTransform;
		public AICharacterControl characterControl;

		public bool closeUpEnabled = true;

		void Start () {
			characterTransform = GameObject.FindGameObjectWithTag("Character").transform;
			characterControl = GameObject.FindGameObjectWithTag("Character").GetComponent<AICharacterControl>();
		}

		// Change camera's position and rotation based on the character's position
		void LateUpdate () {
			if (closeUpEnabled) {  // Close up shots on objects
				if (characterControl.nextAction != null) {
					float distance = Vector3.Distance(characterTransform.position, characterControl.nextAction.obj.position);
					if (characterControl.nextAction.name == "Pick up right" && distance >= 1.2f && distance <= 2.9f) {
						if (characterControl.nextAction.obj.name == "Razor") {
							transform.position = new Vector3(1.38f, 1f, 3.79f);
							transform.rotation = Quaternion.Euler(new Vector3(26f, 304f, 3f));
						}
						else if (characterControl.nextAction.obj.name == "Kitchen cup") {
							transform.position = new Vector3(1.4f, 1.03f, 8.05f);
							transform.rotation = Quaternion.Euler(new Vector3(23f, 238f, 1f));
						}
						else if (characterControl.nextAction.obj.name == "Comb") {
							transform.position = new Vector3(1.3f, 1.02f, 4.82f);
							transform.rotation = Quaternion.Euler(new Vector3(21f, -52.5f, -1f));
						}
						return;
					}
				}
			}

			if (characterTransform.position.x < 5.2f && characterTransform.position.z >= 2.2f && characterTransform.position.z < 5.7f) {
				transform.position = new Vector3(1.26f, 1.8f, 0.6f);
				transform.rotation = Quaternion.Euler(new Vector3(17.73f, 22.64f, 3.33f));
				
                // Camera behind character in bathroom
				if (characterTransform.position.x < 3f) {
					transform.position = new Vector3(4.5f, 1.55f, 2.85f);
					transform.rotation = Quaternion.Euler(new Vector3(11.2f, 296.4f, 2.86f));
				}
			}
			else if (characterTransform.position.x < 5.2f && characterTransform.position.z < 2.2f) {
				transform.position = new Vector3(2.985f, 1.8f, 5.264f);
				transform.rotation = Quaternion.Euler(new Vector3(15.94f, 180f, 0.62f));
			}
			else if ((characterTransform.position.x >= 4.8f && characterTransform.position.x < 8.2f && characterTransform.position.z >= 3.6f && characterTransform.position.z < 7f) || 
				characterTransform.position.x >= 5f && characterTransform.position.x < 8.8f && characterTransform.position.z < 3.6f) {
				transform.position = new Vector3(6.7f, 2f, 1.94f);
				transform.LookAt(characterTransform);  // Camera rotates
			}

			// Bedroom
			else if ((characterTransform.position.x >= 8f && characterTransform.position.z >= 3.72f && characterTransform.position.z < 9.7f) ||
				(characterTransform.position.x >= 9f && characterTransform.position.z < 3.7f)) {
				if (characterControl.isLyingDown) {
					transform.position = new Vector3(9.2f, 1.6f, 4.8f);
					transform.rotation = Quaternion.Euler(new Vector3(17, 10, -1));
				}
				else {
					transform.position = new Vector3(13.143f, 2f, 4.984f);
                    transform.LookAt(characterTransform);  // Camera rotates
				}
			}
			else {
				transform.position = new Vector3(5.4f, 2f, 13f);
                transform.LookAt(characterTransform);  // Camera rotates
			}
		}
	}
}