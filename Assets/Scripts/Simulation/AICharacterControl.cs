using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script controls character's movement

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (NavMeshAgent))]
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class AICharacterControl : MonoBehaviour {
		public ActivityPlayback activityPlayback;
		public ActivityPlaylist playlist;
		public ActionInstance nextAction = null;
		public int currentActivity = -1;
		public bool arrivedAtDestination;
		public bool activityFinished = false;
		public bool isLyingDown = false;
		public NavMeshAgent navAgent { get; private set; } // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
		public Animator animator;
		public GameObject leftObject = null;
		public GameObject rightObject = null;
		public GameObject mobilePhone;

        private void Start() {
			Init();
        }

        private void Update() {
			if (nextAction != null && !arrivedAtDestination) {
				character.Move(navAgent.desiredVelocity, false, false);
				if (!navAgent.pathPending) {
					if (navAgent.remainingDistance <= navAgent.stoppingDistance) {
                        // Move character to destination when he is near the destination but has stopped moving
						if (Vector3.Distance(transform.position, nextAction.location) > 0.25f) {
							character.Move((nextAction.location - transform.position), false, false);
						}
					    else {
							arrivedAtDestination = true;

                            // Rotate character
							transform.rotation = Quaternion.LookRotation(nextAction.obj.characterRotation);

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

        // Init character's components
		private void Init() {
			activityPlayback = new ActivityPlayback();
			activityPlayback.Init(this.name);

			playlist = new ActivityPlaylist();
				
			character = GetComponent<ThirdPersonCharacter>();
			animator = GetComponentInChildren<Animator>();
			animator.SetBool("actionFinished", true);
			
			navAgent = GetComponentInChildren<NavMeshAgent>();
			navAgent.updateRotation = false;
			navAgent.updatePosition = true;

			// Hide the mobile phone
			mobilePhone = GameObject.Find("Mobile phone");
			mobilePhone.SetActive(false);
		}

        // Use ActivityPlayback to create an action sequence and then play all actions
		public void PlayActivity(int id) {
			if (id < 0) {
				print("Illegal activity id, less than 0?"+ id);
				return;
			}

			activityPlayback.CreateActionSequence(id);
			
			if (activityPlayback.actionQueue.Count > 0) {
				nextAction = activityPlayback.actionQueue.Dequeue();
				navAgent.destination = nextAction.location;
				currentActivity = id;
				activityFinished = false;
			}
		}
    }
}