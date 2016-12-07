using UnityEngine;
using System.Collections;

public class FinalIKAnimations : MonoBehaviour {
	public static Vector3[] AnimateObject(GameObject animationObject, GameObject endObject) {
		// Start & End Positions
		Vector3 startPosition = animationObject.transform.position;
		Vector3 endPosition = endObject.transform.position;

		// Start & End Rotations
		Vector3 startRotation = animationObject.transform.eulerAngles;
		Vector3 endRotation = endObject.transform.localEulerAngles;
		endRotation.z = endRotation.z-360.0f;

		// Position Curves
		AnimationCurve x_curve = AnimationCurve.EaseInOut (0.0f, startPosition.x, 1.5f, endPosition.x);
		AnimationCurve y_curve = AnimationCurve.EaseInOut (0.0f, startPosition.y, 1.5f, endPosition.y);
		AnimationCurve z_curve = AnimationCurve.EaseInOut (0.0f, startPosition.z, 1.5f, endPosition.z);

		// Rotation Curves
		AnimationCurve roll_curve  = AnimationCurve.Linear(0.0f, startRotation.x, 1.5f, endRotation.x);
		AnimationCurve yaw_curve   = AnimationCurve.Linear(0.0f, startRotation.y, 1.5f, endRotation.y);
		AnimationCurve pitch_curve = AnimationCurve.Linear(0.0f, startRotation.z, 1.5f, endRotation.z);

		// Create a new AnimationClip
		AnimationClip clip = new AnimationClip();
		clip.legacy = true;
		clip.wrapMode = WrapMode.Once;

		// Position Curves
		clip.SetCurve ("", typeof(Transform), "localPosition.x", x_curve);
		clip.SetCurve ("", typeof(Transform), "localPosition.y", y_curve);
		clip.SetCurve ("", typeof(Transform), "localPosition.z", z_curve);
		// Rotation Curves
		clip.SetCurve ("", typeof(Transform), "localEulerAngles.z", pitch_curve);
		clip.SetCurve ("", typeof(Transform), "localEulerAngles.y", yaw_curve);
		clip.SetCurve ("", typeof(Transform), "localEulerAngles.x", roll_curve);

	 	// Now animate the GameObject	
		Animation anim = animationObject.GetComponent<Animation> ();
		anim.AddClip (clip, "AnimateObject");
		anim.Play ("AnimateObject");
		Vector3[] pr = new Vector3[] {startPosition, endPosition, startRotation, endRotation};
		return pr;
	}

	public static void ReverseAnimateObject(GameObject animationObject, Vector3[] pr) {
		// Start & End Positions
		Vector3 startPosition = pr[1];
		Vector3 endPosition = pr[0];

		// Start & End Rotations
		Vector3 startRotation = pr[3];
		Vector3 endRotation = pr[2];

		// Position Curves
		AnimationCurve x_curve = AnimationCurve.EaseInOut (0.0f, startPosition.x, 1.5f, endPosition.x);
		AnimationCurve y_curve = AnimationCurve.EaseInOut (0.0f, startPosition.y, 1.5f, endPosition.y);
		AnimationCurve z_curve = AnimationCurve.EaseInOut (0.0f, startPosition.z, 1.5f, endPosition.z);

		// Rotation Curves
		AnimationCurve roll_curve  = AnimationCurve.Linear(0.0f, startRotation.x, 1.5f, endRotation.x);
		AnimationCurve yaw_curve   = AnimationCurve.Linear(0.0f, startRotation.y, 1.5f, endRotation.y);
		AnimationCurve pitch_curve = AnimationCurve.Linear(0.0f, startRotation.z, 1.5f, endRotation.z);

		// Create a new AnimationClip
		AnimationClip clip = new AnimationClip();
		clip.legacy = true;
		clip.wrapMode = WrapMode.Once;

		// Position Curves
		clip.SetCurve ("", typeof(Transform), "localPosition.x", x_curve);
		clip.SetCurve ("", typeof(Transform), "localPosition.y", y_curve);
		clip.SetCurve ("", typeof(Transform), "localPosition.z", z_curve);
		// Rotation Curves
		clip.SetCurve ("", typeof(Transform), "localEulerAngles.z", pitch_curve);
		clip.SetCurve ("", typeof(Transform), "localEulerAngles.y", yaw_curve);
		clip.SetCurve ("", typeof(Transform), "localEulerAngles.x", roll_curve);

		// Now animate the GameObject	
		Animation anim = animationObject.GetComponent<Animation> ();
		anim.AddClip (clip, "AnimateObjectRev");
		anim.Play ("AnimateObjectRev");
		
	}
}
