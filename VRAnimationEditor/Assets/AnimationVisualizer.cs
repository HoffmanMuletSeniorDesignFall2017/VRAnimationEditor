using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AnimationVisualizer : MonoBehaviour {

	private AnimationClip currentClip;
	private GameObject currentGameObject;

	private List<AnimationCurve> animCurves;
	private List<AnimationCurveVisualizer> animCurves_Visualizers;

	public GameObject keyframeWorkArea;	//The keyframe work area for this visualizer.	(NOTE: just the visual component needs to be on the gameobject; we'll add the script in Start() of this class
	public GameObject keyframeObject;	//The keyframe gameobject that will be the symbol for keyframes

	public TextMesh title;
	public TextMesh values;
	//private string valuesText;

	public void SetCurrentClipAndGameObject(AnimationClip animClip, GameObject go){
		currentClip = animClip;
		title.text = animClip.name;
		currentGameObject = go;

		if (keyframeWorkArea.GetComponent<KeyframeWorkArea> ().timelineVisualizer != null) {
			keyframeWorkArea.GetComponent<KeyframeWorkArea> ().timelineVisualizer.animator = currentGameObject.GetComponent<Animator> ();
		}

		RefreshCurves ();
	}

	public void RefreshCurves(){
		//Animation Curve Setup
		animCurves.Clear ();

		for (int i = 0; i < animCurves_Visualizers.Count; i++) {
			animCurves_Visualizers [i].Clear ();
		}

		animCurves_Visualizers.Clear ();
		keyframeWorkArea.GetComponent<KeyframeWorkArea> ().RefreshBounds (0f);
		values.text = "";

		for (int i = 0; i < AnimationUtility.GetCurveBindings (currentClip).Length; i++) {

			animCurves.Add (AnimationUtility.GetEditorCurve (currentClip, AnimationUtility.GetCurveBindings (currentClip) [i]));

			//Add a visualizer for each curve
			AnimationCurveVisualizer acv = new AnimationCurveVisualizer();

			acv.curveNumber = i;
			acv.animCurve = animCurves [i];
			acv.keyframeObject = keyframeObject;
			acv.keyframeWorkArea= keyframeWorkArea.GetComponent<KeyframeWorkArea>();

			acv.Refresh ();


			animCurves_Visualizers.Add (acv);
		}

	}

	public void TogglePlayAnimation(){
		if (currentGameObject.GetComponent<Animator> ().speed > 0 || currentGameObject.GetComponent<Animator> ().speed < 0) {
			currentGameObject.GetComponent<Animator> ().speed = 0;
		} else {
			currentGameObject.GetComponent<Animator> ().speed = 1;
		}
	}

	public void PauseAnimation(){
		currentGameObject.GetComponent<Animator> ().speed = 0;
	}


	// Use this for initialization
	void Start () {
		animCurves = new List<AnimationCurve> ();
		animCurves_Visualizers = new List<AnimationCurveVisualizer> ();

		if (keyframeWorkArea.GetComponent<KeyframeWorkArea> () == null) {
			keyframeWorkArea.AddComponent<KeyframeWorkArea> ();
		}
	}
	
	// Update is called once per frame
	void Update () {

		//TODO: Get rid of this HACK

		for (int i = 0; i < animCurves.Count; i++) {
			if(i == 0)
				values.text = AnimationUtility.GetCurveBindings (currentClip) [i].propertyName + "\n";
			else
				values.text += AnimationUtility.GetCurveBindings (currentClip) [i].propertyName + "\n";
			//Then would do stuff to actually draw keyframes, etc.
		}

		//-----END HACK
	}
}
