#define CONTCONDITION
//#undef CONTCONDITION

//#define CONTACTIVITY
#undef CONTACTIVITY

//#define NEXTCONT
#undef NEXTCONT

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


namespace UnityStandardAssets.Characters.ThirdPerson {
	public class MainScreenGUI : MonoBehaviour {
		// NOTE: HOW TO ADD TO NEW PROJECT
		// Comment out all code below the instaniations. Add this script to the main camera. 
		// Then with the camera this is attatched to in the Inspector,drag the GUISkin within the 
		// GUI assets folder into "skin", then drag Ethan (or whichever person Object has the script AICharacterControl) onto playlist
		public GUISkin skin;
		public List<string> nameList = new List<string>();
		public List<string> toDoList = new List<string>();
		public Vector2 scrollPosition = Vector2.zero;
		public Vector2 scrollPositionToDoList = Vector2.zero;
		public AICharacterControl characterControl;
		public SimulationEntity simEntity;

		public bool play;
		bool contextTabActive;
		public bool needsPlay;
		string timeText = "";
		private float prevTime = 1;
		string currActivity = "No Activity";
		public bool menu = false;
		public float time = 0;

		private float screenWidth;
		private float screenHeight;

		DateTime todayTime;
		StateSpaceManager stateSpaceManager;	

		void Start() {
			stateSpaceManager = GameObject.Find("Camera").GetComponent<StateSpaceManager>();
			characterControl = GameObject.FindGameObjectWithTag("Character").GetComponent<AICharacterControl>();
			simEntity = GameObject.FindGameObjectWithTag("Character").GetComponent<SimulationEngine>().SimEntity;

			Screen.SetResolution(1280, 800, false);
			play = true; // Play button is visible, so it is currently paused
			contextTabActive = false;
			needsPlay = false;
			for (int i = 0; i < characterControl.activityPlayback.activities.Count; i++) {
				nameList.Add(characterControl.activityPlayback.activities[i].name);
			}
		}

