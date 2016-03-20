using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (NavMeshAgent))]
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class AICharacterControl : MonoBehaviour {
		public ActivityPlayback activityPlayback;
		public ActionInstance nextAction = null;
		public bool activityFinished = false;
		public LinkedList<Int32> activityQueue;
		private bool arrivedAtDestination;

		public NavMeshAgent navAgent { get; private set; } // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
		public Animator animator;
		public GameObject leftObject = null;
		public GameObject rightObject = null;
		public bool willPutDownLeftObject;
		public bool willPutDownRightObject;

		public GameObject mobilePhone;

		Camera[] cameras;

		// 0.  Bathing
		// 1.  Brushing teeth
		// 2.  Cleaning countertops
		// 3.  Combing hair
		// 4.  Doing laundry
		// 5.  Dressing
		// 6.  Drinking water
		// 7.  Eating a meal
		// 8.  Falling down
		// 9.  Getting up
		// 10. Going to bed
		// 11. Leaving home
		// 12. Preparing a meal
		// 13. Shaving
		// 14. Taking medication
		// 15. Taking out trash
		// 16. Undressing
		// 17. Using bathroom
		// 18. Using cellphone
		// 19. Using computer
		// 20. Vacuuming floors
		// 21. Washing dishes
		// 22. Washing face
		// 23. Washing hands
		// 24. Watching TV

        private void Start() {
			Init();
			PlayActivity(17);
        }

        private void Update() {
			if (nextAction != null) {
				character.Move(navAgent.desiredVelocity, false, false);
				print(navAgent.desiredVelocity);
				if (!navAgent.pathPending) {
					if (navAgent.remainingDistance <= navAgent.stoppingDistance) {
						print (Vector3.Distance(transform.position, nextAction.location));
						if (Vector3.Distance(transform.position, nextAction.location) > 0.2f && !arrivedAtDestination) {
							character.Move((nextAction.location - transform.position)/3, false, false);
						}
						else {
							arrivedAtDestination = true;
							Quaternion lookRotation = Quaternion.LookRotation(nextAction.obj.characterRotation);
							transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 20f);

							animator.SetInteger("nextAction", nextAction.animation);
						}

//						if (!navAgent.hasPath || navAgent.velocity.sqrMagnitude == 0f) {						
//							// Rotate the character
//							Quaternion lookRotation = Quaternion.LookRotation(nextAction.obj.characterRotation);
//							transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 20f);
//
//							// Play animation
//							animator.SetInteger("nextAction", nextAction.animation);
//						}
					}

				}
			}
			else {
				// We still need to call the character's move function, but we send zeroed input as the move param.
				character.Move(Vector3.zero, false, false);
			}
			UseRightCamera();
        }

		// Read configuration data of actions and objects
		private void Init() {
			activityPlayback = new ActivityPlayback();
			activityPlayback.Init();

			activityQueue = new LinkedList<Int32>();

			character = GetComponent<ThirdPersonCharacter>();
			animator = GetComponentInChildren<Animator>();
			animator.SetBool("actionToWalk", true);
			
			navAgent = GetComponentInChildren<NavMeshAgent>();
			navAgent.updateRotation = false;
			navAgent.updatePosition = true;

			// Get all cameras and set only one camera active
			cameras = FindObjectsOfType(typeof(Camera)) as Camera[];
			Array.Sort(cameras, delegate(Camera cam1, Camera cam2) {
				return cam1.name.CompareTo(cam2.name);
			});
			UseRightCamera();

			// Hide the mobile phone
			mobilePhone.SetActive(false);
		}

		private void PlayActivity(int id) {
			activityPlayback.CreateActionSequence(id);
			
			if (activityPlayback.actionQueue.Count > 0) {
				nextAction = activityPlayback.actionQueue.Dequeue();
				navAgent.destination = nextAction.location;
				activityFinished = false;
			}
		}

		// Switch camera based on the character's position
		private void UseRightCamera() {
			if (transform.position.x < 4.76f && transform.position.z < 6f) {
				cameras[0].enabled = false;
				cameras[1].enabled = true;
			}
			else {
				cameras[0].enabled = true;
				cameras[1].enabled = false;
			}
		}

		void OnGUI() {
			GUI.Label (new Rect (10, 5,1000, 20), transform.position.ToString());
		}
    }
}