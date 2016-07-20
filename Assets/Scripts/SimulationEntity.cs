using UnityEngine;
using System;
using System.Xml;
using System.Collections.Generic;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public static class SimulationEntity {
		public static List<ObjectInfo> Objects = new List<ObjectInfo> (); 								// a set of objects
		public static List<Action> Actions = new List<Action> ();										// a set of actions
		public static List<Activity> Activities = new List<Activity> ();								// a set of activities
		public static ContextGraph ContextGraph = new ContextGraph();									// context graph

		public static Context CurContext;
		public static ActivityPlaylist ActivityPlayList = new ActivityPlaylist ();

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
	}
}

