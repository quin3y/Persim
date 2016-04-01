using UnityEngine;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace UnityStandardAssets.Characters.ThirdPerson{

	public class xButtonScript : MonoBehaviour, IPointerClickHandler {
	public GameObject thisButton;
	public AICharacterControl Playlist;
	public Tutorial_Button Activity_Button;
	int index;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

		public void OnPointerClick(PointerEventData e)
		{
			Destroy (thisButton);
			for(int i = 0; i < Activity_Button.Buttonss.Count;i++)
			{
				if (Activity_Button.Buttonss[i] == null)
				{
					index = i;
					break;
				}
			}
			Playlist.playlist.DeleteActivity(index);
			Activity_Button.Buttonss.RemoveAll(item => item == null);






		}
}
}
