using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class ActionInstance
	{
		public string name {get; set;}
		public int id {get; set;}
		public int animation {get; set;}
		public ObjectInfo obj {get; set;}
		public Vector3 location {get; set;}
		
		public ActionInstance(Action action, ObjectInfo obj) {
			this.name = action.name;
			this.id = action.id;
			this.animation = action.animation;
			this.obj = obj;

			Vector3 loc;
			if (obj.characterPosition.x == 0 && obj.characterPosition.z == 0) {
				loc = GameObject.Find(obj.name).transform.position;
			}
			else {
				loc = obj.characterPosition;
			}
			loc.y = 0;
			this.location = loc;
		}
		
	}
}
