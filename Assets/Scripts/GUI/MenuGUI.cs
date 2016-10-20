using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class MenuGUI : MonoBehaviour {
		public GUISkin skin;

		float actionheight = 0;
		int activityCounter = 0;
		int actionCounter = 0;
		bool activityPopup = false;
		bool objectPopup = false;
		bool charScreenUp = true;
		bool objectScreenUp = false;
		bool actionScreenUp = false;
		bool activityScreenUp = false;
		bool contextsScreenUp = false;


		bool menuUp;
		
		// Use this for initialization
		void Start() {

		}
		
		// Update is called once per frame
		void Update() {
		
		}

		void OnGUI() {
			GUI.skin = skin;
			menuUp = GameObject.Find("Camera").GetComponent<MainScreenGUI>().menu;

			if (menuUp) {
				GUI.Box(new Rect (0, 0, Screen.width, Screen.height), "", "mainMenu");
				if (GUI.Button (new Rect (55f/1280f*Screen.width, 700f/800f*Screen.height, 150f/1280f*Screen.width, 50f/800f*Screen.height), "Back","menuButton")) {
					GameObject.Find("Camera").GetComponent<MainScreenGUI>().menu = !menuUp;
					print(GameObject.Find("Camera").GetComponent<MainScreenGUI>().menu);
				}
			/*		if (GUI.Button (new Rect ((60f/1280f)*Screen.width, (140f/800f)*Screen.height, (150f/1280f)*Screen.width, (50f/800f)*Screen.height), "Characters","menuButton")) {
						charScreenUp = true;
						objectScreenUp = false;
						actionScreenUp = false;
						activityScreenUp = false;
						contextsScreenUp = false;
					}
					if (GUI.Button (new Rect ((60f/1280f)*Screen.width, (200f/800f)*Screen.height, (150f/1280f)*Screen.width, (50f/800f)*Screen.height), "Objects","menuButton")) {
						charScreenUp = false;
						objectScreenUp = true;
						actionScreenUp = false;
						activityScreenUp = false;
						contextsScreenUp = false;
					}
					if (GUI.Button (new Rect ((60f/1280f)*Screen.width, (260f/800f)*Screen.height, (150f/1280f)*Screen.width, (50f/800f)*Screen.height), "Actions","menuButton")) {
						charScreenUp = false;
						objectScreenUp = false;
						actionScreenUp = true;
						activityScreenUp = false;
						contextsScreenUp = false;
					}
					if (GUI.Button (new Rect ((60f/1280f)*Screen.width, (320f/800f)*Screen.height, (150f/1280f)*Screen.width, (50f/800f)*Screen.height), "Activities","menuButton")) {
						charScreenUp = false;
						objectScreenUp = false;
						actionScreenUp = false;
						activityScreenUp = true;
						contextsScreenUp = false;
					}
					if (GUI.Button (new Rect ((60f/1280f)*Screen.width, (380f/800f)*Screen.height, (150f/1280f)*Screen.width, (50f/800f)*Screen.height), "Contexts","menuButton")) {
						charScreenUp = false;
						objectScreenUp = false;
						actionScreenUp = false;
						activityScreenUp = false;
						contextsScreenUp = true;
					}
					if (charScreenUp){
						GUI.Box (new Rect ( 500f/1280f*Screen.width, 180f/800f*Screen.height, 140f/1280f*Screen.width, 210f/800f*Screen.height), "", "char1");
						GUI.Box (new Rect ( 650f/1280f*Screen.width, 180f/800f*Screen.height, 140f/1280f*Screen.width, 210f/800f*Screen.height), "", "char1");
						GUI.Box (new Rect (500f/1280f*Screen.width, 400f/800f*Screen.height, 140f/1280f*Screen.width, 210f/800f*Screen.height), "", "char1");
						GUI.Box (new Rect ( 650f/1280f*Screen.width, 400f/800f*Screen.height, 140f/1280f*Screen.width, 210f/800f*Screen.height), "", "char1");
					}
					if (objectScreenUp) {
						int scrollViewHeightObj = 126 + Playlist.activityPlayback.GetObjectList().Count * 46;
						scrollPosition = GUI.BeginScrollView(new Rect (300f/1280f*Screen.width, 160f/800f*Screen.height, 440, 580f/800f*Screen.height), scrollPosition, new Rect (Screen.width-405, 126, 320, scrollViewHeightObj));
						for (int i = 0;i<Playlist.activityPlayback.GetObjectList().Count;i++  ){
							if (GUI.Button (new Rect (Screen.width - 405, 126+47*i, 320, 40), Playlist.activityPlayback.GetObjectList ()[i] , "objectButton")) {
							}
						}
						GUI.EndScrollView ();
					}
					if (actionScreenUp) {
						int scrollViewHeightAction = 126 +Playlist.activityPlayback.actions.Count * 46;
						scrollPosition = GUI.BeginScrollView(new Rect (300f/1280f*Screen.width, 160f/800f*Screen.height, 440, 580f/800f*Screen.height), scrollPosition, new Rect (Screen.width-405, 126, 320, scrollViewHeightAction));
						for (int i = 0;i<Playlist.activityPlayback.actions.Count;i++  ){
							if (GUI.Button (new Rect (Screen.width - 405, 126+47*i, 320, 40), Playlist.activityPlayback.actions[i].name, "objectButton")) {
							}
						}

						GUI.EndScrollView ();
					}
					if (activityScreenUp) {
						float height = 0;
						int scrollViewHeightActivity = 126 +Playlist.activityPlayback.activities.Count*6 * 66;
						scrollPosition = GUI.BeginScrollView(new Rect (260f/1280f*Screen.width, 160f/800f*Screen.height, 900f/1280f*Screen.width, 580f/800f*Screen.height), scrollPosition, new Rect (220, 126, 320, scrollViewHeightActivity));
						for (int i = 0; i < Playlist.activityPlayback.activities.Count; i++) {
							// Displays the activty name and the action and object columns
							if (GUI.Button (new Rect (220, 126 + 22 * height, 120, 40), Playlist.activityPlayback.activities [i].name, "activity")) {
							}
							if (GUI.Button (new Rect (500, 126 + 22 * height, 120, 40), "Action", "actOrObject")) {
							}
							if (GUI.Button (new Rect (700, 126 + 22 * height, 120, 40), "Object", "actOrObject")) {
							}
							if (GUI.Button (new Rect (500, 156 + 22 * height, 576, 1), "", "line")) {
							}
							// This loop creates the actions and objects next to the activity 
							for (int j = 0; j < Playlist.activityPlayback.activities [i].objectNames.Count; j++) {
								height += 1;
								if (GUI.Button (new Rect (1050f/1280f*Screen.width, 136+ 22 * (height), 18, 18), "", "nothing")) { //delete button on hover
									Playlist.activityPlayback.activities [i].DeleteAction (j);
								}
								if (GUI.Button (new Rect (680, 140+ 22 * (height), 9, 9), "", "nothing")) { // action popup 
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
								// Creates action name
								if (GUI.Button (new Rect (500, 126 + 22 * height, 1120, 40), new GUIContent (Playlist.activityPlayback.actions [Playlist.activityPlayback.activities [i].actionIds [j]].name, "height" + height), "activity")) {
								}
								// Creates object name
								if (GUI.Button (new Rect (700, 126 + 22 * height, 120, 40), Playlist.activityPlayback.activities[i].objectNames[j], "activity")) {
								}
								if (GUI.Button (new Rect (500, 156 + 22 * height, 576, 1), "", "line")) {
								}
								if (GUI.tooltip == ("height" + (height))) { // Checks for hover for action x button
									hovering = true;
								} else {
									hovering = false;
								}
								if (hovering&&!activityPopup&&!objectPopup) { //if hovering bring up the action popup menu logic

									if (GUI.Button (new Rect (1050f/1280f*Screen.width, (136+ 22 * (height)), 18, 18), "", "x")) {
										Playlist.activityPlayback.activities [i].DeleteAction (j);
									}
									if (GUI.Button (new Rect (680, 140+ 22 * (height), 9, 9), "", "activityMenuTriangle")) {
										activityPopup = true;
										actionheight = height;
										activityCounter = i;
										actionCounter = j;
									}
									if (GUI.Button (new Rect (880, 140+ 22 * (height), 9, 9), "", "activityMenuTriangle")) {
										objectPopup = true;
										actionheight = height;
										activityCounter = i;
										actionCounter = j;
									}
								}
								if (activityPopup) { // creates action popup menu
									for (int l = 0; l < Playlist.activityPlayback.actions.Count; l++) {
										if (GUI.Button (new Rect (680, 180 + (22 * actionheight), 220, 22), "Cancel", "cancelPopup")) {
											activityPopup = false;
										}
										if (GUI.Button (new Rect (680, 202 + (22 * actionheight)+(22*l), 220, 22), Playlist.activityPlayback.actions[l].name, "actionPopup")) {
											Playlist.activityPlayback.activities [activityCounter].actionIds [actionCounter] = l;
											activityPopup = false;
										}
									}
								}
								if (objectPopup) { // creates object popup menu
									for (int l = 0; l < Playlist.activityPlayback.GetObjectList().Count; l++) {
										if (GUI.Button (new Rect (880, 180 + (22 * actionheight), 220, 22), "Cancel", "cancelPopup")) {
											objectPopup = false;
										}
										if (GUI.Button (new Rect (880, 202 + (22 * actionheight)+(22*l), 220, 22), Playlist.activityPlayback.GetObjectList ()[l], "actionPopup")) {
											Playlist.activityPlayback.activities[activityCounter].objectNames[actionCounter] = Playlist.activityPlayback.GetObjectList ()[l];
											objectPopup = false;
										}
									}
								}

							}
							height += 1;
							if (GUI.Button (new Rect (1050f/1280f*Screen.width, 138+ 22 * (height), 18, 18), "", "plus")) { // plus button
								Playlist.activityPlayback.activities [i].AddAction();
							}
							height += 1;
						}

						GUI.EndScrollView ();
					}

					if (contextsScreenUp) {
						float height = 0;
						int scrollViewHeightActivity = 126 + simEntity.CountActivities() * 6 * 66;
						scrollPosition = GUI.BeginScrollView(new Rect (260f/1280f*Screen.width, 160f/800f*Screen.height, 900f/1280f*Screen.width, 580f/800f*Screen.height), scrollPosition, new Rect (220, 126, 320, scrollViewHeightActivity));
						for (int i = 0; i < simEntity.CountContexts(); i++) {
							// Displays the context name and three columns for object conditions, activities, and next contexts
							if (GUI.Button (new Rect (220, 126 + 22 * height, 120, 40), simEntity.GetContextName(i), "activity")) { 
							}
							#if CONTCONDITION
							if (GUI.Button (new Rect (500, 126 + 22 * height, 120, 40), "Object", "actOrObject")) {
							}
							if (GUI.Button (new Rect (700, 126 + 22 * height, 120, 40), "Condition", "actOrObject")) {
							}
							if (GUI.Button (new Rect (500, 156 + 22 * height, 576, 1), "", "line")) {
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
							for (int j = 0; j < simEntity.CountContextConditions(i); j++) {				
							height += 1;
							if (GUI.Button (new Rect (1050f/1280f*Screen.width, 136+ 22 * (height), 18, 18), "", "nothing")) { //delete button on hover
							simEntity.RemoveContextCondition(i,j);
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
							// Creates action name
							if (GUI.Button (new Rect (500, 126 + 22 * height, 1120, 40), new GUIContent (simEntity.GetContextConditionObject(i,j), "height" + height), "activity")) {
							}
							//
							if (GUI.Button (new Rect (700, 126 + 22 * height, 120, 40), simEntity.GetContextConditionStatus(i,j), "activity")) {
							}
							if (GUI.Button (new Rect (500, 156 + 22 * height, 576, 1), "", "line")) {
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
							// Creates action name	
							if (GUI.Button (new Rect (500, 126 + 22 * height, 1120, 40), new GUIContent (simEntity.GetContextActivityName(i,j), "height" + height), "activity")) {
							}
							// Creates object name
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
							// Creates action name	
							if (GUI.Button (new Rect (500, 126 + 22 * height, 1120, 40), new GUIContent (simEntity.GetNextContextName(i,j), "height" + height), "activity")) {
							}
							// Creates object name	if (GUI.Button (new Rect (700, 126 + 22 * height, 120, 40), simEntity.GetNextContextProb(i,j).ToString(), "activity")) {
							}
							if (GUI.Button (new Rect (500, 156 + 22 * height, 576, 1), "", "line")) {
							}
							#endif

							if (GUI.tooltip == ("height" + (height))) { // Checks for hover for action x button
								hovering = true;
							} else {
								hovering = false;
							}

							#if CONTCONDITION
							if (hovering&&!activityPopup&&!objectPopup) { //if hovering bring up the action popup menu logic

							if (GUI.Button (new Rect (1050f/1280f*Screen.width, (136+ 22 * (height)), 18, 18), "", "x")) {
							simEntity.RemoveContextCondition(i,j);
							}
							if (GUI.Button (new Rect (680, 140+ 22 * (height), 9, 9), "", "activityMenuTriangle")) {
							activityPopup = true;
							actionheight = height;
							activityCounter = i;
							actionCounter = j;
							}
							if (GUI.Button (new Rect (880, 140+ 22 * (height), 9, 9), "", "activityMenuTriangle")) {
							objectPopup = true;
							actionheight = height;
							activityCounter = i;
							actionCounter = j;
							}
							}
							if (activityPopup) { 									// creates object popup menu								
							for (int l = 0; l < stateSpaceManager.CountObjects(); l++) {
							if (GUI.Button (new Rect (680, 180 + (22 * actionheight), 220, 22), "Cancel", "cancelPopup")) {
							activityPopup = false;
							}
							if (GUI.Button (new Rect (680, 202 + (22 * actionheight)+(22*l), 220, 22), stateSpaceManager.GetObjectName(l), "actionPopup")) {										
							simEntity.SetContextConditionObject(activityCounter,actionCounter,stateSpaceManager.GetObjectName(l));
							activityPopup = false;
							}
							}
							}
							if (objectPopup) { 										// creates condition popup menu
							for (int l = 0; l < simEntity.CountConditionStatus(); l++) {
							if (GUI.Button (new Rect (880, 180 + (22 * actionheight), 220, 22), "Cancel", "cancelPopup")) {
							objectPopup = false;
							}
							if (GUI.Button (new Rect (880, 202 + (22 * actionheight)+(22*l), 220, 22), simEntity.GetConditionStatus(l), "actionPopup")) {
							simEntity.SetContextConditionStatus(activityCounter,actionCounter,simEntity.GetConditionStatus(l));
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
						if (GUI.Button (new Rect (1050f/1280f*Screen.width, 138+ 22 * (height), 18, 18), "", "plus")) { 	// plus button
						simEntity.AddContextCondition (i);										// add a context condition							
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

					GUI.EndScrollView ();
				}
			}*/
			}
		}
	}
}
