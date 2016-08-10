using UnityEngine;
using System;
using System.Xml;
using System.Collections.Generic;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class SimulationEntity {
		public static List<ObjectInfo> Objects = new List<ObjectInfo> (); 				// static varaible: a set of objects
		List<Action> actions = new List<Action> ();										// a set of actions
		List<Activity> activities = new List<Activity> ();								// a set of activities
		ContextGraph contextGraph = new ContextGraph();									// context graph

		Context curContext;
		int startContextId;																// starting context id
		int endContextId;																// ending context id

		List<int> scheduledActivities = new List<int>();								// list of performed activities

		// static method: read object models
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

					Objects.Add(obj);
				}
			}
			reader.Close();
		}

		// manage actions
		public List<Action> Actions {
			get { return actions; }
			set { actions = value; }
		}

		// manage objects
		public List<Activity> Activities {
			get { return activities; }
			set { activities = value; }
		}

		// manage objects
		public ContextGraph ContextGraph {
			get { return contextGraph; }
			set { contextGraph = value; }
		}

		// manage current context
		public Context CurContext {
			get { return curContext; }
			set { curContext = value; }
		}

		// manage starting context
		public int StartContextId {
			get { return startContextId; }
			set { startContextId = value; }
		}

		// manage ending context
		public int EndContextId {
			get { return endContextId; }
			set { endContextId = value; }
		}

		public List<int> ScheduledActivities {
			get { return scheduledActivities; }
			set { scheduledActivities = value; }
		}

		// read context models
		public void ReadContextXml(string characterName) {
			XmlReader reader = XmlReader.Create("Assets/Files/" + characterName + "/contextgraph.xml");

			while (reader.Read()) {					
				if (reader.IsStartElement()) {
					switch (reader.LocalName.ToString ()) {
					case "context":						
						Context context = new Context (int.Parse (reader.GetAttribute ("id")), reader.GetAttribute ("name"));
						curContext = context;
						contextGraph.AddContext (context);
						if (reader.GetAttribute ("status") == "start")
							startContextId = int.Parse (reader.GetAttribute ("id"));
						else if (reader.GetAttribute ("status") == "end")
							endContextId = int.Parse (reader.GetAttribute ("id"));
						break;
					case "contextcondition":
						curContext.AddContextCondition (reader.GetAttribute ("object"), reader.GetAttribute ("status"));
						break;
					case "contextactivity":						
						curContext.AddContextActivity (int.Parse(reader.GetAttribute ("id")));
						break;
					case "nextcontext":						
						curContext.AddNextContext (int.Parse(reader.GetAttribute("id")),float.Parse(reader.GetAttribute("prob")));
						break;
					}				
				}
			}

			curContext = contextGraph.GetContext (startContextId);
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
		public Activity GetActivity(int id) {
			if (id < activities.Count)
				return activities [id];
			else
				return null;
		}

		// return an activity with id
		public Context GetContext(int id) {
			if (id < contextGraph.CountContexts())
				return contextGraph.GetContext(id);
			else
				return null;
		}

		// check whether simulation can end or not
		public bool IsEnd() {
			if (curContext.ID == endContextId)
				return true;
			else 
				return false;
		}
	}
}

