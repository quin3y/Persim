using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class ActionInstance
	{
		public string name {get; set;}
		public int id {get; set;}
		public int animation {get; set;}
		public float characterDistance {get; set;}
		public ObjectInfo obj {get; set;}
		public Vector3 location {get; set;}
		
		public ActionInstance(Action action, ObjectInfo obj) {
			this.name = action.name;
			this.id = action.id;
			this.animation = action.animation;
			this.obj = obj;
			
			if (obj.characterPosition.x == 0 && obj.characterPosition.z == 0) {
				this.location = GameObject.Find(obj.name).transform.position;
				this.characterDistance = 1.5f;
			}
			else {
				this.location = obj.characterPosition;
				this.characterDistance = 0.2f;
			}
		}
		
	}
}
