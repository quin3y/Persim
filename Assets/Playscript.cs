using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;


namespace UnityStandardAssets.Characters.ThirdPerson{
	
	public class Playscript : MonoBehaviour, IPointerClickHandler{
		
	public AICharacterControl Playlist;
		bool play = false;
		public Sprite pause;
		public Sprite playButton;
		float prevTime = 4;
	// Use this for initialization
		void Start(){
			gameObject.GetComponent<Image>().sprite = playButton;
			Time.timeScale = 4;
		}

		void Update()
		{
			if (Playlist.activityPlayback.actionQueue.Count == 0&&Time.timeScale>0&&Playlist.playlist.Count () > 0)
			{
				gameObject.GetComponent<Image> ().sprite = playButton;
			}
		}

		public void OnPointerClick(PointerEventData e)
		{
			if (Playlist.playlist.Count () > 0 && Playlist.activityPlayback.actionQueue.Count == 0) {
				Playlist.PlayActivity (Playlist.playlist.Pop ());
				gameObject.GetComponent<Image> ().sprite = pause;
			} else if (gameObject.GetComponent<Image> ().sprite == pause) {
				prevTime = Time.timeScale;
				Time.timeScale = 0;
				gameObject.GetComponent<Image> ().sprite = playButton;

			} else if (Time.timeScale == 0) {
				Time.timeScale = prevTime;
				gameObject.GetComponent<Image> ().sprite = pause;
			}
				


				



		}
	}
}
