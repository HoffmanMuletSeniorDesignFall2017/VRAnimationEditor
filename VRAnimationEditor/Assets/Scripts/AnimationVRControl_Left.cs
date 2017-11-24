using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AnimationVRControl_Left : MonoBehaviour, IButtonAxisReciever
{

    const int BUTTON_X = 1;
    const int BUTTON_Y = 2;
    const int THUMBSTICK = 3;
    const int AXIS_Y = 0;

    public AnimationVisualizer animVisual;

    public VRControllerInteractor controller;
    public VRControllerInteractor otherController;
    public GameObject playButton_IButtonAxisReciever;

    public GameObject modelAnchor;

    public float scrubbingSpeed = 3f;

    private GameObject dummyNode;
    private Vector3 origScale;
    private bool scaling = false;

    private float offset = 0f;

    // Use this for initialization
    void Start()
    {
        if (animVisual == null)
            Debug.LogError("Error! Animation VR Control was not given an animation visualizer to talk to");

        if (controller == null)
            Debug.LogError("Error! Animation VR Control was not given a controller to listen to");

        if (modelAnchor == null)
            Debug.LogError("Error! Animation VR control was not given a model anchor to move");

        controller.AddButtonAxisFocus(this.gameObject);

        if(dummyNode == null)
            dummyNode = new GameObject();
        dummyNode.name = "VR control Left dummy Node";
    }

    // Update is called once per frame
    void Update()
    {
        if (scaling)
        {
            modelAnchor.transform.localScale = origScale * (Vector3.Distance(otherController.gameObject.transform.position, controller.gameObject.transform.position) + offset);
        }
    }

    public void OnRecieveButton(int sourceID, int buttonID, bool buttonState)
    {
        if (buttonState == true)
        {
            if (buttonID == BUTTON_X)
            {
                animVisual.TogglePlayAnimation();
                playButton_IButtonAxisReciever.GetComponent<IButtonAxisReciever>().OnRecieveButton(sourceID, buttonID, buttonState);
            }
            else if (buttonID == BUTTON_Y)
            {

                AnimationClip newAnimClip = animVisual.GetCurrentClip();

                AssetDatabase.CreateAsset(newAnimClip, string.Concat("Assets/", "Output", ".anim"));
            }
            else if (buttonID == THUMBSTICK)
            {
                if (scaling == false)
                {
                    dummyNode.transform.position = controller.gameObject.transform.position + controller.transform.TransformDirection(1, 0, 0);
                    origScale = modelAnchor.transform.localScale;
                    offset = 1f - Vector3.Distance(otherController.gameObject.transform.position, controller.gameObject.transform.position);
                    scaling = true;
                }
                else
                {
                    scaling = false;
                }
            }
        }
    }

    public void OnRecieveAxis(int sourceID, int axisID, float axisValue)
    {
        /*if (axisID == AXIS_Y)
        {
            animVisual.PlayAnimationAtSpeed(axisValue * scrubbingSpeed);
        }*/
    }
}
