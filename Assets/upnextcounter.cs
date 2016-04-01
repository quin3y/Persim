using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace UnityStandardAssets.Characters.ThirdPerson{
public class upnextcounter : MonoBehaviour {
		public AICharacterControl Playlist;
	// Use this for initialization
		Text actCount;
		void Start () {
			actCount = GetComponent<Text>();
		}
	
	// Update is called once per frame
	void Update () {
			actCount.text = "Up Next: " + Playlist.playlist.Count() + " Activities";
	}
}
}
