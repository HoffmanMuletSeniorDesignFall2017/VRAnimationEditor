using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using cakeslice;

public class AnimationCurveVisualizer : Visualizer {//ScriptableObject { //MonoBehaviour {

	string parameterTitle;	//The name of the parameter that this curve is for

	public AnimationCurve animCurve;	//IMPORTANT! The actual curve associated with this animation curve
	public AnimationVisualizer parentAnimVisualizer;

	List<AnimationCurveVisualizer> childrenCurveVisualizers;	//Any children curves related to this (EXPERIMENTAL)

	public GameObject keyframeObject;	//The Gameobject visual representation of a keyframe (JUST the visual part - so can be anything pretty much)

	private List<GameObject> currentKeyframes;	//The current keyframes that have been instantiated 

	//public bool selected = false;				//Whether or not ANY of the keyframes of this animation curve are currently being selected by the user

	public KeyframeWorkArea keyframeWorkArea;	//The Gameobject that will become the parent of all instantiated keyframe objects; it should be controlled by another script that is keeping track of the current time of the animation

	public int curveNumber;	//Assigned from the keyframeWorkArea; the number of this animation curve

	public float X_OFFSET_CONSTANT = 2f;	//Used to make drawing nice
	public float Y_OFFSET_CONSTANT = 1.9550f;

	private GameObject selectedKeyframe;	//The keyframe that the user has selected right now
	private int selectedKeyframeIndex = 0;
	public Visualizer visualizerDummy;

	public bool hasChanged = false;
	public bool needsToRefresh = false;		//If there's new data I guess?

	public GameObject valueVisualizer;

	public GameObject associatedNodeVisualizer;

	// Use this for initialization
	void Start () {
		if(currentKeyframes == null)
			currentKeyframes = new List<GameObject> ();
		
		//visualizerDummy = new Visualizer();

		//Refresh ();

		valueVisualizer = new GameObject();
		valueVisualizer.transform.position = keyframeWorkArea.valueVisualizerCoordinates;
		valueVisualizer.transform.localScale = keyframeWorkArea.valueVisualizerScale;

		if (valueVisualizer.GetComponent<ValueVisualizer> () == null) {
			valueVisualizer.AddComponent<ValueVisualizer> ();
		}
			
		valueVisualizer.GetComponent<ValueVisualizer> ().associatedVisualizer = this;

		valueVisualizer.SetActive (false);
	}
	
	// Update is called once per frame
	void LateUpdate () {
		HandleKeyframeMovement ();
	}

	public void Refresh(){	//called if an outside class has reason to believe that this curve might have new data. It...
									
		InstantiateKeyframes();		//1 . re-instantiates the keyframe objects

	}

	private void InstantiateKeyframes(){

		if (currentKeyframes == null) {
			currentKeyframes = new List<GameObject> ();
		}

		//Remove any existing keyframe gameObjects
		for (int j = 0; j < currentKeyframes.Count; j++) {
			Destroy (currentKeyframes [j]);
		}
		currentKeyframes.Clear ();	

		for (int i = 0; i < animCurve.keys.Length; i++) {	//For every keyframe....
			GameObject nextKeyframe = Instantiate(keyframeObject, keyframeWorkArea.transform);	//Instantiate a new keyframewhose parent is the transform of the current keyframeWorkArea

			//-------Set up the MovableVisualizer component--------
			nextKeyframe.AddComponent<MovableVisualizer> ();
			nextKeyframe.GetComponent<MovableVisualizer> ().associatedVisualizer = this;//visualizerDummy;
			nextKeyframe.GetComponent<MovableVisualizer> ().constrainedToLocalX = true;

			//-------End set up the MovableVisualizer component--------

			//-------Set up the Outline component--------

			nextKeyframe.AddComponent<cakeslice.Outline> ();
			nextKeyframe.GetComponent<cakeslice.Outline> ().enabled = false;

			//-------End set up the Outline component--------

			nextKeyframe.transform.localPosition = new Vector3(animCurve.keys[i].time * keyframeWorkArea.timeScale * X_OFFSET_CONSTANT,  -curveNumber * keyframeWorkArea.verticalZoom * Y_OFFSET_CONSTANT, 0);	//Set it at the appropriate position based on its time and the current KeyframeWorkArea configurations
			currentKeyframes.Add(nextKeyframe);	//Add this to the list so we can keep track of it

			//We also have to update our keyframeWorkArea.
			if (animCurve.keys [i].time * keyframeWorkArea.timeScale * X_OFFSET_CONSTANT > keyframeWorkArea.bounds)
				keyframeWorkArea.RefreshBounds (animCurve.keys [i].time * keyframeWorkArea.timeScale * X_OFFSET_CONSTANT);
		}
			
	}

	public void Clear(){	//Delete any instantiated objects


		if (currentKeyframes != null) {
			for (int j = 0; j < currentKeyframes.Count; j++) {
				Destroy (currentKeyframes [j]);
			}
			currentKeyframes.Clear ();
		}
	}

