using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AnimationVisualizer : Visualizer {

	private AnimationClip currentClip;
	private GameObject currentGameObject;

	private List<AnimationCurve> animCurves;
	private List<AnimationCurveVisualizer> animCurves_Visualizers;
	private int lastSelectedAnimCurve_Visualizer = 0;

	public GameObject keyframeWorkArea;	//The keyframe work area for this visualizer.	(NOTE: just the visual component needs to be on the gameobject; we'll add the script in Start() of this class
	public GameObject keyframeObject;	//The keyframe gameobject that will be the symbol for keyframes

	public TextMesh title;
	public TextMesh values;
	//private string valuesText;
	private int waitFrame = 0;

	private int NUM_WAIT_FRAMES = 5;	//So much hackiness

	public void SetCurrentClipAndGameObject(AnimationClip animClip, GameObject go){
		currentClip = animClip;
		title.text = animClip.name;
		currentGameObject = go;

		if (keyframeWorkArea.GetComponent<KeyframeWorkArea> ().timelineVisualizer != null) {
			keyframeWorkArea.GetComponent<KeyframeWorkArea> ().timelineVisualizer.animator = currentGameObject.GetComponent<Animator> ();
		} else {
			Debug.LogError ("Please set up the keyframeWorkArea to have a timeline Visualizer, please!");
		}

		RefreshCurves ();
	}

	public void RefreshCurves(){
		//Animation Curve Setup
		animCurves.Clear ();

		for (int i = 0; i < animCurves_Visualizers.Count; i++) {
			animCurves_Visualizers [i].Clear ();
			GameObject.Destroy (animCurves_Visualizers [i].gameObject);
		}

		animCurves_Visualizers.Clear ();
		keyframeWorkArea.GetComponent<KeyframeWorkArea> ().RefreshBounds (0f);
		values.text = "";

		for (int i = 0; i < AnimationUtility.GetCurveBindings (currentClip).Length; i++) {

			animCurves.Add (AnimationUtility.GetEditorCurve (currentClip, AnimationUtility.GetCurveBindings (currentClip) [i]));

			//Add a visualizer for each curve
			GameObject dummyGameObject = new GameObject();
			dummyGameObject.AddComponent<AnimationCurveVisualizer> ();

			AnimationCurveVisualizer acv = dummyGameObject.GetComponent<AnimationCurveVisualizer> ();
			//AnimationCurveVisualizer acv = ScriptableObject.CreateInstance<AnimationCurveVisualizer>();//new AnimationCurveVisualizer();

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
	void Awake () {
		animCurves = new List<AnimationCurve> ();
		animCurves_Visualizers = new List<AnimationCurveVisualizer> ();

		if (keyframeWorkArea.GetComponent<KeyframeWorkArea> () == null) {
			keyframeWorkArea.AddComponent<KeyframeWorkArea> ();

		}
	}
	
	// Update is called once per frame
	void LateUpdate () {

		//TODO: Get rid of this HACK

		for (int i = 0; i < animCurves.Count; i++) {
			if(i == 0)
				values.text = AnimationUtility.GetCurveBindings (currentClip) [i].propertyName + "\n";
			else
				values.text += AnimationUtility.GetCurveBindings (currentClip) [i].propertyName + "\n";
			//Then would do stuff to actually draw keyframes, etc.
		}

		//-----END HACK
		for (int i = 0; i < animCurves.Count; i++) {
			if (animCurves_Visualizers [i].needsToRefresh) {



				//We have to keep track of the current time because SetCurve resets it :(
				float currentTime = keyframeWorkArea.GetComponent<KeyframeWorkArea>().timelineVisualizer.GetAnimatorTime();

				//Debug.Log (currentTime);

				StartCoroutine (UpdateAnimationCurveAndResume (AnimationUtility.GetCurveBindings (currentClip) [i].path, AnimationUtility.GetCurveBindings (currentClip) [i].type, AnimationUtility.GetCurveBindings (currentClip) [i].propertyName, animCurves [i], currentTime));
				//currentClip.SetCurve (AnimationUtility.GetCurveBindings (currentClip) [i].path, AnimationUtility.GetCurveBindings (currentClip) [i].type, AnimationUtility.GetCurveBindings (currentClip) [i].propertyName, animCurves [i]);

				//keyframeWorkArea.GetComponent<KeyframeWorkArea> ().timelineVisualizer.ChangeTime (currentTime);

				//keyframeWorkArea.GetComponent<KeyframeWorkArea>().timelineVisualizer.animator.Play (keyframeWorkArea.GetComponent<KeyframeWorkArea>().timelineVisualizer.animator.GetCurrentAnimatorStateInfo (0).shortNameHash, 0, currentTime);
				//RefreshCurves();

			}
		}

		//Have the last updated curve thing here
		for (int i = 0; i < animCurves.Count; i++) {
			if (animCurves_Visualizers [i].selected == true) {
				lastSelectedAnimCurve_Visualizer = i;
				break;
			}
		}
	}

	IEnumerator UpdateAnimationCurveAndResume(string path, System.Type type, string propertyName, AnimationCurve animCurve, float resumeTime){

		/*waitFrame = (++waitFrame) % NUM_WAIT_FRAMES;
		if (waitFrame != 0)
			yield return null;
		*/
		AnimationClip newClip = new AnimationClip ();
		newClip.name = "Test";

		//Perform a deep copy
		for (int i = 0; i < AnimationUtility.GetCurveBindings (currentClip).Length; i++) {
			newClip.SetCurve (AnimationUtility.GetCurveBindings (currentClip) [i].path, AnimationUtility.GetCurveBindings (currentClip) [i].type, AnimationUtility.GetCurveBindings (currentClip) [i].propertyName, AnimationUtility.GetEditorCurve (currentClip, AnimationUtility.GetCurveBindings (currentClip) [i]));
		}

		newClip.SetCurve (path, type, propertyName, animCurve);
		keyframeWorkArea.GetComponent<KeyframeWorkArea> ().timelineVisualizer.ChangeClip (newClip);
		//currentClip.SetCurve (path, type, propertyName, animCurve);
		//yield return new WaitForEndOfFrame ();
		yield return null;

		currentClip = newClip;

		//keyframeWorkArea.GetComponent<KeyframeWorkArea> ().timelineVisualizer.ChangeTime (resumeTime);
		yield return null;
	}




	public AnimationCurveVisualizer GetAnimCurveVisualizer(int index){
		return animCurves_Visualizers [index];
	}

	public AnimationCurveVisualizer GetLastAnimCurveVisualizer(){
		return animCurves_Visualizers [lastSelectedAnimCurve_Visualizer];
	}
}
