using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class ActivityPlaylist : MonoBehaviour{
		private List<int> playlist;
		public bool popped = false;
		public ActivityPlaylist() {
			playlist = new List<int>();
		}
		void Start ()
		{

		//playlist = new List<int>();
		}

		// Param: activity's id
		public void AddActivity(int id) {
			playlist.Add(id);
		}

		// Delete the element at index
		public void DeleteActivity(int index) {
			if (playlist.Count > index) {
				playlist.RemoveAt(index);
			}
			else {
				Debug.Log("Activity index error");
			}
		}

		// Returns the first activity's id and removes it
		public int Pop() {
			if (playlist.Count > 0) {
				int n = playlist[0];
				playlist.RemoveAt(0);
				popped = true;
				return n;
			}
			else {
				return -1;
			}
		}

		// Clear the playlist
		public void Clear() {
			playlist.Clear();
		}

		// Insert current activity's id to the front
		public void RepeatCurrentActivity(int id) {
			playlist.Insert(0, id);
		}

		public int[] GetList() {
			return playlist.ToArray();
		}

		public int Count() {
			return playlist.Count;
		}

		public void Print() {
			foreach (int i in playlist) {
				Debug.Log(i);
			}
		}
	}
}

