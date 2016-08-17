using UnityEngine;
using System;
using System.Collections.Generic;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class ContextGraph
	{
		List<Context> contexts;

		public ContextGraph()
		{
			contexts = new List<Context> ();	
		}

		// return a context
		public Context GetContext(int id) {
			foreach (Context cont in contexts) {
				if (cont.ID == id)
					return cont;
			}

			return null;
		}

		// add a context into context graph
		public void AddContext(Context context) {
			contexts.Add (context);
		}

		public void RemoveContext(int index) {
			contexts.RemoveAt (index);
		}

		// return the number of contexts
		public int CountContexts() {
			return contexts.Count;
		}
	}
}

