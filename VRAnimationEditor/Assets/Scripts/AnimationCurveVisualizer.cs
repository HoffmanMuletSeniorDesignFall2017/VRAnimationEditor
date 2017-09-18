using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCurveVisualizer : ScriptableObject {//MonoBehaviour {

	string parameterTitle;	//The name of the parameter that this curve is for

	public AnimationCurve animCurve;	//IMPORTANT! The actual curve associated with this animation curve

	List<AnimationCurveVisualizer> childrenCurveVisualizers;	//Any children curves related to this (EXPERIMENTAL)

	public GameObject keyframeObject;	//The Gameobject visual representation of a keyframe (JUST the visual part - so can be anything pretty much)

	private List<GameObject> currentKeyframes;	//The current keyframes that have been instantiated 

	public KeyframeWorkArea keyframeWorkArea;	//The Gameobject that will become the parent of all instantiated keyframe objects; it should be controlled by another script that is keeping track of the current time of the animation

	public int curveNumber;	//Assigned from the keyframeWorkArea; the number of this animation curve

	public float X_OFFSET_CONSTANT = 2f;	//Used to make drawing nice
	public float Y_OFFSET_CONSTANT = 2f;

	// Use this for initialization
	void Start () {
		if(currentKeyframes == null)
			currentKeyframes = new List<GameObject> ();
		//Refresh ();
	}
	
	// Update is called once per frame
	void Update () {
		
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
}
