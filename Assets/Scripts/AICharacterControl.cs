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
		public ActivityPlaylist playlist;
		public int currentActivity = -1;

		public ActionInstance nextAction = null;
		public bool arrivedAtDestination;

		public NavMeshAgent navAgent { get; private set; } // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
		public Animator animator;
		public GameObject leftObject = null;
		public GameObject rightObject = null;
		public bool willPutDownLeftObject;
		public bool willPutDownRightObject;

		public GameObject mobilePhone;

		// 0.  Bathing
		// 1.  Brushing teeth
		// 2.  Cleaning countertops
		// 3.  Combing hair
		// 4.  Doing laundry
		// 5.  Dressing
		// 6.  Drinking water
		// 7.  Eating a meal
		// 8.  Falling
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
		// 25. Patrol

        private void Start() {
			Init();
			PlayActivity(playlist.Pop());
        }

        private void Update() {
			if (nextAction != null) {
				character.Move(navAgent.desiredVelocity, false, false);
				if (!navAgent.pathPending) {
					if (navAgent.remainingDistance <= navAgent.stoppingDistance) {
						if (Vector3.Distance(transform.position, nextAction.location) > 0.2f && !arrivedAtDestination) {
							character.Move((nextAction.location - transform.position), false, false);
						}
						else {
							arrivedAtDestination = true;

							// Rotate the character
							Quaternion lookRotation = Quaternion.LookRotation(nextAction.obj.characterRotation);
							transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 20f);

							// Play animation
							animator.SetInteger("nextAction", nextAction.animation);
						}
					}
				}
			}
			else {
				// We still need to call the character's move function, but we send zeroed input as the move param.
				character.Move(Vector3.zero, false, false);
			}
        }

		private void Init() {
			activityPlayback = new ActivityPlayback();
			activityPlayback.Init();

			playlist = new ActivityPlaylist();
//		playlist.AddActivity(3);
//			playlist.AddActivity(8);
//			playlist.AddActivity(13);
//			playlist.AddActivity(17);
//			playlist.AddActivity(18);
//			playlist.AddActivity(19);
//			playlist.AddActivity(22);
	//		playlist.AddActivity(23);
//			playlist.AddActivity(24);
//			playlist.AddActivity(25);


			character = GetComponent<ThirdPersonCharacter>();
			animator = GetComponentInChildren<Animator>();
			animator.SetBool("actionToWalk", true);
			
			navAgent = GetComponentInChildren<NavMeshAgent>();
			navAgent.updateRotation = false;
			navAgent.updatePosition = true;

			// Hide the mobile phone
			mobilePhone.SetActive(false);
		}

		public void PlayActivity(int id) {
			if (id < 0) {
				print("Illegal activity id");
				return;
			}

			activityPlayback.CreateActionSequence(id);
			
			if (activityPlayback.actionQueue.Count > 0) {
				nextAction = activityPlayback.actionQueue.Dequeue();
				navAgent.destination = nextAction.location;
				currentActivity = id;
//				print("starting activity " + id);
			}
		}

		void OnGUI() {
			//GUI.Label (new Rect (10, 5,1000, 20), transform.position.ToString());
		}
    }
}