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
    public class MenuGUI : MonoBehaviour {
		// NOTE: HOW TO ADD TO NEW PROJECT
		// Comment out all code below the instaniations. Add this script to the main camera. 
		// Then with the camera this is attatched to in the Inspector,drag the GUISkin within the 
		// GUI assets folder into "skin", then drag Ethan (or whichever person Object has the script AICharacterControl) onto playlist
		public GUISkin skin;
		public List<string> nameList = new List<string>();
		public List<string> toDoList = new List<string>();
		public Vector2 scrollPosition = Vector2.zero;
		public Vector2 scrollPositionActivity = Vector2.zero;
		public Vector2 scrollPositionToDoList = Vector2.zero;
		public AICharacterControl characterControl;
		string timeText = "";
		public float prevTime = 1;
		public bool hovering = false;
		bool menu = false;
		public bool down = false;
		public float time = 0;
		float actionheight = 0;
		int activityCounter = 0;
		int actionCounter = 0;
		bool activityPopup = false;
		bool objectPopup = false;
		bool prereqPopup = false;
		bool charScreenUp = true;
		bool objectScreenUp = false;
		bool actionScreenUp = false;
		bool activityScreenUp = false;
		bool contextsScreenUp = false;

		bool charSelected = true;
		bool objectSelected = false;
		bool actionSelected = false;
		bool activityBtnSelected = false;
		bool contextSelected = false;

		private float screenWidth;
		private float screenHeight;
		bool contextTabActive;
		int activitySelected = 0;
		public string importance = "0";
		public string maxOccurrence = "1";
		public string prereqName = "N/A";

        TimeSpan timeSpan;
        DateTime todayTime; //= DateTime.Today.Add(timeSpan);
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
//            for (int i = 0; i < characterControl.activityPlayback.activities.Count; i++) {
//                nameList.Add(characterControl.activityPlayback.activities[i].name);
//            }
        }

        void OnGUI() {
			// makes the GUI skin the set skin. Also provides logic for the play button
			GUI.skin = skin;
			screenWidth = Screen.width;
			screenHeight = Screen.height;
			if (!menu) {  // Only show this GUI if the main menu is not up
				if (GUI.Button(new Rect(screenWidth-257, 65, 45, 45), "", "gear")) {
					menu = true;
				}
			}

            // Main Menu
            if (menu) {
				GUI.Box(new Rect(0, 0, screenWidth, screenHeight), "", "mainMenu");
				GUI.Label(new Rect(60, 20, screenWidth, screenHeight), "Main Menu", "fontMenuTitle");
				if (GUI.Button(new Rect(0, 700f / 800f * Screen.height, 201, 60), "Back", "backButton")) {
					menu = !menu;
				}
				if (GUI.Button(new Rect(0, (140f / 800f) * Screen.height, 201, 60), "Characters", "menuButton")) {
					charScreenUp = true;
					objectScreenUp = false;
					actionScreenUp = false;
					activityScreenUp = false;
					contextsScreenUp = false;

					charSelected = true;
					objectSelected = false;
					actionSelected = false;
					activityBtnSelected = false;
					contextSelected = false;
				}
				if (GUI.Button(new Rect(0, (200f / 800f) * Screen.height, 201, 60), "Objects", "menuButton")) {
					charScreenUp = false;
					objectScreenUp = true;
					actionScreenUp = false;
					activityScreenUp = false;
					contextsScreenUp = false;

					charSelected = false;
					objectSelected = true;
					actionSelected = false;
					activityBtnSelected = false;
					contextSelected = false;
				}
				if (GUI.Button(new Rect(0, (260f / 800f) * Screen.height, 201, 60), "Actions", "menuButton")) {
					charScreenUp = false;
					objectScreenUp = false;
					actionScreenUp = true;
					activityScreenUp = false;
					contextsScreenUp = false;

					charSelected = false;
					objectSelected = false;
					actionSelected = true;
					activityBtnSelected = false;
					contextSelected = false;
				}
				if (GUI.Button(new Rect(0, (320f / 800f) * Screen.height, 201, 60), "Activities", "menuButton")) {
					charScreenUp = false;
					objectScreenUp = false;
					actionScreenUp = false;
					activityScreenUp = true;
					contextsScreenUp = false;

					charSelected = false;
					objectSelected = false;
					actionSelected = false;
					activityBtnSelected = true;
					contextSelected = false;
				}
				if (GUI.Button(new Rect(0, (380f / 800f) * Screen.height, 201, 60), "Contexts", "menuButton")) {
                    charScreenUp = false;
                    objectScreenUp = false;
                    actionScreenUp = false;
                    activityScreenUp = false;
                    contextsScreenUp = true;

					charSelected = false;
					objectSelected = false;
					actionSelected = false;
					activityBtnSelected = false;
					contextSelected = true;
                }

				// Displays gradient for which of the five options is selected
				if (charSelected) {
					GUI.Label(new Rect(0, (140f / 800f) * Screen.height, 201, 60), "", "menuButtonSelected");
				} else if (actionSelected) {
					GUI.Label(new Rect(0, (260f / 800f) * Screen.height, 201, 60), "", "menuButtonSelected");
				} else if (objectSelected) {
					GUI.Label(new Rect(0, (200f / 800f) * Screen.height, 201, 60), "", "menuButtonSelected");
				} else if (activityBtnSelected) {
					GUI.Label(new Rect(0, (320f / 800f) * Screen.height, 201, 60), "", "menuButtonSelected");
				} else if (contextSelected) {
					GUI.Label(new Rect(0, (380f / 800f) * Screen.height, 201, 60), "", "menuButtonSelected");
				}

				if (charScreenUp) {
					if (GUI.Button(new Rect(300, 150, 120, 275), "", "char1")) {
						transform.Find("Ethan")
						GameObject.Find("Pearl").SetActive(false);
					}
					if (GUI.Button(new Rect(500, 150, 120, 275), "", "char2")) {
						GameObject.Find("Ethan").SetActive(false);
						GameObject.Find("Pearl").SetActive(true);
					}
//				    GUI.Box(new Rect(500f / 1280f * Screen.width, 400f / 800f * Screen.height, 140f / 1280f * Screen.width, 210f / 800f * Screen.height), "", "char1");
//				    GUI.Box(new Rect(650f / 1280f * Screen.width, 400f / 800f * Screen.height, 140f / 1280f * Screen.width, 210f / 800f * Screen.height), "", "char1");
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
					scrollPosition = GUI.BeginScrollView(new Rect(223, 73, 235, 713), scrollPosition, new Rect(screenWidth-415, 116, 150, scrollViewHeightObj));
					for (int i = 0; i < characterControl.activityPlayback.activities.Count; i++) {
						if (GUI.Button(new Rect(screenWidth-405, 126+38*i, 150, 29), characterControl.activityPlayback.activities[i].name, "toDoList")) {
							activitySelected = i;
						}
					}
					GUI.EndScrollView ();

					float height = 0;
					//int scrollViewHeightActivity = 126 + characterControl.activityPlayback.activities.Count * 300; // Main menu scroll box
					//scrollPosition = GUI.BeginScrollView(new Rect(455, 160f/800f*screenHeight, 825, 715), scrollPosition, new Rect(220, 126, 500, scrollViewHeightActivity));
				
					// Displays the activity name and the action and object columns
					//GUI.Label(new Rect (457, 116 + 22 * height, 120, 40), characterControl.activityPlayback.activities[activitySelected].name, "activity");
					GUI.Label(new Rect (485, 126 + 22 * height, 120, 40), "Action", "fontLarge");
					GUI.Label(new Rect (673, 126 + 22 * height, 120, 40), "Object", "fontLarge");
					GUI.Label(new Rect (839, 126 + 22 * height, 120, 40), "Importance", "fontLarge");
					GUI.Label(new Rect (961, 126 + 22 * height, 120, 40), "Max Occur.", "fontLarge");
					GUI.Label(new Rect (1087, 126 + 22 * height, 120, 40), "Prerequisite", "fontLarge");
					GUI.Label(new Rect (455, 150 + 22 * height, 800, 1), "", "line");

					// This loop creates the actions and objects next to the activity 
					for (int j = 0; j < characterControl.activityPlayback.activities[activitySelected].objectNames.Count; j++) {
						height += 1;
						if (GUI.Button(new Rect(1240, 157 + 30 * (height), 12, 12), "", "x")) { // delete button
							characterControl.activityPlayback.activities [activitySelected].DeleteAction (j);
						}
						/* Creates action name */
						if (GUI.Button(new Rect(445, 155 + 30 * height, 180, 30), new GUIContent(characterControl.activityPlayback.actions[characterControl.activityPlayback.activities[activitySelected].actionIds[j]].name, "height" + height), "dropdown")) {
							activityPopup = true;
							actionheight = height;
							activityCounter = activitySelected;
							actionCounter = j;
						}
						/* Creates object name */
						if (GUI.Button(new Rect(635, 155 + 30 * height, 218, 30), characterControl.activityPlayback.activities[activitySelected].objectNames[j], "dropdownObj")) {
							objectPopup = true;
							actionheight = height;
							activityCounter = activitySelected;
							actionCounter = j;
						}
						/* Creates importance value*/
						importance = GUI.TextField(new Rect(846, 155 + 30 * height, 80, 26), characterControl.activityPlayback.activities[activitySelected].importances[j].ToString());
						characterControl.activityPlayback.activities[activitySelected].importances[j] = int.Parse(importance);
						int importanceTemp = 0;
						if (int.TryParse(importance, out importanceTemp)) { // Restricts importance input to be between 0 or 1
							characterControl.activityPlayback.activities[activitySelected].importances[j] = Mathf.Clamp(importanceTemp, 0, 1);
						} else if (importance == "") {
							characterControl.activityPlayback.activities[activitySelected].importances[j] = 0;
						}

						/* Creates max occurrence value*/
						maxOccurrence = GUI.TextField(new Rect(968, 155 + 30 * height, 80, 26), characterControl.activityPlayback.activities[activitySelected].maxOccurs[j].ToString(), 25);
						int maxOccurTemp = 1;
						if (int.TryParse(maxOccurrence, out maxOccurTemp)) { // Restricts importance input to be 1 or greater
							characterControl.activityPlayback.activities[activitySelected].maxOccurs[j] = Mathf.Clamp(maxOccurTemp, 1, 999999);
						}
						/* Creates prereq name */
						if (characterControl.activityPlayback.activities[activityCounter].prereqs[j] == -1) {
							if (GUI.Button(new Rect(1049, 155 + 30 * height, 180, 30), "N/A", "dropdown")) {
								prereqPopup = true;
								actionheight = height;
								activityCounter = activitySelected;
								actionCounter = j;
							}
						} else {
							for (int k = 0; k < characterControl.activityPlayback.actions.Count; k++) {
								if (characterControl.activityPlayback.actions[k].id == characterControl.activityPlayback.activities[activityCounter].prereqs[j]) {
									prereqName = characterControl.activityPlayback.actions[k].name;
								}
							}
							if (GUI.Button(new Rect(1049, 155 + 30 * height, 180, 30), prereqName, "dropdown")) {
								prereqPopup = true;
								actionheight = height;
								activityCounter = activitySelected;
								actionCounter = j;
							}
						}
						if (GUI.Button (new Rect (455, 178 + 30 * height, 800, 1), "", "line")) {
						}
						if (activityPopup) {
							int scrollViewHeightActivities = 3 + characterControl.activityPlayback.activities.Count * 30;
							scrollPositionActivity = GUI.BeginScrollView(new Rect(455, 179+30*actionheight, 246, 320), scrollPositionActivity, new Rect(500, 123, 110, scrollViewHeightActivities));
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
							int scrollViewHeightActivities = 19 + characterControl.activityPlayback.activities.Count * 24;
							scrollPositionActivity = GUI.BeginScrollView(new Rect(645, 179+30*actionheight, 271, 320), scrollPositionActivity, new Rect(500, 123, 110, scrollViewHeightActivities));
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
						if (prereqPopup) {
							int scrollViewHeightActivities = 25 + characterControl.activityPlayback.activities.Count * 30;
							scrollPositionActivity = GUI.BeginScrollView(new Rect(1047, 179+30*actionheight, 246, 320), scrollPositionActivity, new Rect(500, 123, 110, scrollViewHeightActivities));
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
					//if (!prereqPopup) {
						if (GUI.Button (new Rect (1239, 157 + 30 * (height), 18, 18), "", "plus")) { // plus button
							characterControl.activityPlayback.activities [activitySelected].AddAction ();
						}
					//}
					height += 1;
                    //GUI.EndScrollView();
                }





                if (contextsScreenUp)
                {
                    float height = 0;
                    int scrollViewHeightActivity = 126 + simEntity.CountActivities() * 6 * 66;
                    scrollPosition = GUI.BeginScrollView(new Rect(260f / 1280f * Screen.width, 160f / 800f * Screen.height, 900f / 1280f * Screen.width, 580f / 800f * Screen.height), scrollPosition, new Rect(220, 126, 320, scrollViewHeightActivity));
                    for (int i = 0; i < simEntity.CountContexts(); i++)
                    {
                        // Displays the context name and three columns for object conditions, activities, and next contexts
                        if (GUI.Button(new Rect(220, 126 + 22 * height, 120, 40), simEntity.GetContextName(i), "activity"))
                        {
                        }
#if CONTCONDITION
                        if (GUI.Button(new Rect(500, 126 + 22 * height, 120, 40), "Object", "actOrObject"))
                        {
                        }
                        if (GUI.Button(new Rect(700, 126 + 22 * height, 120, 40), "Condition", "actOrObject"))
                        {
                        }
                        if (GUI.Button(new Rect(500, 156 + 22 * height, 576, 1), "", "line"))
                        {
                        }
#endif
#if CONTACTIVITY
						if (GUI.Button (new Rect (500, 126 + 22 * height, 120, 40), "Activity", "actOrObject")) {
						}
						if (GUI.Button (new Rect (700, 126 + 22 * height, 120, 40), "Probability", "actOrObject")) {
						}
						if (GUI.Button (new Rect (500, 156 + 22 * height, 576, 1), "", "line")) {
						}
#endif
#if NEXTCONT
						if (GUI.Button (new Rect (500, 126 + 22 * height, 120, 40), "Next Context", "actOrObject")) {
						}
						if (GUI.Button (new Rect (700, 126 + 22 * height, 120, 40), "Probability", "actOrObject")) {
						}
						if (GUI.Button (new Rect (500, 156 + 22 * height, 576, 1), "", "line")) {
						}
#endif

#if CONTCONDITION
                        // This loop creates the actions and objects next to the activity 
                        for (int j = 0; j < simEntity.CountContextConditions(i); j++)
                        {
                            height += 1;
                            if (GUI.Button(new Rect(1050f / 1280f * Screen.width, 136 + 22 * (height), 18, 18), "", "fontMedium"))
                            { //delete button on hover
                                simEntity.RemoveContextCondition(i, j);
                            }
							if (GUI.Button(new Rect(680, 140 + 22 * (height), 9, 9), "", "fontMedium"))
                            { // action popup button
                                activityPopup = true;
                                actionheight = height;
                                activityCounter = i;
                                actionCounter = j;
                            }
							if (GUI.Button(new Rect(880, 140 + 22 * (height), 9, 9), "", "fontMedium"))
                            { // action popup button
                                objectPopup = true;
                                actionheight = height;
                                activityCounter = i;
                                actionCounter = j;
                            }
                            /* Creates action name */
                            if (GUI.Button(new Rect(500, 126 + 22 * height, 1120, 40), new GUIContent(simEntity.GetContextConditionObject(i, j), "height" + height), "activity"))
                            {
                            }
                            /* Creates object name */
                            if (GUI.Button(new Rect(700, 126 + 22 * height, 120, 40), simEntity.GetContextConditionStatus(i, j), "activity"))
                            {
                            }
                            if (GUI.Button(new Rect(500, 156 + 22 * height, 576, 1), "", "line"))
                            {
                            }
#endif

#if CONTACTIVITY
						// This loop creates the actions and objects next to the activity 
						for (int j = 0; j < simEntity.CountContextActivities(i); j++) {				
							height += 1;
							if (GUI.Button (new Rect (1050f/1280f*Screen.width, 136+ 22 * (height), 18, 18), "", "nothing")) { //delete button on hover
								simEntity.RemoveContextActivity(i,j);
							}
							if (GUI.Button (new Rect (680, 140+ 22 * (height), 9, 9), "", "nothing")) { // action popup button
								activityPopup = true;
								actionheight = height;
								activityCounter = i;
								actionCounter = j;
							}
							if (GUI.Button (new Rect (880, 140+ 22 * (height), 9, 9), "", "nothing")) { // action popup button
								objectPopup = true;
								actionheight = height;
								activityCounter = i;
								actionCounter = j;
							}
							/* Creates action name */	
							if (GUI.Button (new Rect (500, 126 + 22 * height, 1120, 40), new GUIContent (simEntity.GetContextActivityName(i,j), "height" + height), "activity")) {
							}
							/* Creates object name */	
							if (GUI.Button (new Rect (700, 126 + 22 * height, 120, 40), simEntity.GetContextActivityProb(i,j).ToString(), "activity")) {
							}
							if (GUI.Button (new Rect (500, 156 + 22 * height, 576, 1), "", "line")) {
							}
#endif
#if NEXTCONT
						// This loop creates the actions and objects next to the activity 
						for (int j = 0; j < simEntity.CountNextContexts(i); j++) {				
							height += 1;
							if (GUI.Button (new Rect (1050f/1280f*Screen.width, 136+ 22 * (height), 18, 18), "", "nothing")) { 	//delete button on hover
								simEntity.RemoveNextContext(i,j);
							}
							if (GUI.Button (new Rect (680, 140+ 22 * (height), 9, 9), "", "nothing")) { 						// next context popup button
								activityPopup = true;
								actionheight = height;
								activityCounter = i;
								actionCounter = j;
							}
							if (GUI.Button (new Rect (880, 140+ 22 * (height), 9, 9), "", "nothing")) { 						// next context probability popup button
								objectPopup = true;
								actionheight = height;
								activityCounter = i;
								actionCounter = j;
							}
							/* Creates action name */	if (GUI.Button (new Rect (500, 126 + 22 * height, 1120, 40), new GUIContent (simEntity.GetNextContextName(i,j), "height" + height), "activity")) {
							}
							/* Creates object name */	if (GUI.Button (new Rect (700, 126 + 22 * height, 120, 40), simEntity.GetNextContextProb(i,j).ToString(), "activity")) {
							}
							if (GUI.Button (new Rect (500, 156 + 22 * height, 576, 1), "", "line")) {
							}
#endif

                            if (GUI.tooltip == ("height" + (height)))
                            { // Checks for hover for action x button
                                hovering = true;
                            }
                            else
                            {
                                hovering = false;
                            }

#if CONTCONDITION
                            if (hovering && !activityPopup && !objectPopup)
                            { //if hovering bring up the action popup menu logic

                                if (GUI.Button(new Rect(1050f / 1280f * Screen.width, (136 + 22 * (height)), 18, 18), "", "x"))
                                {
                                    simEntity.RemoveContextCondition(i, j);
                                }
                                if (GUI.Button(new Rect(680, 140 + 22 * (height), 9, 9), "", "activityMenuTriangle"))
                                {
                                    activityPopup = true;
                                    actionheight = height;
                                    activityCounter = i;
                                    actionCounter = j;
                                }
                                if (GUI.Button(new Rect(880, 140 + 22 * (height), 9, 9), "", "activityMenuTriangle"))
                                {
                                    objectPopup = true;
                                    actionheight = height;
                                    activityCounter = i;
                                    actionCounter = j;
                                }
                            }
                            if (activityPopup)
                            {                                   // creates object popup menu								
                                for (int l = 0; l < stateSpaceManager.CountObjects(); l++)
                                {
                                    if (GUI.Button(new Rect(680, 180 + (22 * actionheight), 220, 22), "Cancel", "cancelPopup"))
                                    {
                                        activityPopup = false;
                                    }
                                    if (GUI.Button(new Rect(680, 202 + (22 * actionheight) + (22 * l), 220, 22), stateSpaceManager.GetObjectName(l), "actionPopup"))
                                    {
                                        simEntity.SetContextConditionObject(activityCounter, actionCounter, stateSpaceManager.GetObjectName(l));
                                        activityPopup = false;
                                    }
                                }
                            }
                            if (objectPopup)
                            {                                       // creates condition popup menu
                                for (int l = 0; l < simEntity.CountConditionStatus(); l++)
                                {
                                    if (GUI.Button(new Rect(880, 180 + (22 * actionheight), 220, 22), "Cancel", "cancelPopup"))
                                    {
                                        objectPopup = false;
                                    }
                                    if (GUI.Button(new Rect(880, 202 + (22 * actionheight) + (22 * l), 220, 22), simEntity.GetConditionStatus(l), "actionPopup"))
                                    {
                                        simEntity.SetContextConditionStatus(activityCounter, actionCounter, simEntity.GetConditionStatus(l));
                                        objectPopup = false;
                                    }
                                }
                            }
#endif
#if CONTACTIVITY
							if (hovering&&!activityPopup&&!objectPopup) { //if hovering bring up the action popup menu logic

								if (GUI.Button (new Rect (1050f/1280f*Screen.width, (136+ 22 * (height)), 18, 18), "", "x")) {
									simEntity.RemoveContextActivity(i,j);
								}
								if (GUI.Button (new Rect (680, 140+ 22 * (height), 9, 9), "", "activityMenuTriangle")) {
									activityPopup = true;
									actionheight = height;
									activityCounter = i;
									actionCounter = j;
								}
								// TODO create a text box
//								if (GUI.Button (new Rect (880, 140+ 22 * (height), 9, 9), "", "activityMenuTriangle")) {		
//									objectPopup = true;
//									actionheight = height;
//									activityCounter = i;
//									actionCounter = j;
//								}
							}
							if (activityPopup) { 				// creates activities popup menu								
								for (int l = 0; l < simEntity.CountActivities(); l++) {
									if (GUI.Button (new Rect (680, 180 + (22 * actionheight), 220, 22), "Cancel", "cancelPopup")) {
										activityPopup = false;
									}
									if (GUI.Button (new Rect (680, 202 + (22 * actionheight)+(22*l), 220, 22), simEntity.GetActivityName(l), "actionPopup")) {										
										simEntity.SetContextActivityName(activityCounter,actionCounter,simEntity.GetActivityName(l));
										activityPopup = false;
									}
								}
							}
							if (objectPopup) { 					// activate the textbox to change the value
								// TODO change the value in text box
								float newProb = 0.0f;
								simEntity.SetContextActivityProb(activityCounter,actionCounter,newProb);
							}
#endif
#if NEXTCONT
							if (hovering&&!activityPopup&&!objectPopup) { //if hovering bring up the action popup menu logic

								if (GUI.Button (new Rect (1050f/1280f*Screen.width, (136+ 22 * (height)), 18, 18), "", "x")) {
									simEntity.RemoveContextActivity(i,j);
								}
								if (GUI.Button (new Rect (680, 140+ 22 * (height), 9, 9), "", "activityMenuTriangle")) {
									activityPopup = true;
									actionheight = height;
									activityCounter = i;
									actionCounter = j;
								}
								// TODO create a text box
//								if (GUI.Button (new Rect (880, 140+ 22 * (height), 9, 9), "", "activityMenuTriangle")) {		
//									objectPopup = true;
//									actionheight = height;
//									activityCounter = i;
//									actionCounter = j;
//								}
							}
							if (activityPopup) { 				// creates action popup menu								
								for (int l = 0; l < simEntity.CountContexts(); l++) {
									if (GUI.Button (new Rect (680, 180 + (22 * actionheight), 220, 22), "Cancel", "cancelPopup")) {
										activityPopup = false;
									}
									if (GUI.Button (new Rect (680, 202 + (22 * actionheight)+(22*l), 220, 22), simEntity.GetContextName(l), "actionPopup")) {										
										simEntity.SetNextContextName(activityCounter,actionCounter,simEntity.GetContextName(l));
										activityPopup = false;
									}
								}
							}
							if (objectPopup) { 					// activate the textbox to change the value
								// TODO change the value in text box
								float newProb = 0.0f;
								simEntity.SetNextContextProb(activityCounter,actionCounter,newProb);
							}
#endif
                        }
                        height += 1;
#if CONTCONDITION
                        if (GUI.Button(new Rect(1050f / 1280f * Screen.width, 138 + 22 * (height), 18, 18), "", "plus"))
                        {   // plus button
                            simEntity.AddContextCondition(i);                                       // add a context condition							
                        }
#endif
#if CONTACTIVITY
						if (GUI.Button (new Rect (1050f/1280f*Screen.width, 138+ 22 * (height), 18, 18), "", "plus")) { 	// plus button
							simEntity.AddContextActivity (i,0.0f);									// add a context activty							
						}
#endif
#if NEXTCONT
						if (GUI.Button (new Rect (1050f/1280f*Screen.width, 138+ 22 * (height), 18, 18), "", "plus")) { 	// plus button
							simEntity.AddNextContext (i,0.0f);										// add a next context
						}
#endif
                        height += 1;
                    }

                    GUI.EndScrollView();
                }
            }
        }
		/*void DoMyWindow(int windowID) {								// creates object popup menu								
			for (int l = 0; l < stateSpaceManager.CountObjects(); l++) {
				if (GUI.Button (new Rect (680, 202 + (22 * actionheight)+(22*l), 220, 22), stateSpaceManager.GetObjectName(l), "actionPopup")) {										
					simEntity.SetContextConditionObject(activityCounter,actionCounter,stateSpaceManager.GetObjectName(l));
					activityPopup = false;
				}
			}
		}*/
    }
}
