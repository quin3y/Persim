using UnityEngine;
using System;
using System.Xml;
using System.Collections.Generic;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public static class SimulationEntity {
		public static Dictionary<string, ObjectInfo> Objects = new Dictionary<string, ObjectInfo> (); 	// a set of objects
		public static List<Action> Actions = new List<Action> ();										// a set of actions
		public static List<Activity> Activities = new List<Activity> ();								// a set of activities
		public static ContextGraph ContextGraph = new ContextGraph();									// context graph

		public static Context CurContext;
		public static ActivityPlaylist ActivityPlayList = new ActivityPlaylist ();

		public static void ReadContextXml() {
			XmlReader reader = XmlReader.Create("Assets/Files/contextgraph.xml");

			while (reader.Read()) {					
				if (reader.IsStartElement()) {
					switch (reader.LocalName.ToString ()) {
					case "context":						
						Context context = new Context (int.Parse (reader.GetAttribute ("id")), reader.GetAttribute ("name"));
						CurContext = context;
						SimulationEntity.ContextGraph.AddContext (context);
						break;
					case "contextcondition":
						CurContext.AddContextCondition (reader.GetAttribute ("object"), reader.GetAttribute ("status"));
						break;
					case "contextactivity":						
						CurContext.AddContextActivity (int.Parse(reader.GetAttribute ("id")));
						break;
					case "nextcontext":						
						CurContext.AddNextContext (int.Parse(reader.GetAttribute("id")),float.Parse(reader.GetAttribute("prob")));
						break;
					}				
				}
			}

			CurContext = SimulationEntity.ContextGraph.GetContext (0);
			reader.Close();
		}

		public static Activity GetActivity(int id) {
			if (id < Activities.Count)
				return Activities [id];
			else
				return null;
		}

		public class StateSpace {
			public Dictionary<string, ObjectInfo> objects = new Dictionary<string, ObjectInfo> ();

			public void SetStateSpace() {
				this.objects = SimulationEntity.Objects;
			}

			public void UpdateStateSpace(string objName, string status) {
				
			}
		}
	}
}

