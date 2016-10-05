using UnityEngine;
using System;
using System.Collections;


namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class ActionEvents : StateMachineBehaviour {
		 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			AICharacterControl characterController = GameObject.FindGameObjectWithTag("Character").GetComponent<AICharacterControl>();
			ChangeObjectStatusAtBeginning(characterController.nextAction);
	    }

		// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
		//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		//
		//}

		// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
		override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			AICharacterControl characterController = GameObject.FindGameObjectWithTag("Character").GetComponent<AICharacterControl>();

			if (characterController.nextAction.name == "Stand up" || characterController.nextAction.name == "Put down right" ||
				characterController.nextAction.name == "Open door" || characterController.nextAction.name == "Close door" ||
				characterController.nextAction.name == "Get up" || characterController.nextAction.name == "Close door" ||
				characterController.nextAction.name == "Turn on light" || characterController.nextAction.name == "Turn off light" ||
				characterController.nextAction.name == "Wash hands" || characterController.nextAction.name == "Dry hands" ||
				characterController.nextAction.name == "Wash face" || characterController.nextAction.name == "Dry face" ||
				characterController.nextAction.name == "Get up off ground") {
				characterController.arrivedAtDestination = false;
			}

			ChangeObjectStatusAtEnd(characterController.nextAction);

			if (characterController.nextAction.name == "Sit down" || characterController.nextAction.name == "Use toilet" ||
				characterController.nextAction.name == "Use computer" || characterController.nextAction.name == "Lie down" ||
				characterController.nextAction.name == "Turn off lamp" || characterController.nextAction.name == "Text" ||
				characterController.nextAction.name == "Pick up right" || characterController.nextAction.name == "Take pills" ||
				characterController.nextAction.name == "Drink" || characterController.nextAction.name == "Shave" ||
				characterController.nextAction.name == "Comb hair" || characterController.nextAction.name == "Use TV remote") {
				// Go to next animation directly
				animator.SetInteger("nextAction", characterController.activityPlayback.actionQueue.Peek().animation);
			}
			else {    
				// Go back to grounded
				animator.SetInteger("nextAction", 0);
			}
	        
			// If activity not finished
			if (characterController.activityPlayback.actionQueue.Count > 0) {
				// Set the location of the next action as the character's destination
				characterController.nextAction = characterController.activityPlayback.actionQueue.Dequeue();
				characterController.navAgent.destination = characterController.nextAction.location;
			}

			// Activity finished
			else {
				characterController.activityFinished = true;

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

		private void ChangeObjectStatusAtBeginning(ActionInstance action) {
			StateSpaceManager stateSpaceManager = GameObject.Find("Camera").GetComponent<StateSpaceManager>();
			string actionName = action.name;

			if (actionName == "Wash hands" || actionName == "Dry hands" || actionName == "Wash face" || actionName == "Dry face" ||
				actionName == "Use computer" || actionName == "Text" || actionName == "Flush toilet") {
				stateSpaceManager.UpdateStateSpace(stateSpaceManager.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))), action.obj.id, "on");
//				Debug.Log(stateSpaceManager.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))) + ", " + action.obj.name + ", on");
			}
		}

		private void ChangeObjectStatusAtEnd(ActionInstance action) {
			StateSpaceManager stateSpaceManager = GameObject.Find("Camera").GetComponent<StateSpaceManager>();
			string actionName = action.name;

			if (actionName == "Turn off light" || actionName == "Wash hands" || actionName == "Dry hands" || actionName == "Wash face" ||
				actionName == "Dry face" || actionName == "Stand up" || actionName == "Use computer" || actionName == "Text" ||
				actionName == "Put down right" || actionName == "Flush toilet" || actionName == "Open door" || actionName == "Turn off lamp" ||
				actionName == "Get up") {
				stateSpaceManager.UpdateStateSpace(stateSpaceManager.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))), action.obj.id, "off");
			}
			else if (actionName == "Turn on light" || actionName == "Sit down" || actionName == "Pick up right" ||
				actionName == "Close door" || actionName == "Lie down") {
				stateSpaceManager.UpdateStateSpace(stateSpaceManager.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))), action.obj.id, "on");
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