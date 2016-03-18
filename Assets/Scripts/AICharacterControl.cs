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

		public NavMeshAgent navAgent { get; private set; } // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
		public Animator animator;
		public GameObject leftObject = null;
		public GameObject rightObject = null;
		public bool willPutDownLeftObject;
		public bool willPutDownRightObject;

		Camera[] cameras;

        // Use this for initialization
		// 0.  Getting up
		// 1.  Bathing
		// 2.  Brushing teeth
		// 3.  Washing face
		// 4.  Dressing
		// 5.  Undressing
		// 6.  Washing hands
		// 7.  Drinking water
		// 8.  Eating a meal
		// 9.  Combing hair
		// 10. Shaving
		// 11. Going to bed
		// 12. Toileting
		// 13. Leaving home
		// 14. Using cellphone
		// 15. Using computer
		// 16. Watching TV
		// 17. Preparing a meal
		// 18. Washing dishes
		// 19. Cleaning countertops
		// 20. Doing laundry
		// 21. Taking medication
		// 22. Taking out trash
		// 23. Vacuuming floors
		// 24. Falling down
        private void Start() {
			Init();
			PlayActivity(16);
        }


        // Update is called once per frame
        private void Update() {
			if (nextAction != null) {
				character.Move(navAgent.desiredVelocity, false, false);
				if (!navAgent.pathPending) {
					if (navAgent.remainingDistance <= navAgent.stoppingDistance) {
						if (!navAgent.hasPath || navAgent.velocity.sqrMagnitude == 0f)
						{						
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
			UseRightCamera();
        }

		// Read configuration data of actions and objects
		private void Init() {
			activityPlayback = new ActivityPlayback();
			activityPlayback.Init();

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