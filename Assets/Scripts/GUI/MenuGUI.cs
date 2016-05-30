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
	int actionheight = 0;
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
	
	void Start()
		{
			// Not sure why this loop for making the activity nameList doesn't work. Maybe it runs before the other list is initiated?
			/*Debug.Log (Playlist.activityPlayback.activities.Count);
			for (int i = 0;i<Playlist.activityPlayback.activities.Count;i++  ){
				NameList.Add (Playlist.activityPlayback.activities[i].name);

			}
			NameList.Add("Brushing Teeth");
			NameList.Add("Bathing");
			NameList.Add("Cleaning countertops");
			NameList.Add("Combing hair");
			NameList.Add("Doing laundry");
			NameList.Add("Dressing");
			NameList.Add("Drinking water");
			NameList.Add("Eating a meal");
			NameList.Add("Falling");
			NameList.Add("Getting up");
			NameList.Add("Going to bed");
			NameList.Add("Leaving home");
			NameList.Add("Preparing a meal");
			NameList.Add("Taking medication");
			NameList.Add("Taking out trash");
			NameList.Add("Undressing");
			NameList.Add("Using bathroom");
			NameList.Add("Using cellphone");
			NameList.Add("Using computer");
			NameList.Add("Vacuuming floors");
			NameList.Add("Washing dishes");
			NameList.Add("Washing face");
			NameList.Add("Washing hands");
			NameList.Add("Watching TV");
			NameList.Add("Patrol");*/
			//NameList.Sort ();
			Screen.SetResolution(1280, 800, true);
		}

		void Update()
		{

			// Logic for time in the top right
			timeSpan = TimeSpan.FromSeconds(time);
			DateTime todayTime = DateTime.Today.Add(timeSpan);
			timeText = todayTime.ToString("hh:mm tt");

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
						Debug.Log ("i is " +i);
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
							Playlist.PlayActivity (NameList.IndexOf (toDoList [0]));
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
					Time.timeScale = Time.timeScale/2;
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
				if (GUI.Button (new Rect (55, Screen.height - 100, 150, 50), "Back","menuButton")) {
					menu = !menu;
				}
				if (GUI.Button (new Rect (60, 140, 150, 50), "Characters","menuButton")) {
					charScreenUp = true;
					objectScreenUp = false;
					actionScreenUp = false;
					activityScreenUp = false;
					contextsScreenUp = false;
				}
				if (GUI.Button (new Rect (60, 200, 150, 50), "Objects","menuButton")) {
					charScreenUp = false;
					objectScreenUp = true;
					actionScreenUp = false;
					activityScreenUp = false;
					contextsScreenUp = false;
				}
				if (GUI.Button (new Rect (60, 260, 150, 50), "Actions","menuButton")) {
					charScreenUp = false;
					objectScreenUp = false;
					actionScreenUp = true;
					activityScreenUp = false;
					contextsScreenUp = false;
				}
				if (GUI.Button (new Rect (60, 320, 150, 50), "Activities","menuButton")) {
					charScreenUp = false;
					objectScreenUp = false;
					actionScreenUp = false;
					activityScreenUp = true;
					contextsScreenUp = false;
				}
				if (GUI.Button (new Rect (60, 380, 150, 50), "Contexts","menuButton")) {
					charScreenUp = false;
					objectScreenUp = false;
					actionScreenUp = false;
					activityScreenUp = false;
					contextsScreenUp = true;
				}
				if (charScreenUp){
					GUI.Box (new Rect ( 500, 180, 100, 210), "", "char1");
					GUI.Box (new Rect ( 650, 180, 100, 210), "", "char1");
					GUI.Box (new Rect (500, 400, 100, 210), "", "char1");
					GUI.Box (new Rect ( 650, 400, 100, 210), "", "char1");
				}
				if (objectScreenUp) {
					int scrollViewHeightObj = 126 + Playlist.activityPlayback.GetObjectList().Count * 46;
					scrollPosition = GUI.BeginScrollView(new Rect (Screen.width - 815, 126, 340, 480), scrollPosition, new Rect (Screen.width-405, 126, 320, scrollViewHeightObj));
					for (int i = 0;i<Playlist.activityPlayback.GetObjectList().Count;i++  ){
						if (GUI.Button (new Rect (Screen.width - 405, 126+47*i, 320, 40), Playlist.activityPlayback.GetObjectList ()[i] , "objectButton")) {
						}
					}
					GUI.EndScrollView ();
				}
				if (actionScreenUp) {
					int scrollViewHeightAction = 126 +Playlist.activityPlayback.actions.Count * 46;
					scrollPosition = GUI.BeginScrollView(new Rect (Screen.width - 815, 126, 340, 480), scrollPosition, new Rect (Screen.width-405, 126, 320, scrollViewHeightAction));
					for (int i = 0;i<Playlist.activityPlayback.actions.Count;i++  ){
						if (GUI.Button (new Rect (Screen.width - 405, 126+47*i, 320, 40), Playlist.activityPlayback.actions[i].name, "objectButton")) {
						}
					}

					GUI.EndScrollView ();
				}
				if (activityScreenUp) {
					int height = 0;
					int scrollViewHeightActivity = 126 +Playlist.activityPlayback.activities.Count*6 * 66;
					scrollPosition = GUI.BeginScrollView(new Rect (220, 126, 980, 480), scrollPosition, new Rect (220, 126, 320, scrollViewHeightActivity));
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
							if (GUI.Button (new Rect (1050, 136+ 22 * (height), 18, 18), "", "nothing")) { //delete button on hover
								Playlist.activityPlayback.activities [i].DeleteAction (j);
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

								if (GUI.Button (new Rect (1050, 136+ 22 * (height), 18, 18), "", "x")) {
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
									if (GUI.Button (new Rect (680, 104 + (22 * actionheight), 220, 22), "Cancel", "cancelPopup")) {
										activityPopup = false;
									}
									if (GUI.Button (new Rect (680, 126 + (22 * actionheight)+(22*l), 220, 22), Playlist.activityPlayback.actions[l].name, "actionPopup")) {
										Playlist.activityPlayback.activities [activityCounter].actionIds [actionCounter] = l;
										activityPopup = false;
									}
								}
							}
							if (objectPopup) { // creates object popup menu
								for (int l = 0; l < Playlist.activityPlayback.GetObjectList().Count; l++) {
									if (GUI.Button (new Rect (880, 104 + (22 * actionheight), 220, 22), "Cancel", "cancelPopup")) {
										objectPopup = false;
									}
									if (GUI.Button (new Rect (880, 126 + (22 * actionheight)+(22*l), 220, 22), Playlist.activityPlayback.GetObjectList ()[l], "actionPopup")) {
										Playlist.activityPlayback.activities[activityCounter].objectNames[actionCounter] = Playlist.activityPlayback.GetObjectList ()[l];
										objectPopup = false;
									}
								}
							}

						}
						height += 1;
						if (GUI.Button (new Rect (1050, 138+ 22 * (height), 18, 18), "", "plus")) { // plus button
							Playlist.activityPlayback.activities [i].AddAction();
						}
						height += 1;
					}
						
					GUI.EndScrollView ();
				}
				if (contextsScreenUp) {
				}
			}



		
	}
}
}