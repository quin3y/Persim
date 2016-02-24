using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (NavMeshAgent))]
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class AICharacterControl : MonoBehaviour
    {
		public NavMeshAgent navAgent { get; private set; } // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
		public Animator animator;
		public Queue<ActionInfo> actionQueue;
		public ActionInfo nextAction;

		public GameObject leftObject;
		public GameObject rightObject;

		public bool actionStarted = false;
		public bool willPutDownLeftObject;
		public bool willPutDownRightObject;

        // Use this for initialization
        private void Start()
        {
            character = GetComponent<ThirdPersonCharacter>();

			animator = GetComponentInChildren<Animator>();
			animator.SetBool("actionToWalk", true);
			
			actionQueue = new Queue<ActionInfo>();
			actionQueue.Enqueue(new ActionInfo("Turn on light", 1, GameObject.Find("Light Switch").transform.position, "Light Switch"));
			actionQueue.Enqueue(new ActionInfo("Pick up", 1, GameObject.Find("Glass").transform.position, "Glass"));
			actionQueue.Enqueue(new ActionInfo("Put down", 1, GameObject.Find("Glass").transform.position, "Glass"));
			actionQueue.Enqueue(new ActionInfo("Lie down", 2, GameObject.Find("Cube").transform.position, "Cube"));
			actionQueue.Enqueue(new ActionInfo("Idle", 3, GameObject.Find("Table").transform.position, "Table"));

			if (actionQueue.Count > 0) {
				nextAction = actionQueue.Dequeue();
			}
			
			navAgent = GetComponentInChildren<NavMeshAgent>();
			navAgent.updateRotation = false;
			navAgent.updatePosition = true;
			navAgent.destination = nextAction.location;

        }


        // Update is called once per frame
        private void Update()
        {
			if (nextAction != null) {
				character.Move(navAgent.desiredVelocity, false, false);

				if (Vector3.Distance(transform.position, nextAction.location) < 1.5 && !actionStarted) {
					navAgent.Stop();
					actionStarted = true;
					animator.SetInteger("nextAction", nextAction.id);
				}
			}
			else {
				// We still need to call the character's move function, but we send zeroed input as the move param.
				character.Move(Vector3.zero, false, false);
			}

        }

		void OnGUI()
		{
			GUI.Label (new Rect (10, 5,1000, 20), transform.position.ToString());
		}
    }
}