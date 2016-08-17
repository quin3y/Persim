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


namespace UnityStandardAssets.Characters.ThirdPerson
{
public class MenuGUI : MonoBehaviour {
	// NOTE: HOW TO ADD TO NEW PROJECT
	// Comment out all code below the instaniations. Add this script to the main camera. 
	// Then with the camera this is attatched to in the Inspector,drag the GUISkin within the 
	// GUI assets folder into "skin", then drag Ethan (or whichever person Object has the script AICharacterControl) onto playlist
	public GUISkin skin;
	public List<string> NameList = new List<string>();
	public List<string> toDoList = new List<string>();
	public Vector2 scrollPosition = Vector2.zero;
	public Vector2 scrollPositionToDoList = Vector2.zero;
	public AICharacterControl Playlist;
	bool play = true;
	string timeText ="";
	public float prevTime = 1;
	bool needsPlay = false;
	string currActivity = "(No Activity)";
	bool hovver = false;
	public bool hovering = false;
	bool menu = false;
	public bool down = false;
	public float time=0;
	float hours= 0;
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


	TimeSpan timeSpan;
	DateTime todayTime; //= DateTime.Today.Add(timeSpan);
	StateSpaceManager stateSpaceManager;	
	SimulationEntity simEntity;

	
	void Start()
		{
			// Not sure why this loop for making the activity nameList doesn't work. Maybe it runs before the other list is initiated?
			/*for (int i = 0;i<Playlist.activityPlayback.activities.Count;i++  ){
				NameList.Add (Playlist.activityPlayback.activities[i].name);
			}*/
			Screen.SetResolution(1280, 800, false);
			stateSpaceManager = GameObject.Find("Camera").GetComponent<StateSpaceManager>();
			simEntity = GameObject.Find("Ethan").GetComponent<SimulationEngine>().SimEntity;

		}

		void Update()
		{

			// Logic for time in the top right
			timeSpan = TimeSpan.FromSeconds(time);
			DateTime todayTime = DateTime.Today.Add(stateSpaceManager.startTime.Add(TimeSpan.FromSeconds(Mathf.Round(Time.time))));
			timeText = todayTime.ToString("hh:mm:ss tt");
			for (int i = 0;i<Playlist.activityPlayback.activities.Count;i++  ){
				NameList.Add (Playlist.activityPlayback.activities[i].name);

			}

		}
			
