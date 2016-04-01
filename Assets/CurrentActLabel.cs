using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace UnityStandardAssets.Characters.ThirdPerson{
public class CurrentActLabel : MonoBehaviour {
		
	public Text curActivity;
	private string Name;
	// Use this for initialization
	void Start () {
			curActivity = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
			
	}

		public void SetName(string name)
		{
			Name = name;
			curActivity.text = Name;
		}
	}
}
