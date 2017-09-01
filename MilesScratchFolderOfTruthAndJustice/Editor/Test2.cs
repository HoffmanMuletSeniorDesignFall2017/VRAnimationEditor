using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class Test2 : MonoBehaviour {

	[MenuItem("My Commands/First Command _p")]
	static void FirstCommand() {

		if (Selection.activeGameObject.GetComponent<Animation> () != null) {
			Debug.Log("Deleting animation curves!!");

			Animation anim = Selection.activeGameObject.GetComponent<Animation> ();
			AnimationClip animClip = anim.clip;

			animClip.ClearCurves ();
		}

	}
	[MenuItem("My Commands/Special Command %g")]
	static void SpecialCommand() {
		if (Selection.activeGameObject.GetComponent<Animation> () != null) {
			Debug.Log("Adding test curve1");

			Animation anim = Selection.activeGameObject.GetComponent<Animation> ();
			AnimationClip animClip = anim.clip;

			AnimationCurve testCurve = new AnimationCurve();

			Keyframe key1 = new Keyframe();
			Keyframe key2 = new Keyframe();

			key1.time = 0f;
			key1.value = 2;

			key2.time = 2f;
			key2.value = 1;

			testCurve.AddKey (key1);
			testCurve.AddKey (key2);



			animClip.SetCurve("", typeof(Transform), "localPosition.y", testCurve);
		}
	}

	[MenuItem("My Commands/Special Command2 %h")]
	static void SpecialCommand2() {
		if (Selection.activeGameObject.GetComponent<Animation> () != null) {
			Debug.Log("Adding test curve2");

			Animation anim = Selection.activeGameObject.GetComponent<Animation> ();
			AnimationClip animClip = anim.clip;

			AnimationCurve testCurve = new AnimationCurve();
			Keyframe key1 = new Keyframe();
			Keyframe key2 = new Keyframe();

			key1.time = 1f;
			key1.value = 1;

			key2.time = 3f;
			key2.value = 7;

			testCurve.AddKey (key1);
			testCurve.AddKey (key2);

			animClip.SetCurve("", typeof(Transform), "localPosition.y", testCurve);
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.RightArrow)) {
			Debug.Log ("Test!!");
		}
	}

	void FixedUpdate(){
		if (Input.GetKey (KeyCode.RightArrow)) {
			Debug.Log ("Test!!");
		}
	}
}
