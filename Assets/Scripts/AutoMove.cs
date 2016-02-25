using UnityEngine;
using System.Collections;

public class AutoMove : MonoBehaviour {
	int direction = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(new Vector3(0.02f * direction, 0f, 0f));

		if (transform.position.x > -13.5 || transform.position.x < -14)    direction *= -1;
	}
}
