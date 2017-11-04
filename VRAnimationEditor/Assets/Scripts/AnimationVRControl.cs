using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationVRControl : MonoBehaviour, IButtonAxisReciever {

	const int BUTTON_A = 1;

	public AnimationVisualizer animVisual;

	public VRControllerInteractor controller;

	// Use this for initialization
	void Start () {
		if (animVisual == null)
			Debug.LogError ("Error! Animation VR Control was not given an animation visualizer to talk to");

		if (controller == null)
			Debug.LogError ("Error! Animation VR Control was not given a controller to listen to");

		controller.AddButtonAxisFocus (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnRecieveButton (int sourceID, int buttonID, bool buttonState){
		if (buttonState == true) {
			if (buttonID == BUTTON_A) {
				animVisual.TogglePlayAnimation ();
			}
		}
	}

	public void OnRecieveAxis(int sourceID, int axisID, float axisValue){

	}
}
