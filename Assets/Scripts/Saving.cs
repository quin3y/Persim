using UnityEngine;
using System.Collections;

public class Saving : MonoBehaviour {

	void OnApplicationQuit() {
		PlayerPrefs.SetInt("previousLastIndex", PlayerPrefs.GetInt ("lastIndexInPlaylist"));
		PlayerPrefs.SetInt("prevRun", 1);
//		Debug.Log("previousLastIndex saved as " + PlayerPrefs.GetInt("previousLastIndex"));
		PlayerPrefs.Save();
		Debug.Log("PlayerPrefs Saved");
	}

}
