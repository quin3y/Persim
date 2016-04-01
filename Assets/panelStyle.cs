using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class panelStyle : MonoBehaviour {
	public Image im;
	// Use this for initialization
	void Start () {
	im.GetComponent<CanvasRenderer>().SetAlpha(0.01f);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
