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

		public List<Action> allActions;
		public Dictionary<string, ObjectInfo> allObjects;
		public Queue<ActionInstance> actionQueue;
		public ActionInstance nextAction = null;

		public GameObject leftObject = null;
		public GameObject rightObject = null;

		public bool actionStarted = false;
		public bool willPutDownLeftObject;
		public bool willPutDownRightObject;

        // Use this for initialization
        private void Start()
        {
			Init();
			CreateActionQueue();

            character = GetComponent<ThirdPersonCharacter>();
			animator = GetComponentInChildren<Animator>();
			animator.SetBool("actionToWalk", true);

			navAgent = GetComponentInChildren<NavMeshAgent>();
			navAgent.updateRotation = false;
			navAgent.updatePosition = true;

			if (actionQueue.Count > 0) {
				nextAction = actionQueue.Dequeue();
				navAgent.destination = nextAction.location;
			}

        }


        // Update is called once per frame
        private void Update()
        {

			if (nextAction != null) {
				character.Move(navAgent.desiredVelocity, false, false);
				if (Vector3.Distance(transform.position, nextAction.location) < 1.5 && !actionStarted) {
					navAgent.Stop();
					actionStarted = true;
					animator.SetInteger("nextAction", nextAction.animation);
					print("character stop");
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

		public void Init() {
			allActions = Utils.ReadActionXml();
			allObjects = Utils.ReadObjectXml();
		}

		public void CreateActionQueue() {
			actionQueue = new Queue<ActionInstance>();
			actionQueue.Enqueue(new ActionInstance(allActions[1], "Bathroom light switch"));
			actionQueue.Enqueue(new ActionInstance(allActions[3], "Bathroom sink"));
			actionQueue.Enqueue(new ActionInstance(allActions[4], "Towel rack"));
			actionQueue.Enqueue(new ActionInstance(allActions[2], "Bathroom light switch"));
			actionQueue.Enqueue(new ActionInstance(allActions[0], "Table"));
//			actionQueue.Enqueue(new ActionInstance(allActions[7], "Toilet"));

		}
    }
}