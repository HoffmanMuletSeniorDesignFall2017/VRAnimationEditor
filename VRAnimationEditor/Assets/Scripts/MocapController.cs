using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MocapController : MonoBehaviour {

	//Used to do a naive sort of Motion Capture that will hopefully use VR to animate better I guess? Yeah maybe anyways.
	private bool capturing = false;
	private GameObject nodeMarker;

	private Vector3 startingPosition;	//The starting position of the user's controller when they started capturing
	private Vector3 lastPosition;

	private AnimationCurve currentCurve;
	private AnimationCurveVisualizer currentCurveVis;
	private TimelineVisualizer timeVis;
	private AnimationVisualizer animVis;

	public float scalingFactor = 1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		//IF we are currently capturing...
		if (capturing) {

			//Debug.Log ("Detected that we're capturing");
			if (nodeMarker == null) {
				return;		//We can't do anything since we don't have an  associated node
			}

			//TODO: Edit to use VR controller position
			//Override the position/rotation of the animated object (since we want what we are capturing and not what the animation says it should be here)
			//This is why we need LateUpdate()!
			//Debug.Log ("Attempting to update nodeMarker position");

			Debug.Log (Input.mousePosition - startingPosition);

			nodeMarker.transform.parent.position = (Input.mousePosition - startingPosition)*scalingFactor*Time.deltaTime + lastPosition;

			//Debug.Log (Input.mousePosition);

			startingPosition = Input.mousePosition;

			lastPosition = nodeMarker.transform.parent.position;

			//TODO:
			//Record the position/rotation of the captured object
			Vector3 currentPosition = nodeMarker.transform.parent.position;
			Vector3 currentRotation = nodeMarker.transform.parent.position;

			//Make this into a keyframe or something
			float currentTime = timeVis.GetAnimatorTime();
			Keyframe k1 = new Keyframe(currentTime, currentPosition.x);
			Keyframe k2 = new Keyframe(currentTime, currentPosition.y);
			Keyframe k3 = new Keyframe(currentTime, currentPosition.z);

			Keyframe k4 = new Keyframe(currentTime, currentRotation.x);
			Keyframe k5 = new Keyframe(currentTime, currentRotation.y);
			Keyframe k6 = new Keyframe(currentTime, currentRotation.z);


			//Write new keyframe to the given animation curve.
			//TODO: Change this obviously since we ignore like half of the things
			currentCurveVis.AddExistingKeyframe(k2);


		}
		//end if

	}

	public bool IsCapturing(){
		return capturing;
	}

	public void StartCapturing(GameObject captureThisNode, Vector3 startPosition, AnimationCurveVisualizer animCurveVis, TimelineVisualizer tv){
		nodeMarker = captureThisNode;
		capturing = true;
		startingPosition = startPosition;
		lastPosition = nodeMarker.transform.parent.position;
		currentCurveVis = animCurveVis;
		currentCurve = animCurveVis.animCurve;
		timeVis = tv;
	
	}

	public void StopCapturing(){
		nodeMarker = null;
		capturing = false;
	}
		
}
