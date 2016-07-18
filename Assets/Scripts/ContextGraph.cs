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

		// add a context into context graph
		public void AddContext(Context context) {
			contexts.Add (context);
		}

		// return a context
		public Context GetContext(int id) {
			foreach (Context cont in contexts) {
				if (cont.ID == id)
					return cont;
			}

			return null;
		}

		// return the number of contexts
		public int GetCount() {
			return contexts.Count;
		}
	}
}

