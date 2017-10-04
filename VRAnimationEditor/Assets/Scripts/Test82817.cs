using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Test82817 : MonoBehaviour {

	public GameObject testPrefab;
	public AnimationVisualizer animVisual;
	public AnimationClip animClip;

	public MocapController moCon;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.KeypadEnter)) {
			//animClip = AnimationEditorFunctions.CreateNewAnimation (string.Concat("Test", System.DateTime.Now.Minute.ToString()));
			animClip = AnimationEditorFunctions.CreateNewAnimation ("Test");
			GameObject go = AnimationEditorFunctions.InstantiateWithAnimation (testPrefab, animClip);
			animVisual.SetCurrentClipAndGameObject (animClip, go);

		}

		if (Input.GetKeyDown (KeyCode.KeypadPlus)) {

			//Debug.Log ("Hey!");

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

			animVisual.RefreshCurves ();
		}
		if (Input.GetKeyDown (KeyCode.KeypadMinus)) {

			//Debug.Log ("Hey!");

			AnimationCurve testCurve = new AnimationCurve();
			Keyframe key1 = new Keyframe();
			Keyframe key2 = new Keyframe();

			key1.time = 0f;
			key1.value = 3f;

			key2.time = 4f;
			key2.value = -5f;

			testCurve.AddKey (key1);
			testCurve.AddKey (key2);

			animClip.SetCurve("", typeof(Transform), "localPosition.x", testCurve);

			animVisual.RefreshCurves ();
		}

		if (Input.GetKeyDown (KeyCode.KeypadDivide)) {

			animVisual.TogglePlayAnimation ();
		}

		if (Input.GetKeyDown (KeyCode.KeypadMultiply)) {

			animClip.ClearCurves ();
			animVisual.RefreshCurves ();
		}

		if (Input.GetKeyDown (KeyCode.M)) {
			animVisual.ToggleMotionCapture ();
		}
	}
}