	private void HandleKeyframeMovement(){

		if (selected) {
			valueVisualizer.SetActive (true);
			/*
			if (selectedKeyframe == null) {
				
				for (int i = 0; i < currentKeyframes.Count; i++) {
					if (currentKeyframes [i].GetComponent<MovableVisualizer> ().selected) {
						selectedKeyframe = currentKeyframes [i];
						selectedKeyframeIndex = i;
						break;
					}
				}
				//Here we just do the selection outline
				selectedKeyframe.GetComponent<cakeslice.Outline> ().enabled = true;
			}
*/
			for (int i = 0; i < currentKeyframes.Count; i++) {
				currentKeyframes[i].GetComponent<cakeslice.Outline>().enabled = false;
			}

			for (int i = 0; i < currentKeyframes.Count; i++) {
				if (currentKeyframes [i].GetComponent<MovableVisualizer> ().selected) {
					selectedKeyframe = currentKeyframes [i];
					selectedKeyframeIndex = i;
					break;
				}
			}

			if (selectedKeyframe != null) {
				//Here we just do the selection outline
				selectedKeyframe.GetComponent<cakeslice.Outline> ().enabled = true;


				float adjustedPosition = selectedKeyframe.transform.localPosition.x / keyframeWorkArea.bounds;

				if (adjustedPosition > 1f)
					adjustedPosition = 1f;
				else if (adjustedPosition < 0.01f)
					adjustedPosition = 0.01f;

				selectedKeyframe.transform.localPosition = new Vector3 (adjustedPosition * keyframeWorkArea.bounds, selectedKeyframe.transform.localPosition.y, selectedKeyframe.transform.localPosition.z);

				valueVisualizer.GetComponent<ValueVisualizer> ().UpdateText (animCurve [selectedKeyframeIndex].value);

				valueVisualizer.SetActive (true);



				//Do some node stuff
				if (associatedNodeVisualizer != null) {
					associatedNodeVisualizer.GetComponent<ModelNodeController> ().SetAxisVisibility (true);
				}

				if (childNeedsDeletion) {
					//Delete the selected keyframe
					animCurve.RemoveKey (selectedKeyframeIndex);

					currentKeyframes.Remove (selectedKeyframe);
					GameObject.Destroy (selectedKeyframe);

					selected = false;
					grabbing = false;
					childNeedsDeletion = false;

					needsToRefresh = true;
					parentAnimVisualizer.RefreshAnimationCurve (curveNumber);


					return;
				}

			}
		} else {
			//needsToRefresh = false;
			valueVisualizer.SetActive (false);
		}

		//If a keyframe is being grabbed, then it should be writing its value to the animation curve
		if (grabbing) {

			//The below handles MOVEMENT

			//TODO: change so we can update multiple keyframes, not just one!
			//Debug.Log("Detected a selection!");

			Keyframe newKeyframe = animCurve[selectedKeyframeIndex];
			//Debug.Log(newKeyframe.time);

			//Here we want to find the last keyframe time.
			float biggestTime = 0f;
			for (int i = 0; i < animCurve.length; i++) {
				if (animCurve [i].time > biggestTime)
					biggestTime = animCurve [i].time;
			}

			//TODO: Add support for moving keyframes beyond their original bounds maybe??

			float adjustedPosition = selectedKeyframe.transform.localPosition.x / keyframeWorkArea.bounds;

			newKeyframe.time = adjustedPosition * biggestTime;
			animCurve.MoveKey (selectedKeyframeIndex, newKeyframe);

			//hasChanged = true;
			needsToRefresh = true;
			parentAnimVisualizer.RefreshAnimationCurve (curveNumber);

		} else {
			/*if (hasChanged) {
				needsToRefresh = true;
				hasChanged = false;
			} else {
				needsToRefresh = false;
			}*/
			if (selectedKeyframe != null) {
				selectedKeyframe = null;
			}
			//needsToRefresh = false;
		}

		if (!selected) {
			for (int i = 0; i < currentKeyframes.Count; i++) {
				currentKeyframes[i].GetComponent<cakeslice.Outline>().enabled = false;
			}
		}
	}

	public void AddKeyframe(){
		Keyframe newKeyframe = new Keyframe ();

		float biggestTime = 0f;
		for (int i = 0; i < animCurve.length; i++) {
			if (animCurve [i].time > biggestTime)
				biggestTime = animCurve [i].time;
		}

		newKeyframe.time = (keyframeWorkArea.timelineVisualizer.timeLine.transform.localPosition.x / keyframeWorkArea.timelineVisualizer.bound) * biggestTime; 				//Set this new keyframe's time to wherever the current
		newKeyframe.value = animCurve [selectedKeyframeIndex].value;	//Set this new keyframe's value to whatever the last selected keyframe's value was (essentially a copy-paste)

		animCurve.AddKey (newKeyframe);

		needsToRefresh = true;
		parentAnimVisualizer.RefreshAnimationCurve (curveNumber);

		Refresh ();
	}

	public void AddExistingKeyframe(Keyframe k){

		float biggestTime = 0f;
		for (int i = 0; i < animCurve.length; i++) {
			if (animCurve [i].time > biggestTime)
				biggestTime = animCurve [i].time;
		}

		k.time = (keyframeWorkArea.timelineVisualizer.timeLine.transform.localPosition.x / keyframeWorkArea.timelineVisualizer.bound) * biggestTime;

		bool alreadyHaveKeyframe = false;
		int index;
		for (index = 0; index < animCurve.keys.Length; index++) {
			if (animCurve.keys [index].time == k.time) {
				alreadyHaveKeyframe = true;
				break;
			}
		}

		if (!alreadyHaveKeyframe)
			animCurve.AddKey (k);
		else
			animCurve.MoveKey (index, k);

		needsToRefresh = true;
		parentAnimVisualizer.RefreshAnimationCurve (curveNumber);

		Refresh ();
	}

	public void EditSelectedKeyframeValue(float newValue){
		Keyframe newKeyframe = animCurve [selectedKeyframeIndex];

		newKeyframe.value = newValue;
		animCurve.MoveKey (selectedKeyframeIndex, newKeyframe);

		needsToRefresh = true;
		parentAnimVisualizer.RefreshAnimationCurve (curveNumber);
	}
}
