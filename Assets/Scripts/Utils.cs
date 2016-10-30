using UnityEngine;
using System;
using System.Xml;
using System.Collections.Generic;
using System.Text;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class Utils {
		public static List<Action> ReadActionXml(string characterName) {
			List<Action> actionList = new List<Action>();
			XmlReader reader = XmlReader.Create("Assets/Files/" + characterName + "/actions.xml");
			
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

		public static Dictionary<string, ObjectInfo> ReadObjectXml(string characterName) {
			Debug.Log(characterName);
			Dictionary<string, ObjectInfo> objects = new Dictionary<string, ObjectInfo>();
			XmlReader reader = XmlReader.Create("Assets/Files/" + characterName + "/objects.xml");

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

		public static List<Activity> ReadActivityXml(string characterName) {
			List<Activity> activityList = new List<Activity>();
			XmlReader reader = XmlReader.Create("Assets/Files/" + characterName + "/activities.xml");
			
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
		public static void SaveActivityConfiguration(string characterName, List<Activity> activities) {
//			Debug.Log ("Saving Activity Configuration");
			XmlTextWriter writer = new XmlTextWriter("Assets/Files/" + characterName + "/activities.xml", Encoding.UTF8); //making XmlWriter
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
					writer.WriteAttributeString("max-occur", activities[i].maxOccurs[j].ToString());
					writer.WriteAttributeString("prereq", activities[i].prereqs[j].ToString());
					writer.WriteEndElement();
				}
				writer.WriteEndElement(); //writes </activity>
			}
			writer.WriteEndElement(); // writes </activities>
			writer.Close(); //close file
		}

		public static void SaveSensorConfiguration(string characterName) {
			XmlTextWriter writer = new XmlTextWriter("Assets/Files/" + characterName + "/sensor info.xml", Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();
            writer.WriteStartElement("sensors");    // <sensors>

            GameObject[] sensors = GameObject.FindGameObjectsWithTag("MotionSensor");
            foreach (GameObject sensor in sensors) {
                writer.WriteStartElement("sensor");
                writer.WriteAttributeString("type", "motion");
                writer.WriteAttributeString("attachTo", sensor.transform.parent.name);
                writer.WriteAttributeString("x", sensor.transform.position.x.ToString());
                writer.WriteAttributeString("y", sensor.transform.position.y.ToString());
                writer.WriteAttributeString("z", sensor.transform.position.z.ToString());
                writer.WriteEndElement();
            }

            sensors = GameObject.FindGameObjectsWithTag("IRSensor");
            foreach (GameObject sensor in sensors) {
                writer.WriteStartElement("sensor");
                writer.WriteAttributeString("type", "IR");
                writer.WriteAttributeString("attach-to", sensor.transform.parent.name);
                writer.WriteAttributeString("x", sensor.transform.position.x.ToString());
                writer.WriteAttributeString("y", sensor.transform.position.y.ToString());
                writer.WriteAttributeString("z", sensor.transform.position.z.ToString());
                writer.WriteEndElement();
            }

            sensors = GameObject.FindGameObjectsWithTag("ContactSensor");
            foreach (GameObject sensor in sensors) {
                writer.WriteStartElement("sensor");
                writer.WriteAttributeString("type", "contact");
                writer.WriteAttributeString("attachTo", sensor.transform.parent.name);
                writer.WriteAttributeString("x", sensor.transform.position.x.ToString());
                writer.WriteAttributeString("y", sensor.transform.position.y.ToString());
                writer.WriteAttributeString("z", sensor.transform.position.z.ToString());
                writer.WriteEndElement();
            }

            sensors = GameObject.FindGameObjectsWithTag("RFIDTag");
            foreach (GameObject sensor in sensors) {
                writer.WriteStartElement("sensor");
                writer.WriteAttributeString("type", "RFID tag");
                writer.WriteAttributeString("attachTo", sensor.transform.parent.name);
                writer.WriteAttributeString("x", sensor.transform.position.x.ToString());
                writer.WriteAttributeString("y", sensor.transform.position.y.ToString());
                writer.WriteAttributeString("z", sensor.transform.position.z.ToString());
                writer.WriteEndElement();
            }

            writer.WriteEndElement();    // </sensors>
            writer.Close();
        }

		public static void SaveSpaceInfo(string characterName) {
			XmlTextWriter writer = new XmlTextWriter("Assets/Files/" + characterName + "/space info.xml", Encoding.UTF8);
			writer.Formatting = Formatting.Indented;
			writer.WriteStartDocument();
			writer.WriteStartElement("objects");    // <objects>

			GameObject[] objects = GameObject.FindGameObjectsWithTag("NavStatic");
			foreach (GameObject obj in objects) {
				writer.WriteStartElement("object");
				writer.WriteAttributeString("name", obj.name);
				writer.WriteAttributeString("type", "nav static");
				writer.WriteAttributeString("x", obj.transform.position.x.ToString());
				writer.WriteAttributeString("y", obj.transform.position.y.ToString());
				writer.WriteAttributeString("z", obj.transform.position.z.ToString());
				writer.WriteAttributeString("rotation-x", obj.transform.rotation.x.ToString());
				writer.WriteAttributeString("rotation-y", obj.transform.rotation.y.ToString());
				writer.WriteAttributeString("rotation-z", obj.transform.rotation.z.ToString());
				writer.WriteEndElement();
			}

			objects = GameObject.FindGameObjectsWithTag("NonNavStatic");
			foreach (GameObject obj in objects) {
				writer.WriteStartElement("object");
				writer.WriteAttributeString("name", obj.name);
				writer.WriteAttributeString("type", "non-nav static");
				writer.WriteAttributeString("x", obj.transform.position.x.ToString());
				writer.WriteAttributeString("y", obj.transform.position.y.ToString());
				writer.WriteAttributeString("z", obj.transform.position.z.ToString());
				writer.WriteAttributeString("rotation-x", obj.transform.rotation.x.ToString());
				writer.WriteAttributeString("rotation-y", obj.transform.rotation.y.ToString());
				writer.WriteAttributeString("rotation-z", obj.transform.rotation.z.ToString());
				writer.WriteEndElement();
			}

			writer.WriteEndElement();    // </objects>
			writer.Close();
        }
	}
}
