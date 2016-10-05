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
		public ActionInstance nextAction = null;
		public int currentActivity = -1;
		public bool arrivedAtDestination;
		public bool activityFinished = false;
		public NavMeshAgent navAgent { get; private set; } // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
		public Animator animator;
		public GameObject leftObject = null;
		public GameObject rightObject = null;
		public GameObject mobilePhone;

        private void Start() {
			Init();
//			PlayActivity(playlist.Pop());
        }

        private void Update() {
//			if (nextAction != null) {
//				character.Move(navAgent.desiredVelocity, false, false);
//				if (!navAgent.pathPending) {
//					if (navAgent.remainingDistance <= navAgent.stoppingDistance) {
//						if (Vector3.Distance(transform.position, nextAction.location) > 0.25f && !arrivedAtDestination) {
//							character.Move((nextAction.location - transform.position), false, false);
//						}
//					    else {
//							arrivedAtDestination = true;
//
//							// Rotate the character
//							Quaternion lookRotation = Quaternion.LookRotation(nextAction.obj.characterRotation);
//							transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 20 * Time.deltaTime);
//
//							// Play animation
//							animator.SetInteger("nextAction", nextAction.animation);
//						}
//					}
//				}
//			}
//			else {
//				// We still need to call the character's move function, but we send zeroed input as the move param.
//				character.Move(Vector3.zero, false, false);
//			}

			//=======================================================

			if (nextAction != null && !arrivedAtDestination) {
				character.Move(navAgent.desiredVelocity, false, false);
				print("====================");
				if (!navAgent.pathPending) {
					if (navAgent.remainingDistance <= navAgent.stoppingDistance) {
						if (Vector3.Distance(transform.position, nextAction.location) > 0.25f) {
							character.Move((nextAction.location - transform.position), false, false);
							print("1111111111");
						}
					    else {
							arrivedAtDestination = true;

//							transform.position = nextAction.obj.characterPosition;
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

		private void Init() {
			activityPlayback = new ActivityPlayback();
			activityPlayback.Init(this.name);

			playlist = new ActivityPlaylist();
//			playlist.AddActivity(3);

//			PlayerPrefs.DeleteAll();
//			Debug.Log("Cleared PlayerPrefs");
//
//			//adding activites that weren't finished previous run
//			if (PlayerPrefs.GetInt("prevRun") == 1) {
//				for (int i = 0; i <= PlayerPrefs.GetInt("previousLastIndex"); i++) {
//					Debug.Log("Adding activity " + PlayerPrefs.GetInt("playlistIndex" + i) + " to playlist because it wasn't finished last run");
//					playlist.AddActivity(PlayerPrefs.GetInt ("playlistIndex" + i));
//				}
//			}
				
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
				print("activity finished = " + activityFinished);
			}
		}
    }
}