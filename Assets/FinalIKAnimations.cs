using UnityEngine;
using System.Collections;

/*
 * Class for animating objects using the legacy Animation component: https://docs.unity3d.com/ScriptReference/Animation.html 
 */
public class FinalIKAnimations : MonoBehaviour {
	/// <summary>
	/// Animates a GameObject that contains an Animation component
	/// so that it moves from its start location to the specified end location.
	/// </summary>
	/// <param name="animationObject">
	/// animationObject: The object to be animated using animation curves
	/// </param>
	/// <param name="endObject">
	/// endObject: A placeholder gameobject which defines the transform where the animation should end
	/// </param>
	/// <param name="animTime">
	/// The length of time in seconds the animation should play
	/// </param>
	/// <returns>
	/// Returns a Vector3[] which contains the original start and end positions and rotations:
	/// {startPosition, endPosition, startRotation, endRotation}
	/// </returns>
	public static Vector3[] AnimateObject(GameObject animationObject, GameObject endObject, float animTime) {
		// Start & End Positions
		Vector3 startPosition = animationObject.transform.position;
		Vector3 endPosition = endObject.transform.position;

		// Start & End Rotations
		Vector3 startRotation = animationObject.transform.eulerAngles;
		Vector3 endRotation = endObject.transform.localEulerAngles;
		endRotation.z = endRotation.z-360.0f;  //-360.0f prevents weird spinning during animation

		// Position Curves
		AnimationCurve x_curve = AnimationCurve.EaseInOut (0.0f, startPosition.x, animTime, endPosition.x);
		AnimationCurve y_curve = AnimationCurve.EaseInOut (0.0f, startPosition.y, animTime, endPosition.y);
		AnimationCurve z_curve = AnimationCurve.EaseInOut (0.0f, startPosition.z, animTime, endPosition.z);

		// Rotation Curves
		AnimationCurve roll_curve  = AnimationCurve.Linear(0.0f, startRotation.x, animTime, endRotation.x);
		AnimationCurve yaw_curve   = AnimationCurve.Linear(0.0f, startRotation.y, animTime, endRotation.y);
		AnimationCurve pitch_curve = AnimationCurve.Linear(0.0f, startRotation.z, animTime, endRotation.z);

		// Create a new AnimationClip
		AnimationClip clip = new AnimationClip();
		clip.legacy = true;
		clip.wrapMode = WrapMode.Once;  // will only ploy this animation clip once

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
	/// <summary>
	/// Given original start and end positions and rotations, reverses the animation of the animation object
	/// by moving it from the original end position to the original start position.
	/// </summary>
	/// <param name="animationObject">
	/// animationObject: The object to be animated using animation curves
	/// </param>
	/// <param name="pr">
	/// The original Vector3 start and end positions and rotations from the forward playing animation:
	/// {startPosition, endPosition, startRotation, endRotation}
	/// </param>
	/// <param name="animTime">
	/// The length of time in seconds the animation should play
	/// </param>
	public static void ReverseAnimateObject(GameObject animationObject, Vector3[] pr, float animTime) {
		// Start & End Positions
		Vector3 startPosition = pr[1];
		Vector3 endPosition = pr[0];

		// Start & End Rotations
		Vector3 startRotation = pr[3];
		Vector3 endRotation = pr[2];

		// Position Curves
		AnimationCurve x_curve = AnimationCurve.EaseInOut (0.0f, startPosition.x, animTime, endPosition.x);
		AnimationCurve y_curve = AnimationCurve.EaseInOut (0.0f, startPosition.y, animTime, endPosition.y);
		AnimationCurve z_curve = AnimationCurve.EaseInOut (0.0f, startPosition.z, animTime, endPosition.z);

		// Rotation Curves
		AnimationCurve roll_curve  = AnimationCurve.Linear(0.0f, startRotation.x, animTime, endRotation.x);
		AnimationCurve yaw_curve   = AnimationCurve.Linear(0.0f, startRotation.y, animTime, endRotation.y);
		AnimationCurve pitch_curve = AnimationCurve.Linear(0.0f, startRotation.z, animTime, endRotation.z);

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
