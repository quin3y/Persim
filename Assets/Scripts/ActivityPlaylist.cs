using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class ActivityPlaylist{
		private List<int> playlist;

		public ActivityPlaylist() {
			playlist = new List<int>();
		}

		// Param: activity's id
		public void AddActivity(int id) {
			playlist.Add(id);
		}

		// Delete the n-th element in the playlist
		public void DeleteActivity(int n) {
			playlist.RemoveAt(n);
		}

		// Clear the playlist
		public void Clear() {
			playlist.Clear();
		}

		// Insert current activity's id to the front
		public void RepeatCurrentActivity(int id) {
			playlist.Insert(0, id);
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

