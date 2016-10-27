using UnityEngine;
using System;
using System.Collections;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class AnimationEvents : MonoBehaviour {
		StateSpaceManager stateSpaceManager;
		AICharacterControl characterController;

		GameObject bedroomDoor;
        GameObject frontDoor;
		Animator bedroomDoorAnimator;
        Animator frontDoorAnimator;

		// Use this for initialization
		void Start () {
			stateSpaceManager = GameObject.Find("Camera").GetComponent<StateSpaceManager>();
			characterController = GetComponent<AICharacterControl>();

			bedroomDoor = GameObject.Find("Bedroom door");
			bedroomDoorAnimator = bedroomDoor.GetComponent<Animator>();
			bedroomDoorAnimator.SetBool("bedroomDoorOpen", true);

            frontDoor = GameObject.Find("Front door");
            frontDoorAnimator = frontDoor.GetComponent<Animator>();
		}

		// Works for bathroom light only
		void TurnOnOffLight() {
//			string lightName = characterController.nextAction.obj.name;

			if (characterController.nextAction.name == "Turn on light") {
				GameObject.Find("Bathroom light").GetComponent<Light>().intensity = 4;
				stateSpaceManager.AddDataRecord(stateSpaceManager.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))),
					"Bathroom light motion sensor", "on");
			}
			if (characterController.nextAction.name == "Turn off light") {
				GameObject.Find("Bathroom light").GetComponent<Light>().intensity = 0;
				stateSpaceManager.AddDataRecord(stateSpaceManager.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))),
					"Bathroom light motion sensor", "on");
			}
		}

		// Turn off the main light instead
		void TurnOffLamp() {
			GameObject.Find("Main light").GetComponent<Light>().intensity = 0;
			stateSpaceManager.AddDataRecord(stateSpaceManager.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))),
				"Lamp motion sensor", "on");
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
			obj.transform.parent = GameObject.FindGameObjectWithTag("RightHand").transform;
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

        void PickUpTVRemote() {
            GameObject obj = GameObject.Find("TV remote control");
			obj.transform.parent = GameObject.FindGameObjectWithTag("LeftHand").transform;
            obj.transform.localPosition = characterController.activityPlayback.objects["TV remote control"].inHandPosition;
            obj.transform.localRotation =
                Quaternion.Euler(characterController.activityPlayback.objects["TV remote control"].inHandRotation);
            stateSpaceManager.UpdateStateSpace(stateSpaceManager.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))),
                characterController.activityPlayback.objects["TV remote control"].id, "on");
        }

        void PutDownRemote() {
            GameObject obj = GameObject.Find("TV remote control");
            obj.transform.parent = null;
            obj.transform.position = characterController.activityPlayback.objects["TV remote control"].position;
            obj.transform.rotation = Quaternion.Euler(characterController.activityPlayback.objects["TV remote control"].rotation);
            stateSpaceManager.UpdateStateSpace(stateSpaceManager.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))),
                characterController.activityPlayback.objects["TV remote control"].id, "off");
        }

		// Closes bedroom door
		void CloseDoor() {
            if (characterController.nextAction.obj.name == "Bedroom door") {
                bedroomDoorAnimator.SetBool("bedroomDoorOpen", false);
            }
            else if (characterController.nextAction.obj.name == "Front door outside") {
                frontDoorAnimator.SetBool("frontDoorOpen", false);
            }
		}

		// Opens bedroom door
		void OpenDoor() {
            if (characterController.nextAction.obj.name == "Bedroom door") {
                bedroomDoorAnimator.SetBool("bedroomDoorOpen", true);
            }
            else if (characterController.nextAction.obj.name == "Front door inside") {
                frontDoorAnimator.SetBool("frontDoorOpen", true);
            }
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
            characterController.mobilePhone.SetActive(true);

            GameObject.Find("Mobile phone").transform.localPosition = 
                characterController.activityPlayback.objects["Mobile phone"].inHandPosition;
            GameObject.Find("Mobile phone").transform.localRotation = 
                Quaternion.Euler(characterController.activityPlayback.objects["Mobile phone"].inHandRotation);
			stateSpaceManager.AddDataRecord(stateSpaceManager.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))),
				"Mobile phone", "on");
		}

		void FinishUsingCellphone() {
            characterController.mobilePhone.SetActive(false);
			stateSpaceManager.AddDataRecord(stateSpaceManager.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))),
				"Mobile phone", "off");
		}

		void MoveBody() {
			transform.position += new Vector3(0, 0, 2f);
		}

		void MoveCharacterOnBed() {
			GameObject.FindGameObjectWithTag("Character").transform.position = new Vector3(10, 0.016f, 8.7f);
			characterController.isLyingDown = true;
		}

		void LetCharacterFall() {
			FallDetection fall = GameObject.Find("Plane").GetComponent<FallDetection>();
			fall.characterCouldFall = true;
		}
	}
}
