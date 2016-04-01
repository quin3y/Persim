using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace UnityStandardAssets.Characters.ThirdPerson{
	public class toDoList : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

	private string Name;
	public Text ButtonText;
	public Tutorial_ScrollView ScrollView;
	public GameObject Button_Template;
	public GameObject thisButton;
	public AICharacterControl Playlist;
	public int index;
	public toDoList list;
	public Tutorial_Button Activity_Button;
	public CurrentActLabel curAct;

	public void SetName(string name)
	{
		Name = name;
		ButtonText.text = name;
	}
		void Update()
		{

			if(Playlist.playlist.popped)
			{
				curAct.SetName (ButtonText.text);
				Destroy (thisButton);
				Activity_Button.Buttonss.RemoveAll(item => item == null);
				Playlist.playlist.popped = false;
			}




		}
		void DeactivateChildren(GameObject g, bool a) {
			//g.activeSelf = a;

			foreach (Transform child in g.transform) {
				//DeactivateChildren(child.gameObject, a);
				if (child.gameObject.name != "Text") {
					child.gameObject.SetActive (false);
				}
			}
		}

		void ActivateChildren(GameObject g, bool a) {
			//g.activeSelf = a;

			foreach (Transform child in g.transform)
			{
				child.gameObject.SetActive(true);
			} 
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			ActivateChildren (thisButton, true);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			DeactivateChildren (thisButton, true);
		}

		
	public void OnPointerClick(PointerEventData e)
	{
			/*Destroy (thisButton);
			for(int i = 0; i < Activity_Button.Buttonss.Count;i++)
			{
				if (Activity_Button.Buttonss[i] == null)
				{
					index = i;
					break;
				}
			}
			Playlist.playlist.DeleteActivity(index);
			Activity_Button.Buttonss.RemoveAll(item => item == null);*/

		




	}



}
}
