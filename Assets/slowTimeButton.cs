using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;


namespace UnityStandardAssets.Characters.ThirdPerson{
	public class slowTimeButton : MonoBehaviour, IPointerClickHandler {

	// Use this for initialization

		public void OnPointerClick(PointerEventData e)
		{
			Time.timeScale -= 1;
		}




}
}