using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class ActionEvents : StateMachineBehaviour {
	    
		 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			AICharacterControl characterController = GameObject.Find("Ethan").GetComponent<AICharacterControl>();
	        
			// Do this when the last action begins
	        if (characterController.actionQueue.Count == 0) {
				characterController.nextAction = null;
				animator.SetBool("actionToWalk", false);
				animator.SetBool("actionToIdle", true);
	        }
			if (characterController.nextAction.name == "Put down") {
				characterController.willPutDownRightObject = true;
			}
	    }

		// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
		//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		//
		//}

		// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
		override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			AICharacterControl characterController = GameObject.Find("Ethan").GetComponent<AICharacterControl>();
			characterController.actionStarted = false;
			animator.SetInteger("nextAction", 0);
	        
	        if (characterController.actionQueue.Count > 0) {
				characterController.nextAction = characterController.actionQueue.Dequeue();
				characterController.navAgent.destination = characterController.nextAction.location;
				characterController.navAgent.Resume();
			}

			if (characterController.willPutDownRightObject) {
				characterController.rightObject.transform.parent = null;
				characterController.rightObject.transform.position = new Vector3(-11f, 1.11f, -16f);
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