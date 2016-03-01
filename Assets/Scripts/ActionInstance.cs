using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class ActionInstance
	{
		public string name {get; set;}
		public int id {get; set;}
		public int animation {get; set;}
		public string obj {get; set;}
		public Vector3 location {get; set;}
		
		public ActionInstance(Action action, string obj) {
			this.name = action.name;
			this.id = action.id;
			this.animation = action.animation;
			this.obj = obj;
			this.location = GameObject.Find(obj).transform.position;
		}
		
	}
}
