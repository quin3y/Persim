using UnityEngine;
using System;
using System.Collections.Generic;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class Context {
		int id;
		string name;
		List<ContextCondition> contextConditions;
		List<ContextActivity> contextActivities;		// activities to be performed
		List<NextContext> nextContexts;					// next contexts to be heading from the context

		public Context(int id, string name) {
			this.id = id;
			this.name = name;
			contextConditions = new List<ContextCondition> ();
			contextActivities = new List<ContextActivity> ();
			nextContexts = new List<NextContext> ();
		}

		// manage context ID
		public int ID {
			get { return id; }
			set { id = value; }
		}

		// manage context name
		public String Name {
			get { return name;	}
			set { name = value; }
		}

		// manage list of context conditions
		public List<ContextCondition> ContextConditions {
			get { return contextConditions; }
		}

		// manage list of context activities
		public List<ContextActivity> ContextActivities {
			get { return contextActivities; }
		}

		// manage list of next contexts
		public List<NextContext> NextContexts {
			get { return nextContexts; }
		}

		// return the number of context conditions
		public int CountContextConditions() {
			return contextConditions.Count;
		}

		// return a context condition
		public ContextCondition GetContextCondition(int index) {
			if (contextConditions.Count == 0)
				return null;
			else
				return contextConditions [index];
		}

		// add a context condition with object name and status
		public void AddContextCondition(string objName, string objStatus) {
			ContextCondition condition = new ContextCondition (objName, objStatus);
			contextConditions.Add (condition);
		}

		// remove a context condition with object name
		public void RemoveContextCondition(string objName) {
			foreach (ContextCondition cond in contextConditions) {
				if (cond.ObjectName == objName) 
					contextConditions.Remove (cond);				
			}
		}

		// remove a context condition with index
		public void RemoveContextCondition(int index) {
			Debug.Log (index + " out of " + ContextConditions.Count);
			if (index > contextConditions.Count)
				Debug.Log ("context condition is out of range.");
			else
				contextConditions.Remove (contextConditions [index]);
		}

		// return the number of context activities
		public int CountContextActivities() {
			return contextActivities.Count;
		}

		// return a context activity
		public ContextActivity GetContextActivity(int index) {
			if (contextActivities.Count == 0)
				return null;
			else
				return contextActivities [index];
		}

		// add a context activity
		public void AddContextActivity(int id, float prob) {			
			ContextActivity conAct = new ContextActivity (id, prob);
			contextActivities.Add (conAct);
		}

		// remove a context activity
		public void RemoveContextActivity(int id) {
			foreach (ContextActivity contAct in contextActivities) {
				if (contAct.ID == id) 
					contextActivities.Remove (contAct);
			}
		}

		// return the number of next contextts
		public int CountNextContexts() {
			return nextContexts.Count;
		}

		// return a context activity
		public NextContext GetNextContext(int index) {
			if (nextContexts.Count == 0)
				return null;
			else
				return nextContexts [index];
		}

		// add a next context
		public void AddNextContext(int id, float prob) {
			NextContext nextCont = new NextContext (id, prob);
			nextContexts.Add (nextCont);
		}

		// remove a next context
		public void RemoveNextContext(int id) {
			foreach (NextContext nextCont in nextContexts) {
				if (nextCont.ID == id)
					nextContexts.Remove (nextCont);
			}
		}

		// inner class - context condition
		public class ContextCondition	{
			string objName;
			string objStatus;				// TODO currently setting {always, maybe, never}

			public ContextCondition(string objName, string objStatus) {
				this.objName = objName;
				this.objStatus = objStatus;
			}

			// manage object name
			public string ObjectName {
				get { return objName; }
				set { objName = value; }
			}

			// manage object status
			public string ObjectStatus {
				get { return objStatus; }
				set { objStatus = value; }
			}
		}

		public class ContextActivity {
			int id; 						// contextactivity id
			float prob;
			bool performed;

			public ContextActivity(int id, float prob)	{
				this.id = id;
				this.prob = prob;
				this.performed = false;
			}

			// manage context activity ID
			public int ID {
				get { return id; }
			}

			// manage probability
			public float Probability {
				get { return prob; }
				set { prob = value; }
			}

			// manage probability
			public bool Performed {
				get { return performed; }
				set { performed = value; }
			}
		}

		public class NextContext {
			int id;							// next context id
			float prob;						// probability to become the context

			public NextContext(int id, float prob)	{
				this.id = id;
				this.prob = prob;
			}

			// manage next context ID
			public int ID {
				get { return id; }
				set { id = value; }
			}

			// manage probability
			public float Probability {
				get { return prob; }
				set { prob = value; }
			}

			// set a new probability
			public void SetProbability(float prob) {
				this.prob = prob;
			}
		}
	}
}

