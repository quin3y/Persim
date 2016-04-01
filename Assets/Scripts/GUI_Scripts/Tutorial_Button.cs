using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace UnityStandardAssets.Characters.ThirdPerson{
public class Tutorial_Button : MonoBehaviour, IPointerClickHandler {

	private string Name;
	public Text ButtonText;
	public Tutorial_ScrollView ScrollView;
	public GameObject Button_Template;
	public AICharacterControl Playlist;
	public toDoList list;
	int count = 0;
	public List<GameObject> Buttonss = new List<GameObject>();
	

	public void SetName(string name)
	{
		Name = name;
		ButtonText.text = name;
	}
		
	public void OnPointerClick(PointerEventData e)
	{
		Buttonss.Add(Instantiate(Button_Template) as GameObject);
		Buttonss [Buttonss.Count-1].SetActive(true);
		toDoList TB = Buttonss [Buttonss.Count-1].GetComponent<toDoList>();
		TB.SetName(ButtonText.text);
		Buttonss [Buttonss.Count-1].transform.SetParent(Button_Template.transform.parent);

		
			for (int i = 0; i < ScrollView.NameList.Count; i++) 
			{
				if (ButtonText.text == ScrollView.NameList [i]) {
					if (Playlist.playlist.Count() == 0) {
						Playlist.playlist.AddActivity (i);
						//Playlist.PlayActivity (Playlist.playlist.Pop ());
						/* if (Playlist.playlist is_currently_playing)
						 {do nothing}
						 else
						 	Play the new activity*/

						//Playlist.PlayActivity (Playlist.playlist.Pop ());
					}
					else
					Playlist.playlist.AddActivity (i);

				}
				if (i == 1 - ScrollView.NameList.Count) {
					Debug.Log ("Is this a new activity? It wasn't found in the activity bank");
				}
			}


	}

}
}
