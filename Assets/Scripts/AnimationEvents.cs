using UnityEngine;
using System;
using System.Collections;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class AnimationEvents : MonoBehaviour {
		AICharacterControl characterController;
		Light light;
		bool isOn;

		// Use this for initialization
		void Start () {
			characterController = GetComponent<AICharacterControl>();
			light = GameObject.Find("Bathroom light").GetComponent<Light>();
		}

		void TurnOnOffLight() {
			if (characterController.nextAction.name == "Turn on light" && !isOn) {
				light.intensity = 2;
				isOn = true;
				print("light on");
			}
			if (characterController.nextAction.name == "Turn off light" && isOn) {
				light.intensity = 0;
				isOn = false;
				print("light off");
			}
		}

		// Attach the object to character's right hand
		void PickUpRight() {
			string objName = characterController.nextAction.obj.name;
			GameObject obj = GameObject.Find(objName);
			characterController.rightObject = obj;
			obj.transform.parent = GameObject.Find("EthanRightHand").transform;
			obj.transform.localPosition = characterController.activityPlayback.objects[objName].inHandPosition;
			obj.transform.localRotation =
				Quaternion.Euler(characterController.activityPlayback.objects[objName].inHandRotation);

			print(characterController.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))) + ", " + objName + ", picked up right");
		}

		// Detach the object from character's right hand
		void PutDownRight() {
			GameObject obj = characterController.rightObject;
			characterController.rightObject = null;
			obj.transform.parent = null;
			obj.transform.position = characterController.activityPlayback.objects[obj.name].position;
			obj.transform.rotation = Quaternion.Euler(characterController.activityPlayback.objects[obj.name].rotation);

			print(characterController.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))) + ", " + obj.name + ", put down right");
		}
	}
}
