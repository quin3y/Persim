using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityStandardAssets.Characters.ThirdPerson
{
<<<<<<< HEAD
	public class ActivityPlaylist : MonoBehaviour{
		private List<int> playlist;
		public bool popped = false;
=======
	public class ActivityPlaylist {
		private List<int> playlist;
		private int lastIndexInPlaylist;

>>>>>>> Josh-Experiment
		public ActivityPlaylist() {
			playlist = new List<int>();
			lastIndexInPlaylist = -1;
			PlayerPrefs.SetInt("lastIndexInPlaylist", lastIndexInPlaylist);
		}
		void Start ()
		{

		//playlist = new List<int>();
		}

		// Param: activity's id
		public void AddActivity(int id) {
			playlist.Add(id);
			lastIndexInPlaylist++;
			PlayerPrefs.SetInt("lastIndexInPlaylist", lastIndexInPlaylist);
			PlayerPrefs.SetInt("playlistIndex" + lastIndexInPlaylist, id);
			//Debug.Log("playlistIndex" + lastIndexInPlaylist + " set to " + PlayerPrefs.GetInt ("playlistIndex" + lastIndexInPlaylist));
		}

		// Delete the element at index
		public void DeleteActivity(int index) {
			if (playlist.Count > index) {
				playlist.RemoveAt(index);
				PlayerPrefs.DeleteKey("playlistIndex" + index);
				lastIndexInPlaylist--;
				PlayerPrefs.SetInt("lastIndexInPlaylist", lastIndexInPlaylist);
				if (lastIndexInPlaylist != -1) {
					PlayerPrefs.SetInt("playlistIndex" + index, -1);
					for (int i = index; i <= lastIndexInPlaylist + 1; i++) {
						if (i != lastIndexInPlaylist) {
							PlayerPrefs.SetInt("playlistIndex" + i, PlayerPrefs.GetInt ("playlistIndex" + (i + 1)));
						} else {
							PlayerPrefs.SetInt("playlistIndex" + i, PlayerPrefs.GetInt ("playlistIndex" + (i + 1)));
							PlayerPrefs.DeleteKey("playlistIndex" + (i + 1));
						}
					}
				}
			}
			else {
				Debug.Log("Activity index error");
			}
		}

		// Returns the first activity's id and removes it
		public int Pop() {
			//Debug.Log ("Pop Called");
			if (playlist.Count > 0) {
				int n = playlist[0];
				playlist.RemoveAt(0);
<<<<<<< HEAD
				popped = true;
=======
				lastIndexInPlaylist--;
				PlayerPrefs.SetInt("lastIndexInPlaylist", lastIndexInPlaylist);
				PlayerPrefs.DeleteKey("playlistIndex0");
				if (lastIndexInPlaylist != -1) {
					PlayerPrefs.SetInt("playlistIndex0", -1);
					for (int i = 0; i <= lastIndexInPlaylist + 1; i++) {
						if (i != lastIndexInPlaylist) {
							PlayerPrefs.SetInt("playlistIndex" + i, PlayerPrefs.GetInt ("playlistIndex" + (i + 1)));
						} else {
							PlayerPrefs.SetInt("playlistIndex" + i, PlayerPrefs.GetInt ("playlistIndex" + (i + 1)));
							PlayerPrefs.DeleteKey("playlistIndex" + (i + 1));
						}
					}
				}

>>>>>>> Josh-Experiment
				return n;
			}
			else {
				return -1;
			}
		}

		// Clear the playlist
		public void Clear() {
			playlist.Clear();
			lastIndexInPlaylist = -1;
			PlayerPrefs.DeleteAll();
			PlayerPrefs.SetInt("lastIndexInPlaylist", lastIndexInPlaylist);
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

