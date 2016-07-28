using UnityEngine;
using System;
using System.Collections;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class AnimationEvents : MonoBehaviour {
		StateSpaceManager stateSpaceManager;
		AICharacterControl characterController;
		GameObject bedroomDoor;
		Animator bedroomDoorAnimator;

		// Use this for initialization
		void Start () {
			stateSpaceManager = GameObject.Find("Camera").GetComponent<StateSpaceManager>();
			characterController = GetComponent<AICharacterControl>();
			bedroomDoor = GameObject.Find("Bedroom door");
			bedroomDoorAnimator = bedroomDoor.GetComponent<Animator>();
			bedroomDoorAnimator.SetBool("bedroomDoorOpen", true);
		}

		void TurnOnOffLight() {
			string lightName = characterController.nextAction.obj.name;

			if (characterController.nextAction.name == "Turn on light") {
				GameObject.Find(lightName.Substring(0, lightName.Length - 7)).GetComponent<Light>().intensity = 2;
				stateSpaceManager.AddDataRecord(stateSpaceManager.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))),
					lightName, "on");
			}
			if (characterController.nextAction.name == "Turn off light") {
				GameObject.Find(lightName.Substring(0, lightName.Length - 7)).GetComponent<Light>().intensity = 0;
				stateSpaceManager.AddDataRecord(stateSpaceManager.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))),
					lightName, "off");
			}
		}

		// Turn off the main light instead
		void TurnOffLamp() {
			GameObject.Find("Main light").GetComponent<Light>().intensity = 0;
			stateSpaceManager.AddDataRecord(stateSpaceManager.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))),
				"Lamp", "off");
		}

		// Turn on the main light when the character gets up
		void LightUp() {
			GameObject.Find("Main light").GetComponent<Light>().intensity = 0.8f;
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
		}

		// Detach the object from character's right hand
		void PutDownRight() {
			GameObject obj = characterController.rightObject;
			characterController.rightObject = null;
			obj.transform.parent = null;
			obj.transform.position = characterController.activityPlayback.objects[obj.name].position;
			obj.transform.rotation = Quaternion.Euler(characterController.activityPlayback.objects[obj.name].rotation);
		}

		// Closes bedroom door
		void CloseDoor() {
			bedroomDoorAnimator.SetBool("bedroomDoorOpen", false);
		}

		// Opens bedroom door
		void OpenDoor() {
			bedroomDoorAnimator.SetBool("bedroomDoorOpen", true);
		}

		void HandOnToiletHandle() {
			stateSpaceManager.AddDataRecord(stateSpaceManager.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))),
				"Toilet handle", "on");
		}

		void HandOffToiletHandle() {
			stateSpaceManager.AddDataRecord(stateSpaceManager.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))),
				"Toilet handle", "off");
		}

		void StartUsingComputer() {
			stateSpaceManager.AddDataRecord(stateSpaceManager.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))),
				"Computer", "on");
		}

		void FinishUsingComputer() {
			stateSpaceManager.AddDataRecord(stateSpaceManager.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))),
				"Computer", "off");
		}

		void StartUsingCellphone() {
			stateSpaceManager.AddDataRecord(stateSpaceManager.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))),
				"Mobile phone", "on");
		}

		void FinishUsingCellphone() {
			stateSpaceManager.AddDataRecord(stateSpaceManager.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))),
				"Mobile phone", "off");
		}
	}
}
