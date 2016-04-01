using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;


namespace UnityStandardAssets.Characters.ThirdPerson{
	public class clearScript : MonoBehaviour, IPointerClickHandler {
	public AICharacterControl Playlist;
	public  GameObject[] gameObjects;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		public void OnPointerClick(PointerEventData e)
		{
			Playlist.playlist.Clear ();

			gameObjects = GameObject.FindGameObjectsWithTag ("Button");

			for (int i = 0; i < gameObjects.Length; i++) {
				Destroy(gameObjects[i]);
			}
		}
}
}