	void OnGUI () {

		
		// makes the GUI skin the set skin. Also provides logic for the play button
		GUI.skin = skin;
		string hover = GUI.tooltip;
			/*hours + ":" + timer + " AM  " + currActivity*/

		if (!menu) {  //Only show this GUI if the main menu is not up
				if (Playlist.playlist.Count () == 0&& Playlist.activityPlayback.actionQueue.Count == 0) {
				needsPlay = true;
			}
		
				//Activity box
				GUI.Box (new Rect (Screen.width - 218, 70, 215, 518), "");
				//Up next Panel
				GUI.Label (new Rect (3, 10, 200, 30), "  Up Next: " + Playlist.playlist.Count () + " Activities");
				//The panel behind the time and play buttons
				GUI.Label (new Rect (Screen.width-219, 10, 216, 55), "", "timer");
				//prints the Time. SHould be changed to TimeSPan
				GUI.Button (new Rect (Screen.width-253, 15, 242, 30), timeText +"     "+ currActivity, "nothing");
				GUI.Label (new Rect (Screen.width-65, 42, 201, 30), "x"+Time.timeScale, "nothing");

				//Clear Button
				if (GUI.Button (new Rect (160, 17, 30, 14), "", "clear")) {
					Playlist.playlist.Clear ();
					toDoList.Clear ();
				}
		
				//Creates Scroll view box of buttons of the activities
				int scrollViewHeight = 42 + Playlist.activityPlayback.activities.Count * 36;
				scrollPosition = GUI.BeginScrollView (new Rect (Screen.width - 220, 100, 230, 460), scrollPosition, new Rect (Screen.width - 110, 40, 200, scrollViewHeight));
				for (int i = 0; i < Playlist.activityPlayback.activities.Count; i++) {
					if (GUI.Button (new Rect (Screen.width - 90, 52 + 36 * i, 186, 30), Playlist.activityPlayback.activities[i].name)) {
						toDoList.Add (Playlist.activityPlayback.activities[i].name);
						Playlist.playlist.AddActivity (i);
//						Debug.Log ("i is " +i);
					}
				}
				GUI.EndScrollView ();

				// Creates the top left playlist of deletable activities
				int scrollViewHeightToDoList = 48 + toDoList.Count * 32;
				scrollPositionToDoList = GUI.BeginScrollView (new Rect (3, 44, 230, 325), scrollPositionToDoList, new Rect (3, 44, 200, scrollViewHeightToDoList));
				for (int i = 0; i < toDoList.Count; i++) {
					if (GUI.Button (new Rect (174, 48 + 32 * (i), 18, 18), "", "x")) {					// The x button to delete an activty after hovering
						toDoList.RemoveAt (i);
						Playlist.playlist.DeleteActivity (i);
					}
					//The Label of the toDoList activity
					GUI.Label (new Rect (3, 44 + 32 * i, 200, 30), new GUIContent (toDoList [i], "h" + i), "toDoList");
					if (GUI.tooltip == ("h" + (i))) {  //Checks for hovering for bringing the x up on the button
						hovver = true;
					} else {
						hovver = false;
					}
					if (hovver) {

						if (GUI.Button (new Rect (174, 48 + 32 * (i), 18, 18), "", "x")) {
							toDoList.RemoveAt (i);
							Playlist.playlist.DeleteActivity (i);
						}
					}
					if (Playlist.playlist.popped == true) {    //When the activty Playlist pops something it changes the GUI list to match
						if(toDoList.Count>0){
							currActivity = toDoList [0];
							toDoList.RemoveAt (0);
						}
						Playlist.playlist.popped = false;
					}
				}
				GUI.EndScrollView ();
					
				// Play button, speed up, slow down , redo Logic
				if (play) {
					if (GUI.Button (new Rect (Screen.width-149, 37, 40, 27), "", "Button2")) {
						if (!needsPlay) {
							Time.timeScale = prevTime;
							play = false;
						}
						// IF NOTHING PLAYING
						if (needsPlay) {
							if (Time.timeScale == 0) {
								Time.timeScale = prevTime;
							}
							Playlist.PlayActivity (NameList.IndexOf(toDoList [0]));
							Playlist.playlist.Pop();
							play = false;
							needsPlay = false;
						}
					}
				}
				if (!play) {
					if (GUI.Button (new Rect (Screen.width-149, 38, 41, 28), "", "pause")) {
						prevTime = Time.timeScale;
						Time.timeScale = 0;
						play = true;
					}
				}
				if (GUI.Button (new Rect (Screen.width-110, 37, 40, 27), "", "fastf")) {
					if (Time.timeScale < 64) {
						Time.timeScale = Time.timeScale * 2;
					} else
						Time.timeScale = 64;
				}
				if (GUI.Button (new Rect (Screen.width-188, 37, 40, 27), "", "slowd")) {
					if (Time.timeScale > 1) {
						Time.timeScale = Time.timeScale / 2;
					}
				}
				if (GUI.Button (new Rect (Screen.width-72, 37, 40, 27), "", "redo")) {
					toDoList.Insert (0, toDoList [0]);
					Playlist.playlist.RepeatCurrentActivity (NameList.IndexOf (toDoList [0]));
				}
				if (GUI.Button (new Rect (Screen.width - 29, 562, 24, 24), "", "Gear")) {
					menu = true;
				}
		}

			//Main Menu
			if (menu) {
				GUI.Box(new Rect (0, 0, Screen.width, Screen.height), "", "mainMenu");
				if (GUI.Button (new Rect (55f/1280f*Screen.width, 700f/800f*Screen.height, 150f/1280f*Screen.width, 50f/800f*Screen.height), "Back","menuButton")) {
					menu = !menu;
				}
				if (GUI.Button (new Rect ((60f/1280f)*Screen.width, (140f/800f)*Screen.height, (150f/1280f)*Screen.width, (50f/800f)*Screen.height), "Characters","menuButton")) {
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
/* Creates action name */	if (GUI.Button (new Rect (500, 126 + 22 * height, 1120, 40), new GUIContent (Playlist.activityPlayback.actions [Playlist.activityPlayback.activities [i].actionIds [j]].name, "height" + height), "activity")) {
							}
/* Creates object name */	if (GUI.Button (new Rect (700, 126 + 22 * height, 120, 40), Playlist.activityPlayback.activities[i].objectNames[j], "activity")) {
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
							/* Creates action name */	if (GUI.Button (new Rect (500, 126 + 22 * height, 1120, 40), new GUIContent (simEntity.GetContextConditionObject(i,j), "height" + height), "activity")) {
							}
							/* Creates object name */	if (GUI.Button (new Rect (700, 126 + 22 * height, 120, 40), simEntity.GetContextConditionStatus(i,j), "activity")) {
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
			}

		}
	}
}