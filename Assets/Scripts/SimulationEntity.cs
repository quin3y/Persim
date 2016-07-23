using UnityEngine;
using System;
using System.Xml;
using System.Collections.Generic;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public static class SimulationEntity {
		public static List<ObjectInfo> Objects = new List<ObjectInfo> (); 				// a set of objects
		public static List<Action> Actions = new List<Action> ();						// a set of actions
		public static List<Activity> Activities = new List<Activity> ();				// a set of activities
		public static ContextGraph ContextGraph = new ContextGraph();					// context graph

		public static Context CurContext;
		public static int StartContextId { get; set; }									// starting context id
		public static int EndContextId { get; set; }									// ending context id

		public static List<int> ScheduledActivities = new List<int>();					// list of performed activities

		// read object models
		public static void ReadObjectXml() {
			XmlReader reader = XmlReader.Create("Assets/Files/objects.xml");

			while (reader.Read()) {
				if (reader.NodeType == XmlNodeType.Element && reader.Name == "object") {
					ObjectInfo obj = new ObjectInfo();
					obj.id = Int32.Parse(reader.GetAttribute(0));
					obj.name = reader.GetAttribute(1);
					obj.status = reader.GetAttribute (2);

					while (reader.NodeType != XmlNodeType.EndElement) {
						reader.Read();
						if (reader.Name == "position") {
							obj.position = new Vector3(float.Parse(reader.GetAttribute(0)),
								float.Parse(reader.GetAttribute(1)),
								float.Parse(reader.GetAttribute(2)));
						}
						else if (reader.Name == "rotation") {
							obj.rotation = new Vector3(float.Parse(reader.GetAttribute(0)),
								float.Parse(reader.GetAttribute(1)),
								float.Parse(reader.GetAttribute(2)));
						}
						else if (reader.Name == "in-hand-position") {
							obj.inHandPosition = new Vector3(float.Parse(reader.GetAttribute(0)),
								float.Parse(reader.GetAttribute(1)),
								float.Parse(reader.GetAttribute(2)));
						}
						else if (reader.Name == "in-hand-rotation") {
							obj.inHandRotation = new Vector3(float.Parse(reader.GetAttribute(0)),
								float.Parse(reader.GetAttribute(1)),
								float.Parse(reader.GetAttribute(2)));
						}
						else if (reader.Name == "character-position") {
							obj.characterPosition = new Vector3(float.Parse(reader.GetAttribute(0)),
								float.Parse(reader.GetAttribute(1)),
								float.Parse(reader.GetAttribute(2)));
						}
						else if (reader.Name == "character-rotation") {
							obj.characterRotation = new Vector3(float.Parse(reader.GetAttribute(0)),
								float.Parse(reader.GetAttribute(1)),
								float.Parse(reader.GetAttribute(2)));
						}
					}

					SimulationEntity.Objects.Add(obj);
				}
			}
			reader.Close();
		}

		// read context models
		public static void ReadContextXml() {
			XmlReader reader = XmlReader.Create("Assets/Files/contextgraph.xml");

			while (reader.Read()) {					
				if (reader.IsStartElement()) {
					switch (reader.LocalName.ToString ()) {
					case "context":						
						Context context = new Context (int.Parse (reader.GetAttribute ("id")), reader.GetAttribute ("name"));
						CurContext = context;
						SimulationEntity.ContextGraph.AddContext (context);
						if (reader.GetAttribute ("status") == "start")
							StartContextId = int.Parse (reader.GetAttribute ("id"));
						else if (reader.GetAttribute ("status") == "end")
							EndContextId = int.Parse (reader.GetAttribute ("id"));
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

			CurContext = SimulationEntity.ContextGraph.GetContext (StartContextId);
			reader.Close();
		}

		// return an object with the name
		public static ObjectInfo GetObject(string name) {
			foreach (ObjectInfo obj in Objects) {
				if (obj.name == name) {
					return obj;
				}
			}

			return null;
		}

		// return an activity with id
		public static Activity GetActivity(int id) {
			if (id < Activities.Count)
				return Activities [id];
			else
				return null;
		}

		// return an activity with id
		public static Context GetContext(int id) {
			if (id < ContextGraph.CountContexts())
				return ContextGraph.GetContext(id);
			else
				return null;
		}

		// check whether simulation can end or not
		public static bool IsEnd() {
			if (CurContext.ID == EndContextId)
				return true;
			else 
				return false;
		}
	}
}

