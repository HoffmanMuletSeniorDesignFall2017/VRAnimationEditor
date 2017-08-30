using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AnimationVisualizer : MonoBehaviour {

	private AnimationClip currentClip;
	private GameObject currentGameObject;

	private List<AnimationCurve> animCurves;

	public TextMesh title;
	public TextMesh values;
	//private string valuesText;

	public void SetCurrentClipAndGameObject(AnimationClip animClip, GameObject go){
		currentClip = animClip;
		title.text = animClip.name;
		currentGameObject = go;

		RefreshCurves ();
	}

	public void RefreshCurves(){
		//Animation Curve Setup
		animCurves.Clear ();
		values.text = "";

		for (int i = 0; i < AnimationUtility.GetCurveBindings (currentClip).Length; i++) {
			animCurves.Add (AnimationUtility.GetEditorCurve (currentClip, AnimationUtility.GetCurveBindings (currentClip) [i]));
		}
	}

	public void TogglePlayAnimation(){
		/*if (currentGameObject.GetComponent<Animation> ().isPlaying) {
			Debug.Log ("Already playing!");
			currentGameObject.GetComponent<Animation> ().Stop ();
		} else {
			
			if (currentGameObject.GetComponent<Animation> ().Play () == false) {
				Debug.Log ("We got an error in playing the animation!");
			}
		}*/
		if (currentGameObject.GetComponent<Animator> ().speed > 0 || currentGameObject.GetComponent<Animator> ().speed < 0) {
			currentGameObject.GetComponent<Animator> ().speed = 0;
		} else {
			currentGameObject.GetComponent<Animator> ().speed = 1;
		}
		//currentGameObject.GetComponent<Animator> ().Play(0);
	}

	public void PauseAnimation(){
		currentGameObject.GetComponent<Animator> ().speed = 0;
	}


	// Use this for initialization
	void Start () {
		animCurves = new List<AnimationCurve> ();
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < animCurves.Count; i++) {
			if(i == 0)
				values.text = AnimationUtility.GetCurveBindings (currentClip) [i].propertyName + "\n";
			else
				values.text += AnimationUtility.GetCurveBindings (currentClip) [i].propertyName + "\n";
			//Then would do stuff to actually draw keyframes, etc.
		}
	}
}
