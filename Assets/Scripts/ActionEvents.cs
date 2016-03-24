using UnityEngine;
using System.Collections;


namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class ActionEvents : StateMachineBehaviour {
		 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			AICharacterControl characterController = GameObject.Find("Ethan").GetComponent<AICharacterControl>();

			if (characterController.nextAction.name == "Put down left") {
				characterController.willPutDownLeftObject = true;
			}
			if (characterController.nextAction.name == "Put down right") {
				characterController.willPutDownRightObject = true;
			}

			if (characterController.nextAction.name == "Sit down") {
				animator.SetInteger("nextAction", characterController.activityPlayback.actionQueue.Peek().animation);
			}

			// Show the mobile phone
			if (characterController.nextAction.name == "Text") {
				characterController.mobilePhone.SetActive(true);

				GameObject.Find("Mobile phone").transform.localPosition = 
					characterController.activityPlayback.objects["Mobile phone"].inHandPosition;
				GameObject.Find("Mobile phone").transform.localRotation = 
					Quaternion.Euler(characterController.activityPlayback.objects["Mobile phone"].inHandRotation);
			}

			// When the last action begins
			if (characterController.activityPlayback.actionQueue.Count == 0) {
//				characterController.nextAction = null;
//				animator.SetBool("actionToWalk", false);
			}
	    }

		// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
		//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		//
		//}

		// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
		override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			AICharacterControl characterController = GameObject.Find("Ethan").GetComponent<AICharacterControl>();

			if (characterController.nextAction.name == "Sit down") {
				animator.SetInteger("nextAction", characterController.activityPlayback.actionQueue.Peek().animation);
			}
			else {
				animator.SetInteger("nextAction", 0);
			}

			// Hide the mobile phone
			if (characterController.nextAction.name == "Text") {
				characterController.mobilePhone.SetActive(false);
			}



			// If the character is about to put down the object, detach it from the hand
			if (characterController.willPutDownLeftObject) {
				characterController.leftObject.transform.parent = null;
				characterController.leftObject.transform.position =
					characterController.activityPlayback.objects[characterController.leftObject.name].position;
				characterController.leftObject.transform.rotation =
					Quaternion.Euler(characterController.activityPlayback.objects[characterController.leftObject.name].rotation);

				characterController.leftObject = null;
				characterController.willPutDownLeftObject = false;
			}
			if (characterController.willPutDownRightObject) {
				characterController.rightObject.transform.parent = null;
				characterController.rightObject.transform.position =
					characterController.activityPlayback.objects[characterController.rightObject.name].position;
				characterController.rightObject.transform.rotation =
					Quaternion.Euler(characterController.activityPlayback.objects[characterController.rightObject.name].rotation);

				characterController.rightObject = null;
				characterController.willPutDownRightObject = false;
			}
	        
			// Set the location of the next action as the character's destination
			if (characterController.activityPlayback.actionQueue.Count > 0) {
				characterController.nextAction = characterController.activityPlayback.actionQueue.Dequeue();
				characterController.navAgent.destination = characterController.nextAction.location;

				Debug.Log("next action = " + characterController.nextAction.name);
			}

			else {    // Activity finished
				Debug.Log("finish");
				// Start next activity
				if (characterController.playlist.Count() > 0) {
					characterController.PlayActivity(characterController.playlist.Pop());
				}
				// End of simulation
				else {
					animator.SetInteger("nextAction", 0);
					characterController.nextAction = null;
					characterController.currentActivity = -1;
				}
			}
	    }

		// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
		//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		//
		//}

		// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
		//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		//
		//}
	}
}