		void Update() {
			// Logic for time in the top right
			todayTime = DateTime.Today.Add(stateSpaceManager.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))));
			timeText = todayTime.ToString("hh:mm:ss tt");
		}

		void OnGUI() {
			// makes the GUI skin the set skin. Also provides logic for the play button
			GUI.skin = skin;

			screenWidth = Screen.width;
			screenHeight = Screen.height;

			if (!menu) {  // Only show this GUI if the main menu is not up
				// characterControl.playlist.Count is amount of activities in queue
				if (characterControl.playlist.Count() == 0 && characterControl.activityPlayback.actionQueue.Count == 0) {
					needsPlay = true;
				}

				// Control buttons panel
				GUI.Box(new Rect(screenWidth-268, 7, 260, 114), "", "timer");
				// Displays time and current activity
				GUI.Label(new Rect(screenWidth-260, 23, 201, 30), timeText + "     " + currActivity, "fontMedium");
				// Displays play speed
				GUI.Label(new Rect(screenWidth-58, 78, 201, 30), "x"+Time.timeScale, "fontLarge");

				// Activity panel
				GUI.Box(new Rect(screenWidth-268, 159, 260, 360), "");
				if (!contextTabActive) { 	// Activity tab is active
					GUI.Button(new Rect(screenWidth-267, 130, 135, 50), "", "activeTab");
					// Context tab becomes active if it is clicked
					if (GUI.Button (new Rect (screenWidth-138, 130, 135, 50), "", "inactiveTab")) {
						contextTabActive = true;
					}
				} else { 						// Context tab is active
					GUI.Button(new Rect(screenWidth-138, 130, 135, 50), "", "activeTab");
					// Activity tab becomes active if it is clicked
					if (GUI.Button(new Rect (screenWidth-267, 130, 135, 50), "", "inactiveTab")) {
						contextTabActive = false;
					}
				}
				// Text labels for the tabs
				GUI.Label(new Rect(screenWidth-232, 141, 112, 45), "Activity", "fontLarge");
				GUI.Label(new Rect(screenWidth-109, 141, 112, 45), "Context", "fontLarge");

				// When Activity tab is active
				if (!contextTabActive) {
					// List of activities in the Activities panel's scroll box
					int scrollViewHeight = characterControl.activityPlayback.activities.Count * 39;
					scrollPosition = GUI.BeginScrollView(new Rect(screenWidth-262, 177, 310, 342), scrollPosition, new Rect(screenWidth-110, 38, 200, scrollViewHeight));
					for (int i = 0; i < characterControl.activityPlayback.activities.Count; i++) {
						if (GUI.Button(new Rect(screenWidth-90, 44+38*i, 200, 29), characterControl.activityPlayback.activities[i].name, "toDoList")) {
							toDoList.Add(characterControl.activityPlayback.activities[i].name);
							characterControl.playlist.AddActivity(i);
						}
					}
					GUI.EndScrollView();
				}


				// Context tab is active - hard coded build button
				if (contextTabActive) {
					if (GUI.Button (new Rect (Screen.width-185, 190, 90, 33), "Build", "buildButton")) {
						for (int i = 0; i < simEntity.contextActivities.Length; i++) {
							toDoList.Add(characterControl.activityPlayback.activities[simEntity.contextActivities[i]].name);
							characterControl.playlist.AddActivity(simEntity.contextActivities[i]);
						}
					}
					// Labels for "Current" and "Next"
					GUI.Label(new Rect(screenWidth-255, 250, 260, 270), "Current: ", "fontMedium");
					GUI.Label(new Rect(screenWidth-255, 280, 260, 270), "Next: ", "fontMedium");

					String curContext = "";
					String nextContext = "";
					if (currActivity == "Use bathroom" || currActivity == "Wash face" || currActivity == "Comb hair") {
						curContext = "Personal Hygiene";
						nextContext = "Leisure";
					}
					else if (currActivity == "Use cellphone" || currActivity == "Watch TV" || currActivity == "Use computer") {
						curContext = "Leisure";
						nextContext = "Health Care";
					}
					else if (currActivity == "Take medication") {
						curContext = "Health Care";
						nextContext = "None";
					}

					GUI.Label(new Rect(screenWidth-185, 250, 260, 270), curContext, "fontMedium");
					GUI.Label(new Rect(screenWidth-205, 280, 260, 270), nextContext, "fontMedium");
				}

				// Queue panel - is hidden when context tab is active
				GUI.Label(new Rect(screenWidth-268, 525, 260, 270), "");
				GUI.Label(new Rect(screenWidth-235, 537, 222, 45), "Queue", "fontLarge");
				//Clear Button
				if (GUI.Button(new Rect(screenWidth-77, 526, 45, 45), "", "clear")) {
					characterControl.playlist.Clear();
					toDoList.Clear();
				}

				// List of upcoming activities in the Queue panel
				int scrollViewHeightToDoList = toDoList.Count * 39;
				scrollPositionToDoList = GUI.BeginScrollView(new Rect(screenWidth-242, 572, 290, 222), scrollPositionToDoList, new Rect(3, 38, 200, scrollViewHeightToDoList));
				for (int i = 0; i < toDoList.Count; i++) {
					//The Label of the toDoList activity
					GUI.Label(new Rect(3, 44+38*i, 200, 29), new GUIContent(toDoList[i], "h" + i), "toDoList");
					if (GUI.Button(new Rect(181, 53+38*i, 12, 12), "", "x")) { 					// The x button to delete an activity
						toDoList.RemoveAt(i);
						characterControl.playlist.DeleteActivity(i);
					}
					if (characterControl.playlist.popped == true) {    //When the activty characterControl pops something it changes the GUI list to match
						if (toDoList.Count > 0) {
							currActivity = toDoList[0];
							toDoList.RemoveAt(0);
						}
						characterControl.playlist.popped = false;
					}
				}
				GUI.EndScrollView();


				// Play button, speed up, slow down, redo Logic
				if (play) {
					if (GUI.Button(new Rect(screenWidth-167, 59, 60, 60), "", "play")) {
						if (!needsPlay) {
							Time.timeScale = prevTime;
							play = false;
						}
						// IF NOTHING PLAYING
						if (needsPlay) {
							if (Time.timeScale == 0) {
								Time.timeScale = prevTime;
							}
							characterControl.PlayActivity(nameList.IndexOf(toDoList[0]));
							characterControl.playlist.Pop();
							play = false;
							needsPlay = false;
						}
					}
				} else {
					if (GUI.Button(new Rect(screenWidth-167, 59, 60, 60), "", "pause")) {
						prevTime = Time.timeScale;
						Time.timeScale = 0;
						play = true;
					}
				}
				if (GUI.Button(new Rect(screenWidth-119, 63, 50, 50), "", "fastf")) {
					if (Time.timeScale < 64) {
						Time.timeScale = Time.timeScale * 2;
					} else {
						Time.timeScale = 64;
					}
				}
				if (GUI.Button(new Rect(screenWidth-208, 63, 50, 50), "", "slowd")) {
					if (Time.timeScale > 1) {
						Time.timeScale = Time.timeScale / 2;
					}
				}

				String[] dataRecords = stateSpaceManager.GetLastNDataRecords(5);
				for (int i = 0; i < 5; i++) {
					GUI.Label(new Rect(20, 550 + i * 25, 300, 20), dataRecords[i], "fontMedium");
				}
			}
		}
	}
}