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
    public class MenuGUI : MonoBehaviour {
		// NOTE: HOW TO ADD TO NEW PROJECT
		// Comment out all code below the instaniations. Add this script to the main camera. 
		// Then with the camera this is attatched to in the Inspector,drag the GUISkin within the 
		// GUI assets folder into "skin", then drag Ethan (or whichever person Object has the script AICharacterControl) onto playlist
		public GameObject Ethan;
		public GameObject Pearl;

		public GUISkin skin;
		public List<string> toDoList = new List<string>();
		public AICharacterControl characterControl;
		private float screenWidth;
		private float screenHeight;
		string timeText = "";
		public float prevTime = 1;
		public float time = 0;

		public Vector2 scrollPosition = Vector2.zero; 			// Scrollview height for various tabs (Actions, Objects tabs)
		public Vector2 scrollPositionActivity = Vector2.zero; 	// Scrollview height for popups in Activities tab

		// 1 out of 5 tabs is displayed when active
		bool charScreenUp = true;
		bool objectScreenUp = false;
		bool actionScreenUp = false;
		bool activityScreenUp = false;
		bool contextsScreenUp = false;

		// Activities-tab-specific variables
		float actionHeight = 0; 								// Positions entries in Activities tab table
		int activityCounter = 0; 								// Which activity in the sidebar is selected
		int actionCounter = 0; 									// Action in entry for Activities tab table
		bool activityPopup = false; 							// Activities tab action popup
		bool objectPopup = false;								// Activities tab object popup
		bool prereqPopup = false;								// Activities tab prereq popup
		public string importance = "0"; 						// Importance textfield
		public string maxOccurrence = "1"; 						// Max Occurrence textfield

        TimeSpan timeSpan;
        DateTime todayTime;
        StateSpaceManager stateSpaceManager;
        SimulationEntity simEntity;

        void Start() {
            stateSpaceManager = GameObject.Find("Camera").GetComponent<StateSpaceManager>();
			simEntity = GameObject.FindGameObjectWithTag("Character").GetComponent<SimulationEngine>().SimEntity;
			Screen.SetResolution(1280, 800, false);
			characterControl = GameObject.FindGameObjectWithTag("Character").GetComponent<AICharacterControl>();
        }

		void Update() {
            // Logic for time in the top right
            timeSpan = TimeSpan.FromSeconds(time);
            DateTime todayTime = DateTime.Today.Add(stateSpaceManager.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))));
            timeText = todayTime.ToString("hh:mm:ss tt");
        }

        void OnGUI() {
			// makes the GUI skin the set skin
			GUI.skin = skin;
			screenWidth = Screen.width;
			screenHeight = Screen.height;

			if (!gameObject.GetComponent<MainScreenGUI>().menu) {  // Only show this GUI if the main menu is not up
				if (GUI.Button(new Rect(screenWidth-257, 65, 45, 45), "", "gear")) {
					// When main menu is displayed, main screen is paused
					gameObject.GetComponent<MainScreenGUI>().menu = true;
					prevTime = Time.timeScale;
					Time.timeScale = 0;
					gameObject.GetComponent<MainScreenGUI>().play = true;
				}
			}

            // Main Menu
			if (gameObject.GetComponent<MainScreenGUI>().menu) { // Displays main menu
				GUI.Box(new Rect(0, 0, screenWidth, screenHeight), "", "mainMenu");
				GUI.Label(new Rect(60, 20, screenWidth, screenHeight), "Main Menu", "fontMenuTitle");
				// Buttons on the left hand side to select which screen is active
				if (GUI.Button(new Rect(0, 700f / 800f * Screen.height, 201, 60), "Back", "backButton")) {
					// Go back to main screen, which is still paused
					gameObject.GetComponent<MainScreenGUI>().menu = !gameObject.GetComponent<MainScreenGUI>().menu;
				}
				if (GUI.Button(new Rect(0, (140f / 800f) * Screen.height, 201, 60), "Characters", "menuButton")) {
					charScreenUp = true;
					objectScreenUp = false;
					actionScreenUp = false;
					activityScreenUp = false;
					contextsScreenUp = false;
				}
				if (GUI.Button(new Rect(0, (200f / 800f) * Screen.height, 201, 60), "Objects", "menuButton")) {
					charScreenUp = false;
					objectScreenUp = true;
					actionScreenUp = false;
					activityScreenUp = false;
					contextsScreenUp = false;
				}
				if (GUI.Button(new Rect(0, (260f / 800f) * Screen.height, 201, 60), "Actions", "menuButton")) {
					charScreenUp = false;
					objectScreenUp = false;
					actionScreenUp = true;
					activityScreenUp = false;
					contextsScreenUp = false;
				}
				if (GUI.Button(new Rect(0, (320f / 800f) * Screen.height, 201, 60), "Activities", "menuButton")) {
					charScreenUp = false;
					objectScreenUp = false;
					actionScreenUp = false;
					activityScreenUp = true;
					contextsScreenUp = false;
				}
				if (GUI.Button(new Rect(0, (380f / 800f) * Screen.height, 201, 60), "Contexts", "menuButton")) {
                    charScreenUp = false;
                    objectScreenUp = false;
                    actionScreenUp = false;
                    activityScreenUp = false;
                    contextsScreenUp = true;
                }

				// Displays gradient for which of the five options is selected
				if (charScreenUp) {
					GUI.Label(new Rect(0, (140f / 800f) * Screen.height, 201, 60), "", "menuButtonSelected");
				} else if (objectScreenUp) {
					GUI.Label(new Rect(0, (200f / 800f) * Screen.height, 201, 60), "", "menuButtonSelected");
				} else if (actionScreenUp) {
					GUI.Label(new Rect(0, (260f / 800f) * Screen.height, 201, 60), "", "menuButtonSelected");
				} else if (activityScreenUp) {
					GUI.Label(new Rect(0, (320f / 800f) * Screen.height, 201, 60), "", "menuButtonSelected");
				} else if (contextsScreenUp) {
					GUI.Label(new Rect(0, (380f / 800f) * Screen.height, 201, 60), "", "menuButtonSelected");
				}

				if (charScreenUp) {
					if (GUI.Button(new Rect(300, 150, 200, 275), "", "char1")) {
						// Disable Pearl
						Pearl.SetActive(false);

						// Enable Ethan
						Ethan.SetActive(true);

						// Change character for MainScreenGUI
						characterControl = Ethan.GetComponent<AICharacterControl>();
						MainScreenGUI mainScreenGUI = this.GetComponent<MainScreenGUI>();
						mainScreenGUI.characterControl = characterControl;

						// Change character for camera movement
						CameraLookAtCharacter cameraMovement = this.GetComponent<CameraLookAtCharacter>();
						cameraMovement.characterTransform = Ethan.transform;
						cameraMovement.characterControl = characterControl;
					}
					if (GUI.Button(new Rect(520, 150, 200, 275), "", "char2")) {
						// Disable Ethan
						Ethan.SetActive(false);

						// Enable Pearl
						Pearl.SetActive(true);
//						Pearl.transform.position = new Vector3(6, 0, 10);

						// Change character for MainScreenGUI
						characterControl = Pearl.GetComponent<AICharacterControl>();
						MainScreenGUI mainScreenGUI = this.GetComponent<MainScreenGUI>();
						mainScreenGUI.characterControl = characterControl;

						// Change character for camera movement
						CameraLookAtCharacter cameraMovement = this.GetComponent<CameraLookAtCharacter>();
						cameraMovement.characterTransform = Pearl.transform;
						cameraMovement.characterControl = characterControl;
					}
					// In Characters tab, highlight the character image that is active
					if (Ethan.activeInHierarchy) {
						GUI.Label(new Rect(300, 150, 200, 275), "", "charSelected");
					} else if (Pearl.activeInHierarchy) {
						GUI.Label(new Rect(520, 150, 200, 275), "", "charSelected");
					}
				}
                if (objectScreenUp) {
                    int scrollViewHeightObj = 126 + characterControl.activityPlayback.GetObjectList().Count * 46;
                    scrollPosition = GUI.BeginScrollView(new Rect(300f / 1280f * Screen.width, 160f / 800f * Screen.height, 440, 580f / 800f * Screen.height), scrollPosition, new Rect(Screen.width - 405, 126, 320, scrollViewHeightObj));
                    for (int i = 0; i < characterControl.activityPlayback.GetObjectList().Count; i++) {
                        if (GUI.Button(new Rect(Screen.width - 405, 126 + 47 * i, 320, 40), characterControl.activityPlayback.GetObjectList()[i], "objectButton")) {
                        }
                    }
                    GUI.EndScrollView();
                }
                if (actionScreenUp) {
                    int scrollViewHeightAction = 126 + characterControl.activityPlayback.actions.Count * 47;
                    scrollPosition = GUI.BeginScrollView(new Rect(300f / 1280f * Screen.width, 160f / 800f * Screen.height, 440, 580f / 800f * Screen.height), scrollPosition, new Rect(Screen.width - 405, 126, 320, scrollViewHeightAction));
                    for (int i = 0; i < characterControl.activityPlayback.actions.Count; i++) {
                        if (GUI.Button(new Rect(Screen.width - 405, 126 + 47 * i, 320, 40), characterControl.activityPlayback.actions[i].name, "objectButton")) {
                        }
                    }
                    GUI.EndScrollView();
                }
				if (activityScreenUp) {
					// Activity sidebar
					int scrollViewHeightObj = characterControl.activityPlayback.activities.Count * 41;
					scrollPosition = GUI.BeginScrollView(new Rect(210, 83, 235, 690), scrollPosition, new Rect(screenWidth-415, 116, 150, scrollViewHeightObj));
					for (int i = 0; i < characterControl.activityPlayback.activities.Count; i++) {
						if (GUI.Button(new Rect(screenWidth-405, 126+38*i, 150, 29), characterControl.activityPlayback.activities[i].name, "toDoList")) {
							activityCounter = i;
						}
					}
					GUI.Label(new Rect(screenWidth-405, 126+38*activityCounter, 150, 29), "", "sidebarSelected");
					GUI.EndScrollView ();

					float height = 0;
					// Displays the activity name and the action and object columns
					GUI.Label(new Rect(442, 100 + 22 * height, 120, 40), characterControl.activityPlayback.activities[activityCounter].name, "fontMenuTitle");
					GUI.Label(new Rect(472, 146 + 22 * height, 120, 40), "Action", "fontLarge");
					GUI.Label(new Rect(660, 146 + 22 * height, 120, 40), "Object", "fontLarge");
					GUI.Label(new Rect(821, 146 + 22 * height, 120, 40), "Importance", "fontLarge");
					GUI.Label(new Rect(948, 146 + 22 * height, 120, 40), "Max Occur.", "fontLarge");
					GUI.Label(new Rect(1074, 146 + 22 * height, 120, 40), "Prerequisite", "fontLarge");
					GUI.Label(new Rect(442, 170 + 22 * height, 800, 1), "", "line");

					// This loop creates the actions and objects next to the activity 
					for (int j = 0; j < characterControl.activityPlayback.activities[activityCounter].objectNames.Count; j++) {
						height += 1;
						if (GUI.Button(new Rect(1227, 157 + 30 * (height), 12, 12), "", "x")) { // delete button
							characterControl.activityPlayback.activities [activityCounter].DeleteAction (j);
						}
						// Creates action name
						if (GUI.Button(new Rect(432, 155 + 30 * height, 180, 30), new GUIContent(characterControl.activityPlayback.actions[characterControl.activityPlayback.activities[activityCounter].actionIds[j]].name, "height" + height), "dropdown")) {
							activityPopup = true;
							actionHeight = height;
							actionCounter = j;
						}
						// Creates object name
						if (GUI.Button(new Rect(622, 155 + 30 * height, 218, 30), characterControl.activityPlayback.activities[activityCounter].objectNames[j], "dropdownObj")) {
							objectPopup = true;
							actionHeight = height;
							actionCounter = j;
						}
						// Creates importance value
						importance = GUI.TextField(new Rect(833, 155 + 30 * height, 80, 26), characterControl.activityPlayback.activities[activityCounter].importances[j].ToString());
						characterControl.activityPlayback.activities[activityCounter].importances[j] = int.Parse(importance);
						int importanceTemp = 0;
						if (int.TryParse(importance, out importanceTemp)) { // Restricts importance input to be between 0 or 1
							characterControl.activityPlayback.activities[activityCounter].importances[j] = Mathf.Clamp(importanceTemp, 0, 1);
						} else if (importance == "") {
							characterControl.activityPlayback.activities[activityCounter].importances[j] = 0;
						}

						// Creates max occurrence value
						maxOccurrence = GUI.TextField(new Rect(955, 155 + 30 * height, 80, 26), characterControl.activityPlayback.activities[activityCounter].maxOccurs[j].ToString(), 25);
						int maxOccurTemp = 1;
						if (int.TryParse(maxOccurrence, out maxOccurTemp)) { // Restricts importance input to be 1 or greater
							characterControl.activityPlayback.activities[activityCounter].maxOccurs[j] = Mathf.Clamp(maxOccurTemp, 1, 999999);
						}
						// Creates prereq name
						int prereqEntry = characterControl.activityPlayback.activities[activityCounter].prereqs[j];
						if (prereqEntry == -1) { // If -1 then prereq name is N/A
							if (GUI.Button(new Rect(1036, 155 + 30 * height, 180, 30), new GUIContent("N/A", "height" + height), "dropdown")) {
								prereqPopup = true;
								actionHeight = height;
								actionCounter = j;
							}
						} else {				// Otherwise, prereq ID is index of action name
							if (GUI.Button(new Rect(1036, 155 + 30 * height, 180, 30), new GUIContent(characterControl.activityPlayback.actions[prereqEntry].name, "height" + height), "dropdown")) {
								prereqPopup = true;
								actionHeight = height;
								actionCounter = j;
							}
						}
						GUI.Label(new Rect(442, 178 + 30 * height, 800, 1), "", "line"); 		// End of row entry

						if (activityPopup) { // creates action popup menu
							int scrollViewHeightActivities = 262 + characterControl.activityPlayback.activities.Count * 22;
							scrollPositionActivity = GUI.BeginScrollView(new Rect(442, 179+30*actionHeight, 246, 320), scrollPositionActivity, new Rect(500, 123, 110, scrollViewHeightActivities));
							for (int l = 0; l < characterControl.activityPlayback.actions.Count; l++) {
								if (GUI.Button (new Rect (500, 122, 200, 22), "Cancel", "cancelPopup")) {
									activityPopup = false;
								}
								if (GUI.Button (new Rect (500, 144 + (22 * l), 200, 22), characterControl.activityPlayback.actions [l].name, "actionPopup")) {
									characterControl.activityPlayback.activities [activityCounter].actionIds [actionCounter] = l;
									activityPopup = false;
								}
							}
							GUI.EndScrollView();
						}
						if (objectPopup) { // creates object popup menu
							int scrollViewHeightActivities = 5 + characterControl.activityPlayback.activities.Count * 22;
							scrollPositionActivity = GUI.BeginScrollView(new Rect(632, 179+30*actionHeight, 271, 320), scrollPositionActivity, new Rect(500, 123, 110, scrollViewHeightActivities));
							for (int l = 0; l < characterControl.activityPlayback.GetObjectList().Count; l++) {
								if (GUI.Button (new Rect (500, 122, 220, 22), "Cancel", "cancelPopup")) {
									objectPopup = false;
								}
								if (GUI.Button (new Rect (500, 144 + (22 * l), 220, 22), characterControl.activityPlayback.GetObjectList () [l], "actionPopup")) {
									characterControl.activityPlayback.activities [activityCounter].objectNames [actionCounter] = characterControl.activityPlayback.GetObjectList()[l];
									objectPopup = false;
								}
							}
							GUI.EndScrollView();
						}
						if (prereqPopup) { // creates prereq popup menu
							int scrollViewHeightActivities = 284 + characterControl.activityPlayback.activities.Count * 22;
							scrollPositionActivity = GUI.BeginScrollView(new Rect(1034, 179+30*actionHeight, 246, 320), scrollPositionActivity, new Rect(500, 123, 110, scrollViewHeightActivities));
							for (int l = 0; l < characterControl.activityPlayback.actions.Count; l++) {
								if (GUI.Button (new Rect (500, 122, 200, 22), "Cancel", "cancelPopup")) {
									prereqPopup = false;
								}
								if (GUI.Button (new Rect (500, 144, 200, 22), "N/A", "actionPopup")) {
									characterControl.activityPlayback.activities[activityCounter].prereqs[actionCounter] = -1;
									prereqPopup = false;
								}
								if (GUI.Button (new Rect (500, 166 + (22 * l), 200, 22), characterControl.activityPlayback.actions[l].name, "actionPopup")) {
									characterControl.activityPlayback.activities[activityCounter].prereqs[actionCounter] = l;
									prereqPopup = false;
								}
							}
							GUI.EndScrollView();
						}

					}
					height += 1;
					// Add new row entry, filled with default values
					if (GUI.Button (new Rect (1226, 157 + 30 * (height), 18, 18), "", "plus")) { // plus button
						characterControl.activityPlayback.activities [activityCounter].AddAction ();
					}
					height += 1;
                }

                if (contextsScreenUp) {
    
                }
			} // if (menu)
        } // OnGUI
    }
}
