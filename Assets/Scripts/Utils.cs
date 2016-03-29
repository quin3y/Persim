using UnityEngine;
using System;
using System.Xml;
using System.Collections.Generic;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class Utils {
		public static List<Action> ReadActionXml() {
			List<Action> actionList = new List<Action>();
			XmlReader reader = XmlReader.Create("Assets/Files/actions.xml");
			
			while (reader.Read()) {
				if (reader.NodeType == XmlNodeType.Element && reader.Name == "action") {
					Action action = new Action();
					action.id = Int32.Parse(reader.GetAttribute(0));
					action.name = reader.GetAttribute(1);
					action.animation = Int32.Parse(reader.GetAttribute(2));

					actionList.Add(action);
				}
			}
			reader.Close();
			return actionList;
		}

		public static Dictionary<string, ObjectInfo> ReadObjectXml() {
			Dictionary<string, ObjectInfo> objects = new Dictionary<string, ObjectInfo>();
			XmlReader reader = XmlReader.Create("Assets/Files/objects.xml");

			while (reader.Read()) {
				if (reader.NodeType == XmlNodeType.Element && reader.Name == "object") {
					ObjectInfo obj = new ObjectInfo();
					obj.name = reader.GetAttribute(0);

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

					objects.Add(obj.name, obj);
				}
			}
			reader.Close();
			return objects;
		}

		public static List<Activity> ReadActivityXml() {
			List<Activity> activityList = new List<Activity>();
			XmlReader reader = XmlReader.Create("Assets/Files/activities.xml");
			
			while (reader.Read()) {
				if (reader.NodeType == XmlNodeType.Element && reader.Name == "activity") {
					Activity act = new Activity();
					act.id = Int32.Parse(reader.GetAttribute(0));
					act.name = reader.GetAttribute(1);
					
					while (reader.NodeType != XmlNodeType.EndElement) {
						reader.Read();
						if (reader.Name == "action") {
							act.actionIds.Add(Int32.Parse(reader.GetAttribute(0)));
							act.objectNames.Add(reader.GetAttribute(1));
						}
					}
					activityList.Add(act);
				}
			}
			reader.Close();
			return activityList;
		}
	}
}
