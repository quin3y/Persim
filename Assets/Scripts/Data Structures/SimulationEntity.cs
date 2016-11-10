using UnityEngine;
using System;
using System.Xml;
using System.Collections.Generic;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class SimulationEntity {
		
		List<Action> actions = new List<Action> ();										// a set of actions
		List<Activity> activities = new List<Activity> ();								// a set of activities
		ContextGraph contextGraph = new ContextGraph();									// context graph

		List<string> conditionStatus;

		Context curContext;
		int startContextId;																// starting context id
		int endContextId;																// ending context id

		List<int> scheduledActivities = new List<int>();								// list of performed activities
		public int[] contextActivities = {14, 19, 3, 16, 21, 12};

		public SimulationEntity() {			
			conditionStatus = new List<string> {"always", "maybe", "never"};
		}

		// static method: read object models
		public static void ReadObjectXml(String characterName) {
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

					StateSpaceManager.Objects.Add(obj);
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

		// manage list of condition status
		public List<string> ConditionStatus {
			get { return conditionStatus; }
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

		// return an object with the name
		public static ObjectInfo GetObject(string name) {
			foreach (ObjectInfo obj in StateSpaceManager.Objects) {
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

		public string GetActivityName(int id) {
			if (id < activities.Count)
				return activities [id].name;
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

		// API for MenuGUI
		// count contexts
		public int CountContexts () {
			return ContextGraph.CountContexts ();
		}

		// count activities
		public int CountActivities() {
			return activities.Count;
		}

		// count actions
		public int CountActions() {
			return actions.Count;
		}

		// count condition status
		public int CountConditionStatus() {
			return conditionStatus.Count;
		}

		// return an activity with id
		public Context GetContext(int id) {
			if (id < contextGraph.CountContexts())
				return contextGraph.GetContext(id);
			else
				return null;
		}

		// add a new context
		public void AddContext() {
			Context newContext = new Context (CountContexts (), "new");
			contextGraph.AddContext (newContext);
		}

		// remove a context
		public void RemoveContext(int indContext) {
			contextGraph.RemoveContext (indContext);
		}

		// get a name of context
		public string GetContextName (int indContext) {
			if (indContext < ContextGraph.CountContexts ())
				return GetContext (indContext).Name;
			else {
				Debug.Log ("out of index to access the context");
				return null;
			}
		}

		// set a new name of context
		public void SetContextName (int indContext, string newName) {			
			if (indContext < CountContexts ()){				
				GetContext (indContext).Name = newName;
			}
			else {
				Debug.Log("out of index to access the context");
			}
		}

		// get context conditions for a context
		public List<Context.ContextCondition> GetContextConditions (int indContext) {
			if (indContext < CountContexts ()) {
				return GetContext(indContext).ContextConditions;
			}
			else {
				return null;
			}
		}

		// count context conditions for a context
		public int CountContextConditions (int indContext) {
			if (indContext < CountContexts ()) {
				return GetContext(indContext).CountContextConditions();
			}
			else {
				return 0;
			}
		}

		// get a context condition for a context
		public Context.ContextCondition GetContextCondition (int indContext, int indCondition) {
			if (indContext < CountContexts ()) {
				if (indCondition < CountContextConditions(indContext))
					return GetContext (indContext).GetContextCondition (indCondition);
				else
					return null;
			}
			else {
				return null;
			}
		}

		// add a context condition for a context
		public void AddContextCondition (int indContext) {
			string objName = StateSpaceManager.Objects[0].name;
			string objStatus = "maybe";							// default is maybe		

			GetContext (indContext).AddContextCondition (objName,objStatus);
		}

		// remove a context condition for a context
		public void RemoveContextCondition (int indContext, int indCondition) {
			GetContext(indContext).RemoveContextCondition(indCondition);
		}

		// get a name of object defined in a context condition
		public string GetContextConditionObject (int indContext, int indCondition) {
			if (indContext < CountContexts ()) {
				if (indCondition < CountContextConditions (indContext))
					return GetContext (indContext).GetContextCondition (indCondition).ObjectName;
				else
					return null;
			}
			else
				return null;			
		}

		// set a name of object defined in a context condition
		public void SetContextConditionObject (int indContext, int indCondition, string newName) {
			if (indContext < CountContexts ()) {
				if (indCondition < CountContextConditions (indContext))
					GetContext (indContext).GetContextCondition (indCondition).ObjectName = newName;
				else
					Debug.Log ("out of condition");
			}
			else
				Debug.Log ("out of context");			
		}

		// get a name of object defined in a context condition
		public string GetContextConditionStatus (int indContext, int indCondition) {
			if (indContext < CountContexts ()) {
				if (indCondition < GetContext (indContext).CountContextConditions ())
					return GetContext (indContext).GetContextCondition (indCondition).ObjectStatus;
				else
					return null;
			}
			else
				return null;			
		}

		// set a name of object defined in a context condition
		public void SetContextConditionStatus (int indContext, int indCondition, string newStatus) {
			if (indContext < CountContexts ()) {
				if (indCondition < GetContext (indContext).CountContextConditions ())
					GetContext (indContext).GetContextCondition (indCondition).ObjectStatus = newStatus;
				else
					Debug.Log ("out of condition");
			}
			else
				Debug.Log ("out of context");
		}

		// get a name of object defined in a context condition
		public string GetConditionStatus (int index) {
			return conditionStatus[index];
		}

		// get context activities for a context
		public List<Context.ContextActivity> GetContextActivities (int indContext) {
			if (indContext < CountContexts ()) {
				return GetContext(indContext).ContextActivities;
			}
			else {
				return null;
			}
		}

		// count context activities for a context
		public int CountContextActivities (int indContext) {
			if (indContext < CountContexts ()) {
				return GetContext(indContext).CountContextActivities();
			}
			else {
				return 0;
			}
		}

		// add a context activity for a context
		public void AddContextActivity (int indContext, float prob) {			
			GetContext (indContext).AddContextActivity(0, prob);
		}

		// remove a context activity for a context
		public void RemoveContextActivity (int indContext, int indActivity) {
			GetContext(indContext).RemoveContextActivity(indActivity);
		}

		// get a context activity for a context
		public Context.ContextActivity GetContextActivity (int indContext, int indActivity) {
			if (indContext < CountContexts ()) {
				if (indActivity < CountContextActivities(indContext))
					return GetContext (indContext).GetContextActivity (indActivity);
				else
					return null;
			}
			else {
				return null;
			}
		}

		// get a name of context activity for a context
		public string GetContextActivityName (int indContext, int indActivity) {
			if (indContext < CountContexts ()) {
				if (indActivity < CountContextActivities (indContext)) {
					return GetActivity(GetContext (indContext).GetContextActivity (indActivity).ID).name;
				}
				else
					return null;
			}
			else {
				return null;
			}
		}

		// set a name of context activity for a context
		public void SetContextActivityName (int indContext, int indActivity, string newName) {
			if (indContext < CountContexts ()) {
				if (indActivity < CountContextActivities (indContext)) {
					GetActivity (GetContext (indContext).GetContextActivity (indActivity).ID).name = newName;
				} 
				else {
					Debug.Log ("out of context activities");
				}
			}
			else {
				Debug.Log ("out of contexts");
			}
		}

		// get a probability of context activity for a context
		public float GetContextActivityProb (int indContext, int indActivity) {
			if (indContext < CountContexts ()) {
				if (indActivity < CountContextActivities(indContext))
					return GetContext (indContext).GetContextActivity (indActivity).Probability;
				else
					return -0.1f;
			}
			else {
				return -1.1f;
			}
		}

		// get a probability of context activity for a context
		public void SetContextActivityProb (int indContext, int indActivity, float newProb) {
			if (indContext < CountContexts ()) {
				if (indActivity < CountContextActivities(indContext))
					GetContext (indContext).GetContextActivity (indActivity).Probability = newProb;
				else
					Debug.Log("out of context activity");
			}
			else {
				Debug.Log("out of context");
			}
		}

		// count next contexts for a context
		public int CountNextContexts (int indContext) {
			if (indContext < CountContexts ()) {
				return GetContext(indContext).CountNextContexts();
			}
			else {
				return 0;
			}
		}

		// add a next context for a context
		public void AddNextContext (int indContext, float prob) {			
			GetContext (indContext).AddNextContext(0, prob);
		}

		// remove a next context for a context
		public void RemoveNextContext (int indContext, int indNextContext) {
			GetContext(indContext).RemoveNextContext(indNextContext);
		}

		// get a next context for a context
		public Context.NextContext GetNextContext (int indContext, int indNextContext) {
			if (indContext < CountContexts ()) {
				if (indNextContext < CountNextContexts(indContext))
					return GetContext (indContext).GetNextContext (indNextContext);
				else
					return null;
			}
			else {
				return null;
			}
		}

		// get a name of context activity for a context
		public string GetNextContextName (int indContext, int indNextContext) {
			if (indContext < CountContexts ()) {
				if (indNextContext < CountNextContexts (indContext)) {
					return GetContext(GetContext (indContext).GetNextContext (indNextContext).ID).Name;
				}
				else
					return null;
			}
			else {
				return null;
			}
		}

		// set a name of context activity for a context
		public void SetNextContextName (int indContext, int indNextContext, string newName) {
			if (indContext < CountContexts ()) {
				if (indNextContext < CountNextContexts (indContext)) {
					GetContext (GetContext (indContext).GetNextContext (indNextContext).ID).Name = newName;
				} 
				else {
					Debug.Log ("out of context activities");
				}
			}
			else {
				Debug.Log ("out of contexts");
			}
		}

		// get a probability of context activity for a context
		public float GetNextContextProb (int indContext, int indNextContext) {
			if (indContext < CountContexts ()) {
				if (indNextContext < CountNextContexts(indContext))
					return GetContext (indContext).GetNextContext (indNextContext).Probability;
				else
					return -0.1f;
			}
			else {
				return -1.1f;
			}
		}

		// get a probability of context activity for a context
		public void SetNextContextProb (int indContext, int indNextContext, float newProb) {
			if (indContext < CountContexts ()) {
				if (indNextContext < CountNextContexts(indContext))
					GetContext (indContext).GetNextContext (indNextContext).Probability = newProb;
				else
					Debug.Log("out of context activity");
			}
			else {
				Debug.Log("out of context");
			}
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
						curContext.AddContextActivity (int.Parse(reader.GetAttribute ("id")),float.Parse(reader.GetAttribute("prob")));
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
	}
}

