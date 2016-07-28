using UnityEngine;
using System;
using System.Xml;
using System.Collections.Generic;
using System.Text;

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
					Activity activity = new Activity();
                    activity.id = Int32.Parse(reader.GetAttribute(0));
                    activity.name = reader.GetAttribute(1);
					
					while (reader.NodeType != XmlNodeType.EndElement) {
						reader.Read();
						if (reader.Name == "action") {
                            activity.actionIds.Add(Int32.Parse(reader.GetAttribute(0)));
                            activity.objectNames.Add(reader.GetAttribute(1));
                            activity.importances.Add(Int32.Parse(reader.GetAttribute(2)));
                            activity.maxOccurs.Add(Int32.Parse(reader.GetAttribute(3)));
                            activity.prereqs.Add(Int32.Parse(reader.GetAttribute(4)));
						}
					}
                    activityList.Add(activity);
				}
			}
			reader.Close();
			return activityList;
		}

		//writes new xml file called activitesCopy according to list of activities passed in
		public static void SaveActivityConfiguration(List<Activity> activities) {
//			Debug.Log ("Saving Activity Configuration");
			XmlTextWriter writer = new XmlTextWriter("Assets/Files/activitiesCopy.xml", Encoding.UTF8); //making XmlWriter
			writer.Formatting = Formatting.Indented; //making the sure the xml file will be indented
			writer.WriteStartDocument(); //writes <?xml version="1.0" encoding="utf-8"?>
			writer.WriteStartElement("activities"); // writes <activities>
			for (int i = 0; i < activities.Count; i++) {
				// writes <activity id="id#" name="activityName"> for each activity 
				writer.WriteStartElement("activity");
				writer.WriteAttributeString("id", i.ToString());
				writer.WriteAttributeString("name", activities[i].name);

				List<Int32> actions = activities[i].actionIds; //getting all the actions for the ith activity
				for (int j = 0; j < actions.Count; j++) { 
					writer.WriteStartElement("action");
					writer.WriteAttributeString("id", actions[j].ToString());
					writer.WriteAttributeString("object", activities[i].objectNames[j]);
					writer.WriteAttributeString("importance", activities[i].importances[j].ToString());
					writer.WriteAttributeString("maxOccur", activities[i].maxOccurs[j].ToString());
					writer.WriteAttributeString("prereq", activities[i].prereqs[j].ToString());
					writer.WriteEndElement();
				}
				writer.WriteEndElement(); //writes </activity>
			}
			writer.WriteEndElement(); // writes </activities>
			writer.Close(); //close file
		}
	}
}
