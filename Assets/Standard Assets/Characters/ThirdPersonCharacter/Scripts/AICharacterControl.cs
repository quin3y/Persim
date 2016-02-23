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
        public Transform target; // target to aim for

		Animator animator;
		public Queue<ActionInfo> actionQueue;
		public ActionInfo nextAction;
		public bool actionStarted = false;

        // Use this for initialization
        private void Start()
        {
            character = GetComponent<ThirdPersonCharacter>();

			animator = GetComponentInChildren<Animator>();
			animator.SetBool("actionToWalk", true);
			
			actionQueue = new Queue<ActionInfo>();
			actionQueue.Enqueue(new ActionInfo("Open door", 1, new Vector3(3, 1, 0), "Door"));
			actionQueue.Enqueue(new ActionInfo("Lie down", 2, new Vector3(-4, 0, 0), "Cube"));
			actionQueue.Enqueue(new ActionInfo("Idle", 3, new Vector3(1, 0, -4), "Table"));

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
				if (Vector3.Distance(transform.position, nextAction.location) < 1 && !actionStarted) {
					actionStarted = true;
					animator.SetInteger("nextAction", nextAction.id);
				}
			}

			if (target != null){
			//	navAgent.SetDestination(target.position);
				character.Move(navAgent.desiredVelocity, false, false);
            }
            else {
                // We still need to call the character's move function, but we send zeroed input as the move param.
                character.Move(Vector3.zero, false, false);
            }

        }


        public void SetTarget(Transform target)
        {
            this.target = target;
        }

		void OnGUI()
		{
			GUI.Label (new Rect (10, 5,1000, 20), transform.position.ToString());
		}
    }
}


public class CharacterController : MonoBehaviour 
{


	// Use this for initialization
	void Start() 
	{	

		
	}
	
	// Update is called once per frame
	void Update() 
	{

		
	}
	

	
}
