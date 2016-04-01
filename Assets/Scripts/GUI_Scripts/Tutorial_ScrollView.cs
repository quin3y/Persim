using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


namespace UnityStandardAssets.Characters.ThirdPerson
{
	
	public class Tutorial_ScrollView : MonoBehaviour{

	public GameObject Button_Template;
	public List<string> NameList = new List<string>();
	public ActivityPlaylist playlist;


	// Use this for initialization
	void Start () {
	
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
		NameList.Add("Patrol");
		NameList.Sort ();

		foreach(string str in NameList)
		{
			GameObject go = Instantiate(Button_Template) as GameObject;
			go.SetActive(true);
			Tutorial_Button TB = go.GetComponent<Tutorial_Button>();
			TB.SetName(str);
			go.transform.SetParent(Button_Template.transform.parent);

		}


	}
	
	public void ButtonClicked(string str)
	{
		Debug.Log(str + " button clicked.");

	}
	}
}
