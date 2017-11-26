using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AnimationVRControl : MonoBehaviour, IButtonAxisReciever {

	const int BUTTON_A = 1;
	const int BUTTON_B = 2;
    const int THUMBSTICK = 3;
    const int AXIS_X = 0;
    const int AXIS_Y = 1;

    const int FRAME_TIMER_LIMIT = 5;   //How many frames of getting no joystick input should we consider that the user is definitely not using the joystick?

	public AnimationVisualizer animVisual;

	public VRControllerInteractor controller;
	public GameObject playButton_IButtonAxisReciever;

    public GameObject modelAnchor;

    public float scrubbingSpeed = 3f;
    public float scrollingSpeed = 3f;

    private int joystickFrameTimer = 0;     //Used to see if we haven't gotten joystick input in a while; this means that the joystick is likely in nuetral position and so we should reset as necessary
    private bool usedJoystick = false;

	// Use this for initialization
	void Start () {
		if (animVisual == null)
			Debug.LogError ("Error! Animation VR Control was not given an animation visualizer to talk to");

		if (controller == null)
			Debug.LogError ("Error! Animation VR Control was not given a controller to listen to");

        if (modelAnchor == null)
            Debug.LogError("Error! Animation VR control was not given a model anchor to move");

		controller.AddButtonAxisFocus (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		if(joystickFrameTimer++ > FRAME_TIMER_LIMIT && usedJoystick)
        {
            animVisual.PlayAnimationAtSpeed(0);     //Pause output
            usedJoystick = false;
        }
	}

	public void OnRecieveButton (int sourceID, int buttonID, bool buttonState){
		if (buttonState == true) {
			if (buttonID == BUTTON_A) {
				animVisual.TogglePlayAnimation ();
				playButton_IButtonAxisReciever.GetComponent<IButtonAxisReciever> ().OnRecieveButton (sourceID, buttonID, buttonState);
			} else if (buttonID == BUTTON_B) {

				AnimationClip newAnimClip = animVisual.GetCurrentClip ();

				AssetDatabase.CreateAsset(newAnimClip, string.Concat("Assets/", "Output", ".anim"));
			} else if (buttonID == THUMBSTICK)
            {
                if(modelAnchor.transform.parent == null)
                {
                    modelAnchor.transform.parent = controller.gameObject.transform;
                }
                else
                {
                    modelAnchor.transform.parent = null;
                }
            }
		}
	}

    private float movement = 0;

	public void OnRecieveAxis(int sourceID, int axisID, float axisValue){
        if(axisID == AXIS_X)
        {
            animVisual.PlayAnimationAtSpeed(axisValue*scrubbingSpeed);

            joystickFrameTimer = 0;
            usedJoystick = true;
        }
        if(axisID == AXIS_Y)
        {
            
            //animVisual.ToggleToggle();
            
            Vector3 old = animVisual.keyframeWorkArea.GetComponent<KeyframeWorkArea>().keyframeSectionObject.transform.localPosition;
            if (old.y + axisValue * scrollingSpeed < 0)
            {
                animVisual.keyframeWorkArea.GetComponent<KeyframeWorkArea>().keyframeSectionObject.transform.localPosition = new Vector3(old.x, 0, old.z);
            }
            else
            {
                animVisual.keyframeWorkArea.GetComponent<KeyframeWorkArea>().keyframeSectionObject.transform.localPosition = new Vector3(old.x, old.y + axisValue * scrollingSpeed, old.z);
            }
            
            //Transform t = animVisual.keyframeWorkArea.GetComponent<KeyframeWorkArea>().keyframeSectionObject.transform;

            //float ss;

            //if(movement + axisValue * scrollingSpeed < 0)
            //{
            //    if (movement == 0)
            //        ss = 0;
            //    else
            //        ss = movement;

            //    movement = 0;
                
            //}
            //else
            //{
            //    ss = axisValue * scrollingSpeed;
            //    movement += axisValue * scrollingSpeed;
            //}

            //for(int i = 0; i < t.childCount; i++)
            //{
            //    Vector3 old = t.GetChild(i).localPosition;
            //    t.GetChild(i).localPosition = new Vector3(old.x, old.y + ss, old.z);
            //}
        }
	}
}
