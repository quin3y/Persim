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

		Camera[] cameras;

        // Use this for initialization
        private void Start()
        {
			Init();
			CreateActionQueue();

			if (actionQueue.Count > 0) {
				nextAction = actionQueue.Dequeue();
				navAgent.destination = nextAction.location;
			}

			cameras = FindObjectsOfType(typeof(Camera)) as Camera[];
			Array.Sort(cameras, delegate(Camera cam1, Camera cam2) {
				return cam1.name.CompareTo(cam2.name);
			});
        }


        // Update is called once per frame
        private void Update()
        {
			if (nextAction != null) {
				character.Move(navAgent.desiredVelocity, false, false);
//				if (Vector3.Distance(transform.position, nextAction.location) < nextAction.characterDistance && !actionStarted) {
//					navAgent.Stop();
//					actionStarted = true;
//					animator.SetInteger("nextAction", nextAction.animation);
//
//					Vector3 direction = new Vector3(-1, 0, -1).normalized;
//					Quaternion lookRotation = Quaternion.LookRotation(direction);
//					transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
//
//				}



				if (!navAgent.pathPending) {
					if (navAgent.remainingDistance <= navAgent.stoppingDistance) {
					//	if (!navAgent.hasPath || navAgent.velocity.sqrMagnitude == 0f)
						{
							character.Move(Vector3.zero, false, false);
							navAgent.Stop();

							animator.SetFloat("Turn", 0);
//							Vector3 direction = new Vector3(-1, 0, -1).normalized;
//							Quaternion lookRotation = Quaternion.LookRotation(direction);
//							transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
						}
					}
				}



			}
			else {
				// We still need to call the character's move function, but we send zeroed input as the move param.
				character.Move(Vector3.zero, false, false);
			}
			SwitchCamera();
        }

		void OnGUI()
		{
//			GUI.Label (new Rect (10, 5,1000, 20), transform.position.ToString());
		}

		// Read configuration data of actions and objects
		public void Init() {
			allActions = Utils.ReadActionXml();
			allObjects = Utils.ReadObjectXml();

			character = GetComponent<ThirdPersonCharacter>();
			animator = GetComponentInChildren<Animator>();
			animator.SetBool("actionToWalk", true);
			
			navAgent = GetComponentInChildren<NavMeshAgent>();
			navAgent.updateRotation = false;
			navAgent.updatePosition = true;
		}

		public void CreateActionQueue() {
			actionQueue = new Queue<ActionInstance>();
//			actionQueue.Enqueue(new ActionInstance(allActions[1], allObjects["Bathroom light switch"]));
//			actionQueue.Enqueue(new ActionInstance(allActions[7], allObjects["Toilet"]));
//			actionQueue.Enqueue(new ActionInstance(allActions[5], allObjects["Bathroom sink"]));
//			actionQueue.Enqueue(new ActionInstance(allActions[6], allObjects["Towel rack"]));
//			actionQueue.Enqueue(new ActionInstance(allActions[2], allObjects["Bathroom light switch"]));
			actionQueue.Enqueue(new ActionInstance(allActions[0], allObjects["Table"]));

		}

		// Switch camera based on the character's position
		public void SwitchCamera() {
			if (transform.position.x < 4.76f && transform.position.z < 6f) {
				cameras[0].enabled = false;
				cameras[1].enabled = true;
			}
			else {
				cameras[0].enabled = true;
				cameras[1].enabled = false;
			}
		}
    }
}