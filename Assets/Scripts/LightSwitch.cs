using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class LightSwitch : MonoBehaviour {
		AICharacterControl characterController;
		Light light;
		bool isOn;

		// Use this for initialization
		void Start () {
			characterController = GameObject.Find("Ethan").GetComponent<AICharacterControl>();
			light = GameObject.Find("Bathroom light").GetComponent<Light>();
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		void OnTriggerEnter(Collider col) {
			if (col.gameObject.name == "EthanRightHand") {
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
		}
	}
}
