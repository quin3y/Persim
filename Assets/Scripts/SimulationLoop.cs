using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class SimulationLoop : MonoBehaviour {
		public static Dictionary<string, ObjectInfo> objects;
		public static List<Action> actions;
		public static List<Activity> activities;
		public static List<Context> contexts;


		void Start() {
			print("starting simulation loop");

		}

		void Update() {
			print("========");
		}
			
		public SimulationLoop()
		{
			objects = Utils.ReadObjectXml();
			actions = Utils.ReadActionXml();
			activities = Utils.ReadActivityXml();
//			contexts = null;
		}

//		public void Init ()
//		{
//		}

	}
}

