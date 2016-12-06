using UnityEngine;
using System.Collections;

// Data structure for object. Refer to objects.xml.

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class ObjectInfo {
		public int id {get; set;}
		public string name {get; set;}
		public string status {get; set;}
		public Vector3 position {get; set;}
		public Vector3 rotation {get; set;}
		public Vector3 inHandPosition {get; set;}
		public Vector3 inHandRotation {get; set;}
		public Vector3 characterPosition {get; set;}
		public Vector3 characterRotation {get; set;}
	}
}
