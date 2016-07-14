using UnityEngine;
using System;
using System.Collections.Generic;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	enum Object_Status {ALWAYS, MAYBE, NEVER};

	public class Context
	{
		public int id;
		public String name;
		List<Condition> ContextConditions;
		List<ContextActivity> ContextActivities;
		List<NextContext> NextContexts;

		public Context ()
		{
			
		}

		class Condition
		{
			String obj_name;
			String obj_status;

			Condition (String obj_name, String obj_status)
			{
				this.obj_name = obj_name;
				this.obj_status = obj_status;
			}
		}

		class ContextActivity
		{
			Activity activity; 				// contextactivity id

			ContextActivity(int id)
			{
				
			}
		}

		class NextContext
		{
			Context context;				// next context
			float prob;						// probability to become the context

			NextContext(int id, float prob)
			{
				
			}
		}
	}
}

