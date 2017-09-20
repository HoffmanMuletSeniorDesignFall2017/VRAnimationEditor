using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCurveVisualizer : ScriptableObject {//Visualizer {//MonoBehaviour {

	string parameterTitle;	//The name of the parameter that this curve is for

	public AnimationCurve animCurve;	//IMPORTANT! The actual curve associated with this animation curve

	List<AnimationCurveVisualizer> childrenCurveVisualizers;	//Any children curves related to this (EXPERIMENTAL)

	public GameObject keyframeObject;	//The Gameobject visual representation of a keyframe (JUST the visual part - so can be anything pretty much)

	private List<GameObject> currentKeyframes;	//The current keyframes that have been instantiated 

	public bool selected = false;				//Whether or not ANY of the keyframes of this animation curve are currently being selected by the user

	public KeyframeWorkArea keyframeWorkArea;	//The Gameobject that will become the parent of all instantiated keyframe objects; it should be controlled by another script that is keeping track of the current time of the animation

	public int curveNumber;	//Assigned from the keyframeWorkArea; the number of this animation curve

	public float X_OFFSET_CONSTANT = 2f;	//Used to make drawing nice
	public float Y_OFFSET_CONSTANT = 2f;

	private GameObject selectedKeyframe;	//The keyframe that the user has selected right now
	private int selectedKeyframeIndex;
	public Visualizer visualizerDummy;

	// Use this for initialization
	void Start () {
		if(currentKeyframes == null)
			currentKeyframes = new List<GameObject> ();
		
		visualizerDummy = new Visualizer();

		//Refresh ();
	}
	
	// Update is called once per frame
	void Update () {
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
			nextKeyframe.GetComponent<MovableVisualizer> ().associatedVisualizer = visualizerDummy;
			nextKeyframe.GetComponent<MovableVisualizer> ().constrainedToLocalX = true;

			//TODO: HERE is where I think the bug is! Visualizer is not getting set correctly!

			//-------End set up the MovableVisualizer component--------

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
		//If a keyframe is selected, then it should be writing its value to the animation curve
		if (visualizerDummy.selected) {

			//The below handles MOVEMENT
			Debug.Log("Detected a selection!");

			if (selectedKeyframe == null) {

				for (int i = 0; i < currentKeyframes.Count; i++) {
					if (currentKeyframes [i].GetComponent<MovableVisualizer> ().selected) {
						selectedKeyframe = currentKeyframes [i];
						selectedKeyframeIndex = i;
						break;
					}
				}

			}

			float adjustedPosition = selectedKeyframe.transform.localPosition.x / keyframeWorkArea.bounds;

			if (adjustedPosition > 1f)
				adjustedPosition = 1f;
			else if (adjustedPosition < 0)
				adjustedPosition = 0;

			Keyframe newKeyframe = animCurve[selectedKeyframeIndex];
			Debug.Log(newKeyframe.time);

			//newKeyframe.time = 

			selectedKeyframe.transform.localPosition = new Vector3 (adjustedPosition * keyframeWorkArea.bounds, selectedKeyframe.transform.localPosition.y, selectedKeyframe.transform.localPosition.z);

		} else {
			if (selectedKeyframe != null) {
				selectedKeyframe = null;
			}
		}
	}
}
