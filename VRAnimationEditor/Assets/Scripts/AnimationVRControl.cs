using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AnimationVRControl : MonoBehaviour, IButtonAxisReciever {

	const int BUTTON_A = 1;
	const int BUTTON_B = 2;
    const int AXIS_X = 0;

	public AnimationVisualizer animVisual;

	public VRControllerInteractor controller;
	public GameObject playButton_IButtonAxisReciever;

    public float scrubbingSpeed = 3f;

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
				playButton_IButtonAxisReciever.GetComponent<IButtonAxisReciever> ().OnRecieveButton (sourceID, buttonID, buttonState);
			} else if (buttonID == BUTTON_B) {

				AnimationClip newAnimClip = animVisual.GetCurrentClip ();

				AssetDatabase.CreateAsset(newAnimClip, string.Concat("Assets/", "Output", ".anim"));
			}
		}
	}

	public void OnRecieveAxis(int sourceID, int axisID, float axisValue){
        if(axisID == AXIS_X)
        {
            animVisual.PlayAnimationAtSpeed(axisValue*scrubbingSpeed);
        }
	}
}
